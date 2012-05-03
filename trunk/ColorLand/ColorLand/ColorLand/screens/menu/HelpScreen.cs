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
    class HelpScreen : BaseScreen
    {

        private const String cSOUND_HIGHLIGHT = "sound\\fx\\highlight8bit";
        private SpriteBatch mSpriteBatch;

        //Lista dos backgrounds
        private Background mBackgroundImage;

        private Background mCurrentBackground;

        private List<Background> mList = new List<Background>();

        private int currentScreen=0;

        private bool mMousePressing;

        private KeyboardState oldState;

        private Fade mFade;
        private Fade mCurrentFade;

        /***
         * BUTTONS
         * */
        private Button mButtonBack;
        private Button mButtonNext;
        private Button mButtonPrevious;

        //pog
        private Texture2D mNext;
        private Texture2D mPrevious;
        //pog

        Button mCurrentHighlightButton;


        private GameObjectsGroup<Button> mGroupButtons;

        MainMenuScreen owner;
        PauseScreen owner2;

        private bool mFromMainMenu;

        Background mBackground;

        public HelpScreen(PauseScreen owner_)
        {
            mFromMainMenu = false;
            owner2 = owner_;
            init();
        }

        public HelpScreen(MainMenuScreen owner_)
        {
            mFromMainMenu = true;
            owner = owner_;
            init();

        }

        private void init()
        {
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackground = new Background("mainmenu\\help\\bg_listras");
            mBackground.loadContent(Game1.getInstance().getScreenManager().getContent());

            mBackgroundImage = new Background("mainmenu\\help\\01_objective");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());
            mBackgroundImage.setLocation(0, 50);
            mList.Add(mBackgroundImage);
            mBackgroundImage = new Background("mainmenu\\help\\02_movement");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());
            mBackgroundImage.setLocation(0, 50);
            mList.Add(mBackgroundImage);
            mBackgroundImage = new Background("mainmenu\\help\\03_payattention");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());
            mBackgroundImage.setLocation(0, 50);
            mList.Add(mBackgroundImage);
            mBackgroundImage = new Background("mainmenu\\help\\04_mouseclicks");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());
            mBackgroundImage.setLocation(0, 50);
            mList.Add(mBackgroundImage);
            mBackgroundImage = new Background("mainmenu\\help\\05_items");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());
            mBackgroundImage.setLocation(0, 50);
            mList.Add(mBackgroundImage);

            mCurrentBackground = mList.ElementAt(0);

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

            mButtonBack = new Button("mainmenu\\help\\Menu_back", "mainmenu\\help\\Menu_back_select", "mainmenu\\help\\Menu_back_selected", new Rectangle(45, 450, 178, 128));
            mButtonPrevious = new Button("mainmenu\\help\\previous", "mainmenu\\help\\previous_select", "mainmenu\\help\\previous_selected", new Rectangle(350, 474, 80, 86));
            mButtonNext = new Button("mainmenu\\help\\next", "mainmenu\\help\\next_select", "mainmenu\\help\\next_selected", new Rectangle(586, 474, 80, 86));

            mGroupButtons = new GameObjectsGroup<Button>();
            mGroupButtons.addGameObject(mButtonBack);
            mGroupButtons.addGameObject(mButtonPrevious);
            mGroupButtons.addGameObject(mButtonNext);
            mGroupButtons.loadContent(Game1.getInstance().getScreenManager().getContent());

            mNext = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\help\\next_disabled");
            mPrevious = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\help\\previous_disabled");
            //mMenu = new MenuGrade();

            SoundManager.LoadSound(cSOUND_HIGHLIGHT);

            mFade = new Fade(this, "fades\\blackfade", Fade.SPEED.ULTRAFAST);

            //executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);
        }


        public override void update(GameTime gameTime)
        {
            mCurrentBackground.update();
            mGroupButtons.update(gameTime);
            mCursor.update(gameTime);
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

            mBackground.draw(mSpriteBatch);

            mCurrentBackground.draw(mSpriteBatch);

            mGroupButtons.draw(mSpriteBatch);
            if (currentScreen == 4)
            {
                mSpriteBatch.Draw(mNext, new Rectangle(586, 474, 80, 86), Color.White);
            }
            if (currentScreen == 0)
            {
                mSpriteBatch.Draw(mPrevious, new Rectangle(350, 474, 80, 86), Color.White);
            }
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

            //dispara evento

        }

        public override void handleInput(InputState input)
        {
            base.handleInput(input);

            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Escape))
            {
                if (!oldState.IsKeyDown(Keys.Escape))
                {
                    SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU,false);
                }
            }else
            if (newState.IsKeyDown(Keys.Left))
            {
                if (!oldState.IsKeyDown(Keys.Left))
                {
                    previousPage();
                }
            }else
            if (newState.IsKeyDown(Keys.Right))
            {
                if (!oldState.IsKeyDown(Keys.Right))
                {
                    nextPage();
                }
            }

            oldState = newState;
        }

        private void checkCollisions()
        {

            if (mGroupButtons.checkCollisionWith(mCursor))
            {
                mCurrentHighlightButton = (Button)mGroupButtons.getCollidedObject();

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

        private void nextPage()
        {
            if (currentScreen == 4)
                return;
            else
                currentScreen++;
            SoundManager.PlaySound(cSOUND_HIGHLIGHT);
            mCurrentBackground = mList.ElementAt(currentScreen);
        }

        private void previousPage()
        {
            if (currentScreen == 0)
                return;
            else
                currentScreen--;
            SoundManager.PlaySound(cSOUND_HIGHLIGHT);
            mCurrentBackground = mList.ElementAt(currentScreen);
        }



        private void processButtonAction(Button button)
        {
            if (button == mButtonBack)
            {
                SoundManager.PlaySound(cSOUND_HIGHLIGHT);

                if (mFromMainMenu)
                {
                    owner.cSCREEN = MainMenuScreen.SCREENS.MAINMENU_SCREEN;
                }
                else
                {
                    if (owner != null)
                    {
                        owner.cSCREEN = MainMenuScreen.SCREENS.MAINMENU_SCREEN;
                    }
                    else if (owner2 != null)
                    {
                        owner2.bHelpScreen = false;
                    }
                    //executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                }
            }

            if (button == mButtonNext)
            {
                nextPage();
            }

            if (button == mButtonPrevious)
            {
                previousPage();
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
                if (owner != null)
                {
                    owner.cSCREEN = MainMenuScreen.SCREENS.MAINMENU_SCREEN;
                }
                else if (owner2 != null) {
                    owner2.bHelpScreen = false;
                }
            }

        }
 
    }
}
