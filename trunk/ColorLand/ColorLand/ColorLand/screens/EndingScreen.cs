using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Timers;

namespace ColorLand
{
    public class EndingScreen : BaseScreen
    {

        private Video mVideo;
        private VideoPlayer mVideoPlayer;
        private Texture2D mVideoTexture;

        private SpriteBatch mSpriteBatch;

        private MTimer mTimer;

        private Texture2D mTextureClickToStart;

        private MTimer mTimerBlinkText;

        private Rectangle mRectangleExhibitionTexture;
        private MouseState oldStateMouse;
        private bool mMousePressing;
        private KeyboardState oldState;

        private bool mShowTextClickToStart;

        private bool mClicked; //ready to go to main menu

        private Fade mFade;
        private Fade mCurrentFade;

        public EndingScreen()
        {

            mVideo = Game1.getInstance().getScreenManager().getContent().Load<Video>("story\\videos\\ENDING_WITH_MUSIC");
            mVideoPlayer = new VideoPlayer();

            mVideoPlayer.Play(mVideo);

            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mFade = new Fade(this, "fades\\blackfade");
            executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

            mTimer = new MTimer(true);

            mTextureClickToStart = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\clicktoskip");

            mTimerBlinkText = new MTimer(true);

        }
                

        private void goToMainMenu()
        {
            ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setCurrentStage(1));

            if (mTimer != null)
            {
                mTimer.stop();
                mTimer = null;
            }

            mVideoPlayer.Stop();
            Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, true, true);

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

                if (mTimer.getTimeAndLock(39))
                {
                    executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                    //TODO diminuir volume da musica
                }

            }
        }

        public override void update(GameTime gameTime)
        {
            if (mFade != null)
            {
                mFade.update(gameTime);
            }

            mCursor.update(gameTime);
            updateMouseInput();
            updateTimer(gameTime);
            updateTimerBlinkText(gameTime);

            /*mCamera.update();
            
            if (mAuthorizeUpdate)
            {
                updateTimer(gameTime);
                /*mButtonNext.update(gameTime);
                
                checkCollisions();* /
            }


            switch (mOrder)
            {
                //mapa
                case 1:
                    mCamera.zoomOut(0.0005f);
                break;
            }



            if (mFade != null)
            {
                mFade.update(gameTime);
            }*/
        }

        public override void draw(GameTime gameTime)
        {

            if (mVideoPlayer != null)
            {
                if (mVideoPlayer.State != MediaState.Stopped)
                    mVideoTexture = mVideoPlayer.GetTexture();
            }

            if (mVideoTexture != null)
            {
                mSpriteBatch.Begin();
                mSpriteBatch.Draw(mVideoTexture, new Rectangle(0, 0, 800, 600), Color.White);
                mSpriteBatch.End();
            }


            if (mShowTextClickToStart && !mClicked)
            {
                mSpriteBatch.Begin();
                mSpriteBatch.Draw(mTextureClickToStart, new Vector2(280, 520), Color.White);
                mSpriteBatch.End();
            }

            mSpriteBatch.Begin();
            mCursor.draw(mSpriteBatch);
            
            if (mFade != null)
            {
                mFade.draw(mSpriteBatch);
            }
            mSpriteBatch.End();

                /*mSpriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    null,
                    null,
                    null,
                    null,
                    mCamera.get_transformation(Game1.getInstance().GraphicsDevice));


                if (mCurrentTexture != null)
                {
                    mSpriteBatch.Draw(mCurrentTexture, mRectangleExhibitionTexture, Color.White);
                }
                //mButtonNext.draw(mSpriteBatch);
                mCursor.draw(mSpriteBatch);

                if (mFade != null)
                {
                    mFade.draw(mSpriteBatch);
                }


                mSpriteBatch.End();
         */
        }

        public override void handleInput(InputState input)
        {
            base.handleInput(input);

            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Enter) || newState.IsKeyDown(Keys.Escape) || newState.IsKeyDown(Keys.Space))
            {
                if (!oldState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Space))
                {
                    //goToMainMenu();
                }
            }

            oldState = newState;
        }

        private void updateMouseInput()
        {
           /*MouseState ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed)
            {
                // mMousePressing = true;
                executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                //TODO diminuir volume da musica
            }
            else
            {
                

                mMousePressing = false;
            }
            */
            MouseState mouseState = Mouse.GetState();


            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (oldStateMouse.LeftButton != ButtonState.Pressed)
                {
                    mFade = new Fade(this, "fades\\blackfade", Fade.SPEED.FAST);
                    mClicked = true;
                    executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                }
            }

            oldStateMouse = mouseState;
            //dispara evento

        }

        private void processButtonAction(Button button)
        {
            
            /*if (button == mButtonNext)
            {
                mFade = new Fade(this, "fades\\blackfade");
                executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                //goToGameScreen();
            }*/

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

                restartTimer();
            }else
            if (fadeObject.getEffect() == Fade.sFADE_OUT_EFFECT_GRADATIVE)
            {
                goToMainMenu();
            }

        }

        private void updateTimerBlinkText(GameTime gameTime)
        {
            if (mTimerBlinkText != null)
            {
                mTimerBlinkText.update(gameTime);

                if (mTimerBlinkText.getTimeAndLock(0.2f))
                {
                    mShowTextClickToStart = true;
                }
                if (mTimerBlinkText.getTimeAndLock(0.4f))
                {
                    mShowTextClickToStart = false;
                    mTimerBlinkText.start();
                }
            }
        }

    }
}
