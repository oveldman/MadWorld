
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Website.Manager.Models;

namespace Website.Manager.Interfaces
{
    public interface ICodeEditorManager
    {
        Task<List<MetadataReference>> GetAllReferenceStreams();
        CompiledCodeResult CompileCode(string code, List<MetadataReference> references);
        List<string> RunCode(Assembly assembly, string methodToTest, List<string> compileLogs);
    }
}
