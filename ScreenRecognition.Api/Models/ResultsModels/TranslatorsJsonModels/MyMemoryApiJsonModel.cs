namespace ScreenRecognition.Api.Models.ResultsModels.TranslatorsJsonModels
{
    public class responseData
    {
        public object? translatedText { get; set; }
        public object? match { get; set; }
        public object? quotaFinished { get; set; }
        public object? mtLangSupported { get; set; }
        public object? responseDetails { get; set; }
        public object? responseStatus { get; set; }
        public object? responsderId { get; set; }
        public object? exception_code { get; set; }

        public Match[]? matches { get; set; }
    }
    public class Match
    {
        public object? id { get; set; }
        public string? segment { get; set; }
        public string? translation { get; set; }
        public string? source { get; set; }
        public string? target { get; set; }
        public object? quality { get; set; }
        public object? reference { get; set; }
        public object? usage_count { get; set; }
        public object? subject { get; set; }
        public object? created_by { get; set; }
        public object? last_updated_by { get; set; }
        public object? create_date { get; set; }
        public object? last_update_date { get; set; }
        public double? match { get; set; }
    }
}