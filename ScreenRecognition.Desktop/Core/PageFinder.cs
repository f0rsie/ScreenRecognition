using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ScreenRecognition.Desktop.Core
{
    public class PageFinder
    {
        public static Page? FindPageByName(string? name)
        {
            if (name == null)
            {
                return null;
            }

            var page = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(type => type.Name == $"{name}Page");

            var result = (Page?)Activator.CreateInstance(page);

            return result;
        }
    }
}
