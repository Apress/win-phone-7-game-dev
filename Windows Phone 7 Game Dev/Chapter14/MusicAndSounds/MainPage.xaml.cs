using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using SLGameFramework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MusicAndSounds
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Add a Rendering handler so we can display the song status
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        private void pianoButton_Click(object sender, RoutedEventArgs e)
        {
            // Play the Piano sound effect
            AudioPlayer.PlaySoundEffect("Sounds/Piano.wav");
        }

        private void motorbikeButton_Click(object sender, RoutedEventArgs e)
        {
            // Play the Motorbike sound effect using a SoundEffectInstance
            SoundEffectInstance sound = AudioPlayer.CreateSoundEffectInstance("Sounds/Motorbike.wav");
            // Set a random pitch
            sound.Pitch = GameHelper.RandomNext(-1.0f, 1.0f);
            sound.Play();
        }

        private void musicButton_Click(object sender, RoutedEventArgs e)
        {
            // Start, pause or resume the song
            switch (AudioPlayer.SongState)
            {
                case MediaState.Stopped:
                    AudioPlayer.PlaySong("Sounds/JoshWoodward-Breadcrumbs-NoVox-02-2020.mp3");
                    break;
                case MediaState.Paused:
                    AudioPlayer.ResumeSong();
                    break;
                case MediaState.Playing:
                    AudioPlayer.PauseSong();
                    break;
            }
        }


        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            // Display information about the song state
            switch (AudioPlayer.SongState)
            {
                case MediaState.Playing:
                    musicStatus.Text = AudioPlayer.SongPosition.ToString();
                    break;
                case MediaState.Stopped:
                    musicStatus.Text = "Stopped";
                    break;
                case MediaState.Paused:
                    // Blink the text once per second
                    if (DateTime.Now.Millisecond > 500)
                    {
                        musicStatus.Text = "Paused";
                    }
                    else
                    {
                        musicStatus.Text = "";
                    }
                    break;
            }
        }

        private void fxVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AudioPlayer.SoundEffectMasterVolume = (float)e.NewValue / 100.0f;
        }

        private void musicVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AudioPlayer.SongMasterVolume = (float)e.NewValue / 100.0f;
        }


    }
}