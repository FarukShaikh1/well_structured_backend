using FMS_Collection.Application.Services;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Infrastructure.Data;
using FMS_Collection.Infrastructure.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Register services
//builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));
builder.Services.AddSingleton<DbConnectionFactory>();
builder.Services.AddScoped<ISpecialOccasionRepository, SpecialOccasionRepository>();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<ICoinNoteCollectionRepository, CoinNoteCollectionRepository>();
builder.Services.AddScoped<ICommonListRepository, CommonListRepository>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IOtpRepository, OtpRepository>();
builder.Services.AddScoped<INotificationSender, NotificationSender>();

builder.Services.AddScoped<AssetService>();
builder.Services.AddScoped<CoinNoteCollectionService>();
builder.Services.AddScoped<CommonListService>();
builder.Services.AddScoped<SettingsService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<SpecialOccasionService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OtpService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false)
        );
    });

// ✅ Define a named CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",                    // Angular dev
            "http://farukshaikh-001-site1.ltempurl.com" // Deployed site (if needed)
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger should be available in all environments (optional)
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Use CORS *before* routing / authenticationUseAuthorization

app.UseCors("AllowAngularApp");
// ✅ Optional: Handle preflight requests explicitly
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        return;
    }
    await next();
});

//app.UseAuthentication();   // (Add this later if you use auth)
//app.UseAuthorization();

app.MapControllers();
app.Run();
