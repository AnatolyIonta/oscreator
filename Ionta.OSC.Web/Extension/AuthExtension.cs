using Ionta.OSC.App.Services;
using Ionta.OSC.App.Services.Auth;
using Ionta.OSC.Core.Auth;
using Ionta.OSC.ToolKit.Auth;
using Ionta.OSC.Web.Infrastructure;

using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Ionta.OSC.Web.Extension
{
    public static class AuthExtension
    {
        public static IServiceCollection AddAuthenticationJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
            var authOptionsCofiguration = configuration.GetSection("Auth").Get<AuthOptions>();

            services.AddSingleton(authOptionsCofiguration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                {
                    var authOptions = authOptionsCofiguration;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issure,
                        ValidateAudience = false,
                        ValidateActor = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                }
                );

            services.AddScoped<IUserProvider, UserProvider>();
            services.AddSingleton(serviceProvider => (IAuthenticationService)new AuthenticationService(configuration["Secret"]));
            return services;
        }
    }
}
