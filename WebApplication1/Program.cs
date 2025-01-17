using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApplication1;
using WebApplication1.Data;
using WebApplication1.interfaces;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Service;
using Scalar.AspNetCore;
using WebApplication1.Tracing;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Extensions.Docker.Resources;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"])),
        ValidateLifetime = true,
    };
});
builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options => {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddSingleton(new AppMetrics(GlobalData.SourceName, GlobalData.ApplicationVersion));
// Shared resources for OTEL signals
var resourceBuilder = ResourceBuilder.CreateDefault()
    // add attributes for the name and version of the service
    .AddService(GlobalData.ApplicationName, serviceVersion: GlobalData.ApplicationVersion)
    // add attributes for the OpenTelemetry SDK version
    .AddTelemetrySdk()
    // Populate platform details
    .AddDetector(new DockerResourceDetector())
    // add custom attributes
    .AddAttributes(new Dictionary<string, object>
    {
        [ResourceSemanticConventions.AttributeHostName] = Environment.MachineName,
        [ResourceSemanticConventions.AttributeOsDescription] = RuntimeInformation.OSDescription,
        [ResourceSemanticConventions.AttributeDeploymentEnvironment] =
            builder.Environment.EnvironmentName.ToLowerInvariant(),
    });


// Check if the console exporter is enabled
bool isConsoleExporterEnabled = bool.Parse(
    builder.Configuration["OTLP:ConsoleExporter"] ?? "false"
);
// Configure tracing and metrics
builder.Services
    .AddOpenTelemetry()
    .WithMetrics(meterProviderBuilder => {
        meterProviderBuilder
            // add rich tags to our metrics
            .SetResourceBuilder(resourceBuilder)
            // receive metrics from built-in sources
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            // receive metrics from custom sources
            .AddMeter(GlobalData.SourceName)
            // expose metrics in Prometheus exposition format
            .AddPrometheusExporter();

        // stream metrics to the console
        if(isConsoleExporterEnabled) {
            meterProviderBuilder.AddConsoleExporter();
        }
    });

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.UseSwaggerUI();
    app.MapScalarApiReference();
}

// Enable the /metrics endpoint which will be scraped by Prometheus
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapHealthChecks("/health", new HealthCheckOptions {
    AllowCachingResponses = false,
    ResultStatusCodes = {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();


app.Run();
