using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System.Timers;

namespace ColorLand
{
    public class StoryScreen : BaseScreen
    {
        private const int cTOTAL_RESOURCES = 20;

        private int mTickCount = 0;

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

        private bool mMousePressing;
        private KeyboardState oldState;


        private Fade mFade;
        private Fade mCurrentFade;

        //essa flag eh pra autorizar o update do timer apenas quando o fade-in terminar
        private bool mAuthorizeUpdate;

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
             
            mButtonNext = new Button("mainmenu\\buttons\\mapa_next", "mainmenu\\buttons\\mapa_next_select", "mainmenu\\buttons\\mapa_next_selected", new Rectangle(605,5, 195, 168));
            mButtonNext.loadContent(Game1.getInstance().getScreenManager().getContent());

            next();

            mFade = new Fade(this, "fades\\blackfade");
            executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);

            Cursor.getInstance().loadContent(Game1.getInstance().getScreenManager().getContent());

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
                mTimer = null;
            }

            //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_GAMEPLAY, true);
  //          Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MACROMAP, true);
//=======
            Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_GAMEPLAY, true, true);
//>>>>>>> .r87
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
                        mFade = new Fade(this, "fades\\blackfade");
                        executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                    }
                    else
                    {
                        if (mTimer.getTimeAndLock(time))
                        {
                            next();
                        }

                    }
                }
                
            }
        }

        public override void update(GameTime gameTime)
        {
            Cursor.getInstance().update(gameTime);
            if (mAuthorizeUpdate)
            {
                updateTimer(gameTime);
                mButtonNext.update(gameTime);
                updateMouseInput();
                checkCollisions();
            }
            if (mFade != null)
            {
                mFade.update(gameTime);
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
            Cursor.getInstance().draw(mSpriteBatch);

            if (mFade != null)
            {
                mFade.draw(mSpriteBatch);
            }

            mSpriteBatch.End();

        }

        public override void handleInput(InputState input)
        {
            base.handleInput(input);

            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Enter) || newState.IsKeyDown(Keys.Escape) || newState.IsKeyDown(Keys.Space))
            {
                if (!oldState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Space))
                {
                    goToGameScreen();
                }
            }

            oldState = newState;
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

            if (mButtonNext.collidesWith(Cursor.getInstance()))
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
                mFade = new Fade(this, "fades\\blackfade");
                executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                //goToGameScreen();
            }

        }



        /**************************
         * 
         * FADE
         * 
         * **************************/
        public override void executeFade(Fade fadeObject, int effect)
        {
            base.executeFade(fadeObject, effect);

            mCurrentFade = fadeObject;
            fadeObject.execute(effect);
        }

        public override void fadeFinished(Fade fadeObject)
        {
            if(fadeObject.getEffect() == Fade.sFADE_IN_EFFECT_GRADATIVE){

                mAuthorizeUpdate = true;
                restartTimer();

                mFade = null;
            }else
            if (fadeObject.getEffect() == Fade.sFADE_OUT_EFFECT_GRADATIVE)
            {
                goToGameScreen();
            }

        }

    }
}
