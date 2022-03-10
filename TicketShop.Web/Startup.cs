using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicketShop.Repository;
using TicketShop.Repository.Interface;
using TicketShop.Repository.Implementation;
using TicketShop.Services.Interface;
using TicketShop.Services.Implementation;
using TicketShop.Domain.Identity;
using TicketShop.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;
using TicketShop.Services;
using TicketShop.Domain;
using Stripe;

namespace TicketShop.Web
{
    public class Startup
    {
        private EmailSettings emailSettings;
        public Startup(IConfiguration configuration)
        {
            emailSettings = new EmailSettings();
            Configuration = configuration;
            Configuration.GetSection("EmailSettings").Bind(emailSettings);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Role.ADMIN,
               policy => policy.RequireClaim(Role.ADMIN));
            options.AddPolicy(Role.STANDARD_USER,
                policy => policy.RequireClaim(Role.STANDARD_USER));
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));

            services.AddScoped<EmailSettings>(es => emailSettings);
            services.Configure<StripeConfig>(Configuration.GetSection("StripeConfig"));
            services.AddScoped<IEmailService, EmailService>(email => new EmailService(emailSettings));
            services.AddScoped<IBackgroundEmailSender, BackgroundEmailSender>();
            services.AddHostedService<ConsumeServices>();

            //services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));


            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<IOrderService, Services.Implementation.OrderService>();
            services.AddTransient<IUserService, UserService>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StripeConfiguration.SetApiKey(Configuration.GetSection("StripeConfig")["SecretKey"]);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
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
