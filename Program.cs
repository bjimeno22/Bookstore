using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Bookstore;
using Bookstore.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Bookstore") ?? throw new InvalidOperationException("Connection string 'VballTeamDbContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors( policy => 
{
        policy.AddPolicy("bookstore-allowall", config => 
        {
            config.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

#region JWT Validation
/*******************************************
 *  Start JWT Security Configuration
 *  ****************************************/
var secret = "MyVerySuperSecureSecretSharedKey";
var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
var issuer = "http://www.ecu.edu";
var audience = "http://www.ecu.edu";

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.ClaimsIssuer = issuer;
    options.MapInboundClaims = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = secretKey,

        ValidateIssuer = true,
        ValidIssuer = issuer,

        ValidateAudience = true,
        ValidAudience = audience,

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            return Task.CompletedTask;

            // ToDo: can check for different kinds of failures
        }
    };
});

/*****************************************
 *  End JWT Security Configuration
 *  **************************************/
#endregion

var app = builder.Build();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin Only", policy => policy.RequireClaim("rol", "Admin"));
    options.AddPolicy("Authenticated", policy => policy.RequireClaim("rol", " User", "Admin"));
});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// one calls the next below it
app.UseHttpsRedirection();
app.UseCors("bookstore-allowall");

app.UseAuthorization();

app.MapControllers();

app.Run();//endpoints live here
