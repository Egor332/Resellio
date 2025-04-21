
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
using StackExchange.Redis;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.Creators.Implementations;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Repositories.Implementations;
using ResellioBackend.TransactionManager;
using ResellioBackend.TicketPurchaseSystem.DatabaseServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.DatabaseServices.Implementations;
using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Implementations;
using ResellioBackend.TicketPurchaseSystem.RedisServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisServices.Implementations;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Implementations;
using Stripe;
using ResellioBackend.Paging;

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
            builder.Services.AddDbContext<ResellioDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

            // Transaction manager
            builder.Services.AddScoped<IDatabaseTransactionManager, DatabaseTransactionManager>();

            // .NET services
            builder.Services.AddHttpContextAccessor();
            // builder.Services.AddTransient<LinkGenerator>();

            // Kafka
            builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

            // Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConnectionString = configuration["Redis:ConnectionString"];
                return ConnectionMultiplexer.Connect(redisConnectionString);
            });
            builder.Services.AddScoped<ICartRedisRepository, CartRedisRepository>();
            builder.Services.AddScoped<ITicketRedisRepository, TicketRedisRepository>();
            builder.Services.AddScoped<IRedisService, RedisService>();

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
            builder.Services.AddTransient<IConfirmEmailService, ConfirmEmailService>();
            builder.Services.AddTransient<IEmailVerificationService, EmailVerificationService>();
            builder.Services.AddTransient<IRequestEmailVerificationService, RequestEmailVerificationService>();
            builder.Services.AddTransient<IPasswordResetTokenService, PasswordResetTokenService>();
            builder.Services.AddTransient<IResetPasswordService, ResetPasswordService>();
            builder.Services.AddTransient<IEventCreatorService, EventCreatorService>();
            builder.Services.AddTransient<ITicketTypeCreatorService, TicketTypeCreatorService>();
            builder.Services.AddTransient<ITicketCreatorService, TicketCreatorService>();
            builder.Services.AddScoped<ITicketLockerService, TicketLockerService>();
            builder.Services.AddScoped<ITicketUnlockerService, TicketUnlockerService>();
            builder.Services.AddScoped<ITicketSellerService, TicketSellerService>();
            builder.Services.AddScoped<IPurchaseLockService, PurchaseLockService>();
            builder.Services.AddScoped<ICheckoutSessionCreatorService, StripeCheckoutSessionCreatorService>();
            builder.Services.AddTransient<IPurchaseItemCreatorService, StripePurchaseItemsCreatorService>();
            builder.Services.AddTransient<ICheckoutEventProcessor, StripeCheckoutEventProcessor>();
            builder.Services.AddScoped<ICheckoutSessionManagerService, StripeCheckoutSessionManagerService>();
            builder.Services.AddScoped<IRefundService, StripeRefundService>();
            builder.Services.AddTransient<IPagingService, PagingService>();

            // Database services
            builder.Services.AddScoped<ITicketStatusService, TicketStatusService>();

            // Repositories
            builder.Services.AddScoped(typeof(IUsersRepository<>), typeof(UsersRepository<>));
            builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
            builder.Services.AddScoped<IEventsRepository, EventsRepository>();
            builder.Services.AddScoped<ITicketTypesRepository, TicketTypesRepository>();
            builder.Services.AddScoped<ITicketsRepository, TicketsRepository>();
            
            
            // Factory
            builder.Services.AddTransient<IUserFactory, UserFactory>();

            // Stripe configuration
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

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
