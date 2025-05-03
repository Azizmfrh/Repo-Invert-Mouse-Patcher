using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace InvertMousePatcher
{
    class Program
    {
        static int Main(string[] args)
        {
            // 1) Locate assembly
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dllPath = Path.Combine(baseDir, "Managed", "assembly-csharp.dll");
            if (!File.Exists(dllPath))
            {
                Console.Error.WriteLine("❌ assembly-csharp.dll not found in Managed/");
                return 1;
            }

            // 2) Load module for read/write
            var module = ModuleDefinition.ReadModule(dllPath, new ReaderParameters { ReadWrite = true });

            // 3) Find the target method
            var semiFunc = module.GetType("SemiFunc");
            var inputMethod = semiFunc?.Methods
                .FirstOrDefault(m => m.Name == "InputMouseY" && m.ReturnType.FullName == "System.Single");
            if (inputMethod == null)
            {
                Console.Error.WriteLine("❌ Cannot locate SemiFunc.InputMouseY()");
                return 2;
            }

            // 4) IL patch: invert the float
            var il = inputMethod.Body.GetILProcessor();
            var ret = inputMethod.Body.Instructions.Last(i => i.OpCode == OpCodes.Ret);
            il.InsertBefore(ret, il.Create(OpCodes.Ldc_R4, -1f));
            il.InsertBefore(ret, il.Create(OpCodes.Mul));
            inputMethod.Body.MaxStackSize = Math.Max(inputMethod.Body.MaxStackSize, 2);

            // 5) Write changes
            module.Write();
            Console.WriteLine("✅ Patched InputMouseY() – Y inverted.");
            return 0;
        }
    }
}
