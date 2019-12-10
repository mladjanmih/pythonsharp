using Microsoft.AspNetCore.Http;
using Pythonware.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pythonware.Storage
{
    public interface IScriptStorage
    {

        PythonScript GetScript(string name);
    }
}
