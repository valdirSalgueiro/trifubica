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

        private KeyboardState oldState;
        
        
        private Fade mFade;
        private Fade mCurrentFade;

        //essa flag eh pra autorizar o update do timer apenas quando o fade-in terminar
        private bool mAuthorizeUpdate;

        private bool mMousePressing;

        private static MacroMapState mCurrentMacroMapState;

        private static Background mBackgroundBefore;
        private static Background mBackgroundImage;

        private MacromapPlayer mMacromapPlayer;
        private MacromapShip mMacromapShip;
        private Texture2D mTexturePlayerFalling;
        private float mFallingScale = 2;

        private bool mShowTextureFallingPlayer;

        private ExplosionManager mExplosionManager;

        public enum MacroMapState
        {
            FirstStage,
            SecondStage,
            ThirdStage,
            FourthStage
        }

        
        public MacroMapScreen()
        {
            //debug purposes oinly
            ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, new ProgressObject(1), ProgressObject.PlayerColor.BLUE);
            //
            
            if (!SoundManager.isPlaying())
            {
                //SoundManager.PlayMusic("sound\\music\\historia1");
            }

            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            
            mRectangleExhibitionTexture = new Rectangle(0, 0, 800, 600);

            //load resources
             
            mFade = new Fade(this, "fades\\blackfade");
            //executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

            mMacromapPlayer = new MacromapPlayer(Color.Red, new Vector2(100, 130));
            mMacromapPlayer.loadContent(Game1.getInstance().getScreenManager().getContent());

            mTimer = new MTimer(true);

            //ExtraFunctions.saveLevel(2);
            //Game1.print(" >>> " + ExtraFunctions.loadLevel().getCurrentStage());
            if (ExtraFunctions.loadLevel().getCurrentStage() == 1)
            {
            //    Game1.print("AGAIN 1");
              //  setMacroMapState(MacroMapState.FirstStage);
            }
            else
            {
                setMacroMapState(MacroMapState.SecondStage); 
            }

            setMacroMapState(MacroMapState.FirstStage);

        }

        public void setMacroMapState(MacroMapState state)
        {

            mCurrentMacroMapState = state;

            switch (state)
            {
                case MacroMapState.FirstStage:

                    mMacromapPlayer.setCenter(214, 265);
                    mMacromapPlayer.setVisible(false);
                    mMacromapPlayer.setScale(0.1f);

                    mTexturePlayerFalling = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\macromap\\caindo_mapa_03");

                    //mMacromapPlayer.setDestiny(500, 500);
                    
                    mBackgroundImage = new Background("gameplay\\macromap\\mapa_bg_00");
                    mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mMacromapShip = new MacromapShip(new Vector2(40, 100),MacromapShip.ColorStatus.Black_And_White);
                    mMacromapShip.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mShowTextureFallingPlayer = true;
                    break;

                case MacroMapState.SecondStage:

                    mMacromapPlayer.setCenter(214, 265);
                    mMacromapPlayer.perfectSize();
                    //mMacromapPlayer.setVisible(true);
                    mMacromapPlayer.setDestiny(83, 117);
                    mMacromapPlayer.moveTo(new Vector2(83, 117));

                    mBackgroundBefore = new Background("gameplay\\macromap\\mapa_bg_00");
                    mBackgroundBefore.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mBackgroundImage = new Background("gameplay\\macromap\\mapa_bg_04");
                    mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mMacromapShip = new MacromapShip(new Vector2(40, 100), MacromapShip.ColorStatus.Black_And_White);
                    mMacromapShip.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mExplosionManager = new ExplosionManager();
                    mExplosionManager.addExplosion(10, Color.Red, Game1.getInstance().getScreenManager().getContent());

                    mShowTextureFallingPlayer = false;
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

        private void updateTimer(GameTime gameTime)
        {
            if (mTimer != null)
            {
                mTimer.update(gameTime);

                if (mCurrentMacroMapState == MacroMapState.FirstStage)
                {
                    if (mTimer.getTimeAndLock(3))
                    {
                        mMacromapPlayer.setVisible(true);
                        mMacromapPlayer.growUp(0.1f);
                    }
                }
                if (mCurrentMacroMapState == MacroMapState.SecondStage)
                {
                    if (mTimer.getTimeAndLock(1))
                    {
                        mBackgroundBefore = null;
                        mExplosionManager.getNextOfColor(Color.Red).explode(224,140);
                        mExplosionManager.getNextOfColor(Color.Red).explode(185,205);
                        mExplosionManager.getNextOfColor(Color.Red).explode(109,225);
                        mExplosionManager.getNextOfColor(Color.Red).explode(75, 316);
                    }

                }
               
            }
        }

        private float reduceScale()
        {
            //if(
            if (mFallingScale > 0.008)
            {
                mFallingScale -= 0.028f;
            }
            else
            {
                mShowTextureFallingPlayer = false;
            }

            return mFallingScale;
        }

        public override void update(GameTime gameTime)
        {

            mCursor.update(gameTime);

            updateTimer(gameTime);

            if (mCurrentMacroMapState == MacroMapState.FirstStage)
            {

                if (mMacromapShip != null)
                {
                    mMacromapShip.update(gameTime);
                }

               

                //if stage 1
                //mTimer.update(gameTime);
                //updateTimer aqui no caso de bug

            }else
            if (mCurrentMacroMapState == MacroMapState.SecondStage)
            {
                mExplosionManager.update(gameTime);

                if (mMacromapShip != null)
                {
                    mMacromapShip.update(gameTime);
                    //mMacromapShip.getCurrentSprite().setFlip(true);
                }

            }

            mMacromapPlayer.update(gameTime);

            if (mFade != null)
            {
                mFade.update(gameTime);
            }
        }

        public override void draw(GameTime gameTime)
        {

            mSpriteBatch.Begin();

            if (mBackgroundBefore != null)
            {
                mBackgroundBefore.draw(mSpriteBatch);
            }

            if (mBackgroundImage != null && mBackgroundBefore == null)
            {
                mBackgroundImage.draw(mSpriteBatch);
            }

            if (mExplosionManager != null)
            {
                mExplosionManager.draw(mSpriteBatch);
            }
            
            if (mMacromapShip != null)
            {
                mMacromapShip.draw(mSpriteBatch);
            }

            if (mShowTextureFallingPlayer)
            {
                mSpriteBatch.Draw(mTexturePlayerFalling, new Vector2(176, 235), new Rectangle(0, 0, mTexturePlayerFalling.Width, mTexturePlayerFalling.Height), Color.White, 0f, new Vector2(30, 30), reduceScale(), SpriteEffects.None, 0);
            }

            mMacromapPlayer.draw(mSpriteBatch);


            mCursor.draw(mSpriteBatch);

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
            
            if (newState.IsKeyDown(Keys.F2))
            {
                if (!oldState.IsKeyDown(Keys.F2))
                {
                    //ExtraFunctions.saveLevel(2);
                    //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MACROMAP, true,false);
                    mMacromapShip.setFlip(true);
                    mMacromapShip.moveTo(new Vector2(-180, (int)mMacromapShip.mY));
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
