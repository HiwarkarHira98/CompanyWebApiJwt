using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CompanyApiJwt.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//var key = Encoding.ASCII.GetBytes("YourSecretKeyHere");

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Company API", Version = "v1" });

    // Enable JWT Authentication in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});
var secretKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
{
    throw new ArgumentException("JWT Secret Key must be at least 32 characters long.");
}

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));



// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"]
            };
        });

    builder.Services.AddAuthorization();
    var app = builder.Build();
      // Enable Swagger Middleware
     if (app.Environment.IsDevelopment())
     {
     app.UseSwagger();
     app.UseSwaggerUI(c =>
     {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company API V1");
     });
     }



   

// Login Endpoint
app.MapPost("/login", ([FromBody] LoginModel login) =>
{
    if (login.Username == "admin" && login.Password == "password")
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, login.Username) }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Results.Ok(new { token = tokenHandler.WriteToken(token) });
    }
    return Results.Unauthorized();
});

var companies = new List<Company>();

// CRUD Endpoints
app.MapPost("/companies", [Authorize] (HttpContext context, [FromBody] Company company) =>
{
    var token = context.Request.Headers["Authorization"].ToString();
    Console.WriteLine("Received Token: " + token);

    if (companies.Any(c => c.CompanyCode == company.CompanyCode))
        return Results.BadRequest("Company with this code already exists.");

    companies.Add(company);
    return Results.Created($"/companies/{company.CompanyCode}", company);
});

app.MapGet("/companies", [Authorize] () => Results.Ok(companies));

app.MapGet("/companies/{companyCode}", [Authorize] (string companyCode) =>
{
    var company = companies.FirstOrDefault(c => c.CompanyCode == companyCode);
    return company is not null ? Results.Ok(company) : Results.NotFound();
});

app.MapPut("/companies/{companyCode}", [Authorize] (string companyCode, [FromBody] Company updatedCompany) =>
{
    var company = companies.FirstOrDefault(c => c.CompanyCode == companyCode);
    if (company is null) return Results.NotFound();

    company.CompanyName = updatedCompany.CompanyName;
    company.CompanyAddress = updatedCompany.CompanyAddress;
    company.CompanyGSTN = updatedCompany.CompanyGSTN;
    company.CompanyUserId = updatedCompany.CompanyUserId;
    company.CompanyStatus = updatedCompany.CompanyStatus;
    company.CompanyStartDate = updatedCompany.CompanyStartDate;
    company.CompanyNatureOfWork = updatedCompany.CompanyNatureOfWork;

    return Results.Ok(company);
});

app.MapDelete("/companies/{companyCode}", [Authorize] (string companyCode) =>
{
    var company = companies.FirstOrDefault(c => c.CompanyCode == companyCode);
    if (company is null) return Results.NotFound();

    companies.Remove(company);
    return Results.NoContent();
});
app.UseAuthentication();
app.UseAuthorization();
app.Run();


internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
