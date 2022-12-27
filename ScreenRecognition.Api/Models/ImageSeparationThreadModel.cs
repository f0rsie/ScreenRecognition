namespace ScreenRecognition.Api.Models
{
    public class ImageSeparationThreadModel
    {
        public byte[] ImagePart { get; set; }
        public int Number { get; set; }
        public string Type { get; set; }

        public ImageSeparationThreadModel() { }

        public ImageSeparationThreadModel(byte[] imagePart, int number, string type) : this()
        {
            ImagePart = imagePart;
            Number = number;
            Type = type;
        }
    }
}
