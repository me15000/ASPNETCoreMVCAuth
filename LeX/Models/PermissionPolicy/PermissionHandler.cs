using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LeX.Models.PermissionPolicy
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //赋值用户权限
            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;
            //请求Url
            var questUrl = httpContext.Request.Path.Value.ToLower();
            //是否经过验证
            var isAuthenticated = httpContext.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {

                //用户名
                var userName = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Sid).Value;

                context.Succeed(requirement);

                if (false)
                {
                    //无权限跳转到拒绝页面
                    httpContext.Response.Redirect("/denied");
                }

            }

            return Task.CompletedTask;
        }
    }
}
