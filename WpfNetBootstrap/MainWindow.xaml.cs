using System.Windows;

namespace WpfNetBootstrap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Control_Click(object sender, RoutedEventArgs e)
        {
            Test window = new Test();

            window.Show();
        }
    }
}