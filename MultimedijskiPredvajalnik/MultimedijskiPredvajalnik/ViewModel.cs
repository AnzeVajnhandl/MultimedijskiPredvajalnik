using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace MultimedijskiPredvajalnik
{
    public class ViewModel: ViewModelBase
    {
        private ObservableCollection<Media> playlist;
        private Media selectedItem;
        private bool isWindowOpen = false;
        private Media? currentPlayingVideo;
 
        public ViewModel()
        {
            playlist = new ObservableCollection<Media>
            {};
        }

        public void AddMedia() //Rocno dodajanje posnetka
        {
            if(AddMediaWindow.toBeAdded is not null)
            {
                playlist.Add(AddMediaWindow.toBeAdded);
                OnPropertyChanged("playlist");
            }
                
            else
                MessageBox.Show("Napaka med dodajanjem!", "OPOZORILO!");
        }

        private void DeleteMedia() //Brisanje posnetka
        {
            if (SelectedItem != null)
            {
                if(ReferenceEquals(currentPlayingVideo, SelectedItem))
                {
                    currentPlayingVideo = null;
                    OnPropertyChanged("currentPlayingVideo");
                }
                    
                playlist.Remove(SelectedItem);
                OnPropertyChanged("playlist");
            }
            else
                MessageBox.Show("Prosim izberite posnetek!", "OPOZORILO!");
        }

        private void UpdateMedia() //Posodabljanje posnetka
        {
            if (SelectedItem != null)
            {
                int i = playlist.IndexOf(SelectedItem);
                playlist[i].Title = SelectedItem.Title;
                playlist[i].Path = SelectedItem.Path;
                playlist[i].Image = SelectedItem.Image;
                playlist[i].Author = SelectedItem.Author;
                playlist[i].Genre =  SelectedItem.Genre;
                OnPropertyChanged("playlist");
            }
            else
                MessageBox.Show("Prosim izberite posnetek!", "OPOZORILO!");
        }

        private void ImportMedia(){ //Uvoz XML datoteke
            OpenFileDialog openFileDialog = new();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "XML Files|*.XML;";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Media>));
                using FileStream stream = File.OpenRead(Path.GetFullPath(openFileDialog.FileName));
                var temp = (ObservableCollection<Media>)serializer.Deserialize(stream);
                if(temp != null)
                {
                    playlist = new ObservableCollection<Media>(playlist.Concat(temp));
                }
                OnPropertyChanged("playlist");
            }
            else
                MessageBox.Show("Napaka med uvozom datoteke!", "OPOZORILO!");
        }

        private void ExportMedia() //Izvoz XML datoteke
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML Files|*.XML;";
            dialog.DefaultExt = "xml";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string fileName = dialog.FileName;
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Media>));
                using TextWriter writer = new StreamWriter(fileName);
                serializer.Serialize(writer, playlist);
            }
            else
            {
                MessageBox.Show("Napaka med izvozom datoteke!", "OPOZORILO!");
            }
        }

        private void UpdateCurrentlyPlaying() //Posodabljanje trenutno predvajanje vsebine
        {
            if (SelectedItem != null)
            {
                if(currentPlayingVideo == null)
                {
                    currentPlayingVideo = selectedItem;
                    int j = playlist.IndexOf(SelectedItem);
                    playlist[j].IsPlaying = true;
                }
                else
                {
                    int i = playlist.IndexOf(currentPlayingVideo);
                    playlist[i].IsPlaying = false;
                    int k = playlist.IndexOf(selectedItem);
                    currentPlayingVideo = playlist[k];
                    playlist[k].IsPlaying = true;
                }
                OnPropertyChanged("currentPlayingVideo");
            }
        }

        private void OpenSettings() //Odpiranje okna za nastavitve
        {
            SettingsWindow settingsWindow = new();
            settingsWindow.ShowDialog();
        }

        private void OpenAdd() //Odpiranje okna za dodajanje
        {
            AddMediaWindow addMediaWindow = new();
            addMediaWindow.ShowDialog();
        }

        private void OpenUpdate() //Odpiranje okna za urejanje
        {
            if(selectedItem!= null)
            {
                if(isWindowOpen == false)
                {
                    isWindowOpen= true;
                    UpdateWindow updateWindow = new();
                    updateWindow.Show();
                }          
            }
            else
                MessageBox.Show("Prosim izberite posnetek!", "OPOZORILO!");
        }

        public void DoubleClick() //Predvajanje na dvojni klik
        {
            if (currentPlayingVideo != null)
            {
                int i = playlist.IndexOf(currentPlayingVideo);
                playlist[i].IsPlaying = false;
                currentPlayingVideo = selectedItem;
                i = playlist.IndexOf(currentPlayingVideo);
                playlist[i].IsPlaying = true;
            }
            else
            {
                currentPlayingVideo = selectedItem;
                int i = playlist.IndexOf(currentPlayingVideo);
                playlist[i].IsPlaying = true;
            }

            OnPropertyChanged("currentPlayingVideo");
        }

        public void NextMedia() //Preklop na naslednji posnetek
        {
            if (currentPlayingVideo != null && playlist.Count > 1)
            {
                int i = playlist.IndexOf(currentPlayingVideo);
                playlist[i].IsPlaying = false;

                if (i == playlist.Count - 1)
                {
                    currentPlayingVideo = playlist[0];
                    playlist[0].IsPlaying = true;
                }
                else
                {
                    currentPlayingVideo = playlist[i + 1];
                    playlist[i + 1].IsPlaying = true;
                }
                OnPropertyChanged("currentPlayingVideo");
            }
        }

        public void PrevMedia() //Preklop na prejsnji posnetek
        {
            if (currentPlayingVideo != null && playlist.Count > 1)
            {
                int i = playlist.IndexOf(currentPlayingVideo);
                playlist[i].IsPlaying = false;

                if (i == 0)
                {
                    currentPlayingVideo = playlist[playlist.Count - 1];
                    playlist[playlist.Count - 1].IsPlaying = true;
                }
                else
                {
                    currentPlayingVideo = playlist[i - 1];
                    playlist[i - 1].IsPlaying = true;
                }
                OnPropertyChanged("currentPlayingVideo");
            }
        }

        //Setterji in getterji

        public ObservableCollection<Media> Playlist { 
            get { return playlist; } 
            set {
                if(playlist != value)
                {
                    playlist = value;
                    OnPropertyChanged("playlist");
                }
            } 
        }

        public Media SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    OnPropertyChanged("selectedItem");
                }
            }

        }

        public bool IsWindowOpen
        {
            get { return isWindowOpen; }
            set
            {
                isWindowOpen = value;
            }
        }

        public Media? CurrentPlayingVideo
        {
            get { return currentPlayingVideo; }
            set { currentPlayingVideo = value; OnPropertyChanged("currentPlayingVideo"); }

        }

        //Komande
        public ICommand AddCommand{ get { return new Command(AddMedia);} }
        public ICommand DeleteCommand { get { return new Command(DeleteMedia); } }
        public ICommand UpdateCommand { get { return new Command(UpdateMedia); } }

        public ICommand OpenSettingsCommand { get { return new Command(OpenSettings); } }
        public ICommand OpenAddWindowCommand { get { return new Command(OpenAdd); } }
        public ICommand OpenUpdateWindowCommand { get { return new Command(OpenUpdate); } }

        public ICommand UpdateCurrentlyPlayingCommand { get { return new Command(UpdateCurrentlyPlaying); } }


        public ICommand ImportCommand { get { return new Command(ImportMedia);} }
        public ICommand ExportCommand { get { return new Command(ExportMedia); } }

    }
}
