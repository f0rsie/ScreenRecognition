using System.Drawing;

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
