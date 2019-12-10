using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pythonware.Resolvers;
using Pythonware.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pythonware
{
    public static class PythonwareMiddlewareExtensions
    {
        public static IServiceCollection AddPythonware(this IServiceCollection services)
        {
            services.AddTransient<IScriptNameResolver, EndpointScriptNameResolver>();
            services.AddTransient<IScriptStorage, InMemoryScriptStorage>();
            return services;
        }


        public static void UsePythonware(this IApplicationBuilder app)
        {
            app.UseMiddleware<PythonwareMiddleware>();
        }

    }
}
