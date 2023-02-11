using ScreenRecognition.Desktop.ViewModel.PageViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ScreenRecognition.Desktop.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для TranslateHistoryPage.xaml
    /// </summary>
    public partial class HistoryAndTranslatePage : Page
    {
        public HistoryAndTranslatePage()
        {
            InitializeComponent();
        }

        private void GetTranslateButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as HistoryAndTranslatePageViewModel).GetTranslate(selectedImage.Source);
        }

        private void ClearHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as HistoryAndTranslatePageViewModel).HistoryList.Property = new();
            (DataContext as HistoryAndTranslatePageViewModel).ClearHistory();
        }
    }
}
