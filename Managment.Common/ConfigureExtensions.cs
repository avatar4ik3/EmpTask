using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Managment.Common;

public static class ConfigureExtensions
{
    /*
    Использовать этот метод необходимо только в сервисах, где необходима верефикация по jwt, а возможно этот метод вообще не нужен
    */
    public static void ConfigureJwt(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true);

    }

    /*
    Метод должен добавить кнопочку логина на страничку свагера
    */
    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {

    }

    /*
    Метод добавляет политики, необходимые для управления пользователем программой
    */
    public static void ConfigureAutharization(this WebApplicationBuilder builder)
    {
        //TODO запилить политики
        builder.Services.AddAuthorization(options =>{
            options.AddPolicy("admin_policy",policy => policy.RequireClaim("admin"));
        });
    }

    /*
     Метод добавляет аутентификацию по jwt bearer токену. Для работы метода необходимо поле JwtConfig : Secret в конфиг файле
    */
    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
        var tokenValidationParameter = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = false,
            ClockSkew = TimeSpan.Zero
        };
        //builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
        var config = new JwtConfig();
        builder.Configuration.Bind("JwtConfig",config);
        builder.Services.AddSingleton<JwtConfig>(config);
        builder.Services.AddSingleton(tokenValidationParameter);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwt =>
        {
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = tokenValidationParameter;
        });
    }
}