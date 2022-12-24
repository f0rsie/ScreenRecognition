namespace ScreenRecognition.Api.Models
{
    public class ImageSeparationThreadModel
    {
        public int FloodValue { get; set; }
        public byte[] ImagePart { get; set; }
        public int Number { get; set; }
        public bool BackgroundMoreThanText { get; set; } = false;

        public ImageSeparationThreadModel() { }

        public ImageSeparationThreadModel(int floodValue, byte[] imagePart, int number, bool backgroundMoreThanText) : this()
        {
            FloodValue = floodValue;
            ImagePart = imagePart;
            Number = number;
            BackgroundMoreThanText = backgroundMoreThanText;
        }
    }
}
