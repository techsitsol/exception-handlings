﻿using Newtonsoft.Json;
using Serilog;
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
                Log.Information("HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms",
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
            context.Response.ContentType = "application/json";

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
            Log.Error(ex, "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms",
                context.Request.Method, context.Request.Path, context.Response.StatusCode, sw.Elapsed.TotalMilliseconds);

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
