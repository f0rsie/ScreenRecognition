namespace ScreenRecognition.Api.Resources.Exceptions
{
    public class RecognitionException : Exception
    {
        public RecognitionException() : base("Ошибка распознавания") { }
    }
}