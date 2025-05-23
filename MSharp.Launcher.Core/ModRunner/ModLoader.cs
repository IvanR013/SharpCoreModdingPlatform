using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MSharp.ModAPI;

namespace MSharp.Launcher.Core.ModRunner
{
    public static class ModLoader
    {
        public static List<IMsharpMod> CargarMods(string ruta)
        {
            List<IMsharpMod> mods = [];

            if (!Directory.Exists(ruta))
            {
                Console.WriteLine($"📂 Carpeta de mods inexistente: {ruta}");
                return mods;
            }

            foreach (var archivo in Directory.GetFiles(ruta, "*.dll"))
            {
                try
                {
                    var asm = Assembly.LoadFrom(archivo);
                    var tipos = asm.GetTypes()
                        .Where(t => typeof(IMsharpMod).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var tipo in tipos)
                    {
                        if (Activator.CreateInstance(tipo) is IMsharpMod mod)
                        {
                            mods.Add(mod);
                            Console.WriteLine($"✅ Mod cargado: {tipo.FullName}");
                        }
                        else
                        {
                            Console.WriteLine($"⚠️ No se pudo instanciar: {tipo.FullName}");
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Console.WriteLine($"❌ Error de tipos en {archivo}: {ex.Message}");

                    foreach (var loaderEx in ex.LoaderExceptions) if (loaderEx != null) Console.WriteLine($"   - {loaderEx.Message}");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error cargando {archivo}: {ex.Message}");
                }
            }

            return mods;
        }
    }
}
