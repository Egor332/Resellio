
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ResellioBackend.Kafka;
using ResellioBackend.UserManagementSystem.Factories.Abstractions;
using ResellioBackend.UserManagementSystem.Factories.Implementations;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Repositories.Implementations;
using ResellioBackend.UserManagementSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Services.Implementations;
using ResellioBackend.UserManagementSystem.Statics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ResellioBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DbContext
            var connectionString = configuration.GetConnectionString("DbConnectionString");
            builder.Services.AddDbContext<ResellioDbContext>(options => options.UseSqlServer(connectionString));

            // .NET services
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<LinkGenerator>();

            // Kafka
            builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

            // Authentication and Authorization
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtParameters:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtParameters:AuthenticationSecretKey"]))
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.CustomerPolicy, policy => policy.RequireClaim(BearerTokenClaimsNames.Role, "Customer"));
                options.AddPolicy(AuthorizationPolicies.OrganiserPolicy, policy => policy.RequireClaim(BearerTokenClaimsNames.Role, "Organiser"));
                options.AddPolicy(AuthorizationPolicies.AdminPolicy, policy => policy.RequireClaim(BearerTokenClaimsNames.Role, "Admin"));
            });

            // Add authorization to swager
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Services
            builder.Services.AddTransient<IPasswordService, Hmacsha256PasswordService>();
            builder.Services.AddTransient<ITokenGenerator, JwtGenerator>();
            builder.Services.AddTransient<IRegistrationService, RegistrationService>();
            builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
            builder.Services.AddTransient<IEmailVerificationTokenService, EmailVerificationTokenService>();

            // Repositories
            builder.Services.AddScoped(typeof(IUsersRepository<>), typeof(UsersRepository<>));

            // Factory
            builder.Services.AddTransient<IUserFactory, UserFactory>();

            // CORS
            var allowedOrigins = configuration["AllowedOrigins"];

            if (allowedOrigins != null)
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", corsPolicyBuilder =>
                    {
                        corsPolicyBuilder.WithOrigins(allowedOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
