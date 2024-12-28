using JWTMiddleware.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JWTMiddleware.Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Authorization : Attribute, IAuthorizationFilter
    {
        private readonly IList<Role> _roles;
        public Authorization(params Role[] roles)
        {
            _roles = roles??new Role[] { };
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isRolePermission = false;
            User user = (User)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unthorized",
                    StatusCode = StatusCodes.Status401Unauthorized 
                });
            }

            if(user!=null && _roles.Any()) 
            {
                foreach (var userRole in user.Roles)
                {
                    foreach (var AuthRole in this._roles)
                    {

                        if (userRole == AuthRole)
                        {
                            isRolePermission = true;
                        }
                    }
                }
            }

            if (!isRolePermission) 
            {
                context.Result = new JsonResult(new
                {
                    message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }



        }
    }
}
