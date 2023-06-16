using FluentValidation.AspNetCore;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.BackendServer.Extensions;
using KnowledgeSpace.BackendServer.IdentityServer;
using KnowledgeSpace.BackendServer.Services;
using KnowledgeSpace.ViewModels;
using KnowledgeSpace.ViewModels.Systems;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace KnowledgeSpace.BackendServer
{
    public class Startup
    {
        private readonly string KspSpecificOrigins = "KspSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KnowledgeSpace.BackendServer", Version = "v1" });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(Configuration["AuthorityUrl"] + "/connect/authorize"),
                            Scopes = new Dictionary<string, string> { 
                                { "api.knowledgespace", "KnowledgeSpace API" } 
                            }
                        },
                    },
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>{ "api.knowledgespace" }
                    }
                });

            });

            // Data Seed
            services.AddTransient<DbInitializer>();
            // Automatic Registration: This will automatically find any public, non-abstract types that inherit from AbstractValidator and register them with the container (open generics are not supported).
            /*
            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RoleVmValidator>());
            */
            // --- Chuẩn hóa API Error Response và Áp dụng Global Exception Handler ---
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllersWithViews()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RoleCreateRequestValidator>());

            //var secret = new IdentityServer4.Models.Secret("secret".Sha256());
            //1. === Setup entity framework ===  AddDbContext -> AddDbContextPool(Tang hieu qua su dung lai connect truoc do)
            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //2. === Setup idetntity ===
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            // --- Identity Server ---
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                /*
                options.UserInteraction.LoginUrl = "/Account/Login";
                options.UserInteraction.LogoutUrl = "/Account/Logout";
                options.Authentication = new AuthenticationOptions()
                {
                    CookieLifetime = TimeSpan.FromHours(10), // ID server cookie timeout set to 10 hours
                    CookieSlidingExpiration = true
                };
                */
            })
            /*
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                options.EnableTokenCleanup = true;
            })
            */
            .AddInMemoryIdentityResources(Config.Ids)
            .AddInMemoryApiResources(Config.Apis)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Configuration.GetSection("IdentityServer:Clients"))

            .AddAspNetIdentity<User>()
            .AddProfileService<IdentityProfileService>()
            // signing credential is configured. create JWT token
            .AddDeveloperSigningCredential();

            services.AddCors(options =>
            {
                options.AddPolicy(KspSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(Configuration["AllowOrigins"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // --- /.Identity Server ---
            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            });

            // Add Razor Pages: Identity Server
            // services.AddRazorPages();
            
            services.AddRazorPages(options =>
            {
                options.Conventions.AddAreaFolderRouteModelConvention("Identity", "/Account/", model =>
                {
                    foreach (var selector in model.Selectors)
                    {
                        var attributeRouteModel = selector.AttributeRouteModel;
                        attributeRouteModel.Order = -1;
                        attributeRouteModel.Template = attributeRouteModel.Template.Remove(0, "Identity".Length);
                    }
                });
            });
            // --- khong co cung duoc ---
            services.AddAuthentication()
               .AddLocalApi("Bearer", option =>
               {
                   option.ExpectedScope = "api.knowledgespace";
               });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy =>
                {
                    policy.AddAuthenticationSchemes("Bearer");
                    policy.RequireAuthenticatedUser();
                });
            });
            // --- /.khong co cung duoc ---

            services.AddTransient<IEmailSender, EmailSenderService>();
            services.AddTransient<ISequenceService, SequenceService>();
            services.AddTransient<IStorageService, FileStorageService>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IViewRenderService, ViewRenderService>();

            services.AddDistributedSqlServerCache(o =>
            {
                o.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                o.SchemaName = "dbo";
                o.TableName = "CacheTable";
            });

            services.AddTransient<ICacheService, DistributedCacheService>();
            services.AddTransient<IOneSignalService, OneSignalService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // --- Add Security headers --- 
                app.UseHsts(hsts => hsts.MaxAge(365).IncludeSubdomains().Preload());
                app.UseXContentTypeOptions();
                app.UseReferrerPolicy(opts => opts.NoReferrer());
                app.UseXXssProtection(options => options.EnabledWithBlockMode());
                app.UseXfo(options => options.Deny());
                //app.UseCsp(opts => opts
                //        .BlockAllMixedContent()
                //        .StyleSources(s => s.Self())
                //        .StyleSources(s => s.UnsafeInline())
                //        .FontSources(s => s.Self())
                //        .FormActions(s => s.Self())
                //        .FrameAncestors(s => s.Self())
                //        .ImageSources(s => s.Self())
                //        .ScriptSources(s => s.Self())
                //    );
                // --- /.Add Security headers ---
            }

            // --- log errors --- 
            if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            // Create log folder if not exists
            if (!Directory.Exists(Path.Combine(env.WebRootPath, "logs"))) Directory.CreateDirectory(Path.Combine(env.WebRootPath, "logs"));

            loggerFactory.CreateLogger($"{Path.Combine(env.WebRootPath, "logs")}/log.txt");
            // --- /.log errors --- 

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.OAuthClientId("swagger");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "KnowledgeSpace.BackendServer v1");
            });

            app.UseErrorWrapping();// call Extensions/MiddlewareExtensions (Exception Handler)

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(KspSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
