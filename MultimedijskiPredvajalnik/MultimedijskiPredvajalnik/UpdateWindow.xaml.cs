using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace MultimedijskiPredvajalnik
{

    public partial class UpdateWindow : Window
    {

        public UpdateWindow()
        {
            DataContext = MainWindow.viewModel;
            InitializeComponent();
        }

        private void NewMediaFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Video Files|*.MP4;";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
                InputPath.Content = Path.GetFullPath(openFileDialog.FileName);
            else
                MessageBox.Show("Datoteka ne obstaja!", "OPOZORILO");
        }

        private void NewImageFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image Files|*.PNG;*.JPG;*.GIF;";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
                InputImage.Source = new BitmapImage(new Uri(Path.GetFullPath(openFileDialog.FileName)));
            else
                MessageBox.Show("Datoteka ne obstaja!", "OPOZORILO");
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            MainWindow.viewModel.IsWindowOpen = false;
            Close();      
        }

        private void CloseWindow2(object sender, EventArgs e)
        {
            MainWindow.viewModel.IsWindowOpen = false;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Topmost = true;
        }
    }

   
}
