﻿using System;
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

        private const String cMUSIC_MAP = "sound\\music\\mapa1";
        private const String cSOUND_FALLING = "sound\\fx\\queda8bit";

        private SpriteBatch mSpriteBatch;
              
        private MTimer mTimer;
        private MTimer mTimerBlinkText;

        private Rectangle mRectangleExhibitionTexture;

        private KeyboardState oldState;

        private bool mPlayerInsideShip;
        
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

        private bool mStartFeatures; //pra verificar quando autoriza a as acoes da tela. EX: demorar 1 segundo antes do boneco cair no mapa

        private bool mShowTextureFallingPlayer;

        private Texture2D mTextureBorder;
        private Texture2D mTexturePinoBussola;
        private Texture2D mTextureBussola;
        private Texture2D mTextureBussolaPointer;

        private float angleBussola;

        private ExplosionManager mExplosionManager;

        private Texture2D mTextureClickToStart;
        private bool mShowTextClickToStart;
        private bool mClicked; //ready to go to advance

        private MouseState oldStateMouse;

        //fade
        private Fade mFade;
        private Fade mCurrentFade;

        public enum MacroMapState
        {
            FirstStage,
            SecondStage,
            ThirdStage,
            FourthStage,
            Finish
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
             
          //  mFade = new Fade(this, "fades\\blackfade");
            //executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

            mTextureBorder = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\macromap\\mapaborda");
            mTexturePinoBussola = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\macromap\\bussola_ponteiro_pino");
            mTextureBussola = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\macromap\\bussola_bg");
            mTextureBussolaPointer = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\macromap\\bussola_ponteiro");

            mTimer = new MTimer(true);

            //ExtraFunctions.saveLevel(2);
            //Game1.print(" >>> " + ExtraFunctions.loadLevel().getCurrentStage());

            //Game1.progressObject.setCurrentStage(2);
            //ExtraFunctions.saveProgress(Game1.progressObject);

            if (ExtraFunctions.loadProgress().getCurrentStage() == 1)
            {
            //    Game1.print("AGAIN 1");
              //  setMacroMapState(MacroMapState.FirstStage);
                SoundManager.LoadSound(cSOUND_FALLING);
                setMacroMapState(MacroMapState.FirstStage);
            }
            if (ExtraFunctions.loadProgress().getCurrentStage() == 2)
            {
                setMacroMapState(MacroMapState.SecondStage); 
            }
            if (ExtraFunctions.loadProgress().getCurrentStage() == 3)
            {
                setMacroMapState(MacroMapState.ThirdStage); 
            }
            if (ExtraFunctions.loadProgress().getCurrentStage() == 4)
            {
                setMacroMapState(MacroMapState.FourthStage);
            }
            if (ExtraFunctions.loadProgress().getCurrentStage() == 5)
            {
                setMacroMapState(MacroMapState.Finish);
            }

            setMacroMapState(MacroMapState.SecondStage);

            //setMacroMapState(MacroMapState.Finish);

            SoundManager.PlayMusic(cMUSIC_MAP);

            mFade = new Fade(this, "fades\\blackfade", Fade.SPEED.FAST);

            executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);

            mTextureClickToStart = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\clicktoskip");

            mTimerBlinkText = new MTimer(true);

        }

        public void setMacroMapState(MacroMapState state)
        {

            mCurrentMacroMapState = state;

            switch (state)
            {
                case MacroMapState.FirstStage:
                                                                             
                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.RED)
                    {
                        mTexturePlayerFalling = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\macromap\\caindo_mapa_02");
                        mMacromapPlayer = new MacromapPlayer(Color.Red, new Vector2(100, 130));
                    }
                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.GREEN)
                    {
                        mTexturePlayerFalling = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\macromap\\caindo_mapa_01");
                        mMacromapPlayer = new MacromapPlayer(Color.Green, new Vector2(100, 130));
                    }
                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.BLUE)
                    {
                        mTexturePlayerFalling = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\macromap\\caindo_mapa_03");
                        mMacromapPlayer = new MacromapPlayer(Color.Blue, new Vector2(100, 130));
                    }

                    mMacromapPlayer.loadContent(Game1.getInstance().getScreenManager().getContent());
                    mMacromapPlayer.setCenter(214, 265);
                    mMacromapPlayer.setVisible(false);
                    mMacromapPlayer.setScale(0.1f);

                    //mMacromapPlayer.setDestiny(500, 500);
                    
                    mBackgroundImage = new Background("gameplay\\macromap\\mapa_bg_00");
                    mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mMacromapShip = new MacromapShip(new Vector2(40, 100),MacromapShip.ColorStatus.Black_And_White);
                    mMacromapShip.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mShowTextureFallingPlayer = true;

                    break;

                case MacroMapState.SecondStage:

                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.RED) mMacromapPlayer = new MacromapPlayer(Color.Red, new Vector2(100, 130));
                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.GREEN) mMacromapPlayer = new MacromapPlayer(Color.Green, new Vector2(100, 130));
                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.BLUE) mMacromapPlayer = new MacromapPlayer(Color.Blue, new Vector2(100, 130));
                                        
                    mMacromapPlayer.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mMacromapPlayer.setCenter(214, 265);
                    mMacromapPlayer.perfectSize();
                    //mMacromapPlayer.setVisible(true);

                    mBackgroundBefore = new Background("gameplay\\macromap\\mapa_bg_00");
                    mBackgroundBefore.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mBackgroundImage = new Background("gameplay\\macromap\\mapa_bg_04");
                    mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mMacromapShip = new MacromapShip(new Vector2(40, 100), MacromapShip.ColorStatus.Black_And_White);
                    mMacromapShip.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mExplosionManager = new ExplosionManager();
                    mExplosionManager.addExplosion(10, Color.Red, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(10, Color.Green, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(10, Color.Blue, Game1.getInstance().getScreenManager().getContent());

                    mShowTextureFallingPlayer = false;
                    break;

                case MacroMapState.ThirdStage:

                    mPlayerInsideShip = true;

                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.RED) mMacromapPlayer = new MacromapPlayer(Color.Red, new Vector2(100, 130));
                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.GREEN) mMacromapPlayer = new MacromapPlayer(Color.Green, new Vector2(100, 130));
                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.BLUE) mMacromapPlayer = new MacromapPlayer(Color.Blue, new Vector2(100, 130));

                    mMacromapPlayer.loadContent(Game1.getInstance().getScreenManager().getContent());
                                      
                    mBackgroundBefore = new Background("gameplay\\macromap\\mapa_bg_04");
                    mBackgroundBefore.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mBackgroundImage = new Background("gameplay\\macromap\\mapa_bg_01");
                    mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mMacromapShip = new MacromapShip(new Vector2(-100, 465), MacromapShip.ColorStatus.Colored);
                    mMacromapShip.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mMacromapPlayer.setCenter(mMacromapShip.getX() + 100, mMacromapShip.getY() + 70);
                    mMacromapPlayer.perfectSize();

                    mExplosionManager = new ExplosionManager();
                     mExplosionManager.addExplosion(10, Color.Red, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(10, Color.Green, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(10, Color.Blue, Game1.getInstance().getScreenManager().getContent());

                    mShowTextureFallingPlayer = false;
                    break;

                case MacroMapState.FourthStage:

                    mPlayerInsideShip = true;

                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.RED) mMacromapPlayer = new MacromapPlayer(Color.Red, new Vector2(100, 130));
                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.GREEN) mMacromapPlayer = new MacromapPlayer(Color.Green, new Vector2(100, 130));
                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.BLUE) mMacromapPlayer = new MacromapPlayer(Color.Blue, new Vector2(100, 130));

                    mMacromapPlayer.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mBackgroundBefore = new Background("gameplay\\macromap\\mapa_bg_01");
                    mBackgroundBefore.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mBackgroundImage = new Background("gameplay\\macromap\\mapa_bg_02");
                    mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mMacromapShip = new MacromapShip(new Vector2(251, 335), MacromapShip.ColorStatus.Colored);
                    mMacromapShip.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mMacromapPlayer.setCenter(507, 302);
                    mMacromapPlayer.perfectSize();

                    mExplosionManager = new ExplosionManager();
                     mExplosionManager.addExplosion(10, Color.Red, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(10, Color.Green, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(10, Color.Blue, Game1.getInstance().getScreenManager().getContent());

                    mShowTextureFallingPlayer = false;
                    break;

                case MacroMapState.Finish:

                    mPlayerInsideShip = true;

                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.RED) mMacromapPlayer = new MacromapPlayer(Color.Red, new Vector2(100, 130));
                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.GREEN) mMacromapPlayer = new MacromapPlayer(Color.Green, new Vector2(100, 130));
                    if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.BLUE) mMacromapPlayer = new MacromapPlayer(Color.Blue, new Vector2(100, 130));

                    mMacromapPlayer.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mBackgroundBefore = new Background("gameplay\\macromap\\mapa_bg_02");
                    mBackgroundBefore.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mBackgroundImage = new Background("gameplay\\macromap\\mapa_bg_03");
                    mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mMacromapShip = new MacromapShip(new Vector2(251, 335), MacromapShip.ColorStatus.Colored);
                    mMacromapShip.loadContent(Game1.getInstance().getScreenManager().getContent());

                    mMacromapPlayer.setCenter(629, 172);
                    mMacromapPlayer.perfectSize();

                    mExplosionManager = new ExplosionManager();
                    mExplosionManager.addExplosion(10, Color.Red, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(10, Color.Green, Game1.getInstance().getScreenManager().getContent());
                    mExplosionManager.addExplosion(10, Color.Blue, Game1.getInstance().getScreenManager().getContent());

                    mShowTextureFallingPlayer = false;
                    break;
            }
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

                if (mCurrentMacroMapState == MacroMapState.FirstStage)
                {
                    
                    if (mTimer.getTimeAndLock(1))
                    {
                        Game1.print("stage1");
                        mStartFeatures = true;
                        SoundManager.PlaySound(cSOUND_FALLING);
                    }

                    if (mTimer.getTimeAndLock(4))
                    {
                        mMacromapPlayer.setVisible(true);
                        mMacromapPlayer.growUp(0.1f);
                        mTimerBlinkText = new MTimer(true);
                    }
                }
                if (mCurrentMacroMapState == MacroMapState.SecondStage)
                {
                    Game1.print("stage2");
                    //explode cenario
                    if (mTimer.getTimeAndLock(1))
                    {
                        mBackgroundBefore = null;
                        mExplosionManager.getNextOfColor(Color.Red).explode(200,140);
                        mExplosionManager.getNextOfColor(Color.Green).explode(185,205);
                        mExplosionManager.getNextOfColor(Color.Blue).explode(109,225);
                        mExplosionManager.getNextOfColor(Color.Red).explode(75, 316);
                        mExplosionManager.getNextOfColor(Color.Red).explode(144, 387);
                        mExplosionManager.getNextOfColor(Color.Green).explode(214, 344);
                        mExplosionManager.getNextOfColor(Color.Green).explode(102, 223);
                        mExplosionManager.getNextOfColor(Color.Blue).explode(108, 143);
                        mExplosionManager.getNextOfColor(Color.Green).explode(131, 285);
                    }

                    //anda em direcao ao barco
                    if (mTimer.getTimeAndLock(3))
                    {
                        mMacromapPlayer.setDestiny(83, 117);
                        mMacromapPlayer.moveTo(new Vector2(83, 117));
                    }

                    //barco anda junto com jogador pra fora da tela
                    if (mTimer.getTimeAndLock(7))
                    {
                        mMacromapShip.setFlip(true);
                        mMacromapShip.moveTo(new Vector2(-180, (int)mMacromapShip.mY));

                        mMacromapPlayer.setDestiny(-180, (int)mMacromapShip.mY);
                        mMacromapPlayer.moveTo(new Vector2(-180, (int)mMacromapShip.mY));
                    }
                 
                }
                if (mCurrentMacroMapState == MacroMapState.ThirdStage)
                {
                    //explode cenario
                    if (mTimer.getTimeAndLock(1))
                    {
                        mBackgroundBefore = null;
                        mExplosionManager.getNextOfColor(Color.Blue).explode(224, 10);
                        mExplosionManager.getNextOfColor(Color.Red).explode(185, 25);
                        mExplosionManager.getNextOfColor(Color.Blue).explode(66, 75);
                        mExplosionManager.getNextOfColor(Color.Green).explode(243, 107);
                        mExplosionManager.getNextOfColor(Color.Red).explode(224, 10);
                        mExplosionManager.getNextOfColor(Color.Green).explode(185, 25);
                        mExplosionManager.getNextOfColor(Color.Red).explode(76, 473);
                        mExplosionManager.getNextOfColor(Color.Red).explode(355, 151);
                        mExplosionManager.getNextOfColor(Color.Blue).explode(201, 477);
                        mExplosionManager.getNextOfColor(Color.Red).explode(52, 170);
                        mExplosionManager.getNextOfColor(Color.Green).explode(216, 205);
                        mExplosionManager.getNextOfColor(Color.Green).explode(229, 402);

                        mMacromapShip.setFlip(false);
                        mMacromapShip.moveTo(new Vector2(40, 465));
                    }

                    if (mTimer.getTimeAndLock(5))
                    {
                        mMacromapShip.setFlip(false);
                        mMacromapShip.moveTo(new Vector2(251, 335));
                    }

                    if (mTimer.getTimeAndLock(15))
                    {
                        mMacromapPlayer.moveTo(new Vector2(360, 285));
                    }

                }
                if (mCurrentMacroMapState == MacroMapState.FourthStage)
                {
                    //explode cenario
                    if (mTimer.getTimeAndLock(1))
                    {
                        mBackgroundBefore = null;
                        mExplosionManager.getNextOfColor(Color.Red).explode(387, 393);
                        mExplosionManager.getNextOfColor(Color.Green).explode(385, 55);
                        mExplosionManager.getNextOfColor(Color.Blue).explode(409, 305);
                        mExplosionManager.getNextOfColor(Color.Red).explode(437, 123);
                        mExplosionManager.getNextOfColor(Color.Blue).explode(600, 273);
                        mExplosionManager.getNextOfColor(Color.Red).explode(487, 512);
                        mExplosionManager.getNextOfColor(Color.Green).explode(651, 463);
                        mExplosionManager.getNextOfColor(Color.Red).explode(335,489);
                        mExplosionManager.getNextOfColor(Color.Blue).explode(260, 246);
                        mExplosionManager.getNextOfColor(Color.Blue).explode(551, 362);
                        mExplosionManager.getNextOfColor(Color.Green).explode(479, 212);
                    }

                    if (mTimer.getTimeAndLock(5))
                    {
                        mMacromapPlayer.moveTo(new Vector2(629, 172));
                    }

                }

                if (mCurrentMacroMapState == MacroMapState.Finish)
                {
                    //explode cenario
                    if (mTimer.getTimeAndLock(1))
                    {
                        mBackgroundBefore = null;
                        mExplosionManager.getNextOfColor(Color.Red).explode(595, 69);
                        mExplosionManager.getNextOfColor(Color.Green).explode(600, 80);
                        mExplosionManager.getNextOfColor(Color.Blue).explode(509, 100);
                        mExplosionManager.getNextOfColor(Color.Red).explode(537, 120);
                        mExplosionManager.getNextOfColor(Color.Blue).explode(600, 135);
                        mExplosionManager.getNextOfColor(Color.Red).explode(587, 155);
                        mExplosionManager.getNextOfColor(Color.Green).explode(651, 200);
                        mExplosionManager.getNextOfColor(Color.Red).explode(700, 300);
                        mExplosionManager.getNextOfColor(Color.Blue).explode(660, 350);
                        mExplosionManager.getNextOfColor(Color.Blue).explode(751, 400);
                        mExplosionManager.getNextOfColor(Color.Green).explode(639, 212);
                        mExplosionManager.getNextOfColor(Color.Red).explode(6665, 93);
                    }

                    if (mTimer.getTimeAndLock(5))
                    {
                        //mMacromapPlayer.moveTo(new Vector2(629, 172));
                    }

                }
               
            }

            if (mTimerBlinkText != null)
            {
                mTimerBlinkText.update(gameTime);

                if (mTimerBlinkText.getTimeAndLock(0.8f))
                {
                    mShowTextClickToStart = true;
                }
                if (mTimerBlinkText.getTimeAndLock(1.6f))
                {
                    mShowTextClickToStart = false;
                    mTimerBlinkText.start();
                }
            }

        }

        private float reduceScale()
        {
            //if(
            if (mFallingScale > 0.010)
            {
                mFallingScale -= 0.050f;
            }
            else
            {
                mShowTextureFallingPlayer = false;
            }

            return mFallingScale;
        }

        public override void update(GameTime gameTime)
        {

            Vector2 direction = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - new Vector2(400, 0);
            angleBussola = (float)(Math.Atan2(direction.Y, direction.X));

            mCursor.update(gameTime);
            updateMouseInput();
            updateTimer(gameTime);

            if (mCurrentMacroMapState == MacroMapState.FirstStage)
            {

                if (mMacromapShip != null)
                {
                    mMacromapShip.update(gameTime);
                }
                
            }else
            if (mCurrentMacroMapState == MacroMapState.SecondStage)
            {
                mExplosionManager.update(gameTime);

                if (mMacromapShip != null)
                {
                    mMacromapShip.update(gameTime);
                    //mMacromapShip.getCurrentSprite().setFlip(true);
                }

            }else
            if (mCurrentMacroMapState == MacroMapState.ThirdStage)
            {
                mExplosionManager.update(gameTime);

                if (mMacromapShip != null)
                {
                    mMacromapShip.update(gameTime);
                }


                if (mPlayerInsideShip)
                {
                    mMacromapPlayer.setCenter(mMacromapShip.getX() + 100, mMacromapShip.getY() + 70);
                    mMacromapPlayer.perfectSize();
                }

            }else
            if (mCurrentMacroMapState == MacroMapState.FourthStage)
            {
                mExplosionManager.update(gameTime);

                if (mMacromapShip != null)
                {
                    mMacromapShip.update(gameTime);
                }
                               
            }else
            if (mCurrentMacroMapState == MacroMapState.Finish)
            {
                mExplosionManager.update(gameTime);

                if (mMacromapShip != null)
                {
                    mMacromapShip.update(gameTime);
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

            if (mMacromapShip != null)
            {
                mMacromapShip.draw(mSpriteBatch);
            }

            mMacromapPlayer.draw(mSpriteBatch);

            mSpriteBatch.Draw(mTextureBorder, new Rectangle(0, 0, 800, 600), Color.White);
            mSpriteBatch.Draw(mTextureBussola, new Rectangle(800-168, 422, 158, 165), Color.White);
            mSpriteBatch.Draw(mTextureBussolaPointer, new Vector2(710, 516), null, Color.White, angleBussola - (float)Math.PI / 2, new Vector2(mTextureBussolaPointer.Width / 2, mTextureBussolaPointer.Height / 2), 1, SpriteEffects.None, 0f);
            mSpriteBatch.Draw(mTexturePinoBussola, new Rectangle(632, 425, 158, 165), Color.White);
            
            if (mExplosionManager != null)
            {
                mExplosionManager.draw(mSpriteBatch);
            }
                     


            if (mStartFeatures && mShowTextureFallingPlayer)
            {
                mSpriteBatch.Draw(mTexturePlayerFalling, new Vector2(176, 235), new Rectangle(0, 0, mTexturePlayerFalling.Width, mTexturePlayerFalling.Height), Color.White, 0f, new Vector2(30, 30), reduceScale(), SpriteEffects.None, 0);
            }

            

            if (mShowTextClickToStart && !mClicked)
            {
                mSpriteBatch.Draw(mTextureClickToStart, new Vector2(245, 484), Color.White);
            }

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


            MouseState mouseState = Mouse.GetState();


            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (oldStateMouse.LeftButton != ButtonState.Pressed)
                {
                    mFade = new Fade(this, "fades\\blackfade", Fade.SPEED.SLOW);
                    mClicked = true;                    
                    executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                }
            }

            oldStateMouse = mouseState;
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
                SoundManager.StopMusic();
                if (mCurrentMacroMapState != MacroMapState.Finish)
                {
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_GAMEPLAY, true, true);
                }
                else
                {
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_ENDING_SCREEN, true, true);
                }
            }

        }

    }
}
