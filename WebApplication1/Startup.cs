using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Identity.IdentityPolicy;
using Identity.CustomPolicy;
using Identity.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace WebApplication1
{
    public class Startup
    {
        private IHostingEnvironment _hostingEnvironment;
        public Startup(IHostingEnvironment env)
        {


     
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            _hostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            //services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordPolicy>();
            services.AddTransient<IUserValidator<AppUser>, CustomUsernameEmailPolicy>();
            //services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            //services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
           // services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();


            //fedService
            //services.AddAuthentication(sharedOptions =>
            //{
            //    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
            //})
            //.AddWsFederation(options =>
            //{
            //    options.Wtrealm = "https://localhost:44358";
            //    options.MetadataAddress = "https://login.microsoftonline.com/ae9d6e9a-cc18-4204-ac29-43a0ccb860e8/federationmetadata/2007-06/federationmetadata.xml";

            //})
            //.AddCookie();

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = "Saml2";
            });

            services.AddAuthentication()
                .AddSaml2(options =>
                {
                    options.SPOptions.EntityId = new EntityId("https://localhost:44358/Saml2");
                   
                    options.IdentityProviders.Add(
                        new IdentityProvider(
                         //new EntityId("https://sts.windows.net/ae9d6e9a-cc18-4204-ac29-43a0ccb860e8/"), options.SPOptions)
                         new EntityId("https://sts.windows.net/fa8c194a-f8e2-43c5-bc39-b637579e39e0/"), options.SPOptions)


                        {
                            //MetadataLocation = "https://login.microsoftonline.com/ae9d6e9a-cc18-4204-ac29-43a0ccb860e8/federationmetadata/2007-06/federationmetadata.xml?appid=2449fe1f-1bff-4223-b85c-783904943b1f"
                            //TECO configuration below:
                           MetadataLocation = "https://login.microsoftonline.com/fa8c194a-f8e2-43c5-bc39-b637579e39e0/federationmetadata/2007-06/federationmetadata.xml?appid=d12ad0cb-26ea-4528-92c1-53d3e5969069"

                        }); 

                    //options.SPOptions.ServiceCertificates.Add(new X509Certificate2("cert.pfx", ));
                })
                .AddCookie();

            //services.AddIdentity<UserManager<AppUser>, IdentityRole>();

            //ToDo For Authorization
            //options.Cookie.Name = ".AspNetCore.Identity.Application";
            //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            //options.SlidingExpiration = true;

            //services.AddAuthorization(opts =>
            //{
            //    opts.AddPolicy("AspManager", policy =>
            //    {
            //        policy.RequireRole("Manager");
            //        policy.RequireClaim("Coding-Skill", "ASP.NET Core MVC");
            //    });
            //});

            //services.AddTransient<IAuthorizationHandler, AllowUsersHandler>();
            //services.AddAuthorization(opts => {
            //    opts.AddPolicy("AllowTom", policy => {
            //        policy.AddRequirements(new AllowUserPolicy("tom"));
            //    });
            //});

            //services.AddTransient<IAuthorizationHandler, AllowPrivateHandler>();
            //services.AddAuthorization(opts =>
            //{
            //    opts.AddPolicy("PrivateAccess", policy =>
            //    {
            //        policy.AddRequirements(new AllowPrivatePolicy());
            //    });
            //});
            //services.AddTransient<UserManager<AppUser>>();
            //services.AddIdentity<AppUser, IdentityRole>();//.AddEntityFrameworkStores()
            //services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

            //services.TryAddScoped<UserManager<AppUser>>();
            //services.AddIdentity<AppUser, IdentityRole>().AddUserStore<AppUser>();

            //            services.AddIdentity<Account, AccountUserRole>()
            //.AddUserStore();
            //services.TryAddScoped<SignInManager<AppUser>>();
            // services.AddTransient<SignInManager<AppUser>>();
            // services.AddIdentity<ApplicationUs, IdentityRole>();
            //services.AddSession();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();

        

            // app.UseIdentity();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }


}