# WebApplication1
This is a simple web application that displays a list of items in a table. The items are stored in a database and are retrieved using Entity Framework.

## Endpoints

### api/account

##### POST `/api/account/login` - Logs in a user
###### Request Body
```json
{
  "username": "string",
  "password": "string"
}
```

##### POST `/api/account/register` - Registers a new user
###### Request Body
```json
{
  "username": "string",
  "password": "string"
}
```

### api/comments
##### GET `/api/comment` - Retrieves a list of comments
##### GET `/api/comment/{id}` - Retrieves a comment by ID

##### POST `/api/comment` - Adds a new comment
###### Request Body
```json
{
  "title": "string",
  "content": "string"
}
```

##### PUT `/api/comment/{id}` - Updates a comment by ID
###### Request Body
```json
{
  "title": "string",
  "content": "string"
}
```

##### DELETE `/api/comment/{id}` - Deletes a comment by ID

### api/stock
##### GET `/api/stock` - Retrieves a list of stocks
##### GET `/api/stock/{id}` - Retrieves a stock by ID

##### POST `/api/stock` - Adds a new stock
###### Request Body
```json
{
  "symbol": "string",
  "companyName": "string",
  "purchase": 0,
  "lastDiv": 0.00,
  "industry": "string",
  "marketCap": 0.00
}
```

##### PUT `/api/stock/{id}` - Updates a stock by ID
###### Request Body
```json
{
  "symbol": "string",
  "companyName": "string",
  "purchase": 0,
  "lastDiv": 0.00,
  "industry": "string",
  "marketCap": 0.00
}

```
##### DELETE `/api/stock/{id}` - Deletes a stock by ID

### api/portfolio
##### GET `/api/portfolio` - Retrieves a list of portfolios

##### POST `/api/portfolio` - Adds a new portfolio
###### Request Body
```json
{
  "symbol": "string"
}
```

##### DELETE `/api/portfolio/{id}` - Deletes a portfolio by ID
