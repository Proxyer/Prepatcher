﻿using Mono.Cecil;
using Prepatcher.Process;

namespace Tests;

public class TestsNegative : TestProcessorBase
{
    private TypeDefinition typeFail;

    [OneTimeSetUp]
    public override void Setup()
    {
        base.Setup();
        typeFail = testAsm.ModuleDefinition.GetType($"{nameof(Tests)}.{nameof(BadFields)}");
    }

    [Test]
    public void TestBadFieldAccessors()
    {
        foreach (var accessor in FieldAdder.GetAllPrepatcherFieldAccessors(TestExtensions.EnumerableOf(typeFail)))
            Assert.Throws<LogErrorException>(() =>
            {
                processor.FieldAdder.ProcessAccessor(accessor);
            }, accessor.Name);
    }
}
