using Newtonsoft.Json;
using System.Diagnostics;

namespace ExceptionHandlings.Shared.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await _next(context);
                sw.Stop();

                // Log successful requests as "Information"

                Console.WriteLine("HTTP {0} {1} responded {2} in {3} ms",
                    context.Request.Method, context.Request.Path, context.Response.StatusCode, sw.Elapsed.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                await HandleExceptionAsync(context, sw, ex);
            }
        }

        public static Task HandleExceptionAsync(HttpContext context, Stopwatch sw, Exception ex)
        {
            if (ex is RecordNotFound or ParsingException)
            {
                context.Response.StatusCode = 403;
            }
            else if (ex is DatabaseException)
            {
                context.Response.StatusCode = 400;
            }
            var response = new
            {
                error = ex.Message
            };

            // Log exceptions as "Error" within the HandleExceptionAsync method
            Console.Error.WriteLine("HTTP {0} {1} responded {2} in {3} ms with error {4}",
                   context.Request.Method, context.Request.Path, context.Response.StatusCode, sw.Elapsed.TotalMilliseconds, ex.Message);

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
