﻿using System.IO;
using System.Reflection;
using Mono.Cecil;

namespace Prepatcher.Process;

public class ModifiableAssembly
{
    private Assembly? SourceAssembly { get; }
    public AssemblyDefinition AsmDefinition { get; }
    public ModuleDefinition ModuleDefinition => AsmDefinition.MainModule;
    public bool NeedsReload { get; set; }
    public bool Modified { get; set; } // Used to dump modified assemblies
    public bool Modifiable { get; set; } = true;
    public bool ProcessAttributes { get; set; }
    public byte[] Bytes { get; private set; }

    public ModifiableAssembly(Assembly sourceAssembly, IAssemblyResolver resolver)
    {
        SourceAssembly = sourceAssembly;
        AsmDefinition = AssemblyDefinition.ReadAssembly(new MemoryStream(UnsafeAssembly.GetRawData(sourceAssembly)),
            new ReaderParameters
            {
                AssemblyResolver = resolver
            });
    }

    public ModifiableAssembly(string path, IAssemblyResolver resolver)
    {
        AsmDefinition = AssemblyDefinition.ReadAssembly(
            path,
            new ReaderParameters { AssemblyResolver = resolver, InMemory = true}
        );
    }

    public void PrepareByteArray()
    {
        var stream = new MemoryStream();
        AsmDefinition.Write(stream);
        Bytes = stream.ToArray();
    }

    public void SetSourceRefOnly()
    {
        UnsafeAssembly.SetReflectionOnly(SourceAssembly!, true);
    }

}
