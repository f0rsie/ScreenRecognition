using System.Reflection;

namespace ScreenRecognition.Modules.Modules
{
    // v1 assemblyName: System.Windows.Application.ResourceAssembly.FullName
    // v2 assemblyName: Assembly.GetExecutingAssembly().FullName
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
