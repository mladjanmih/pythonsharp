using IronPython.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Scripting.Hosting;
using Pythonware.Models;
using Pythonware.Resolvers;
using Pythonware.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Pythonware
{
    public class PythonwareMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<string> _importedLibraries;
        private readonly ScriptEngine _engine;


        public PythonwareMiddleware(
            RequestDelegate next,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _next = next;
            _serviceProvider = serviceProvider;
            _importedLibraries = new List<string>();
            _engine = Python.CreateEngine();
            _engine.SetSearchPaths(configuration.GetSection("IronPythonLibPaths").Get<string[]>());
        }

        public async Task Invoke(HttpContext context)
        {
            var resolver = (IScriptNameResolver)_serviceProvider.GetService(typeof(IScriptNameResolver));
            var name = resolver.ResolveScriptName();

            var storage = (IScriptStorage)_serviceProvider.GetService(typeof(IScriptStorage));
            var script = storage.GetScript(name);

            if (script != null)
            {
                if (script.ExecuteDownstream)
                {
                    await ExecuteScriptDownstream(context, script);
                    if (script.StopDownstreamExecution) return;
                }

                using (var memStream = new MemoryStream())
                {
                    var body = context.Response.Body;

                    context.Response.Body = memStream;

                    await _next.Invoke(context);

                    if (script.ExecuteUpstream)
                    {
                        await ExecuteScriptUpstream(memStream, script);
                    }

                    context.Response.Body.Position = 0;
                    await context.Response.Body.CopyToAsync(body);

                    context.Response.Body = body;
                }

                return;
            }

            await _next.Invoke(context);
        }

        private async Task ExecuteScriptDownstream(HttpContext context, PythonScript script)
        {
            var scope = _engine.CreateScope();
            var source = _engine.CreateScriptSourceFromString(script.Code, Microsoft.Scripting.SourceCodeKind.File);

            //TODO: Finish downstream script execution
        }

        private async Task ExecuteScriptUpstream(MemoryStream responseStream, PythonScript script)
        {
            var scope = _engine.CreateScope();
            var source = _engine.CreateScriptSourceFromString(script.Code, Microsoft.Scripting.SourceCodeKind.File);

            responseStream.Position = 0;
            var streamReader = new StreamReader(responseStream);
            var body = streamReader.ReadToEnd();
            if (script.PassResponseBody)
                scope.SetVariable(script.ResponseBodyParameterName, body);

            foreach(var lib in script.RequiredLibraries)
            {
                scope.ImportModule(lib);
            }

            source.Execute(scope);
            string resp = scope.GetVariable<string>(script.ResponseBodyParameterName);
            responseStream.Position = 0;
            var streamWriter = new StreamWriter(responseStream);
            streamWriter.Write(resp);
            streamWriter.Flush();
        }
    }
}
