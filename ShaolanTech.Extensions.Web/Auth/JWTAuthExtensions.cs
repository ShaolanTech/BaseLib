


namespace ShaolanTech.Extensions.Web.Auth
{
    public static class JWTAuthExtensions
    {
 
        private const string signKey = "shaolantech internal signing key, users could overwrite this value in production environment";
        /// <summary>
        /// Auto config jwt authentication
        /// </summary>
        /// <param name="builder"></param>
        public static void AddShaolanTechJWTAuthentication<TAuthHandler>(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder,
            string validAudience = "https://www.shaolantech.cn", string validIssuer = "shaolantech", string issuerSigningKey = "",
            Dictionary<string, IAuthorizationRequirement> authorizationRequirements = null
            ) where TAuthHandler : class, IAuthorizationHandler
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IAuthorizationHandler, TAuthHandler>();
            //builder.Services.AddAuthentication(options => 
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(o => 
            //{
            //    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters 
            //    {
            //        ValidateAudience=true,
            //        ValidateIssuer=true,
            //        ValidateLifetime=false,
            //        ValidateIssuerSigningKey=true,
            //        ValidIssuer= validIssuer,
            //        ValidAudience=validAudience,
            //        IssuerSigningKey=new SymmetricSecurityKey( Encoding.UTF8.GetBytes(issuerSigningKey==""?Convert.ToBase64String(Encoding.UTF8.GetBytes(signKey)): issuerSigningKey))
            //    };
            //});

            var multiSchemePolicy = new AuthorizationPolicyBuilder(
                CookieAuthenticationDefaults.AuthenticationScheme,
                JwtBearerDefaults.AuthenticationScheme
                ).RequireAuthenticatedUser()
                .Build();
            
            var services = builder.Services;
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "JWT_OR_COOKIE";
                options.DefaultChallengeScheme = "JWT_OR_COOKIE";
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = validIssuer,
                    ValidAudience = validAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey == "" ? Convert.ToBase64String(Encoding.UTF8.GetBytes(signKey)) : issuerSigningKey))
                };
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
            }).AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                        return JwtBearerDefaults.AuthenticationScheme;

                    return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            });
            builder.Services.AddAuthorization(option =>
            {
                option.DefaultPolicy = multiSchemePolicy;
                if (authorizationRequirements != null)
                {
                    foreach (var item in authorizationRequirements)
                    {
                        option.AddPolicy(item.Key, policy => policy.Requirements.Add(item.Value));
                    }
                }
              
            });
        }
 
    }
}
