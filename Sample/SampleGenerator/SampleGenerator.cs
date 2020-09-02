using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Diagnostics;
using System.Text;

namespace SampleGenerator
{
    [Generator]
    public class SampleGenerator : ISourceGenerator
    {
        public void Execute(SourceGeneratorContext context)
        {
            Debugger.Break();
            context.AddSource("example", SourceText.From("public class ExampleClass{}", Encoding.UTF8));
        }

        public void Initialize(InitializationContext context)
        {
        }
    }
}
