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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultimedijskiPredvajalnik
{
    public partial class MediaPlayerControl : UserControl
    {
        public MediaPlayerControl()
        {
            DataContext = MainWindow.viewModel;
            InitializeComponent();     
        }

        public delegate void ButtonClickedEventHandler(object sender, EventArgs e);

        public event ButtonClickedEventHandler? PlayMediaEvent;
        public event ButtonClickedEventHandler? PauseMediaEvent;
        public event ButtonClickedEventHandler? NextMediaEvent;
        public event ButtonClickedEventHandler? PrevMediaEvent;
        public event ButtonClickedEventHandler? ReplayMediaEvent;
        public event ButtonClickedEventHandler? StopMediaEvent;

        private void PlayVideo(object sender, RoutedEventArgs e)
        {
            PlayMediaEvent?.Invoke(this, EventArgs.Empty);
        }  
        
        private void PauseVideo(object sender, RoutedEventArgs e)
        {
            PauseMediaEvent?.Invoke(this, EventArgs.Empty);
        }

        private void NextVideo(object sender, RoutedEventArgs e)
        {
            NextMediaEvent?.Invoke(this, EventArgs.Empty);
        }

        private void PrevVideo(object sender, RoutedEventArgs e)
        {
            PrevMediaEvent?.Invoke(this, EventArgs.Empty);
        }

        private void ReplayVideo(object sender, RoutedEventArgs e)
        {
            ReplayMediaEvent?.Invoke(this, EventArgs.Empty);
        }

        private void StopVideo(object sender, RoutedEventArgs e)
        {
            StopMediaEvent?.Invoke(this, EventArgs.Empty);
        }

    }
}
