using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApiCore.Filters;
using WebApiCore.Options;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = new JwtSettings();
builder.Configuration.Bind("Token",jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudiences = jwtSettings.Audiences,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Read the token from the cookie
            var accessToken = context.Request.Cookies["accessToken"];
            if (!string.IsNullOrEmpty(accessToken)) context.Token = accessToken;
            else if (context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();

builder.Services.AddMemoryCache();

// Register filters in DI for [ServiceFilter] and DI resolution
builder.Services.AddScoped<CacheAsyncActionFilter>();
builder.Services.AddScoped<ExecutionTimerActionFilter>();
builder.Services.AddScoped<ApiExceptionFilter>();
builder.Services.AddScoped<StandardizedResponseEnvelopeFilter>();

builder.Services.AddControllers(options =>
{
    // Global filters that execute across all endpoints:
    options.Filters.Add<CustomResourceFilter>();  // Tracks overall pipeline duration
    options.Filters.Add<CustomActionFilter>();    // Logs before and after action execution
    options.Filters.Add<CustomResultFilter>();    // Injects diagnostic headers (X-Correlation-ID, X-Processed-At)
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();