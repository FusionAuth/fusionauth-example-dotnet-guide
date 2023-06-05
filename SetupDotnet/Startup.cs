using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;

namespace SetupDotnet
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
            services.AddAntiforgery(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            services.AddRazorPages();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "cookie";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("cookie", options =>
                {
                    options.Cookie.Name = Configuration["SetupDotnet:CookieName"];
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = Configuration["SetupDotnet:Authority"];

                    options.ClientId = Configuration["SetupDotnet:ClientId"];
                    options.ClientSecret = Configuration["SetupDotnet:ClientSecret"];

                    options.ResponseType = "code";
                    options.RequireHttpsMetadata = false;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
            IdentityModelEventSource.ShowPII = true;
        }
    }
}