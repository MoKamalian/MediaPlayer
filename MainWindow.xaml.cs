using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

/**
 
 references: 
[1] https://wpf-tutorial.com/audio-video/how-to-creating-a-complete-audio-video-player/
 
 */


namespace Assignment1_media_player_amir_kamalian
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool musicIsPlaying = false;
        private bool progressBarChanged = false;
        public TagLib.File currentFile;
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += music_time;
            timer.Start(); 
        }

        /** opening and loading music file for playing */
        private void CanExecute_open(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Executed_open(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic); /* initial directory in the music folder */
            dialog.Filter = "MP3 files (*.mp3)|*.mp3 | All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                currentFile = TagLib.File.Create(dialog.FileName);
                mediaPlayer.Source = new Uri(dialog.FileName);

                /* display tag information */
                nowPlayingWindow.Title.Content = currentFile.Tag.Title;
                nowPlayingWindow.Artist.Content = currentFile.Tag.FirstPerformer;
                nowPlayingWindow.Album.Content = currentFile.Tag.Album + " (" + currentFile.Tag.Year + ")" + " | " + currentFile.Tag.FirstGenre + " | ";
            }
        }

        /** for playing the music */
        private void CanExecute_play(object sender, CanExecuteRoutedEventArgs e)
        {
            /* check if media has been loaded first */
            e.CanExecute = (mediaPlayer != null) && (mediaPlayer.Source != null);
        }

        private void Executed_play(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayer.Play();
            musicIsPlaying = true;
        }

        /** for pausing the music */
        private void CanExecute_pause(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = musicIsPlaying;
        }

        private void Executed_pause(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        /** stops music player from playing completely */
        private void CanExecute_stop(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = musicIsPlaying; /* check if music is playing first */
        }

        private void Executed_stop(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayer.Stop();
            musicIsPlaying = false;
        }

        /** slider bar functionality */
        private void progressBar_Started(object sender, DragStartedEventArgs e)
        {
            progressBarChanged = true;
        }

        private void progressBar_Completed(object sender, DragCompletedEventArgs e)
        {
            progressBarChanged = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(progressBar.Value); /* start playing from the new position on the slider */
        }

        /** displaying the music time */
        private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            musicTimer.Text = TimeSpan.FromSeconds(progressBar.Value).ToString(@"hh\:mm\:ss");
        }

        /* Displays the progression of the track in the musci timer */
        private void music_time(object sender, EventArgs e)
        {   /* if there is music present with an actual duration and slider is not being dragged */
            if ((mediaPlayer != null) && (mediaPlayer.NaturalDuration.HasTimeSpan) && (!progressBarChanged))
            {
                /* updating the position of the slider */
                progressBar.Minimum = 0;
                progressBar.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                progressBar.Value = mediaPlayer.Position.TotalSeconds;
            }
        }

        /** open the edit tags window (another user control) so that user can edit file tags */
        private void openEditTagsWindow_Click(object sender, RoutedEventArgs e)
        {
            if (currentFile != null)
            {
                // EditTags editTagsWindow = new EditTags();
                //editTagsWindow.; 
                EditTagsWindow edw = new EditTagsWindow(currentFile);
                edw.ShowDialog();

                /** update the tags being displayed with the new tags */
                nowPlayingWindow.Title.Content = currentFile.Tag.Title;
                nowPlayingWindow.Artist.Content = currentFile.Tag.Performers[0];
                nowPlayingWindow.Album.Content = currentFile.Tag.Album + " (" + currentFile.Tag.Year + ")" + " | " + currentFile.Tag.FirstGenre + " | ";
            }
        }

        /** change the volume of music playing */
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mediaPlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1; 
        }
    }
}
