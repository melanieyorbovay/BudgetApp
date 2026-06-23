using System.Globalization;
using BudgetApp.ModelsV2;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("BudgetApp"))
            );

            var cultureInfo = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
            {
                options.ModelBinderProviders.Insert(0,
                    new Microsoft.AspNetCore.Mvc.ModelBinding.Binders.SimpleTypeModelBinderProvider());

            });
            builder.Services.AddOptions<Microsoft.AspNetCore.Routing.RouteOptions>()
                .Configure(options => { });
            //Forcer le model binder à utiliser la culture invariante

            builder.Services.AddMvc()
                .AddMvcOptions(options =>
                {
                    options.ModelMetadataDetailsProviders.Clear();
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            

            app.Run();
        }
    }
}
