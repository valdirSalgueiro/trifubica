using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ColorLand
{
    public class SoundManager
    {

        /**
         * MUSIC
         */
        public const int MUSIC_HISTORY = 0;
        public const int MUSIC_GAME    = 1;
        
        public static int mCURRENT_MUSIC = -1;

        /**
         * FX
         * */
        
        private ContentManager mContent;
        
        private static SoundManager instance;

        private Song mMusicHistory;
        private Song mMusicGame;

        //private SoundEffectInstance mGameMusicInstance;


        public const int FX_NARRACAO = 0;
        public const int FX_INICIAR = 1;

        private const int FX_MAX_COUNT = 2;

        private SoundEffect[] mEffects;// = new SoundEffect[MAX_FX_COUNT];


        private SoundManager()
        {
            obtainContent();
            loadSounds();
        }


        private void obtainContent()
        {
            mContent = Game1.getInstance().getScreenManager().getContent();
            MediaPlayer.Volume = 0.5f;

        }

        public void loadSounds()
        {

            this.obtainContent();

            //mMusicHeroiSertao = mContent.Load<Song>("musictest");//vilarejo");//Stage1.getInstance().getCurrentScene().getContent().Load<Song>("sounds\\music1");
            //Game1.print("LOADING MUSIC ROOM SOUNDS");

            /*mMusicHistory =  mContent.Load<Song>("history_love");
            mMusicGame    =  mContent.Load<Song>("theme");

            mEffects = new SoundEffect[2];
            mEffects[0] = mContent.Load<SoundEffect>("narracao");
            mEffects[1] = mContent.Load<SoundEffect>("iniciar");
            */
        }

        public void loadMainMenuSounds()
        {
           // mEffects = new SoundEffect[FX_MAX_COUNT];
           // mEffects[FX_INICIAR] = mContent.Load<SoundEffect>("iniciar");
        }

        public void playMusic(String resourceName)
        {
            //mMusicHeroiSertao = mContent.Load<Song>(resourceName);//vilarejo");//Stage1.getInstance().getCurrentScene().getContent().Load<Song>("sounds\\music1");
            //MediaPlayer.Play(mMusicHeroiSertao);
            //MediaPlayer.IsRepeating = true;
        }

        public void releaseSounds()
        {
            //mEffects[0].Dispose();
            //mEffects[1].Dispose();
        }

        private void diminuiVolume()
        {
            for (float x = 1; x > 0; x -= 0.01f)
                MediaPlayer.Volume -= x;
        }

        private void aumentaVolume()
        {
            for (float x = 0; x < 1; x += 0.0001f)
                MediaPlayer.Volume += x;
        }


        public void playFX(int soundId)
        {

           // if (mEffects[soundId].IsDisposed)
             //   mEffects[FX_STAR] = mContent.Load<SoundEffect>("BoulderHit");

            if (mEffects[soundId].IsDisposed)
            {
                mEffects[soundId].CreateInstance();
            }
            mEffects[soundId].Play();
            
        }

        public void stopFX(int soundId)
        {
            if(mEffects[soundId].IsDisposed == false)
            mEffects[soundId].Dispose();
        }

        public void playWAV(String name)
        {
            SoundEffect effect = mContent.Load<SoundEffect>(name);
            effect.Play();
        }


        public void playMusic(int musicId)
        {

            switch (musicId)
            {

                case MUSIC_HISTORY:

                    MediaPlayer.Play(mMusicHistory);
                    MediaPlayer.IsRepeating = true;
                    mCURRENT_MUSIC = MUSIC_HISTORY;
                    break;

                case MUSIC_GAME:

                    if(mMusicGame == null ){
                        mMusicGame = mContent.Load<Song>("theme");
                    }

                    MediaPlayer.Play(mMusicGame);
                    MediaPlayer.IsRepeating = false;
                    mCURRENT_MUSIC = MUSIC_GAME;
                    break;

            }

        }

        public void stop()
        {

            //switch (soundId)
            // {
            //  case MUSIC_HEROI_DO_SERTAO:



            MediaPlayer.Stop();
            mCURRENT_MUSIC = -1;
            //    break;
            //mGameMusicInstance = mGameMusic.Play(2.0f, 0.0f, 0.0f, true);
            /*
          case SOUND_STAR:
              mEffects[SOUND_STAR].Play();
              break;
          case SOUND_JUMP:
              mEffects[SOUND_JUMP].Play();
              break;
          case SOUND_DIE:
              mEffects[SOUND_DIE].Play();
              break;*/
            //}

        }


        public static SoundManager getInstance()
        {

            if (instance == null)
                instance = new SoundManager();
            return instance;

        }

    }
}
