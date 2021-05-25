﻿using Application.Catalog.Products;
using Application.Common;
using Application.System;
using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilties.Constant;

namespace Backend_API
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
            services.AddDbContext<DB_Context>(opt =>
        opt.UseSqlServer(Configuration.GetConnectionString(SystemConstants.MainConnectionString)));

            //register
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<DB_Context>().AddDefaultTokenProviders();

            //Declare DI
            services.AddTransient<IFileStorageService, FileStorageService>();
            services.AddTransient<IPublicProductService, PublicProductService>();
            services.AddTransient<IManageProductService, ManageProductService>();
            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
            services.AddTransient<IUserService, UserService>();

            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web Demo API Asp.Net Core", Version = "Nguyễn Đình Dương" });
            });

            //services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Duong Rbt", Version = "v1" });

            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
            //          Enter 'Bearer' [space] and then your token in the text input below.
            //          \r\n\r\nExample: 'Bearer 12345abcdef'",
            //        Name = "Authorization",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "Bearer"
            //    });

            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            //      {
            //        {
            //          new OpenApiSecurityScheme
            //          {
            //            Reference = new OpenApiReference
            //              {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "Bearer"
            //              },
            //              Scheme = "oauth2",
            //              Name = "Bearer",
            //              In = ParameterLocation.Header,
            //            },
            //            new List<string>()
            //          }
            //        });
            //});

            //string issuer = Configuration.GetValue<string>("Tokens:Issuer");
            //string signingKey = Configuration.GetValue<string>("Tokens:Key");
            //byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

            //services.AddAuthentication(opt =>
            //{
            //    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.RequireHttpsMetadata = false;
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuer = true,
            //        ValidIssuer = issuer,
            //        ValidateAudience = true,
            //        ValidAudience = issuer,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ClockSkew = System.TimeSpan.Zero,
            //        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
            //    };
            //});
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dương Rbt");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}