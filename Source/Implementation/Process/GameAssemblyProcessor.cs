﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using UnityEngine;
using Verse;
using FieldAttributes = Mono.Cecil.FieldAttributes;

namespace Prepatcher.Process;

internal class GameAssemblyProcessor : AssemblyProcessor
{
    internal ModifiableAssembly asmCSharp;

    private const string AssemblyCSharp = "Assembly-CSharp";
    private const string VerseGameType = "Verse.Game";
    internal const string PrepatcherMarkerField = "PrepatcherMarker";

    internal override void Process()
    {
        // Mark as visited
        asmCSharp.ModuleDefinition.GetType(VerseGameType).Fields.Add(
        new FieldDefinition(
            PrepatcherMarkerField,
            FieldAttributes.Static,
            asmCSharp.ModuleDefinition.TypeSystem.Int32
        ));

        asmCSharp.NeedsReload = true;
        FindModifiableAssembly("0Harmony")!.NeedsReload = true;

        base.Process();
    }

    protected override void LoadAssembly(ModifiableAssembly asm)
    {
        var loadedAssembly = Assembly.Load(asm.Bytes);
        if (loadedAssembly.GetName().Name == AssemblyCSharp)
        {
            Loader.newAsm = loadedAssembly;
            AppDomain.CurrentDomain.AssemblyResolve += (_, _) => loadedAssembly;
        }
    }
}
