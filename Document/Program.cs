using Document.Data;
using Document.Mapping;
using Document.Service;
using Document.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔗 Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 📦 DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 📦 AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 📦 Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DocumentService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<VersionService>();
builder.Services.AddScoped<ArchiveService>();
builder.Services.AddScoped<ArchiveJob>(); // Hangfire job uchun muhim!

// 📦 Hangfire (PostgreSQL bilan)
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(connectionString));
builder.Services.AddHangfireServer();

// 📦 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "📄 Document API",
        Version = "v1"
    });
    c.OperationFilter<FileUploadOperationFilter>();
});
// hello world
// 📦 MVC
builder.Services.AddControllers();

var app = builder.Build();

// 📊 Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

// 📆 Har kuni arxivlash uchun cron job
RecurringJob.AddOrUpdate<ArchiveJob>(
    "archive-old-documents",
    job => job.ExecuteAsync(),
    Cron.Daily); // Har kuni yarim tunda

// 🌐 Static fayllar
app.UseStaticFiles();

// 🧪 Swagger dev rejimda
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔐 Middlewares
app.UseHttpsRedirection();
app.UseAuthorization();

// 🌐 Routing
app.MapControllers();

// ▶️ Run
app.Run();
