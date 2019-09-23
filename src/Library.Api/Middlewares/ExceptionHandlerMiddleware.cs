using System;
using System.Threading.Tasks;
using FluentValidation;
using Library.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Library.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException e)
            {
                _logger.LogError(e.ToString());
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            catch (ValidationException e)
            {
                _logger.LogError(e.ToString());
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}