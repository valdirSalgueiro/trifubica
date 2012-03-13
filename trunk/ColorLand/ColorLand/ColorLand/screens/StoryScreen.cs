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
        private const int cTOTAL_RESOURCES = 20;

        private Button mButtonSkip;

        private SpriteBatch mSpriteBatch;

        private String[] mImagesNames = new String[cTOTAL_RESOURCES];

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

        private Button mButtonNext;
        private Button mCurrentHighlightButton;

        private Cursor mCursor;
        private bool mMousePressing;


        public StoryScreen()
        {

            if (!SoundManager.isPlaying())
            {
                SoundManager.PlayMusic("sound\\music\\historia1");
            }

            //fill array
            for (int x = 0; x < mImagesNames.Length; x++)
            {
                if(x < 10){
                    if (x != 9)
                    {
                        mImagesNames[x] = "Imagem_0" + (x + 1) + "_pos";
                    }
                    else
                    {
                        mImagesNames[x] = "Imagem_" + (x + 1) + "_pos";
                    }
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

            mButtonNext = new Button("mainmenu\\buttons\\next", "mainmenu\\buttons\\next_select", "mainmenu\\buttons\\next_selected", new Rectangle(650, 10, 160, 192));
            mButtonNext.loadContent(Game1.getInstance().getScreenManager().getContent());

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

        }

        public void next()
        {
            mCurrentIndex++;

            //mPastTexture = mCurrentTexture;
            if (mCurrentIndex < mImages.Length)
            {
                mCurrentTexture = mImages[mCurrentIndex];
                if (mPastTexture != null)
                {
                    //mPastTexture.Dispose();
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
                for (int x = 0, time = 1; x < cTOTAL_RESOURCES; x++, time += 3)
                {
                    if (time >= 63)
                    {
                        goToGameScreen();
                    }
                    else
                    {
                        if (mTimer.getTimeAndLock(time))
                        {
                            next();
                        }

                    }
                }
                /*
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
                */
            }
        }

        public override void update(GameTime gameTime)
        {
            mCursor.update(gameTime);
            updateTimer(gameTime);
            mButtonNext.update(gameTime);
            updateMouseInput();
            checkCollisions();

            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyDown(Keys.Escape) || newState.IsKeyDown(Keys.Enter))
            {
                goToGameScreen();
            }
        }

        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();

            if (mCurrentTexture != null)
            {
                mSpriteBatch.Draw(mCurrentTexture, mRectangleExhibitionTexture, Color.White);
            }
            mButtonNext.draw(mSpriteBatch);
            mCursor.draw(mSpriteBatch);
            mSpriteBatch.End();
        }

        private void updateMouseInput()
        {


            MouseState ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed)
            {
                mMousePressing = true;
            }
            else
            {
                if (mCurrentHighlightButton != null)
                {

                    if (mMousePressing)
                    {
                        processButtonAction(mCurrentHighlightButton);
                    }

                }

                mMousePressing = false;
            }

            //dispara evento

        }

        private void checkCollisions()
        {

            if (mButtonNext.collidesWith(mCursor))
            {
                mCurrentHighlightButton = mButtonNext;

                if (mMousePressing)
                {
                    if (mCurrentHighlightButton.getState() != Button.sSTATE_PRESSED)
                    {
                        mCurrentHighlightButton.changeState(Button.sSTATE_PRESSED);
                    }
                }
                else
                {

                    if (mCurrentHighlightButton.getState() != Button.sSTATE_HIGHLIGH)
                    {
                        mCurrentHighlightButton.changeState(Button.sSTATE_HIGHLIGH);
                    }

                }

            }
            else
            {
                if (mCurrentHighlightButton != null)// && mCurrentHighlightButton.getState() != Button.sSTATE_PRESSED)
                {
                    mCurrentHighlightButton.changeState(Button.sSTATE_NORMAL);
                }
                mCurrentHighlightButton = null;
            }

        }

        private void processButtonAction(Button button)
        {
            
            if (button == mButtonNext)
            {

                goToGameScreen();
            }


        }

    }
}
