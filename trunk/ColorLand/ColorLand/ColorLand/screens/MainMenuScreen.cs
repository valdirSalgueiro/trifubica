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

        public BaseScreen currentScreen;


        public enum SCREENS
        {
            MAINMENU_SCREEN,
            HELP_SCREEN,
            CREDITS_SCREEN
        }

        public SCREENS cSCREEN;

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
            ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setStageAndColor(1, ProgressObject.PlayerColor.BLUE));

            if (!SoundManager.isPlaying())
            {
                SoundManager.PlayMusic("sound\\music\\theme");
            }
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundImage = new Background("mainmenu\\MainMenu_bg");
            mBackgroundImage.addPart(new String[1] { "mainmenu\\MainMenu_barco" }, 1, 125, 110, 80, 600 - 211);
            mBackgroundImage.addPart(new String[1] { "mainmenu\\MainMenu_areia" }, 1, 801, 201, 0, 600 - 201);

            mBackgroundImage.addPart(new String[36] { "mainmenu\\Mato01\\MainMenu_mato01_00000",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00001",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00002",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00003",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00004",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00005",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00006",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00007",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00008",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00009",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00010",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00011",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00012",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00013",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00014",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00015",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00016",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00017",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00018",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00019",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00020",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00021",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00022",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00023",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00024",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00025",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00026",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00027",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00028",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00029",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00030",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00031",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00032",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00033",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00034",
                                                     "mainmenu\\Mato01\\MainMenu_mato01_00035",
            }, 1, 232, 111, 0, 600 - 111);

            mBackgroundImage.addPart(new String[36] { "mainmenu\\Mato02\\Mato02__00000",
                                                     "mainmenu\\Mato02\\Mato02__00001",
                                                     "mainmenu\\Mato02\\Mato02__00002",
                                                     "mainmenu\\Mato02\\Mato02__00003",
                                                     "mainmenu\\Mato02\\Mato02__00004",
                                                     "mainmenu\\Mato02\\Mato02__00005",
                                                     "mainmenu\\Mato02\\Mato02__00006",
                                                     "mainmenu\\Mato02\\Mato02__00007",
                                                     "mainmenu\\Mato02\\Mato02__00008",
                                                     "mainmenu\\Mato02\\Mato02__00009",
                                                     "mainmenu\\Mato02\\Mato02__00010",
                                                     "mainmenu\\Mato02\\Mato02__00011",
                                                     "mainmenu\\Mato02\\Mato02__00012",
                                                     "mainmenu\\Mato02\\Mato02__00013",
                                                     "mainmenu\\Mato02\\Mato02__00014",
                                                     "mainmenu\\Mato02\\Mato02__00015",
                                                     "mainmenu\\Mato02\\Mato02__00016",
                                                     "mainmenu\\Mato02\\Mato02__00017",
                                                     "mainmenu\\Mato02\\Mato02__00018",
                                                     "mainmenu\\Mato02\\Mato02__00019",
                                                     "mainmenu\\Mato02\\Mato02__00020",
                                                     "mainmenu\\Mato02\\Mato02__00021",
                                                     "mainmenu\\Mato02\\Mato02__00022",
                                                     "mainmenu\\Mato02\\Mato02__00023",
                                                     "mainmenu\\Mato02\\Mato02__00024",
                                                     "mainmenu\\Mato02\\Mato02__00025",
                                                     "mainmenu\\Mato02\\Mato02__00026",
                                                     "mainmenu\\Mato02\\Mato02__00027",
                                                     "mainmenu\\Mato02\\Mato02__00028",
                                                     "mainmenu\\Mato02\\Mato02__00029",
                                                     "mainmenu\\Mato02\\Mato02__00030",
                                                     "mainmenu\\Mato02\\Mato02__00031",
                                                     "mainmenu\\Mato02\\Mato02__00032",
                                                     "mainmenu\\Mato02\\Mato02__00033",
                                                     "mainmenu\\Mato02\\Mato02__00034",
                                                     "mainmenu\\Mato02\\Mato02__00035",
            }, 1, 223, 111, 800 - 223, 600 - 111);

            mBackgroundImage.addPart(new String[36] { "mainmenu\\Mato03\\Mato03_00000",
                                                     "mainmenu\\Mato03\\Mato03_00001",
                                                     "mainmenu\\Mato03\\Mato03_00002",
                                                     "mainmenu\\Mato03\\Mato03_00003",
                                                     "mainmenu\\Mato03\\Mato03_00004",
                                                     "mainmenu\\Mato03\\Mato03_00005",
                                                     "mainmenu\\Mato03\\Mato03_00006",
                                                     "mainmenu\\Mato03\\Mato03_00007",
                                                     "mainmenu\\Mato03\\Mato03_00008",
                                                     "mainmenu\\Mato03\\Mato03_00009",
                                                     "mainmenu\\Mato03\\Mato03_00010",
                                                     "mainmenu\\Mato03\\Mato03_00011",
                                                     "mainmenu\\Mato03\\Mato03_00012",
                                                     "mainmenu\\Mato03\\Mato03_00013",
                                                     "mainmenu\\Mato03\\Mato03_00014",
                                                     "mainmenu\\Mato03\\Mato03_00015",
                                                     "mainmenu\\Mato03\\Mato03_00016",
                                                     "mainmenu\\Mato03\\Mato03_00017",
                                                     "mainmenu\\Mato03\\Mato03_00018",
                                                     "mainmenu\\Mato03\\Mato03_00019",
                                                     "mainmenu\\Mato03\\Mato03_00020",
                                                     "mainmenu\\Mato03\\Mato03_00021",
                                                     "mainmenu\\Mato03\\Mato03_00022",
                                                     "mainmenu\\Mato03\\Mato03_00023",
                                                     "mainmenu\\Mato03\\Mato03_00024",
                                                     "mainmenu\\Mato03\\Mato03_00025",
                                                     "mainmenu\\Mato03\\Mato03_00026",
                                                     "mainmenu\\Mato03\\Mato03_00027",
                                                     "mainmenu\\Mato03\\Mato03_00028",
                                                     "mainmenu\\Mato03\\Mato03_00029",
                                                     "mainmenu\\Mato03\\Mato03_00030",
                                                     "mainmenu\\Mato03\\Mato03_00031",
                                                     "mainmenu\\Mato03\\Mato03_00032",
                                                     "mainmenu\\Mato03\\Mato03_00033",
                                                     "mainmenu\\Mato03\\Mato03_00034",
                                                     "mainmenu\\Mato03\\Mato03_00035",
            }, 1, 406, 144, 50, 0);

            //mBackgroundImage.addPart(new String[1] { "mainmenu\\MainMenu_mato01" }, 1, 232, 111, 0, 600 - 111);
            //mBackgroundImage.addPart(new String[1] { "mainmenu\\MainMenu_mato02" }, 1, 345, 135, 800 - 340, 600 - 135);
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

            //SPLASH
            //mGamelogo = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\logo");
            //mTextureClickToStart = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\clicktostart");

            mList.Add(mBackgroundImage);

            mCurrentBackground = mList.ElementAt(0);

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

            mButtonContinue = new Button("mainmenu\\buttons\\adventure_continue", "mainmenu\\buttons\\adventure_continue_select", "mainmenu\\buttons\\adventure_continue_selected", new Rectangle(282, 145, 286, 113));
            mButtonNewGame = new Button("mainmenu\\buttons\\adventure_new", "mainmenu\\buttons\\adventure_new_select", "mainmenu\\buttons\\adventure_new_selected", new Rectangle(282, 250, 286, 113));

            mButtonContinue.loadContent(Game1.getInstance().getScreenManager().getContent());
            mButtonNewGame.loadContent(Game1.getInstance().getScreenManager().getContent());


            mButtonPlay = new Button("mainmenu\\buttons\\mainmenu_play", "mainmenu\\buttons\\mainmenu_play_select", "mainmenu\\buttons\\mainmenu_play_selected", new Rectangle(275, 155, 286, 103));
            mButtonHelp = new Button("mainmenu\\buttons\\mainmenu_help", "mainmenu\\buttons\\mainmenu_help_select", "mainmenu\\buttons\\mainmenu_help_selected", new Rectangle(304, 255, 225, 93));
            mButtonCredits = new Button("mainmenu\\buttons\\mainmenu_credits", "mainmenu\\buttons\\mainmenu_credits_select", "mainmenu\\buttons\\mainmenu_credits_selected", new Rectangle(/*800-45*/ 10, 550, 38, 39));
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
            if (cSCREEN == MainMenuScreen.SCREENS.MAINMENU_SCREEN)
            {

                mGroupButtons.update(gameTime);

                mSoundIcon.update(gameTime);
                mCursor.update(gameTime);
                updateMouseInput();
                checkCollisions();

                if (mFade != null)
                {
                    mFade.update(gameTime);
                }
            }

            //mBackgroundImage.getPart(0).addX(0.05f);
            mBackgroundImage.getPart(0).addX(gameTime.ElapsedGameTime.Milliseconds * 0.005f);

            if (cSCREEN != MainMenuScreen.SCREENS.MAINMENU_SCREEN)
            {
                currentScreen.update(gameTime);
            }


        }


        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            mCurrentBackground.draw(mSpriteBatch);

            if (cSCREEN == MainMenuScreen.SCREENS.MAINMENU_SCREEN)
            {
                if (mShowContinueScreen)
                {
                    mButtonContinue.setVisible(true); mButtonContinue.enableCollision(true);
                    mButtonNewGame.setVisible(true); mButtonNewGame.enableCollision(true);

                    mButtonPlay.setVisible(false); mButtonPlay.enableCollision(false);
                    mButtonHelp.setVisible(false); mButtonHelp.enableCollision(false);
                    mButtonCredits.setVisible(false); mButtonCredits.enableCollision(false);
                    mButtonExit.setVisible(false); mButtonExit.enableCollision(false);
                }
                else
                {
                    mButtonContinue.setVisible(false); mButtonContinue.enableCollision(false);
                    mButtonNewGame.setVisible(false); mButtonNewGame.enableCollision(false);

                    mButtonPlay.setVisible(true); mButtonPlay.enableCollision(true);
                    mButtonHelp.setVisible(true); mButtonHelp.enableCollision(true);
                    mButtonCredits.setVisible(true); mButtonCredits.enableCollision(true);
                    mButtonExit.setVisible(true); mButtonExit.enableCollision(true);
                }

                mGroupButtons.draw(mSpriteBatch);
                mSoundIcon.draw(mSpriteBatch);

                mCursor.draw(mSpriteBatch);

                if (mFade != null)
                {
                    mFade.draw(mSpriteBatch);
                }
            }

            mSpriteBatch.End();

            if (cSCREEN != MainMenuScreen.SCREENS.MAINMENU_SCREEN)
            {
                currentScreen.draw(gameTime);
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
                        }
                        else
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

            ///HOT KEYS

            if (newState.IsKeyDown(Keys.F1))
            {
                if (!oldstateKeyboard.IsKeyDown(Keys.F1))
                {
                    SoundManager.StopMusic();
                    ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setStageAndColor(1, ProgressObject.PlayerColor.BLUE));
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MACROMAP, true,true);
                }
            }
            if (newState.IsKeyDown(Keys.F2))
            {
                if (!oldstateKeyboard.IsKeyDown(Keys.F2))
                {
                    SoundManager.StopMusic();
                    ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setStageAndColor(2, ProgressObject.PlayerColor.BLUE));
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MACROMAP, true, true);
                }
            }
            if (newState.IsKeyDown(Keys.F3))
            {
                if (!oldstateKeyboard.IsKeyDown(Keys.F3))
                {
                    SoundManager.StopMusic();
                    ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setStageAndColor(3, ProgressObject.PlayerColor.BLUE));
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MACROMAP, true, true);
                }
            }
            if (newState.IsKeyDown(Keys.F4))
            {
                if (!oldstateKeyboard.IsKeyDown(Keys.F4))
                {
                    SoundManager.StopMusic();
                    ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setStageAndColor(4, ProgressObject.PlayerColor.BLUE));
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MACROMAP, true, true);
                }
            }
            if (newState.IsKeyDown(Keys.F5))
            {
                if (!oldstateKeyboard.IsKeyDown(Keys.F5))
                {
                    SoundManager.StopMusic();
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_ENDING_SCREEN, true, true);
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
                for (int x = 0; x < mGroupButtons.getSize(); x++)
                {

                    Button b = mGroupButtons.getGameObject(x);

                    if (b != mCurrentHighlightButton)
                    {

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

            }
            else
            {
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
                        //SoundManager.stopMusic();

                        ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setStageAndColor(1, ProgressObject.PlayerColor.BLUE));
                        executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                        mFadeParam = FADE_PARAM.START_GAME;
                    }

                }

                /*
                  */
            }
            else
                if (button == mButtonContinue)
                {
                    //SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                    //mFade = new Fade(this, "fades\\blackfade");
                    //executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);

                    SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                    executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                    mFadeParam = FADE_PARAM.CONTINUE_GAME;

                }
                else
                    if (button == mButtonNewGame)
                    {
                        //SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                        //mFade = new Fade(this, "fades\\blackfade");
                        //executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                        SoundManager.PlaySound(cSOUND_HIGHLIGHT);

                        ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setStageAndColor(1,ProgressObject.PlayerColor.BLUE));
                        executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                        mFadeParam = FADE_PARAM.START_GAME;

                    }
                    else if (button == mButtonHelp)
                    {
                        currentScreen = new HelpScreen(this);
                        cSCREEN = SCREENS.HELP_SCREEN;
                        SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                        // Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU_HELP, false);
                    }
                    else if (button == mButtonCredits)
                    {
                        currentScreen = new CreditsScreen(this);
                        cSCREEN = SCREENS.CREDITS_SCREEN;
                        SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                        //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU_CREDITS, false);
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
                    SoundManager.stopMusic();
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_HISTORY, false);
                }
                if (mFadeParam == FADE_PARAM.CONTINUE_GAME)
                {
                    SoundManager.stopMusic();
                    //ExtraFunctions.saveProgress(Game1.progressObject);
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MACROMAP, false);
                }
                if (mFadeParam == FADE_PARAM.EXIT_GAME)
                {
                    SoundManager.stopMusic();
                    Game1.getInstance().Exit();
                }
            }

        }


    }
}
