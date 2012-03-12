using System;
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

        private String[] mImagesNames =
        {
            ""
        };

        private String[] mSoundFilesNames =
        {
            ""
        };

        private Texture2D[] mImages;

        private int mCurrentIndex;
        private MTimer mTimer;

        private Texture2D mCurrentTexture;


        private Rectangle mRectangleExhibitionTexture;
        //private SoundEffect[] mSound;


        public StoryScreen()
        {
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mImages = new Texture2D[cTOTAL_RESOURCES];

            mRectangleExhibitionTexture = new Rectangle(0, 0, 800, 600);

            //load resources
            for(int x=0; x < cTOTAL_RESOURCES; x++){
                mImages[x] =  Game1.getInstance().getScreenManager().getContent().Load<Texture2D>(mImagesNames[x]);
                SoundManager.LoadSound(mSoundFilesNames[x]);
            }

            restartTimer();

        }

        public void next()
        {
            mCurrentIndex++;

            mCurrentTexture = mImages[mCurrentIndex];


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
                //first image
                if (mTimer.getTimeAndLock(1))
                {

                }else
                if (mTimer.getTimeAndLock(5))
                {
                    //for the last image
                    mTimer.stop();
                    mTimer = null;
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

            mSpriteBatch.Draw(mCurrentTexture, mRectangleExhibitionTexture, Color.White);
            
            mSpriteBatch.End();
        }

    }
}
