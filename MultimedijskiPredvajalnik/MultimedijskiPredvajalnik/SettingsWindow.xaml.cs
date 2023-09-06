using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace MultimedijskiPredvajalnik
{
    public partial class SettingsWindow : Window
    {
        readonly string[] timeIntervals = { "1 Minute","2 Minutes", "5 Minutes", "10 Minutes" };
        string? selectedItem;
        string selectedInterval;
        bool saveSetting;
        public SettingsWindow()
        {
            InitializeComponent();
            selectedInterval = timeIntervals[Array.IndexOf(timeIntervals, Properties.Settings.Default.TimeSetting)];
            saveSetting = Properties.Settings.Default.AutosaveOnOff;
            DataContext = this;
            canvas.Loaded += Canvas_Loaded;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            StartLabelAnimation();
        }

        private void StartLabelAnimation()
        {
            double totalWidth = canvas.ActualWidth / 2;

            lineSegment.Point = new Point(totalWidth, 0);

            DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
            animation.Duration = TimeSpan.FromSeconds(5); 
            animation.RepeatBehavior = RepeatBehavior.Forever; 

            var keyFrame1 = new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0));
            var keyFrame2 = new LinearDoubleKeyFrame(totalWidth, KeyTime.FromPercent(0.5));
            var keyFrame3 = new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1));

            animation.KeyFrames.Add(keyFrame1);
            animation.KeyFrames.Add(keyFrame2);
            animation.KeyFrames.Add(keyFrame3);

            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        public string[] TimeIntervals => timeIntervals;

        public string SelectedInterval
        {
            get { return selectedInterval; }
            set
            {
                selectedInterval = value;
                Properties.Settings.Default.TimeSetting = selectedInterval;
                Properties.Settings.Default.Save();
                MainWindow.viewModel.OnPropertyChanged("selectedInterval");
            }
        }

        public bool SaveSetting
        {
            get { return saveSetting; }
            set
            {
                saveSetting= value;
                Properties.Settings.Default.AutosaveOnOff = saveSetting;
                Properties.Settings.Default.Save();
                MainWindow.viewModel.OnPropertyChanged("saveSetting");
            }
        }

        private void DodajZvrst_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(newZvrst.Text))
            {
                Properties.Settings.Default.Zvrsti.Add(newZvrst.Text);
                Properties.Settings.Default.Save();
                newZvrst.Text = "";
                Zvrsti.Items.Refresh();
            }
            else
                MessageBox.Show("Navedite ime nove zvrsti!", "OPOZORILO!");
        }

        private void DeleteZvrst_Click(object sender, RoutedEventArgs e)
        {
            StringCollection stringCollection = Properties.Settings.Default.Zvrsti;

            if (stringCollection.Count > 1)
            {       
                if (Zvrsti.SelectedItem != null)
                {
                    if(Zvrsti.SelectedIndex != 0)
                    {
                        selectedItem = Zvrsti.SelectedItem.ToString();
                        stringCollection.Remove(selectedItem);
                        Properties.Settings.Default.Save();
                        Zvrsti.Items.Refresh();
                        newZvrst.Text = "";
                    }
                    else
                        MessageBox.Show("Te vrednosti ne morete izbrisati!", "OPOZORILO!");
                }
                else
                    MessageBox.Show("Izberite element!", "OPOZORILO!");
            }
            else
                MessageBox.Show("Te vrednosti ne morete izbrisati!", "OPOZORILO!");
        }

        private void UpdateZvrst_Click(Object sender, RoutedEventArgs e)
        {
            StringCollection myStringCollection = Properties.Settings.Default.Zvrsti;

            if (myStringCollection != null && myStringCollection.Count > 1)
            {
                int i = Zvrsti.SelectedIndex;
                if(i > 0)
                {
                    myStringCollection[i] = newZvrst.Text;
                    Properties.Settings.Default.Save();
                    Zvrsti.Items.Refresh();
                    newZvrst.Text = "";
                }
                else
                    MessageBox.Show("Te vrednosti ne morete posodabljati!", "OPOZORILO!");
            }
            else
                MessageBox.Show("Te vrednosti ne morete posodabljati!", "OPOZORILO!");
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Zvrsti.SelectedItem != null)
            {
                string? selectedItem = Zvrsti.SelectedItem.ToString();
                newZvrst.Text = selectedItem;
            }
        }
    }
}
