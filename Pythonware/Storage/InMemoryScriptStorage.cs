using Pythonware.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pythonware.Storage
{
    public class InMemoryScriptStorage: IScriptStorage
    {
        private readonly Dictionary<string, PythonScript> _scripts;

        public InMemoryScriptStorage()
        {
            _scripts = new Dictionary<string, PythonScript>();
            _scripts.Add("get:/home/helloworld", new PythonScript()
            {
                Id = 1,
                Name = "get:/home/helloworld",
                ExecuteUpstream = true,
                PassResponseBody = false,
                ResponseBodyParameterName = "result",
                Code = "result = 'Hello world from Iron Python!'",
                RequiredLibraries = new List<string>()
            });
        }

        public PythonScript GetScript(string name)
        {
            if (string.IsNullOrEmpty(name) || !_scripts.ContainsKey(name))
            {
                return null;
            }

            return _scripts[name];
        }
    }
}
