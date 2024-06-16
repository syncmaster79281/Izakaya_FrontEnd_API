using Hangfire;
using Hangfire.SqlServer;
using ISPAN.Izakaya.BLL_Service_;
using ISPAN.Izakaya.EFModels.Models;
using ISPAN.Izakaya.IBLL_IService_;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace ISPAN.Izakaya.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddSingleton<EmailHelper>();


            //HttpClient
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<LinePayService>();


            // 注册 Hangfire 服务
            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true
                }));

            builder.Services.AddHangfireServer();

            // 連線SQL
            builder.Services.AddDbContext<IzakayaContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            //要拆分Service 要在這裡注入DI容器
            builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICouponService, CouponService>();



            //開放CORS 寫法一
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowVue",
                builder => builder.WithOrigins("https://localhost:8080").WithMethods("*").WithHeaders("*"));
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
            app.UseRouting();

            //套用CORS策略    要放在 UseHttpsRedirection 前面
            app.UseCors("AllowVue");


            app.UseAuthorization();
            app.UseResponseCaching();


            // 配置 Hangfire Dashboard 和 Hangfire Server
            app.UseHangfireDashboard();

            app.MapControllers();


            app.Run();
        }
    }
}
