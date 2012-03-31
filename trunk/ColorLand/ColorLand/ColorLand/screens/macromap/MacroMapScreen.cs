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
    public class MacroMapScreen : BaseScreen
    {
        
        private SpriteBatch mSpriteBatch;
              
        private MTimer mTimer;

        private Rectangle mRectangleExhibitionTexture;
        
        
        private Fade mFade;
        private Fade mCurrentFade;

        //essa flag eh pra autorizar o update do timer apenas quando o fade-in terminar
        private bool mAuthorizeUpdate;

        private bool mMousePressing;

        private static MacroMapState mCurrentMacroMapState;

        private static Background mBackgroundImage;

        private MacromapPlayer mMacromapPlayer;
        private Texture2D mTexturePlayerFalling;
        private float mFallingScale = 1;

        public enum MacroMapState
        {
            EverythingColoured,
            EverythingBlackAndWhite,
            FirstIslandColoured,
            WestColoured,
            SecondIslandColoured,
            ThirdIslandColoured
        }

        
        public MacroMapScreen()
        {

            if (!SoundManager.isPlaying())
            {
                //SoundManager.PlayMusic("sound\\music\\historia1");
            }

            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            
            mRectangleExhibitionTexture = new Rectangle(0, 0, 800, 600);

            //load resources
             
            mFade = new Fade(this, "fades\\blackfade");
            //executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);

            Cursor.getInstance().loadContent(Game1.getInstance().getScreenManager().getContent());

            mMacromapPlayer = new MacromapPlayer(Color.Red, new Vector2(100, 100));
            mMacromapPlayer.loadContent(Game1.getInstance().getScreenManager().getContent());

            mTexturePlayerFalling = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("logos\\chihuahuagameslogo");

            //mMacromapPlayer.setDestiny(500, 500);
            mMacromapPlayer.setVisible(false);
            mMacromapPlayer.setScale(0.1f);
            mTimer = new MTimer(true);

            setMacroMapState(MacroMapState.EverythingBlackAndWhite);

            if (ObjectSerialization.Load<ProgressObject>(Game1.sPROGRESS_FILE_NAME).getCurrentStage() == 1)
            {
                Game1.print("FASE 1, nesse carai");
            }
            else
            {
                Game1.print("FASE DA PUTA QUE PARIU");
            }


        }

        public static void setMacroMapState(MacroMapState state)
        {

            mCurrentMacroMapState = state;

            switch (state)
            {
                case MacroMapState.EverythingColoured:

                    break;
                case MacroMapState.EverythingBlackAndWhite:
                    mBackgroundImage = new Background("gameplay\\macromap\\mapa1");
                    mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());
                    break;
                case MacroMapState.FirstIslandColoured:
                    mBackgroundImage = new Background("gameplay\\macromap\\mapa2");
                    mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());
                    break;
            }
        }

        /*
        private void goToGameScreen()
        {
            if (mTimer != null)
            {
                mTimer.stop();
                mTimer = null;
            }
            Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_GAMEPLAY, true);
        }*/

        private void restartTimer()
        {
            mTimer = new MTimer();
            mTimer.start();
        }

        /*private void updateTimer(GameTime gameTime)
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
        }*/

        private float reduceScale()
        {
            //if(
            if (mFallingScale > 0.008)
            {
                mFallingScale -= 0.008f;
            }

            return mFallingScale;
        }

        public override void update(GameTime gameTime)
        {
            Cursor.getInstance().update(gameTime);
            if (mAuthorizeUpdate)
            {
                //updateTimer(gameTime);
                updateMouseInput();
                checkCollisions();
               
            }

            mMacromapPlayer.update(gameTime);

            //if stage 1
            mTimer.update(gameTime);
            if (mTimer.getTimeAndLock(3))
           {
                mMacromapPlayer.setVisible(true);
                mMacromapPlayer.growUp(0.1f);
            }


            if (mFade != null)
            {
                mFade.update(gameTime);
            }
        }

        public override void draw(GameTime gameTime)
        {

            mSpriteBatch.Begin();

            if (mBackgroundImage != null)
            {
                mBackgroundImage.draw(mSpriteBatch);
            }

            mMacromapPlayer.draw(mSpriteBatch);

            Cursor.getInstance().draw(mSpriteBatch);

            if (mFade != null)
            {
                mFade.draw(mSpriteBatch);
            }

            mSpriteBatch.Draw(mTexturePlayerFalling, new Vector2(100, 100), new Rectangle(0, 0, mTexturePlayerFalling.Width, mTexturePlayerFalling.Height), Color.Blue, 0.6f, new Vector2(30, 30), reduceScale(), SpriteEffects.None, 0);

            mSpriteBatch.End();

        }

        public override void handleInput(InputState input)
        {
            base.handleInput(input);

            KeyboardState newState = Keyboard.GetState();

            /*if (newState.IsKeyDown(Keys.Enter) || newState.IsKeyDown(Keys.Escape) || newState.IsKeyDown(Keys.Space))
            {
                if (!oldState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Space))
                {
                    goToGameScreen();
                }
            }*/

            //oldState = newState;
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
                /*if (mCurrentHighlightButton != null)
                {

                    if (mMousePressing)
                    {
                        processButtonAction(mCurrentHighlightButton);
                    }

                }*/

                mMousePressing = false;
            }

            //dispara evento

        }

        private void checkCollisions()
        {

            
        }

        private void processButtonAction(Button button)
        {
            
            
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
                //goToGameScreen();
            }

        }

    }
}
