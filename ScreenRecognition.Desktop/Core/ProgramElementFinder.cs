using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ScreenRecognition.Desktop.Core
{
    public class ProgramElementFinder
    {
        public static T? FindByName<T>(string? name)
        {
            if (name == null)
            {
                return default(T);
            }

            var item = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(type => type.Name == name);

            var result = (T?)Activator.CreateInstance(item);

            return result;
        }
    }
}
