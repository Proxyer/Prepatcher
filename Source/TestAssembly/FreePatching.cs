﻿using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Utils;
using Prepatcher;
using TestAssemblyTarget;

namespace Tests;

public static class FreePatching
{
    [FreePatch]
    static void RewriteAssembly(ModuleDefinition module)
    {
        var type = module.GetType($"{nameof(TestAssemblyTarget)}.{nameof(RewriteTarget)}");
        var method = type.FindMethod(nameof(RewriteTarget.Method));

        foreach (var inst in method.Body.Instructions)
            if (inst.OpCode == OpCodes.Ldc_I4_0)
                inst.OpCode = OpCodes.Ldc_I4_1;
    }
}
