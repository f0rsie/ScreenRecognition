using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRecognition.Desktop.Models
{
    public class ServerStatus
    {
        public string? Text { get; private set; }
        public bool? AvailableStatus { get; private set; } = false;
        public Color TextColor { get; private set; } = Color.Red;

        public ServerStatus(string? text, bool? availableStatus)
        {
            Text = text;
            AvailableStatus = availableStatus;

            if (AvailableStatus == false)
                TextColor = Color.Red;
            else
                TextColor = Color.Green;
        }
    }
}
