using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using NetCore.Business.Validation;
using System;

namespace NetCore.Api.Config
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidModelException ex)
            {
                await HandleErrorAsync(context, HttpStatusCode.BadRequest, ex.Errors);
            }
            catch (NotFoundException)
            {
                await HandleErrorAsync(context, HttpStatusCode.NotFound);
            }
            catch (UnauthorizedException)
            {
                await HandleErrorAsync(context, HttpStatusCode.Unauthorized);
            }
            catch (ForbiddenException)
            {
                await HandleErrorAsync(context, HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(context, HttpStatusCode.InternalServerError);
            }
        }

        private Task ClearCacheHeaders(object state)
        {
            var response = (HttpResponse)state;
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers["Access-Control-Allow-Origin"] = "*";
            response.Headers.Remove(HeaderNames.ETag);
            return Task.FromResult(0);
        }

        private async Task HandleErrorAsync(HttpContext context, HttpStatusCode code, object content = null)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)code;
            context.Response.OnStarting(state => ClearCacheHeaders(state), context.Response);
            if (content != null)
            {
                context.Response.ContentType = "application/json";

                var result = JsonConvert.SerializeObject(content);
                await context.Response.WriteAsync(result);
            }
        }
    }
}
