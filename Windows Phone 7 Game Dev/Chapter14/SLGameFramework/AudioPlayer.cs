using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace SLGameFramework
{
    public static class AudioPlayer
    {

        //-------------------------------------------------------------------------------------
        // Class-level variables

        /// <summary>
        /// A dictionary of loaded sound effects, to save memory loading the same sound multiple times
        /// </summary>
        private static Dictionary<string, SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, Song> _songs = new Dictionary<string, Song>();


        //-------------------------------------------------------------------------------------
        // Sound effect functions and properties

        private static float _soundEffectMasterVolume = 1;
        /// <summary>
        /// Allows the volume for sound effect playback to be controlled.
        /// </summary>
        /// <remarks>Set between 0 (silence) and 1 (full volume). This will only
        /// have an effect on sound effects initiated after the volume has been
        /// set; it will have no effect on sounds already playing.</remarks>
        public static float SoundEffectMasterVolume
        {
            get { return _soundEffectMasterVolume; }
            set
            {
                _soundEffectMasterVolume = MathHelper.Clamp(value, 0, 1);
            }
        }


        /// <summary>
        /// Plays the sound effect at the specified content path and filename
        /// </summary>
        /// <param name="soundPath">The path and filename of the sound to play</param>
        public static void PlaySoundEffect(string soundPath)
        {
            PlaySoundEffect(soundPath, 1, 0, 0);
        }

        /// <summary>
        /// Plays the sound effect at the specified content path and filename
        /// </summary>
        /// <param name="soundPath">The path and filename of the sound to play</param>
        /// <param name="volume">The sound volume (0 = silent, 1 = full volume)</param>
        /// <param name="pitch">The sound pitch (-1 = 1 octave down, 0 = normal, 1 = 1 octave up)</param>
        /// <param name="pan">The sound pan (-1 = full left, 0 = center, 1 = full right)</param>
        public static void PlaySoundEffect(string soundPath, float volume, float pitch, float pan)
        {
            // Check that the XNAAsyncDispatcher has been started
            XNAAsyncDispatcher.CheckIsStarted();

            // If we have no volume then there is nothing to play
            if (volume * SoundEffectMasterVolume == 0) return;

            // Get and play the sound effect
            GetSoundEffect(soundPath).Play(volume * SoundEffectMasterVolume, pitch, pan);
        }

        /// <summary>
        /// Creates and returns a SoundEffectInstance for the specified sound
        /// </summary>
        /// <param name="soundPath">The path and filename of the sound to play</param>
        public static SoundEffectInstance CreateSoundEffectInstance(string soundPath)
        {
            SoundEffectInstance instance;
            
            XNAAsyncDispatcher.CheckIsStarted();

            instance = GetSoundEffect(soundPath).CreateInstance();
            instance.Volume = SoundEffectMasterVolume;
            return instance;
        }

        /// <summary>
        /// Checks if the specified sound is cached (and loads and caches it if not),
        /// then returns its SoundEffect object.
        /// </summary>
        /// <param name="soundPath">The path and filename of the sound to play</param>
        private static SoundEffect GetSoundEffect(string soundPath)
        {
            // Convert the path to lowercase so that it is case-insensitive
            soundPath = soundPath.ToLower();

            // Do we already have this sound loaded?
            if (!_soundEffects.ContainsKey(soundPath))
            {
                // No, so load it now and add it to the dictionary
                _soundEffects.Add(soundPath, SoundEffect.FromStream(TitleContainer.OpenStream(soundPath)));
            }

            // Return the sound
            return _soundEffects[soundPath];
        }


        //-------------------------------------------------------------------------------------
        // Song functions and properties

        /// <summary>
        /// Allows the volume for Song playback to be controlled.
        /// </summary>
        /// <remarks>Set between 0 (silence) and 1 (full volume)</remarks>
        public static float SongMasterVolume
        {
            get { return MediaPlayer.Volume; }
            set
            {
                // Clamp -- use a non-zero minimum, as setting zero causes
                // a return to full volume.
                MediaPlayer.Volume = MathHelper.Clamp(value, 0.00001f, 1);
            }
        }

        /// <summary>
        /// Begin a song playing. Defaults to maximum volume and auto repeat.
        /// </summary>
        /// <param name="songPath">The path and filename of the song to play</param>
        public static void PlaySong(string songPath)
        {
            PlaySong(songPath, true);
        }
        /// <summary>
        /// Begin a song playing.
        /// </summary>
        /// <param name="songPath">The path and filename of the song to play</param>
        /// <param name="volume">The song volume (0 = silent, 1 = full volume)</param>
        /// <param name="autoRepeat">Pass true to auto-repeat, false to play just once</param>
        public static void PlaySong(string songPath, bool autoRepeat)
        {
            // Make sure we are in control of the media player
            if (MediaPlayer.GameHasControl)
            {
                // Convert the songPath to lower case so it is case-insensitive
                songPath = songPath.ToLower();
                // Is the song cached?
                if (!_songs.ContainsKey(songPath))
                {
                    // Load the song
                    _songs.Add(songPath, Song.FromUri(songPath, new Uri(songPath, UriKind.Relative)));
                }
                // Get a reference to the song
                Song song = _songs[songPath];
                // Set media player parameters
                MediaPlayer.IsRepeating = autoRepeat;
                MediaPlayer.Volume = SongMasterVolume;
                // Start playing
                MediaPlayer.Play(song);
            }
        }

        /// <summary>
        /// Stop the song that is currently playing
        /// </summary>
        public static void StopSong()
        {
            if (MediaPlayer.GameHasControl) MediaPlayer.Stop();
        }

        /// <summary>
        /// Pause the song that is currently playing
        /// </summary>
        public static void PauseSong()
        {
            if (MediaPlayer.GameHasControl) MediaPlayer.Pause();
        }

        /// <summary>
        /// Resume the song that is currently playing
        /// </summary>
        public static void ResumeSong()
        {
            if (MediaPlayer.GameHasControl) MediaPlayer.Resume();
        }

        /// <summary>
        /// Returns the current state of song playback (Stopped, Playing or Paused)
        /// </summary>
        public static MediaState SongState
        {
            get { return MediaPlayer.State; }
        }

        /// <summary>
        /// Returns the position through the current song
        /// </summary>
        public static TimeSpan SongPosition
        {
            get { return MediaPlayer.PlayPosition; }
        }
        
        /// <summary>
        /// Identifies whether the game is permitted to play songs at the current time
        /// </summary>
        public static bool GameHasControl
        {
            get { return MediaPlayer.GameHasControl; }
        }

    }
}
