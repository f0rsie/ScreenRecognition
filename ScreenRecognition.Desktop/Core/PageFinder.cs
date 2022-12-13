using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ScreenRecognition.Desktop.Core
{
    public class PageFinder : Page
    {
        public static Page? FindPageByName(string? name)
        {
            if (name == null)
            {
                return null;
            }

            List<Page?> pageList = new List<Page?>();

            var f = Assembly.GetAssembly(typeof(Page)).GetTypes()
                .Where(type=> type.IsClass);

            foreach (var item in f)
            {

            }

            Uri? pageUri = new Uri($"/View/Pages/{name}Page.xaml");

            Page? result = new Page();

            if (pageUri != null)
            {

            }

            return result;
        }
    }
}
