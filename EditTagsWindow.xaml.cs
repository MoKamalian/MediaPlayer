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
using System.Windows.Shapes;

namespace Assignment1_media_player_amir_kamalian
{
    /// <summary>
    /// Interaction logic for EditTagsWindow.xaml
    /// </summary>
    public partial class EditTagsWindow : Window
    {
        TagLib.File currentMp3File;
        public EditTagsWindow(TagLib.File f)
        {
            InitializeComponent(); 
            currentMp3File = f;
             
        }

        /** User editing of mp3 file tags */
        private void editTagsBtn_Click(object sender, RoutedEventArgs e)
        {            

            try
            {
                currentMp3File.Tag.Title = titleTag.Text;
                currentMp3File.Tag.Performers[0] = artistTag.Text;
                currentMp3File.Tag.Album = albumTag.Text;
                currentMp3File.Tag.Year = Convert.ToUInt16(yearTag.Text);

                currentMp3File.Save();
                this.Close(); 
            
            }
            catch (Exception ex)
            {
                Popup pop_error = new Popup();
                TextBlock pop_error_text = new TextBlock();
                pop_error_text.Text = "Incorrect input! check if you are inputting/using number for year";
                pop_error.Child = pop_error_text;
            }


        }

    }
}
