using System.Text.Json.Serialization;
using Coravel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CaddyBrowseDeleter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // 設定服務
            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddScheduler();
            builder.Services.AddTransient<DeleteFilesJob>();

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            
            // 自動建立 Migration 並更新資料庫
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            }

            // 設定 Coravel 排程
            var provider = app.Services;
            provider.UseScheduler(scheduler =>
            {
                scheduler.Schedule<DeleteFilesJob>() // 每分鐘執行
                    .EveryMinute().RunOnceAtStart();
                    // .DailyAtHour(3).RunOnceAtStart(); // 每日半夜三點執行

            });

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            // {
                app.UseSwagger();
                app.UseSwaggerUI();
            // }

            // app.UseHttpsRedirection();

            // app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
