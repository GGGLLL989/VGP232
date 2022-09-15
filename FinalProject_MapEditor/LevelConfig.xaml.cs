using System.Windows;
using System.Windows.Controls;

namespace FinalProject_MapEditor
{
    /// <summary>
    /// LevelConfig.xaml 的交互逻辑
    /// </summary>
    public partial class LevelConfig : Window
    {
        private int selectionSize;
        public LevelConfig()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Choose one size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseSize(object sender, RoutedEventArgs e)
        {
            string content = ((RadioButton)sender).Content.ToString();
            if (content.Contains("20"))
            {
                selectionSize = 20;
            }
            else if (content.Contains("25"))
            {
                selectionSize = 25;
            }
            else if (content.Contains("30"))
            {
                selectionSize = 30;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // Switch to MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.SetUp(selectionSize);
            Close();
            mainWindow.Show();
        }
    }
}
