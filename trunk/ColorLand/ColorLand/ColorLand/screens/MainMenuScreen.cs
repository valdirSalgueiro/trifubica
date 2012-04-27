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
    class MainMenuScreen : BaseScreen
    {

        //public static bool sSHOW_SPLASH_SCREEN = false;

        private const String cSOUND_HIGHLIGHT = "sound\\fx\\highlight8bit";
        private SpriteBatch mSpriteBatch;

        private MouseState oldStateMouse;

        //SPLASH
        /*private Texture2D mGamelogo;
        private Texture2D mTextureClickToStart;
        private bool mShowTextClickToStart;
        private float mAlpha = 0f;
        private MTimer mTimerSplash;
        private MTimer mTimerBlinkText;*/

        private KeyboardState oldstateKeyboard;

        //Lista dos backgrounds
        private Background mBackgroundImage;

        private Background mCurrentBackground;

        private List<Background> mList = new List<Background>();

        private const int cMAX_BG_COUNTER = 3;
        private int mBackgroundCounter;

        private Button mCurrentHighlightButton;
        private bool mMousePressing;

        private SoundIcon mSoundIcon;
        private bool mCollidingMouseWithSoundIcon;
        private bool mCollidingWithSomeButton;

        private bool mShowContinueScreen;

        //fade
        private Fade mFade;
        private Fade mCurrentFade;

        private FADE_PARAM mFadeParam;

        public enum FADE_PARAM
        {
            START_GAME,
            CONTINUE_GAME,
            EXIT_GAME
        }


        /***
         * BUTTONS
         * */    
        private Button mButtonPlay;
        private Button mButtonHelp;
        private Button mButtonCredits;
        private Button mButtonFullscreen;
        private Button mButtonExit;

        private Button mButtonContinue;
        private Button mButtonNewGame;

        private GameObjectsGroup<Button> mGroupButtons;

        public MainMenuScreen()
        {
            if (!SoundManager.isPlaying())
            {
                //SoundManager.PlayMusic("sound\\music\\theme");
            }
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundImage = new Background("mainmenu\\MainMenu_cehu");
            mBackgroundImage.addPart(new String[1] { "mainmenu\\MainMenu_barco" }, 1, 125, 110, 80, 600 - 211);
            mBackgroundImage.addPart(new String[1] { "mainmenu\\MainMenu_areia" }, 1, 801, 201, 0, 600 - 201);
            mBackgroundImage.addPart(new String[1] { "mainmenu\\MainMenu_mato01" }, 1, 232, 111, 0, 600 - 111);
            mBackgroundImage.addPart(new String[1] { "mainmenu\\MainMenu_mato02" }, 1, 345, 135, 800 - 340, 600 - 135);
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

            //SPLASH
            //mGamelogo = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\logo");
            //mTextureClickToStart = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\clicktostart");

            ProgressObject p = ExtraFunctions.loadProgress();
            Game1.print("INFORMACAO: P--->STAGE: " + p.getCurrentStage());
            Game1.print("INFORMACAO: P--->COLOR: " + p.getColor());
            
            mList.Add(mBackgroundImage);

            mCurrentBackground = mList.ElementAt(0);
            
            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

            mButtonContinue = new Button("mainmenu\\buttons\\adventure_continue", "mainmenu\\buttons\\adventure_continue_select", "mainmenu\\buttons\\adventure_continue_selected", new Rectangle(282, 145, 286, 113));
            mButtonNewGame = new Button("mainmenu\\buttons\\adventure_new", "mainmenu\\buttons\\adventure_new_select", "mainmenu\\buttons\\adventure_new_selected", new Rectangle(282, 250, 286, 113));

            mButtonContinue.loadContent(Game1.getInstance().getScreenManager().getContent());
            mButtonNewGame.loadContent(Game1.getInstance().getScreenManager().getContent());

            
            mButtonPlay     = new Button("mainmenu\\buttons\\mainmenu_play", "mainmenu\\buttons\\mainmenu_play_select", "mainmenu\\buttons\\mainmenu_play_selected", new Rectangle(275, 155, 286, 103));
            mButtonHelp     = new Button("mainmenu\\buttons\\mainmenu_help", "mainmenu\\buttons\\mainmenu_help_select", "mainmenu\\buttons\\mainmenu_help_selected", new Rectangle(304, 255, 225, 93));
            mButtonCredits  = new Button("mainmenu\\buttons\\mainmenu_credits", "mainmenu\\buttons\\mainmenu_credits_select", "mainmenu\\buttons\\mainmenu_credits_selected", new Rectangle(/*800-45*/ 10, 550, 38, 39));
            mButtonExit = new Button("mainmenu\\buttons\\exit", "mainmenu\\buttons\\exit_select", "mainmenu\\buttons\\exit_selected", new Rectangle(304, 355, 225, 93));

            mButtonFullscreen = new Button("mainmenu\\buttons\\full", "mainmenu\\buttons\\full_select", "mainmenu\\buttons\\full_selected", new Rectangle(650, 530, 64, 47));
         
            mGroupButtons = new GameObjectsGroup<Button>();
            //mGroupButtons.addGameObject(mButtonContinue);
            mGroupButtons.addGameObject(mButtonPlay);
            mGroupButtons.addGameObject(mButtonHelp);
            mGroupButtons.addGameObject(mButtonCredits);
            mGroupButtons.addGameObject(mButtonExit);
            mGroupButtons.addGameObject(mButtonFullscreen);

            mGroupButtons.addGameObject(mButtonContinue);
            mGroupButtons.addGameObject(mButtonNewGame);
            
            mGroupButtons.loadContent(Game1.getInstance().getScreenManager().getContent());

            mSoundIcon = new SoundIcon(new Vector2(710, 514));
            mSoundIcon.loadContent(Game1.getInstance().getScreenManager().getContent());
                        

            mFade = new Fade(this, "fades\\blackfade", Fade.SPEED.FAST);

            executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);

            //mButtonPlay.loadContent(Game1.getInstance().getScreenManager().getContent());
            //mButtonHelp.loadContent(Game1.getInstance().getScreenManager().getContent());
            //mButtonCredits.loadContent(Game1.getInstance().getScreenManager().getContent());            

            //Game1.print("LOC: "  + mGroupButtons.getGameObject(2).getLocation());

            mCursor.backToMenuCursor();

            SoundManager.LoadSound(cSOUND_HIGHLIGHT);

        }


        public override void update(GameTime gameTime)
        {
            //checkCollisions();
            mCurrentBackground.update();

            mGroupButtons.update(gameTime);
            
            mSoundIcon.update(gameTime);
            mCursor.update(gameTime);
            updateMouseInput();
            checkCollisions();

            if (mFade != null)
            {
                mFade.update(gameTime);
            }

            //mBackgroundImage.getPart(0).addX(0.05f);
            mBackgroundImage.getPart(0).addX(gameTime.ElapsedGameTime.Milliseconds * 0.005f);
            
                       
        }


        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            mCurrentBackground.draw(mSpriteBatch);

            if (mShowContinueScreen)
            {
                mButtonContinue.setVisible(true); mButtonContinue.enableCollision(true);
                mButtonNewGame.setVisible(true);  mButtonNewGame.enableCollision(true);

                mButtonPlay.setVisible(false);    mButtonPlay.enableCollision(false);
                mButtonHelp.setVisible(false);    mButtonHelp.enableCollision(false);
                mButtonCredits.setVisible(false); mButtonCredits.enableCollision(false);
                mButtonExit.setVisible(false);    mButtonExit.enableCollision(false);
            }
            else
            {
                mButtonContinue.setVisible(false); mButtonContinue.enableCollision(false);
                mButtonNewGame.setVisible(false);  mButtonNewGame.enableCollision(false);

                mButtonPlay.setVisible(true);    mButtonPlay.enableCollision(true);
                mButtonHelp.setVisible(true);    mButtonHelp.enableCollision(true);
                mButtonCredits.setVisible(true); mButtonCredits.enableCollision(true);
                mButtonExit.setVisible(true);    mButtonExit.enableCollision(true);
            }
            
            mGroupButtons.draw(mSpriteBatch);
            mSoundIcon.draw(mSpriteBatch);                
            
            mCursor.draw(mSpriteBatch);

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


            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (oldStateMouse.LeftButton != ButtonState.Pressed)
                {
                    if (mCollidingMouseWithSoundIcon)
                    {
                        if (mSoundIcon.getState() == SoundIcon.sSTATE_SHAKING)
                        {
                            mSoundIcon.changeState(SoundIcon.sSTATE_OFF);
                            SoundManager.setSound(false);
                        }else
                        if (mSoundIcon.getState() == SoundIcon.sSTATE_OFF)
                        {
                            mSoundIcon.changeState(SoundIcon.sSTATE_NORMAL);
                            SoundManager.setSound(true);
                        }
                    }
                }
            }
            
            oldStateMouse = ms;

        }

        public override void handleInput(InputState input)
        {
            base.handleInput(input);
                        
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Escape))
            {

                if (newState.IsKeyDown(Keys.Escape))
                {
                    if (!oldstateKeyboard.IsKeyDown(Keys.Escape))
                    {
                        if (mShowContinueScreen)
                        {
                            //mShowContinueScreen = false;
                            configureToContinueScreen(false);
                        }
                    }
                }

                
            }

            oldstateKeyboard = newState;
        }//sabe uma notificação que fizeram ontem. Brinco formal demais


        private void configureToContinueScreen(bool isContinueScreen)
        {
            mShowContinueScreen = isContinueScreen;
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

            if (mCursor.collidesWith(mSoundIcon) && !mCollidingWithSomeButton)
            {
                if (mSoundIcon.getState() == SoundIcon.sSTATE_NORMAL)
                {
                    mSoundIcon.changeState(SoundIcon.sSTATE_SHAKING);
                }

                mCollidingMouseWithSoundIcon = true;
            }
            else
            {
                if (mSoundIcon.getState() == SoundIcon.sSTATE_SHAKING)
                {
                    mSoundIcon.changeState(SoundIcon.sSTATE_NORMAL);
                }
                mCollidingMouseWithSoundIcon = false;
            }

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

                mCollidingWithSomeButton = true;

            }else{
                mCollidingWithSomeButton = false;

                if (mCurrentHighlightButton != null)// && mCurrentHighlightButton.getState() != Button.sSTATE_PRESSED)
                {
                    mCurrentHighlightButton.changeState(Button.sSTATE_NORMAL);
                }
                mCurrentHighlightButton = null;
            }

        }

        private void processButtonAction(Button button)
        {
           
            if (button == mButtonPlay)
            {
                SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                mFade = new Fade(this, "fades\\blackfade");
                //
                //TODO if.....
                //ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, new ProgressObject(1, ProgressObject.PlayerColor.GREEN));

                Game1.progressObject = ExtraFunctions.loadProgress();

                if (Game1.progressObject != null)
                {
                    if (Game1.progressObject.getCurrentStage() > 1)
                    {
                        configureToContinueScreen(true);
                    }
                    else
                    {
                        SoundManager.stopMusic();

                        ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setCurrentStage(1));
                        executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                        mFadeParam = FADE_PARAM.START_GAME;
                    }

                }

                /*
                  */              
            }else
            if (button == mButtonContinue)
            {
                //SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                //mFade = new Fade(this, "fades\\blackfade");
                //executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                SoundManager.stopMusic();
                Game1.print("<<CONTINUE BUTTON>>");
                executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                mFadeParam = FADE_PARAM.CONTINUE_GAME;

            }else
            if (button == mButtonNewGame)
            {
                //SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                //mFade = new Fade(this, "fades\\blackfade");
                //executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                Game1.print("<<NEW GAME BUTTON>>");
                SoundManager.stopMusic();

                ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setCurrentStage(1));
                executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                mFadeParam = FADE_PARAM.START_GAME;

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
            else if (button == mButtonExit)
            {
                executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                mFadeParam = FADE_PARAM.EXIT_GAME;
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
                //SoundManager.stopMusic();
                if (mFadeParam == FADE_PARAM.START_GAME)
                {
                    //Game1.progressObject.setCurrentStage(1);
                    //ExtraFunctions.saveProgress(Game1.progressObject);
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_HISTORY, false);
                }
                if (mFadeParam == FADE_PARAM.CONTINUE_GAME)
                {
                    ExtraFunctions.saveProgress(Game1.progressObject);
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MACROMAP, false);
                }
                if (mFadeParam == FADE_PARAM.EXIT_GAME)
                {
                    Game1.getInstance().Exit();
                }
            }

        }

        
    }
}
