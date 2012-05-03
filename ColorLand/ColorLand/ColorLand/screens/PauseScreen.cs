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
    class PauseScreen : BaseScreen
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

        private Texture2D mPauseTitleTexture;
        private Texture2D mPauseBackgroundTexture;

        //fade
        private Fade mFade;
        private Fade mCurrentFade;


        /***
         * BUTTONS
         * */
        private Button mButtonContinue;
        private Button mButtonHelp;
        private Button mButtonExit;

        private GamePlayScreen mOwner;

        private GameObjectsGroup<Button> mGroupButtons;

        public bool bHelpScreen;
        private HelpScreen helpScreen;

        public PauseScreen(GamePlayScreen owner)
        {

            mOwner = owner;

            if (!SoundManager.isPlaying())
            {
                SoundManager.PlayMusic("sound\\music\\theme");
            }
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundImage = new Background("mainmenu\\help\\bg_listras");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

            mList.Add(mBackgroundImage);

            mCurrentBackground = mList.ElementAt(0);

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

            mButtonContinue = new Button("mainmenu\\buttons\\adventure_continue", "mainmenu\\buttons\\adventure_continue_select", "mainmenu\\buttons\\adventure_continue_selected", new Rectangle(400 - 286 / 2, 300 - 103 / 2 - 50, 286, 103));
            mButtonHelp = new Button("mainmenu\\buttons\\mainmenu_help", "mainmenu\\buttons\\mainmenu_help_select", "mainmenu\\buttons\\mainmenu_help_selected", new Rectangle(400 - 225 / 2, 300 - 103 / 2 + 50, 225, 103));
            mButtonExit = new Button("mainmenu\\buttons\\exit", "mainmenu\\buttons\\exit_select", "mainmenu\\buttons\\exit_selected", new Rectangle(400 - 225 / 2, 300 - 103 / 2 + 150, 225, 103));            

            mPauseTitleTexture = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\paused_title_03");
            mPauseBackgroundTexture = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\help\\bg_listras");

            mGroupButtons = new GameObjectsGroup<Button>();
            //mGroupButtons.addGameObject(mButtonContinue);
            mGroupButtons.addGameObject(mButtonContinue);
            mGroupButtons.addGameObject(mButtonExit);
            mGroupButtons.addGameObject(mButtonHelp);
            
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

            if (!bHelpScreen)
            {
                mGroupButtons.update(gameTime);
                mCursor.update(gameTime);
                updateMouseInput();
                checkCollisions();

                if (mFade != null)
                {
                    //mFade.update(gameTime);
                }
            }
            else {
                helpScreen.update(gameTime);
            }
        }


        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            //mCurrentBackground.draw(mSpriteBatch);

            mSpriteBatch.Draw(mPauseBackgroundTexture, new Rectangle(0, 0, 800, 600), new Color(0, 0, 0, 0.5f));

            if (!bHelpScreen)
            {

                mSpriteBatch.Draw(mPauseTitleTexture, new Rectangle(400 - 182 / 2, 100, 182, 43), Color.White);

                mGroupButtons.draw(mSpriteBatch);
                mCursor.draw(mSpriteBatch);

                /*if (mFade != null)
                {
                    mFade.draw(mSpriteBatch);
                }*/
            }

            mSpriteBatch.End();

            if (bHelpScreen)
            {
                helpScreen.draw(gameTime);
            }

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

            if (mGroupButtons.checkCollisionWith(mCursor))
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
                //executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                mOwner.setPauseGame(false);

            }
            if (button == mButtonExit)
            {
                //SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                //mFade = new Fade(this, "fades\\blackfade");
                //executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                SoundManager.StopMusic();
                Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, true,true);
            }

            if (button == mButtonHelp)
            {
                SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                helpScreen = new HelpScreen(this);
                bHelpScreen = true;
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
