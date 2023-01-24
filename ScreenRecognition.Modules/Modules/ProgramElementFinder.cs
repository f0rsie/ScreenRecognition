using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRecognition.Modules.Modules
{
    // System.Windows.Application.ResourceAssembly.FullName
    // Assembly.GetExecutingAssembly().FullName
    public class ProgramElementFinder
    {
        public static T? FindByName<T>(string name, string assemblyName)
        {
            T? result = default(T);

            try
            {
                if (name == null || assemblyName == null)
                    return result;

                var assemblyElement = Assembly.Load(assemblyName);

                var typeElement = assemblyElement.GetExportedTypes()
                    .FirstOrDefault(type => type.Name.ToLower() == name.ToLower());

                if (typeElement == null)
                    return result;

                result = (T?)Activator.CreateInstance(assemblyName, typeElement.FullName)?.Unwrap();
            }
            catch { }

            return result;
        }
    }
}
