using Mono.Cecil;
using Mono.Cecil.Cil;

namespace InvertMousePatcher
{
    class Program
    {
        static int Main(string[] args)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dataRoot = Directory
                .GetDirectories(baseDir, "*_Data", SearchOption.TopDirectoryOnly)
                .FirstOrDefault();
            if (dataRoot == null)
            {
                Console.Error.WriteLine($"❌ No '*_Data' folder found in game root: {baseDir}");
                return 1;
            }

            string managedDir = Path.Combine(dataRoot, "Managed");
            if (!Directory.Exists(managedDir))
            {
                Console.Error.WriteLine($"❌ Managed folder not found: {managedDir}");
                return 2;
            }

            var dllCandidates = Directory
                .GetFiles(managedDir, "*Assembly-CSharp.dll", SearchOption.TopDirectoryOnly);
            if (dllCandidates.Length == 0)
            {
                Console.Error.WriteLine($"❌ Could not find Assembly-CSharp.dll in:\n   {managedDir}");
                return 3;
            }
            string dllPath = dllCandidates[0];

            ModuleDefinition module;
            try
            {
                module = ModuleDefinition.ReadModule(dllPath, new ReaderParameters { ReadWrite = true });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"❌ Failed to read module:\n   {ex.Message}");
                return 4;
            }

            var semiFuncType = module.GetType("SemiFunc");
            var inputMouseY = semiFuncType?
                .Methods
                .FirstOrDefault(m =>
                    m.Name == "InputMouseY" &&
                    m.Parameters.Count == 0 &&
                    m.ReturnType.FullName == "System.Single"
                );
            if (inputMouseY == null)
            {
                Console.Error.WriteLine("❌ Cannot locate method SemiFunc.InputMouseY()");
                return 5;
            }

            var il = inputMouseY.Body.GetILProcessor();
            var retInstr = inputMouseY.Body.Instructions.Last(i => i.OpCode == OpCodes.Ret);

            il.InsertBefore(retInstr, il.Create(OpCodes.Ldc_R4, -1f));
            il.InsertBefore(retInstr, il.Create(OpCodes.Mul));

            inputMouseY.Body.MaxStackSize = Math.Max(inputMouseY.Body.MaxStackSize, 2);

            try
            {
                module.Write();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"❌ Failed to write patched DLL:\n   {ex.Message}");
                return 6;
            }

            Console.WriteLine("✅ Patched InputMouseY() — Y-axis inverted successfully.");
            return 0;
        }
    }
}
