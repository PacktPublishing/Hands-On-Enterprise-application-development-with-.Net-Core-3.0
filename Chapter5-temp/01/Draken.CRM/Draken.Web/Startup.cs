using Draken.Repository;
using Draken.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace Draken.Web
{
    public class Startup
    {
        // Holds the Simple Injector IoC Container
        private readonly Container container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
            });


            services.AddControllersWithViews()
                .AddNewtonsoftJson();
            services.AddRazorPages();

            // Specify the default lifestyle for the objects
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            services.AddHttpContextAccessor();

            services.AddSingleton<IConfiguration>(Configuration);

            // Add the SimpleInjectorControllerActivator to the services
            services.AddSingleton<IControllerActivator>(
                            new SimpleInjectorControllerActivator(container));

            // Add the SimpleInjectorViewComponentActivator to the services
            services.AddSingleton<IViewComponentActivator>(
                            new SimpleInjectorViewComponentActivator(container));

            // Enable the SimpleInjector CrossWiring
            services.EnableSimpleInjectorCrossWiring(container);

            // Enable the SimpleInjector for the request scope
            services.UseSimpleInjectorAspNetRequestScoping(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Register the MVC Controllers with the SimpleInjector Container
            container.RegisterMvcControllers(app);

            // Register the MVC Views with the SimpleInjector Container
            container.RegisterMvcViewComponents(app);

            // Register the ContactService with the SimpleInjector Container
            container.Register<IContactService, ContactService>(Lifestyle.Transient);

            //Register the ContactRepository with the SimpleInjector Container
            container.Register<IContactRepository, ContactRepository>(Lifestyle.Transient);

            // Instruct SimpleInjector to resolve the missing dependencies
            container.AutoCrossWireAspNetComponents(app);

            container.Verify();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

        }
    }
}
