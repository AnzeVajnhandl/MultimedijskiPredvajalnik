using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace MultimedijskiPredvajalnik
{
    [Serializable]
    public class Media: ViewModelBase
    {
        private string genre;
        private string path;
        private string image;
        private string title;
        private string length; //V sekundah
        private string author;
        private bool isPlaying = false;

        public Media(string genre, string path, string image, string title, string length, string author)
        {
            this.genre = genre;
            this.path = path;
            this.image = image;
            this.title = title;
            this.length = length;
            this.author = author;
        }

        public Media()
        {

        }

        public string Genre
        {
            get { return genre; }

            set { 
                if (value != null && value != genre)
                {
                    genre = value;
                    OnPropertyChanged("genre");
                }
            }
        }

        public string Path
        {
            get { return path; }
            set {
                if (File.Exists(value) && path != value)
                {
                    path = value;
                    OnPropertyChanged("path");
                }
            }
        }

        public string Image
        {
            get { return image; }
            set{
                if (File.Exists(value) && image != value)
                {
                    image = value;
                    OnPropertyChanged("image");
                }
            }
        }

        public string Title
        {
            get { return title;}
            set {
                if (value != null && title != value)
                {
                    title = value;
                    OnPropertyChanged("title");
                }
            }
        }

        public string Length
        {
            get { return length;}
            set {  if (value != null && length != value)
                {
                    length = value;
                    OnPropertyChanged("length");
                }                 
            }
        }

        public string Author
        {
            get { return author; }
            set { if (value != null && author != value)
                {
                    author = value;
                    OnPropertyChanged("author");
                } 
            }
        }

        public bool IsPlaying
        {
            get { return isPlaying; }
            set
            {
                isPlaying = value;
                OnPropertyChanged("isPlaying");
            }
        }

        public override string ToString()
        {
            return "Genre: " + genre + " Path: " + path + " Image path: " + Image + " Title: " + title + " Length: " + length + " Author: " + author;
        }
    }
}
