using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Library.Infrastructure.Cache;
using Microsoft.AspNetCore.Http;

namespace Library.Api.Middlewares
{
    public sealed class CachingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _distributedCache;

        public CachingMiddleware(RequestDelegate next, IDistributedCache distributedCache)
        {
            _next = next;
            _distributedCache = distributedCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == "GET")
            {
                var key = context.Request.Path + context.Request.QueryString;
                var cached = await _distributedCache.GetAsync<byte[]>(key, CancellationToken.None);

                if (cached != null)
                {
                    await context.Response.Body.WriteAsync(cached, 0, cached.Length);
                }
                else
                {
                    var originalBody = context.Response.Body;

                    try
                    {
                        using (var memStream = new MemoryStream())
                        {
                            context.Response.Body = memStream;

                            await _next(context);

                            await _distributedCache.SetAsync(key, memStream.GetBuffer(), TimeSpan.FromMinutes(5), CancellationToken.None);
                            
                            memStream.Position = 0;
                            await memStream.CopyToAsync(originalBody);
                        }
                    }
                    finally
                    {
                        context.Response.Body = originalBody;
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}