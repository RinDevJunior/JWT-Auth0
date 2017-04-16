using System.Configuration;
using System.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Linq;
using System.Text;
using Auth0.Owin;
using Microsoft.Owin.Security.OAuth;
using AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode;

[assembly: OwinStartup(typeof(WebApi.Startup))]

namespace WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var domain = $"https://{ConfigurationManager.AppSettings["Auth0Domain"]}/";
            var apiIdentifier = ConfigurationManager.AppSettings["Auth0ApiIdentifier"];

            var keyResolver = new OpenIdConnectSigningKeyResolver(domain);
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = apiIdentifier,
                        ValidIssuer = domain,
                        IssuerSigningKeyResolver = (token, securityToken, identifier, parameters) => keyResolver.GetSigningKey(identifier)
                    }
                });


            //var issuer = $"https://{ConfigurationManager.AppSettings["Auth0Domain"]}/";
            //var audience = ConfigurationManager.AppSettings["Auth0ApiIdentifier"];
            //var secret = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Auth0ApiSecret"]);

            //// Api controllers with an [Authorize] attribute will be validated with JWT
            //app.UseJwtBearerAuthentication(
            //    new JwtBearerAuthenticationOptions
            //    {
            //        AuthenticationMode = AuthenticationMode.Active,
            //        AllowedAudiences = new[] { audience },
            //        IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
            //        {
            //            new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
            //        }
            //    });



            // Configure Web API
            WebApiConfig.Configure(app);
        }
    }
}
