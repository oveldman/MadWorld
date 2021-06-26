using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Website.Manager.Interfaces;
using Website.Manager.Models;

namespace Website.Manager
{
    public class EditorManager : ICodeEditorManager
    {
        public static string ResultToPrint { get; private set; } = string.Empty;

        private HttpClient _client;

        public EditorManager(HttpClient client)
        {
            _client = client;
        }

        public static void Reset()
        {
            ResultToPrint = string.Empty;
        }

        public static void WriteLine(string showText)
        {
            ResultToPrint += showText + Environment.NewLine;
        }

        public async Task<List<MetadataReference>> GetAllReferenceStreams()
        {
            List<MetadataReference> references = new List<MetadataReference>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.IsDynamic)
                {
                    continue;
                }

                references.Add(
                    MetadataReference.CreateFromStream(
                        await _client.GetStreamAsync("/_framework/" + assembly.GetName().Name + ".dll")));
            }

            return references;
        }

        public CompiledCodeResult CompileCode(string code, List<MetadataReference> references)
        {
            CompiledCodeResult result = new();

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(LanguageVersion.Preview));
            foreach (var diagnostic in syntaxTree.GetDiagnostics())
            {
                result.CompileLogs.Add(diagnostic.ToString());
            }

            if (syntaxTree.GetDiagnostics().Any(i => i.Severity == DiagnosticSeverity.Error))
            {
                result.CompileLogs.Add("Parse SyntaxTree Error!");
                return result;
            }

            result.CompileLogs.Add("Parse SyntaxTree Success");

            CSharpCompilation compilation = CSharpCompilation.Create("MadWorldEmulator", new[] { syntaxTree },
                references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (MemoryStream stream = new MemoryStream())
            {
                EmitResult emitResult = compilation.Emit(stream);

                foreach (var diagnostic in emitResult.Diagnostics)
                {
                    result.CompileLogs.Add(diagnostic.ToString());
                }

                if (!emitResult.Success)
                {
                    result.CompileLogs.Add("Compilation error");
                    return result;
                }

                result.CompileLogs.Add("Compilation success!");

                stream.Seek(0, SeekOrigin.Begin);

                result.Assembly = AppDomain.CurrentDomain.Load(stream.ToArray());
                return result;
            }
        }

        public List<string> RunCode(Assembly assembly, string methodToTest, List<string> compileLogs)
        {
            if (assembly is null || string.IsNullOrEmpty(methodToTest)) return compileLogs; 

            try
            {
                var type = assembly.GetExportedTypes().FirstOrDefault();
                var methodInfo = type.GetMethod(methodToTest);
                var instance = Activator.CreateInstance(type);
                methodInfo.Invoke(instance, null);
            }
            catch (Exception ex)
            {
                compileLogs.Add("Invoke Crash: " + ex.Message + " (Hint: Methode not found!)");
            }

            return compileLogs;
        }
    }
}
