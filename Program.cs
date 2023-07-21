
global using simpleapi.Data;

global using Microsoft.EntityFrameworkCore;

// I make 
global using simpleapi.Services.EmailService;
global using simpleapi.Models;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


// add Model/HotelBooking namespace 


/*******
 * Creat 
 * 
 ******/

// this line is like a public static void main(array args[])
var builder = WebApplication.CreateBuilder(args);

// below code to test the api crud without real database it use inMemoryDatabase which is for test only
//builder.Services.AddDbContext<DataContext>(
//  opt => opt.UseInMemoryDatabase("BookingDb"));

// There're two ways to Connection to SQL Server
// Connection to SQL Server Number 1
/*
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
*/

/*******
 * Build 
 * 
 ******/

// Connection to SQL Server Number 2
// Connection SQL Server when the connection string in the DataContext instead of Program.cs
builder.Services.AddDbContext<DataContext>();

// Register a service for SMTP
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ______ Enable JWT Bearer Toekn ________
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // definding the type of token 
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // using a key for validation options
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });



/*******
 * Run 
 * 
 ******/

var app = builder.Build();

// middlware and endpoint
app.MapGet("/", () => "Hello World");

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

// add auth middlware this line should be above the MapContoroller
app.UseAuthorization();

app.MapControllers();

app.Run();
