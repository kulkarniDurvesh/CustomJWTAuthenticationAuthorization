using JWTMiddleware.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JWTMiddleware.Helper
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(IOptions<AppSettings>appsettings,RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
            _appSettings = appsettings.Value;
        }

        public async Task Invoke(HttpContext httpContext, IUserService userService) 
        {
            //string requestBody = string.Empty;

            //// Enable buffering to allow reading the body multiple times
            //httpContext.Request.EnableBuffering();

            //using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, leaveOpen: true))
            //{
            //    requestBody = await reader.ReadToEndAsync();
            //    // Reset the stream position to allow downstream middleware to read it
            //    httpContext.Request.Body.Position = 0;
            //}

            //// You can now process the request body as needed
            //Console.WriteLine($"Request Body: {requestBody}");

            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                attachUserToContext(httpContext, userService, token);
            }

            _next(httpContext);
        }

        private void attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Key));
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _appSettings.Issuer,
                    ValidAudience = _appSettings.Issuer
                }, out SecurityToken validateToken);
                var jwtToken = (JwtSecurityToken)validateToken;
                var userId = int.Parse(jwtToken.Claims.FirstOrDefault(_ => _.Type == "Id").Value);
                context.Items["User"] = userService.GetById(userId);
            }
            catch (Exception ex)
            {
                //throw new Exception(message:ex.ToString());
            }
        }


    }
}
