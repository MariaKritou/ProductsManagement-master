using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using ProductsManagement.Controllers;
using ProductsManagement.Hubs;
using ProductsManagement.Models;
using ProductsManagement.Repositories;
using SQW;
using SQW.Interfaces;

//using SignalRChat.Hubs;

namespace ProductsManagement
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
    {
      ///////////////////////////////////////////////////////////////////////////////////////////
      //συνδεση με βαση oracle (βιβλιοθηκη sqwWorker απο nuGet)

      var databaseOptionsSection = Configuration.GetSection("DatabaseOptions");
      services.Configure<DatabaseOptions>(databaseOptionsSection);
      var databaseOptions = databaseOptionsSection.Get<DatabaseOptions>();
      //πιο απλα :  var databaseOptions = new DatabaseOptions();
      //            Configuration.GetSection("DatabaseOptions").Bind(databaseOptions);

      var oraConfig = new SQWOraConfig
      {
        host = databaseOptions.dbServer,
        instance = databaseOptions.dbInstance,
        userName = databaseOptions.userName,
        password = databaseOptions.password
      };

      var sequenceGenerator = new SQWSequenceGenerator();

      SQWOraWorker worker = new SQWOraWorker(oraConfig, sequenceGenerator);
      services.AddSingleton<ISQWWorker>(worker);

      //////////////////////////////////////////////////////////////////////////////////

      services.Configure<ProductOptions>(Configuration.GetSection("ProductOptions"));
   
      services.Configure<CookiePolicyOptions>(options =>
      {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });

      //προσθεσα για τον φακελο του resx για την γλωσσα
      services.AddLocalization(options =>
      {
        options.ResourcesPath = "Resources";
      });

      services.AddDistributedMemoryCache();

      services.AddSession(options =>
      {

        options.Cookie.Name = ".ProductManagement.Session";
        // Set a short timeout for easy testing.
        options.IdleTimeout = TimeSpan.FromMinutes(10);
        options.Cookie.HttpOnly = true;
        // Make the session cookie essential
        options.Cookie.IsEssential = true;
      });


      services.AddMvc()
        .AddViewLocalization(options => { options.ResourcesPath = "Resources"; })
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      services.Configure<RequestLocalizationOptions>(options =>
      {
        var supportedCultures = new List<CultureInfo>
        {
          new CultureInfo("en"),
          new CultureInfo("el"),
        };

          options.DefaultRequestCulture = new RequestCulture("en"); //default language
          options.SupportedCultures = supportedCultures; //the supported are the ones i created above 
          options.SupportedUICultures = supportedCultures;
      });
 

      //προσθεσα για signalr
      services.AddSignalR();
      // services.AddSingleton<IProductRepository, ProductMemoryRepository>();
      services.AddSingleton<IProductRepository, ProductDatabaseRepository>();
      services.AddSingleton<UserRepository>();
      services.AddSingleton<OrderRepository>();

      //προσθεσα αυτη για αυθεντικοποιηση
      services
        .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => 
        {
          options.LoginPath = "/Account/Login";
          options.AccessDeniedPath = "/Account/UnauthorizedView";
        });
      //προσθεσα για εξουσιοδότιση
      services.AddAuthorization(options =>
      {
        options.AddPolicy("admin", policy => policy.RequireRole("admin"));
        options.AddPolicy("user", policy => policy.RequireRole("user"));
      });

    }

   
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

    

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseSession();

      //προσθεσα αυτο για τις γλωσσες
      var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
      app.UseRequestLocalization(options.Value);       

      app.UseCookiePolicy();
      //προσθεσα αυτη για αυθεντικοποιηση
      app.UseAuthentication();
      //προσθεσα αυτη για signalr
      app.UseSignalR(routes =>
      {
        routes.MapHub<ChatHub>("/chatHub");
      });

    


      app.UseMvc(routes =>
            {
              routes.MapRoute(
                  name: "default",
                  template: "{controller=DashBoard}/{action=Index}/{id?}");
            });
    }
  }
}
