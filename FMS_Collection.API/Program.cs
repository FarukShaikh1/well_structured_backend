using FMS_Collection.Application.Services;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Infrastructure.Data;
using FMS_Collection.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Register services
//builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));
builder.Services.AddSingleton<DbConnectionFactory>();
builder.Services.AddScoped<ISpecialOccasionRepository, SpecialOccasionRepository>();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<ICoinNoteCollectionRepository, CoinNoteCollectionRepository>();
builder.Services.AddScoped<ICommonListRepository, CommonListRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<AssetService>();
builder.Services.AddScoped<CoinNoteCollectionService>();
builder.Services.AddScoped<CommonListService>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddScoped<SettingsService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<SpecialOccasionService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Allow CORS
app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
