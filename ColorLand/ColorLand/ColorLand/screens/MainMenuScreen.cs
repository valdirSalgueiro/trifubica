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
    class MainMenuScreen : BaseScreen
    {
        private const String cSOUND_HIGHLIGHT = "sound\\fx\\highlight8bit";
        private SpriteBatch mSpriteBatch;

        //Lista dos backgrounds
        private Background mBackgroundImage;

        private Background mCurrentBackground;

        private List<Background> mList = new List<Background>();

        private const int cMAX_BG_COUNTER = 3;
        private int mBackgroundCounter;

        private Button mCurrentHighlightButton;
        private bool mMousePressing;

        //fade
        private Fade mFade;
        private Fade mCurrentFade;


        /***
         * BUTTONS
         * */
        private Button mButtonContinue;
        private Button mButtonPlay;
        private Button mButtonHelp;
        private Button mButtonCredits;
        private Button mButtonFullscreen;
        private Button mButtonSound;

        private GameObjectsGroup<Button> mGroupButtons;

        public MainMenuScreen()
        {
            if (!SoundManager.isPlaying())
            {
                SoundManager.PlayMusic("sound\\music\\theme");
            }
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundImage = new Background("mainmenu\\mainmenubg");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

            mList.Add(mBackgroundImage);

            mCurrentBackground = mList.ElementAt(0);

            Cursor.getInstance().loadContent(Game1.getInstance().getScreenManager().getContent());

            mButtonContinue = new Button("mainmenu\\buttons\\mainmenu_play", "mainmenu\\buttons\\mainmenu_play_select", "mainmenu\\buttons\\mainmenu_play_selected", new Rectangle(82, 120, 295, 105));
            mButtonPlay    = new Button("mainmenu\\buttons\\mainmenu_play", "mainmenu\\buttons\\mainmenu_play_select","mainmenu\\buttons\\mainmenu_play_selected",new Rectangle(82, 190, 295, 105));
            mButtonHelp    = new Button("mainmenu\\buttons\\mainmenu_help", "mainmenu\\buttons\\mainmenu_help_select", "mainmenu\\buttons\\mainmenu_help_selected", new Rectangle(82, 295, 229, 103));
            mButtonCredits = new Button("mainmenu\\buttons\\mainmenu_credits", "mainmenu\\buttons\\mainmenu_credits_select", "mainmenu\\buttons\\mainmenu_credits_selected", new Rectangle(82, 400, 229, 103));

            mButtonFullscreen = new Button("mainmenu\\buttons\\full", "mainmenu\\buttons\\full_select", "mainmenu\\buttons\\full_selected", new Rectangle(580, 530, 90, 60));
            mButtonSound = new Button("mainmenu\\buttons\\full", "mainmenu\\buttons\\full_select", "mainmenu\\buttons\\full_selected", new Rectangle(700, 530, 90, 60));

            mGroupButtons = new GameObjectsGroup<Button>();
            //mGroupButtons.addGameObject(mButtonContinue);
            mGroupButtons.addGameObject(mButtonPlay);
            mGroupButtons.addGameObject(mButtonHelp);
            mGroupButtons.addGameObject(mButtonCredits);
            mGroupButtons.addGameObject(mButtonFullscreen);
            mGroupButtons.addGameObject(mButtonSound);

            mGroupButtons.loadContent(Game1.getInstance().getScreenManager().getContent());

            //mButtonPlay.loadContent(Game1.getInstance().getScreenManager().getContent());
            //mButtonHelp.loadContent(Game1.getInstance().getScreenManager().getContent());
            //mButtonCredits.loadContent(Game1.getInstance().getScreenManager().getContent());            

            //Game1.print("LOC: "  + mGroupButtons.getGameObject(2).getLocation());

            SoundManager.LoadSound(cSOUND_HIGHLIGHT);

        }


        public override void update(GameTime gameTime)
        {
            //checkCollisions();
            mCurrentBackground.update();
            mGroupButtons.update(gameTime);
            Cursor.getInstance().update(gameTime);
            updateMouseInput();
            checkCollisions();

            if (mFade != null)
            {
                mFade.update(gameTime);
            }
        }


        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            mCurrentBackground.draw(mSpriteBatch);

            mGroupButtons.draw(mSpriteBatch);
            Cursor.getInstance().draw(mSpriteBatch);

            if (mFade != null)
            {
                mFade.draw(mSpriteBatch);
            }

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


        }

        //hehehehehe
        private void solveHighlightBug()
        {
            if (mCurrentHighlightButton != null)
            {
                for (int x = 0; x < mGroupButtons.getSize(); x++ )
                {

                    Button b = mGroupButtons.getGameObject(x);

                    if(b != mCurrentHighlightButton){

                        b.changeState(Button.sSTATE_NORMAL);

                    }

                }

            }
        }

        private void checkCollisions()
        {

            if (mGroupButtons.checkCollisionWith(Cursor.getInstance()))
            {
                mCurrentHighlightButton = (Button)mGroupButtons.getCollidedObject();

                solveHighlightBug();

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
                
            }else{
                if (mCurrentHighlightButton != null)// && mCurrentHighlightButton.getState() != Button.sSTATE_PRESSED)
                {
                    mCurrentHighlightButton.changeState(Button.sSTATE_NORMAL);
                }
                mCurrentHighlightButton = null;
            }

        }

        private void processButtonAction(Button button)
        {
            if (button == mButtonContinue)
            {
                //SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                //mFade = new Fade(this, "fades\\blackfade");
                executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);

            }
            if (button == mButtonPlay)
            {
                SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                mFade = new Fade(this, "fades\\blackfade");

                executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);

                Game1.print("Salvei fase 1");
                ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, new ProgressObject(1));

            }
            else if (button == mButtonHelp)
            {
                SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU_HELP, false);
            }
            else if (button == mButtonCredits)
            {
                SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU_CREDITS, false);
            }
            else if (button == mButtonFullscreen)
            {
                SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                Game1.getInstance().toggleFullscreen();
            }
            else if (button == mButtonSound)
            {
                SoundManager.toggleSound();
            }
        }

        public override void executeFade(Fade fadeObject, int effect)
        {
            base.executeFade(fadeObject, effect);

            mCurrentFade = fadeObject;
            fadeObject.execute(effect);
        }

         public override void fadeFinished(Fade fadeObject)
        {
            //if(fadeObject.getEffect() == Fade.sFADE_IN_EFFECT_GRADATIVE){
            //}else
            if (fadeObject.getEffect() == Fade.sFADE_OUT_EFFECT_GRADATIVE)
            {
                SoundManager.stopMusic();
                Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_HISTORY, true);
            }

        }

        //timer
        /*private void restartTimer()
        {
            mTimerFade = new MTimer();
            mTimerFade.start();
        }
        
      
        private void updateTimer(GameTime gameTime)
        {
            if (mTimerFade != null)
            {

                mTimerFade.update(gameTime);

                if (mTimerFade.getTimeAndLock(3))
                {

                }

            }
        }*/


    }
}
