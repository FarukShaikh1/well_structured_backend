// API/Program.cs
using FMS_Collection.API.Authorization;
using FMS_Collection.API.Middleware;
using FMS_Collection.Application.Services;
using FMS_Collection.Application.Validators;
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Infrastructure.Data;
using FMS_Collection.Infrastructure.Repositories;
using FMS_Collection.Infrastructure.Security;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

// ── Bootstrap Serilog early so startup errors are captured ───────────────────
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables()
        .Build())
    .Enrich.FromLogContext()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting FMS Collection API");

    var builder = WebApplication.CreateBuilder(args);

    // ── Serilog ───────────────────────────────────────────────────────────────
    builder.Host.UseSerilog((ctx, services, config) =>
        config.ReadFrom.Configuration(ctx.Configuration)
              .ReadFrom.Services(services)
              .Enrich.FromLogContext()
              .Enrich.WithProperty("Application", "FMS_Collection.API"));

    // ── Configuration bindings ────────────────────────────────────────────────
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
        ?? throw new InvalidOperationException("JwtSettings section is missing from configuration.");
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

    // ── Memory cache ──────────────────────────────────────────────────────────
    builder.Services.AddMemoryCache();

    // ── Response compression ──────────────────────────────────────────────────
    builder.Services.AddResponseCompression(opts => opts.EnableForHttps = true);

    // ── JWT Authentication ────────────────────────────────────────────────────
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Skip issuer/audience validation when the config value is empty —
                // avoids SecurityTokenInvalidIssuerException on Azure where env vars
                // may not be set and empty-string validation behaves unexpectedly.
                ValidateIssuer = !string.IsNullOrEmpty(jwtSettings.Issuer),
                ValidateAudience = !string.IsNullOrEmpty(jwtSettings.Audience),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                // 5 minutes is the safe standard for cloud deployments where
                // instances may have small clock differences (30 seconds was too tight).
                ClockSkew = TimeSpan.FromMinutes(5)
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = ctx =>
                {
                    if (ctx.Exception is SecurityTokenExpiredException)
                        ctx.Response.Headers["Token-Expired"] = "true";
                    return Task.CompletedTask;
                },
                // Return a JSON body for 401 so the Angular interceptor can parse it.
                OnChallenge = async ctx =>
                {
                    ctx.HandleResponse();
                    ctx.Response.StatusCode = 401;
                    ctx.Response.ContentType = "application/problem+json";
                    var body = JsonSerializer.Serialize(new
                    {
                        status = 401,
                        title = "Unauthorized",
                        detail = "Authentication required. Please log in."
                    });
                    await ctx.Response.WriteAsync(body);
                },
                // Return a JSON body for 403 so the Angular interceptor can parse it.
                OnForbidden = async ctx =>
                {
                    ctx.Response.StatusCode = 403;
                    ctx.Response.ContentType = "application/problem+json";
                    var body = JsonSerializer.Serialize(new
                    {
                        status = 403,
                        title = "Forbidden",
                        detail = "You do not have permission to access this resource."
                    });
                    await ctx.Response.WriteAsync(body);
                }
            };
        });

    // ── Authorization — dynamic permission policies ───────────────────────────
    builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
    builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
    builder.Services.AddAuthorization();

    // ── CORS ──────────────────────────────────────────────────────────────────
    var allowedOrigins = builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>() ?? [];

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("FrontendPolicy", policy =>
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials());
    });

    // ── Rate limiting ─────────────────────────────────────────────────────────
    builder.Services.AddRateLimiter(options =>
    {
        // Strict limit for auth endpoints (login, OTP)
        options.AddFixedWindowLimiter("login", config =>
        {
            config.PermitLimit = 5;
            config.Window = TimeSpan.FromMinutes(1);
            config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            config.QueueLimit = 0;
        });

        // General API limit
        options.AddFixedWindowLimiter("api", config =>
        {
            config.PermitLimit = 200;
            config.Window = TimeSpan.FromMinutes(1);
            config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            config.QueueLimit = 10;
        });

        options.RejectionStatusCode = 429;
    });

    // ── Health checks ─────────────────────────────────────────────────────────
    builder.Services.AddHealthChecks()
        .AddSqlServer(
            builder.Configuration.GetConnectionString("FMSConnectionString")!,
            name: "sqlserver",
            tags: ["db", "sql"]);

    // ── FluentValidation ──────────────────────────────────────────────────────
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

    // ── JSON options + Controllers ────────────────────────────────────────────
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.Converters.Add(
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false));
        });

    builder.Services.AddEndpointsApiExplorer();

    // ── Swagger with JWT support ──────────────────────────────────────────────
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "FMS Collection API",
            Version = "v1",
            Description = "Financial Management & Collection System"
        });

        var jwtScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter 'Bearer {your JWT token}'",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };
        c.AddSecurityDefinition("Bearer", jwtScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { jwtScheme, Array.Empty<string>() }
        });
    });

    // ── Infrastructure — Data & Repositories ──────────────────────────────────
    builder.Services.AddSingleton<DbConnectionFactory>();

    builder.Services.AddScoped<ISpecialOccasionRepository, SpecialOccasionRepository>();
    builder.Services.AddScoped<IAssetRepository, AssetRepository>();
    builder.Services.AddScoped<ICoinNoteCollectionRepository, CoinNoteCollectionRepository>();
    builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
    builder.Services.AddScoped<ICommonListRepository, CommonListRepository>();
    builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
    builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
    builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
    builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
    builder.Services.AddScoped<IRoutineRepository, RoutineRepository>();
    builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
    builder.Services.AddScoped<ICredentialRepository, CredentialRepository>();
    builder.Services.AddScoped<INotificationSender, NotificationSender>();
    builder.Services.AddSingleton<IOtpRepository, OtpRepository>();
    builder.Services.AddSingleton<IErrorRepository, ErrorRepository>();
    builder.Services.AddScoped<IFamilyRepository, FamilyRepository>();

    // ── Security services ─────────────────────────────────────────────────────
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
    builder.Services.AddScoped<IAuditService, AuditService>();

    // ── Application — Services ────────────────────────────────────────────────
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<RoleService>();
    builder.Services.AddScoped<AssetService>();
    builder.Services.AddScoped<CoinNoteCollectionService>();
    builder.Services.AddScoped<DocumentService>();
    builder.Services.AddScoped<CommonListService>();
    builder.Services.AddScoped<SettingsService>();
    builder.Services.AddScoped<TransactionService>();
    builder.Services.AddScoped<SpecialOccasionService>();
    builder.Services.AddScoped<NotificationService>();
    builder.Services.AddScoped<OtpService>();
    builder.Services.AddScoped<AzureBlobService>();
    builder.Services.AddScoped<AdminService>();
    builder.Services.AddScoped<FamilyService>();

    // ── Build app ─────────────────────────────────────────────────────────────
    var app = builder.Build();

    // ── Middleware pipeline (order matters!) ──────────────────────────────────
    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseSerilogRequestLogging(opts =>
    {
        opts.MessageTemplate =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    });

    app.UseResponseCompression();
    
    //if (app.Environment.IsDevelopment())
    //{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FMS Collection API v1"));
    //}

    app.UseCors("FrontendPolicy");
    app.UseRateLimiter();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
