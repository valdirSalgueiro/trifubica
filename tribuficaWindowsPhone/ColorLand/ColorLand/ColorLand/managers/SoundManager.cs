using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    public static class SoundManager
    {
        private static bool activated = false;

        //the sound effects for our game
        static Dictionary<string, SoundEffect> sounds =
            new Dictionary<string, SoundEffect>();

        //the music for our game
        static Song currentSong;

        //a content manager for later loading sound effects
        static ContentManager content;

        //we now store the sound volume in here
        static float soundVolume = 1f;

        /// <summary>
        /// Initializes the manager.
        /// </summary>
        /// <param name="game">The game instance.</param>
        public static void Initialize(Game game)
        {
            content = game.Content;
        }

        /// <summary>
        /// Plays a sound with a given name.
        /// </summary>
        /// <param name="name">The name of the sound to play.</param>
        public static void PlaySound(string name)
        {
            SoundEffect effect;
            if (sounds.TryGetValue(name, out effect))
                effect.Play(soundVolume, 0.0f, 0.0f);
        }

        /// <summary>
        /// Loads a sound for playing
        /// </summary>
        /// <param name="assetName">The asset name of the sound</param>
        public static void LoadSound(string assetName)
        {
            //load the sound into our dictionary
            if (!sounds.ContainsKey(assetName))
            {
                sounds.Add(assetName, content.Load<SoundEffect>(assetName));
            }
            else if(sounds[assetName].IsDisposed){
                sounds[assetName] = content.Load<SoundEffect>(assetName);
            }
        }


        /// <summary>
        /// Sets the background music to the sound with the given name.
        /// </summary>
        /// <param name="name">The name of the music to play.</param>
        public static void PlayMusic(string name)
        {
            currentSong = null;
            SetMusicVolume(0.8f);
            try
            {
                currentSong = content.Load<Song>(name);
            }
            catch (Exception e)
            {
                //if we didn't find the song, rethrow the exception
                if (currentSong == null)
                    throw e;
            }
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(currentSong);

            //Game1.print("VOLUME: " + MediaPlayer.Volume);
            //Game1.print("SOUND: " + soundVolume);
        }

        public static void PlayMusic(string name, bool repeat)
        {
            currentSong = null;
            SetMusicVolume(0.8f);
            try
            {
                currentSong = content.Load<Song>(name);
            }
            catch (Exception e)
            {
                //if we didn't find the song, rethrow the exception
                if (currentSong == null)
                    throw e;
            }
            MediaPlayer.IsRepeating = repeat;
            MediaPlayer.Play(currentSong);
        }

        /// <summary>
        /// Stops the background music.
        /// </summary>
        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Sets the sound FX volume level.
        /// </summary>
        /// <param name="volume">A volume level in the range [0, 1].</param>
        public static void SetSoundFXVolume(float volume)
        {
            soundVolume = volume;
        }

        /// <summary>
        /// Sets the music volume level.
        /// </summary>
        /// <param name="volume">A volume level in the range [0, 1].</param>
        public static void SetMusicVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        public static bool isPlaying()
        {
            return MediaPlayer.State == MediaState.Playing;
        }
        public static void stopMusic()
        {
            if (isPlaying())
            {
                MediaPlayer.Stop();
            }
        }

        public static void setSound(bool enabled)
        {
            activated = enabled;

            if (activated)
            {
                MediaPlayer.Volume = 1;
                SetSoundFXVolume(1);
            }
            else
            {
                MediaPlayer.Volume = 0;
                SetSoundFXVolume(0);
            }
        }

        public static void toggleSound()
        {
            activated = !activated;

            if (activated)
            {
                MediaPlayer.Volume = 1;
                SetSoundFXVolume(1);
            }
            else
            {
                MediaPlayer.Volume = 0;
                SetSoundFXVolume(0);
            }
        }

    }
}