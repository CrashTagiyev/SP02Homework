using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SP02Homework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string Text { get; set; } = "";
        public char Letter { get; set; }



        private int progressBarValue;

        public int ProgressBarValue
        {
            get { return progressBarValue; }
            set { progressBarValue = value; OnPropertyChanged(nameof(progressBarValue)); }
        }

        bool IsThreadStarted = false;

        OpenFileDialog FromFileDialog = new OpenFileDialog();
        OpenFileDialog ToFileDialog = new OpenFileDialog();

        public event PropertyChangedEventHandler PropertyChanged;

        private Thread DownloadThread { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            FromFileDialog.Filter = "Text Files (*.txt; *.md; *.doc)|*.txt; *.md; *.doc|All Files (*.*)|*.*";
            ToFileDialog.Filter = "Text Files (*.txt; *.md; *.doc)|*.txt; *.md; *.doc|All Files (*.*)|*.*";

        }

        private void Select_From_Text(object sender, RoutedEventArgs e)
        {
            SelectFromText();
        }
        public void SelectFromText()
        {
            if (FromFileDialog.ShowDialog() == true)
            {
                Text = File.ReadAllText(FromFileDialog.FileName);
                Download_Bar.Maximum = Text.Count();
                From_TB.Text = FromFileDialog.FileName;
            }
        }

        private void Start_Downloading(object sender, RoutedEventArgs e)
        {
            Download_Bar.Minimum = 0;
            DownloadThread = new Thread(LoadTextToAnother);
            DownloadThread.IsBackground = true;
            DownloadThread.Start();
        }

        private void Select_To_File(object sender, RoutedEventArgs e)
        {
            SelectTheOtherFIle();
        }

        public void LoadTextToAnother()
        {
            IsThreadStarted = true;
            ProgressBarValue = 0;
            if (Text != null)
                for (int i = 0; i < Text.Count(); i++)
                {
                    Thread.Sleep(1000);
                    ProgressBarValue++;
                    File.AppendAllText(ToFileDialog.FileName, Text[i].ToString());
                }
            IsThreadStarted = false;
        }

        public void SelectTheOtherFIle()
        {
            if (ToFileDialog.ShowDialog() == true)
            {
                To_TB.Text = ToFileDialog.FileName;
            }
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Suspend_Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsThreadStarted)
            {
                DownloadThread.Suspend();
            }

        }

        private void Resume_Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (IsThreadStarted)
            {
                DownloadThread.Resume();
                
            }
        }
    }
}
