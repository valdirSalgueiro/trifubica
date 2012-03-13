﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace ColorLand
{
    public class StoryScreen : BaseScreen
    {
        private const int cTOTAL_RESOURCES = 3;

        private Button mButtonSkip;

        private SpriteBatch mSpriteBatch;

        private String[] mImagesNames = new String[2];

        private String[] mSoundFilesNames =
        {
            ""
        };

        private Texture2D[] mImages;

        private int mCurrentIndex = -1;
        private MTimer mTimer;

        private Texture2D mPastTexture;
        private Texture2D mCurrentTexture;


        private Rectangle mRectangleExhibitionTexture;
        //private SoundEffect[] mSound;


        public StoryScreen()
        {
            //fill array
            for (int x = 0; x < mImagesNames.Length; x++)
            {
                if(x < 10){
                    mImagesNames[x] = "Imagem_0" + (x+1) + "_pos"; 
                }else{
                    mImagesNames[x] = "Imagem_" + (x+1) + "_pos";
                }
                 
                
            }

            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mImages = new Texture2D[cTOTAL_RESOURCES];

            mRectangleExhibitionTexture = new Rectangle(0, 0, 800, 600);

            //load resources
            for(int x=0; x < mImagesNames.Length; x++){
                mImages[x] =  Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("story\\"+mImagesNames[x]);
                //SoundManager.LoadSound(mSoundFilesNames[x]);
            }
             
            restartTimer();

        }

        public void next()
        {
            mCurrentIndex++;

            mPastTexture = mCurrentTexture;
            if (mCurrentIndex < mImages.Length)
            {
                mCurrentTexture = mImages[mCurrentIndex];
                if (mPastTexture != null)
                {
                    mPastTexture.Dispose();
                }
                mPastTexture = null;
            }
           
        }

        private void goToGameScreen()
        {
            if (mTimer != null)
            {
                mTimer.stop();
            }
            Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_GAMEPLAY, true);
        }

        private void restartTimer()
        {
            mTimer = new MTimer();
            mTimer.start();
        }

        private void updateTimer(GameTime gameTime)
        {
            if (mTimer != null)
            {

                mTimer.update(gameTime);

                //first image
                if (mTimer.getTimeAndLock(1))
                {
                    next();
                }else
                if (mTimer.getTimeAndLock(4))
                {
                    //for the last image
                    //mTimer.stop();
                    //mTimer = null;
                    next();
                    
                }else
                if (mTimer.getTimeAndLock(6))
                {
                        //for the last image
                        //mTimer.stop();
                        //mTimer = null;
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_GAMEPLAY, true);
                }

            }
        }

        public override void update(GameTime gameTime)
        {
            updateTimer(gameTime);
        }

        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();

            if (mCurrentTexture != null)
            {
                mSpriteBatch.Draw(mCurrentTexture, mRectangleExhibitionTexture, Color.White);
            }
            mSpriteBatch.End();
        }

    }
}