using ScreenRecognition.Desktop.Resources.Styles.TopWindowPanels;
using System.Windows;

namespace ScreenRecognition.Desktop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private object? _topWindowPanel;

        public MainWindow()
        {
            InitializeComponent();

            InitializeElements();

            topWindowPanelFrame.Content = _topWindowPanel;
        }

        private void InitializeElements()
        {
            _topWindowPanel = new TopWindowPanelPage().Content;
        }
    }
}