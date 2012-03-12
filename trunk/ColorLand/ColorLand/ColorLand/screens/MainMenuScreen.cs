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

        private SpriteBatch mSpriteBatch;

        //Lista dos backgrounds
        private Background mBackgroundImage;

        private Background mCurrentBackground;

        private List<Background> mList = new List<Background>();

        private const int cMAX_BG_COUNTER = 3;
        private int mBackgroundCounter;

        private Cursor mCursor;
        private Button mCurrentHighlightButton;
        private bool mMousePressing;


        /***
         * BUTTONS
         * */
        private Button mButtonPlay;
        private Button mButtonHelp;
        private Button mButtonCredits;

        private GameObjectsGroup<Button> mGroupButtons;

        public MainMenuScreen()
        {
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundImage = new Background("mainmenu\\mainmenubg");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

            mList.Add(mBackgroundImage);

            mCurrentBackground = mList.ElementAt(0);

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

            mButtonPlay    = new Button("mainmenu\\buttons\\mainmenu_play", "mainmenu\\buttons\\mainmenu_play_select","mainmenu\\buttons\\mainmenu_play_selected",new Rectangle(61, 190, 260, 84));
            mButtonHelp    = new Button("mainmenu\\buttons\\mainmenu_help", "mainmenu\\buttons\\mainmenu_help_select", "mainmenu\\buttons\\mainmenu_help_selected", new Rectangle(81, 390, 187, 68));
            mButtonCredits = new Button("mainmenu\\buttons\\mainmenu_credits", "mainmenu\\buttons\\mainmenu_credits_select", "mainmenu\\buttons\\mainmenu_credits_selected", new Rectangle(81, 500, 187, 68));

            mGroupButtons = new GameObjectsGroup<Button>();
            mGroupButtons.addGameObject(mButtonPlay);
            mGroupButtons.addGameObject(mButtonHelp);
            mGroupButtons.addGameObject(mButtonCredits);

            mGroupButtons.loadContent(Game1.getInstance().getScreenManager().getContent());



            //mButtonPlay.loadContent(Game1.getInstance().getScreenManager().getContent());
            //mButtonHelp.loadContent(Game1.getInstance().getScreenManager().getContent());
            //mButtonCredits.loadContent(Game1.getInstance().getScreenManager().getContent());
            
            mButtonPlay.setLocation(50, 300);
            
        }


        public override void update(GameTime gameTime)
        {
            //checkCollisions();
            mCurrentBackground.update();
            mGroupButtons.update(gameTime);
            mCursor.update(gameTime);
            updateMouseInput();
            checkCollisions();
        }


        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();

            mCurrentBackground.draw(mSpriteBatch,100f);
            mGroupButtons.draw(mSpriteBatch);
            mCursor.draw(mSpriteBatch);

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
            if (button == mButtonPlay)
            {
                //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_GAMEPLAY, true);
            }else
            if (button == mButtonHelp)
            {
                Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU_HELP, false);
            }else
            if (button == mButtonCredits)
            {
                Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU_CREDITS, false);
            }
        }
        

        //apenas limpar quando for para o menu principal
        private void clearAllBackgrounds()
        {

        }


    }
}
