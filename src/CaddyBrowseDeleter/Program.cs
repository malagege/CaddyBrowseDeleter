using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CaddyBrowseDeleter;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 設定服務
        builder.Services.AddDbContext<AppDbContext>();

        // 加入 MVC 核心服務並啟用 API 探索
        builder.Services.AddMvcCore()
                        .AddApiExplorer();

        
        // 加入 Swagger 服務
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CaddyBrowseDeleter API", Version = "v1" });
        });

        var app = builder.Build();

        // 自動建立 Migration 並更新資料庫
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }

        // 設定中介軟體
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }


        // 啟用 Swagger 和 Swagger UI
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CaddyBrowseDeleter API v1");
        });
        // 設定 API 控制器
        app.MapControllers();

        app.Run();
    }
}
