using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pythonware.Resolvers
{
    public class EndpointScriptNameResolver : IScriptNameResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EndpointScriptNameResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string ResolveScriptName()
        {
            return _httpContextAccessor.HttpContext.Request.Method.ToLower() + ":" + _httpContextAccessor.HttpContext.Request.Path.ToString().ToLower();
        }
    }
}
