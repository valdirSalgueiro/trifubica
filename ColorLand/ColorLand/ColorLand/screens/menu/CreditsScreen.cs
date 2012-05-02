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
    class CreditsScreen : BaseScreen
    {

        private const String cSOUND_HIGHLIGHT = "sound\\fx\\highlight8bit";
        private SpriteBatch mSpriteBatch;

        //Lista dos backgrounds
        private Background mBackgroundImage;

        private Background mCurrentBackground;

        private List<Background> mList = new List<Background>();

        private bool mMousePressing;

        private KeyboardState oldState;

        /***
         * BUTTONS
         * */
        private Button mButtonBack;
        Button mCurrentHighlightButton;

        private Fade mFade;
        private Fade mCurrentFade;

        MainMenuScreen owner;

        public CreditsScreen(MainMenuScreen owner_)
        {
            owner = owner_;

            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundImage = new Background("mainmenu\\help\\Menu_credits");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());
            mBackgroundImage.setLocation(0, 50);

            mList.Add(mBackgroundImage);

            mCurrentBackground = mList.ElementAt(0);

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

            mButtonBack = new Button("mainmenu\\buttons\\menu_credits_help_back", "mainmenu\\buttons\\menu_credits_help_back_select", "mainmenu\\buttons\\menu_credits_help_back_selected", new Rectangle(50, 464, 175, 124));

            mButtonBack.loadContent(Game1.getInstance().getScreenManager().getContent());

            mFade = new Fade(this, "fades\\blackfade", Fade.SPEED.ULTRAFAST);

            executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);

            //mMenu = new MenuGrade();
            SoundManager.LoadSound(cSOUND_HIGHLIGHT);

        } 


        public override void update(GameTime gameTime)
        {
            mCurrentBackground.update();
            mButtonBack.update(gameTime);
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
            mCurrentBackground.draw(mSpriteBatch);
            mButtonBack.draw(mSpriteBatch);
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

            if (newState.IsKeyDown(Keys.Escape))
            {
                if (!oldState.IsKeyDown(Keys.Escape))
                {
                    SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                    Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, false);
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

            if (mCursor.collidesWith(mButtonBack))
            {
                mCurrentHighlightButton = mButtonBack;

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
            if (button == mButtonBack)
            {
                SoundManager.PlaySound(cSOUND_HIGHLIGHT);
                //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU,false);
                executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
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
            if (fadeObject.getEffect() == Fade.sFADE_OUT_EFFECT_GRADATIVE)
            {
                //SoundManager.stopMusic();
                //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_HISTORY, true);
                //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, false);
                owner.cSCREEN = MainMenuScreen.SCREENS.MAINMENU_SCREEN;
            }

        }
 
    }
}
