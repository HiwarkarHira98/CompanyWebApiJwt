Features:
JWT Authentication: Secure API access using JSON Web Tokens (JWT).
User Login: Generates a JWT token upon successful authentication.
Company Management: Supports creating, reading, updating, and deleting company details.
Swagger Integration: API documentation and testing using Swagger.


Technologies Used:
ASP.NET Core Minimal API
JWT (JSON Web Token) Authentication
Swagger for API documentation
C#

Setup Instructions:
Prerequisites
.NET 8 or later installed
Postman (or any API testing tool)
Visual Studio 

Running the API
Login and Get JWT Token

Endpoint: POST /login
Request Body:
json
{
  "username": "admin",
  "password": "password"
}
Response:
json
{
  "token":  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwibmJmIjoxNzQxMTgyOTQzLCJleHAiOjE3NDExODY1NDMsImlhdCI6MTc0MTE4Mjk0M30.zOHAsfD93uCCbhGSfIXYIl9wViJsbtPMq4OZCK4VxwY"
"
}
Use Token in API Requests

In Postman, add the token in the Authorization header:

Key: Authorization
Value: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwibmJmIjoxNzQxMTgyOTQzLCJleHAiOjE3NDExODY1NDMsImlhdCI6MTc0MTE4Mjk0M30.zOHAsfD93uCCbhGSfIXYIl9wViJsbtPMq4OZCK4VxwY

Company CRUD Operations

Create Company: POST /companies
Get All Companies: GET /companies
Get Company by Code: GET /companies/{companyCode}
Update Company: PUT /companies/{companyCode}
Delete Company: DELETE /companies/{companyCode}

Troubleshooting
401 Unauthorized Error : I am getting this error while I Post a request in Postman. 
