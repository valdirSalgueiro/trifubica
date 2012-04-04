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


        public HelpScreen()
        {
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundImage = new Background("mainmenu\\instruction_1");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());
            mList.Add(mBackgroundImage);
            mBackgroundImage = new Background("mainmenu\\instruction_2");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());
            mList.Add(mBackgroundImage);
            mBackgroundImage = new Background("mainmenu\\instruction_3");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());
            mList.Add(mBackgroundImage);
            mBackgroundImage = new Background("mainmenu\\instruction_4");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());
            mList.Add(mBackgroundImage);

            mCurrentBackground = mList.ElementAt(0);

            Cursor.getInstance().loadContent(Game1.getInstance().getScreenManager().getContent());

            mButtonBack = new Button("mainmenu\\buttons\\menu_credits_help_back", "mainmenu\\buttons\\menu_credits_help_back_select", "mainmenu\\buttons\\menu_credits_help_back_selected", new Rectangle(50, 464, 175, 124));
            mButtonPrevious = new Button("mainmenu\\buttons\\previous", "mainmenu\\buttons\\previous_select", "mainmenu\\buttons\\previous_selected", new Rectangle(350, 484, 80, 86));
            mButtonNext = new Button("mainmenu\\buttons\\next", "mainmenu\\buttons\\next_select", "mainmenu\\buttons\\next_selected", new Rectangle(586, 484, 80, 86));

            mGroupButtons = new GameObjectsGroup<Button>();
            mGroupButtons.addGameObject(mButtonBack);
            mGroupButtons.addGameObject(mButtonPrevious);
            mGroupButtons.addGameObject(mButtonNext);
            mGroupButtons.loadContent(Game1.getInstance().getScreenManager().getContent());

            mNext = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\buttons\\next_disabled");
            mPrevious = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\buttons\\previous_disabled");
            //mMenu = new MenuGrade();

            SoundManager.LoadSound(cSOUND_HIGHLIGHT);

        }


        public override void update(GameTime gameTime)
        {
            mCurrentBackground.update();
            mGroupButtons.update(gameTime);
            Cursor.getInstance().update(gameTime);
            updateMouseInput();
            checkCollisions();
        }

        public override void draw(GameTime gameTime)
        {

            mSpriteBatch.Begin();

            mCurrentBackground.draw(mSpriteBatch);

            mGroupButtons.draw(mSpriteBatch);
            if (currentScreen == 3)
            {
                mSpriteBatch.Draw(mNext, new Rectangle(586, 484, 80, 86), Color.White);
            }
            if (currentScreen == 0)
            {
                mSpriteBatch.Draw(mPrevious, new Rectangle(350, 484, 80, 86), Color.White);
            }
            Cursor.getInstance().draw(mSpriteBatch);

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

            if (mGroupButtons.checkCollisionWith(Cursor.getInstance()))
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
            if (currentScreen == 3)
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
                Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU,false);
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
 
    }
}