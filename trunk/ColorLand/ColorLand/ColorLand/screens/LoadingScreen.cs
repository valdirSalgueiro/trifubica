﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
#if WINDOWS
using System.Timers;
#endif

namespace ColorLand
{
    class LoadingScreen : BaseScreen
    {
#if WINDOWS
        private SpriteBatch mSpriteBatch;

        //Lista dos backgrounds
        private Background mBackgroundLoading;

        private Background mCurrentBackground;

        private static Fade mFadeIn;

        private static Fade mCurrentFade;

        private List<Background> mList = new List<Background>();

        private static Timer mTimer;

        private const int cMAX_BG_COUNTER = 1;
        private int mBackgroundCounter;
        

        public LoadingScreen()
        {
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundLoading = new Background("story\\Imagem_01_pos");
            mBackgroundLoading.loadContent(Game1.getInstance().getScreenManager().getContent());

            mList.Add(mBackgroundLoading);
            
            mCurrentBackground = mList.ElementAt(0);

            mFadeIn = new Fade(this, "fades\\blackfade");
            
            executeFade(mFadeIn,Fade.sFADE_IN_EFFECT_GRADATIVE);

        }


        private void nextBackground()
        {
            //Console.WriteLine("FUNCIONOU!!!!");
        }

        public override void update(GameTime gameTime)
        {
            mCurrentBackground.update();

            mFadeIn.update(gameTime);
           
        }

        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();

            mCurrentBackground.draw(mSpriteBatch);

            mFadeIn.draw(mSpriteBatch);
            
            mSpriteBatch.End();
        }

        public override void executeFade(Fade fadeObject, int effect)
        {
            base.executeFade(fadeObject, effect);

            mCurrentFade = fadeObject;
            fadeObject.execute(effect);
        }

        ///////prototipo
        public override void fadeFinished(Fade fadeObject)
        {
            if(fadeObject.getEffect() == Fade.sFADE_IN_EFFECT_GRADATIVE){
                restartTimer(1);
            }else
            if (fadeObject.getEffect() == Fade.sFADE_OUT_EFFECT_GRADATIVE)
            {
                mBackgroundCounter++;
                if (mBackgroundCounter < cMAX_BG_COUNTER)
                {
                    mCurrentBackground = mList.ElementAt(mBackgroundCounter);
                    restartTimer(1);
                }
                else
                {
                    restartTimer(1);
                }

            }
            
            //GC.KeepAlive(aTimer);

        }

        //qualquer coisa mete static aqui que funciona
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            mTimer.Stop();
            mTimer.Enabled = false;

            if (mCurrentFade.getEffect() == Fade.sFADE_IN_EFFECT_GRADATIVE)
            {
                executeFade(mFadeIn, Fade.sFADE_OUT_EFFECT_GRADATIVE);
            }else

            if (mCurrentFade.getEffect() == Fade.sFADE_OUT_EFFECT_GRADATIVE)
            {
                executeFade(mFadeIn, Fade.sFADE_IN_EFFECT_GRADATIVE);
            }
            
        }

        private void restartTimer(int seconds)
        {
            mTimer = new Timer();
            mTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            mTimer.Interval = seconds * 1000;
            mTimer.Enabled = true;
        }

        //apenas limpar quando for para o menu principal
        private void clearAllBackgrounds()
        {

        }

#endif

    }
}
