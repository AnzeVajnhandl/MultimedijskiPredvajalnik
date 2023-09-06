using Microsoft.Win32;
using System;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;
using System.Configuration;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MultimedijskiPredvajalnik
{
    public partial class AddMediaWindow : Window
    {
        string? selectedValue = "";
        public static Media? toBeAdded;
        public AddMediaWindow()
        {
            DataContext = MainWindow.viewModel;
            InitializeComponent();
            Loaded += AddMediaWindow_Loaded;      
        }

        private void AddMediaWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartLabelAnimation();
        }

        private void StartLabelAnimation()
        {
            double totalWidth = canvas.ActualWidth;

            lineSegment.Point = new Point(totalWidth, 0);

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = totalWidth / 10;
            animation.Duration = TimeSpan.FromSeconds(3);

            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
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

        private void StoreMediaFile(object sender, RoutedEventArgs e)
        {
            ImageSource source = InputImage.Source;
            if (source is BitmapImage bitmap)
            {
                string? path = bitmap.UriSource?.AbsolutePath;
                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(selectedValue))
                {
                    toBeAdded = new Media(selectedValue, InputPath.Content.ToString(), path , InputTitle.Text, "00:00:00", InputAuthor.Text);
                    MainWindow.viewModel.OnPropertyChanged("toBeAdded");
                }
            }       
            Close();
        }

        private void SelectionChanged(object sender, RoutedEventArgs e)
        {
            selectedValue = SelectedZvrst.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedValue))
            {
                selectedValue = "Unknown";
            }            
        }

        private void CancelNewMediaFile(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
