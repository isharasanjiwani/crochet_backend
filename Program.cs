using System.Text;
using CrochetWorldApi.Data;
using CrochetWorldApi.Handlers;
using CrochetWorldApi.Services;
using CrochetWorldApi.Services.Auth;
using CrochetWorldApi.Services.General;
using CrochetWorldApi.Services.Image;
using CrochetWorldApi.Services.Notification;
using CrochetWorldApi.Services.Product;
using CrochetWorldApi.Services.Profile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Error)
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAutoMapper(
    cfg => { },
    typeof(MappingProfile)
);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = new Dictionary<string, string[]>();

            foreach (var state in context.ModelState)
            {
                var rawKey = state.Key;
                
                // 1. Strip JSON path notation (e.g., "$.productId" -> "productId")
                var cleanKey = rawKey.StartsWith("$.") ? rawKey.Substring(2) : rawKey;
                
                // 2. Normalize to PascalCase to match DTO property names (e.g., "productId" -> "ProductId")
                if (!string.IsNullOrEmpty(cleanKey))
                {
                    cleanKey = char.ToUpper(cleanKey[0]) + cleanKey.Substring(1);
                }

                var messages = new List<string>();

                foreach (var error in state.Value.Errors)
                {
                    var errorMessage = error.ErrorMessage;

                    // 3. Intercept default System.Text.Json conversion errors and translate them
                    if (errorMessage.Contains("could not be converted to System.Nullable") || 
                        errorMessage.Contains("JSON value could not be converted"))
                    {
                        errorMessage = $"{cleanKey} is invalid.";
                    }

                    messages.Add(errorMessage);
                }

                if (messages.Count > 0)
                {
                    errors[cleanKey] = messages.ToArray();
                }
            }

            // Construct standard RFC 9110 Problem Details response
            var problemDetails = new ValidationProblemDetails()
            {
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Errors = errors
            };

            return new BadRequestObjectResult(problemDetails);
        };
    });

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IGeneralService, GeneralService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
