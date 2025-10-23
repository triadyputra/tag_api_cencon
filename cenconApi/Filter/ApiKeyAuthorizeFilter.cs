using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace cenconApi.Filter
{
    public class ApiKeyAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public ApiKeyAuthorizeFilter(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;

            if (!request.Headers.TryGetValue("Authorization", out var token))
            {
                context.Result = new JsonResult(new { message = "Token tidak ditemukan" }) { StatusCode = 401 };
                return;
            }

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var controllerName = descriptor?.ControllerName;
            var actionName = descriptor?.ActionName;

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(
                _config["ApiLogin:ValidateUrl"],
                new
                {
                    Token = token.ToString(),
                    Controller = controllerName,
                    Action = actionName
                });

            if (!response.IsSuccessStatusCode)
            {
                context.Result = new JsonResult(new { message = "Akses ditolak" }) { StatusCode = 403 };
            }
        }
    }
}
