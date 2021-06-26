using System;
using System.Collections.Generic;
using System.Reflection;

namespace Website.Manager.Models
{
    public class CompiledCodeResult
    {
        public List<string> CompileLogs { get; set; } = new();
        public Assembly Assembly { get; set; }
    }
}
