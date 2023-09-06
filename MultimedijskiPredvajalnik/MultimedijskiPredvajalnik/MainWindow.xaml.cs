using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;
using Path = System.IO.Path;

namespace MultimedijskiPredvajalnik
{

    public partial class MainWindow : Window
    {
        //Pomožna bool spremenljivke
        private bool userIsDraggingSlider = false;
        public static ViewModel viewModel = new();
        int minutes;

        public MainWindow()
        {
            DataContext = viewModel;

            InitializeComponent();

            MediaPlayerControl mediaPlayerControl = new();
            mediaPlayerControl.PlayMediaEvent += PlayMedia;
            mediaPlayerControl.PauseMediaEvent += PauseMedia;
            mediaPlayerControl.NextMediaEvent += NextMedia;
            mediaPlayerControl.PrevMediaEvent += PrevMedia;
            mediaPlayerControl.ReplayMediaEvent += ReplayMedia;
            mediaPlayerControl.StopMediaEvent += StopMedia;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            timer.Start();

            minutes = 0;
        }

        private void SaveTimerTick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.AutosaveOnOff) //Preverimo ali je avtomatsko shranjevanje vklopljeno
            {
                if (viewModel.Playlist.Count != 0)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Media>));
                    using TextWriter writer = new StreamWriter("autosave.xml");
                    serializer.Serialize(writer, viewModel.Playlist);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer saveTimer = new DispatcherTimer();
            if (Properties.Settings.Default.TimeSetting == "1 Minute")
            {
                minutes = 1;
            }
            else if(Properties.Settings.Default.TimeSetting == "2 Minutes")
            {
                minutes = 2;
            }
            else if (Properties.Settings.Default.TimeSetting == "5 Minutes")
            {
                minutes = 5;
            }
            else
                minutes = 10;

            saveTimer.Interval = TimeSpan.FromMinutes(minutes);
            saveTimer.Tick += SaveTimerTick;
            saveTimer.Start();

            if (File.Exists("autosave.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Media>));
                using FileStream stream = File.OpenRead(Path.GetFullPath("autosave.xml"));
                var temp = (ObservableCollection<Media>)serializer.Deserialize(stream);
                if(temp != null)
                {
                    viewModel.Playlist = temp;
                }
            }
        }

        private void PlayMedia(object sender, EventArgs e)
        {
            Video.Play();
        }

        void PauseMedia(object sender, EventArgs e)
        {
            Video.Pause();
        }

        void NextMedia(object sender, EventArgs e)
        {
            viewModel.NextMedia();   
            Video.Play();
        }

        void PrevMedia(object sender, EventArgs e)
        {
            viewModel.PrevMedia();
            Video.Play();
        }

        void ReplayMedia(object sender, EventArgs e)
        {
            Video.Position = TimeSpan.FromSeconds(0);
            Video.Play();
        }

        void StopMedia(object sender, EventArgs e)
        {
            Video.Stop();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if ((Video.Source != null) && (Video.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                SliderProgress.Minimum = 0;
                SliderProgress.Maximum = Video.NaturalDuration.TimeSpan.TotalSeconds;
                SliderProgress.Value = Video.Position.TotalSeconds;
            }
        }

        private void IsSliderDraggingStart(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void IsSliderDraggingStop(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            Video.Position = TimeSpan.FromSeconds(SliderProgress.Value);
        }

        private void ProgressChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Progress.Content = TimeSpan.FromSeconds(SliderProgress.Value).ToString(@"hh\:mm\:ss");
        }

        void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            viewModel.DoubleClick();
            Video.Play();
        }

        private void VolumeChange(object sender, MouseWheelEventArgs e)
        {
            Video.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        void ExitApp(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
