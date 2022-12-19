using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenRecognition.Desktop.ViewModel
{
    public class HistoryAndTranslatePageViewModel : BaseViewModel
    {
        private readonly UniversalController _controller;

        private List<HistoryOutputModel> _historyList;

        public List<HistoryOutputModel> HistoryList
        {
            get => _historyList;
            set
            {
                _historyList = value;
                OnPropertyChanged(nameof(HistoryList));
            }
        }

        public HistoryAndTranslatePageViewModel()
        {
            _controller = new UniversalController("http://localhost:5046/api/");

            HistoryDefaultSetter();
        }

        private async void HistoryDefaultSetter()
        {
            HistoryList = new List<HistoryOutputModel>
            {
                new HistoryOutputModel
                {
                    Id = 0,
                    Screenshot = null,
                    InputLanguage = new Language
                    {
                        Id = 0,
                        Name = "Английский",
                    },
                    OutputLanguage = new Language
                    {
                        Id = 1,
                        Name = "Русский",
                    },
                    SelectedOcr = new Ocr
                    {
                        Id = 0,
                        Name = "Tesseract",
                    },
                    RecognizedText = "Hello World",
                    RecognizedTextAccuracy = 0.999198,
                    SelectedTranslator = new Translator
                    {
                        Id = 0,
                        Name = "MyMemory",
                    },
                    Translate = "Привет Мир",
                    User = new User
                    {
                        Id = 0,
                        Login = "123",
                        Password = "123",
                        Role = new Role
                        {
                            Id = 0,
                            Name = "Admin",
                        },
                    },
                }
            };
        }
        private async Task GetHistory()
        {

        }
    }
}
