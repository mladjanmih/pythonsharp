using System;
using System.Collections.Generic;
using System.Text;

namespace Pythonware.Models
{
    public class PythonScript
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool ExecuteDownstream { get; set; }

        public bool ExecuteUpstream { get; set; }

        public bool StopDownstreamExecution { get; set; }

        public bool PassRequestBody { get; set; }

        public string RequestBodyParameterName { get; set; }

        public bool PassResponseBody { get; set; }

        public string ResponseBodyParameterName { get; set; }

        public List<string> RequiredLibraries { get; set; }
    }
}
