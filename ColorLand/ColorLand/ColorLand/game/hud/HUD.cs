using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ColorLand
{
    public class HUD
    {

        //player head
        //player energy
        //coin icon
        //coin level

        private Texture2D mTextureHudBG;
        private Texture2D mTexturePlayerHead;
        private Texture2D mTexturePincel;
        private Texture2D mTexturePlayerBarBackground;
        private Texture2D mTexturePlayerBarEnergy;
        private Texture2D mTextureBarBackground;
        private Texture2D mTextureBarEnergy;

        private Button mButtonRed;
        private Button mButtonGreen;
        private Button mButtonBlue;


        //measures
        private Rectangle mRectHead;
        private Rectangle mRectPlayerBarEnergy;
        private Rectangle mRectPlayerBar;
        private Rectangle mRectBarEnergy;
        private Rectangle mRectBar;

        private static HUD instance;

        private GameObjectsGroup<Button> mGroupButtons;
        Button mCurrentHighlightButton;


        private HUD()
        {
            mRectHead = new Rectangle(90,
                                        522,
                                        67,
                                        75);

            mRectPlayerBarEnergy = new Rectangle(128,
                                        566,
                                        181,
                                        23);

            mRectPlayerBar = new Rectangle(128,
                            564,
                            181,    
                            27);

            mRectBarEnergy = new Rectangle(315,
                            566,
                            0,
                            23);

            mRectBar = new Rectangle(315,
                            564,
                            182,
                            27);

            mButtonRed = new Button("gameplay\\hud\\hud_vermelho", "gameplay\\hud\\hud_vermelho_selected", "gameplay\\hud\\hud_vermelho_selected", new Rectangle(534, 556 - 30, 63, 76));
            mButtonGreen = new Button("gameplay\\hud\\hud_verde", "gameplay\\hud\\hud_verde_selected", "gameplay\\hud\\hud_verde_selected", new Rectangle(594, 536 - 30, 63, 76));
            mButtonBlue = new Button("gameplay\\hud\\hud_azul", "gameplay\\hud\\hud_azul_selected", "gameplay\\hud\\hud_azul_selected", new Rectangle(659, 560 - 30, 63, 76));

            mButtonRed.setCollisionRect(11, 22, 40, 40);
            mButtonGreen.setCollisionRect(11, 22, 40, 40);
            mButtonBlue.setCollisionRect(11, 22, 40, 40);

            mGroupButtons = new GameObjectsGroup<Button>();
            mGroupButtons.addGameObject(mButtonRed);
            mGroupButtons.addGameObject(mButtonGreen);
            mGroupButtons.addGameObject(mButtonBlue);
            mGroupButtons.loadContent(Game1.getInstance().getScreenManager().getContent());

        }

        public static HUD getInstance()
        {
            if (instance == null)
            {
                instance = new HUD();
            }
            return instance;
        }


        public void loadContent(ContentManager contentManager)
        {       
            mTexturePlayerHead = contentManager.Load<Texture2D>("gameplay\\hud\\hud_blue");
            mTexturePincel = contentManager.Load<Texture2D>("gameplay\\hud\\hud_pincel");

            mTexturePlayerBarBackground = contentManager.Load<Texture2D>("gameplay\\hud\\hud_bar_blue");
            mTexturePlayerBarEnergy = contentManager.Load<Texture2D>("gameplay\\hud\\hud_pbar_blue");

            mTextureBarBackground = contentManager.Load<Texture2D>("gameplay\\hud\\hud_bar_progress");
            mTextureBarEnergy = contentManager.Load<Texture2D>("gameplay\\hud\\hud_pbar_progress");

            mTextureHudBG = contentManager.Load<Texture2D>("gameplay\\hud\\hud_bg");
        }

        public void update(GameTime gameTime)
        {
            mGroupButtons.update(gameTime);
            //checkCollisions();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTextureHudBG, new Rectangle(28,Game1.sSCREEN_RESOLUTION_HEIGHT-88, 743, 88), Color.White);
            spriteBatch.Draw(mTexturePlayerBarBackground, mRectPlayerBar, Color.White);
            spriteBatch.Draw(mTexturePlayerBarEnergy, mRectPlayerBarEnergy, Color.White);

            spriteBatch.Draw(mTexturePlayerBarBackground, mRectPlayerBar, Color.White);
            spriteBatch.Draw(mTexturePlayerBarEnergy, mRectPlayerBarEnergy, Color.White);

            spriteBatch.Draw(mTextureBarBackground, mRectBar, Color.White);
            spriteBatch.Draw(mTextureBarEnergy, mRectBarEnergy, Color.White);

            mGroupButtons.draw(spriteBatch);

            spriteBatch.Draw(mTexturePincel, new Rectangle(480, 537, 70,56), Color.White);
            

            spriteBatch.Draw(mTexturePlayerHead, mRectHead, Color.White);
        }
        
        public void setPlayerBarLevel(int value)
        {
            mRectPlayerBarEnergy.Width = value;
        }

        public void setBarLevel(int value)
        {
            mRectBarEnergy.Width = value;
        }

        //the hud is an exception. THe checkcollision method must be called by GamePlayScreen class.
        public void checkCollisions(Cursor cursor, bool mousePressing)
        {
            if (mGroupButtons.checkCollisionWith(cursor))
            {
                mCurrentHighlightButton = (Button)mGroupButtons.getCollidedObject();

                Game1.print(" XOE");

                if (mousePressing)
                {
                    if (mCurrentHighlightButton == mButtonRed)
                    {
                        if (cursor.getColor() != Color.Red)
                        {
                            cursor.changeColor(Color.Red);
                            //play sound
                        }
                    }else
                    if (mCurrentHighlightButton == mButtonGreen)
                    {
                        if (cursor.getColor() != Color.Green)
                        {
                            cursor.changeColor(Color.Green);
                            //play sound
                        }
                    }else
                    if (mCurrentHighlightButton == mButtonBlue)
                    {
                        if (cursor.getColor() != Color.Blue)
                        {
                            cursor.changeColor(Color.Blue);
                            //play sound
                        }
                    }
                }


            }


            //if (mGroupButtons.checkCollisionWith(mCursor))
            //{
            //    mCurrentHighlightButton = (Button)mGroupButtons.getCollidedObject();

            //    if (mMousePressing)
            //    {
            //        if (mCurrentHighlightButton.getState() != Button.sSTATE_PRESSED)
            //        {
            //            mCurrentHighlightButton.changeState(Button.sSTATE_PRESSED);
            //        }
            //    }
            //    else
            //    {

            //        if (mCurrentHighlightButton.getState() != Button.sSTATE_HIGHLIGH)
            //        {
            //            mCurrentHighlightButton.changeState(Button.sSTATE_HIGHLIGH);
            //        }

            //    }

            //}
            //else
            //{
            //    if (mCurrentHighlightButton != null)// && mCurrentHighlightButton.getState() != Button.sSTATE_PRESSED)
            //    {
            //        mCurrentHighlightButton.changeState(Button.sSTATE_NORMAL);
            //    }
            //    mCurrentHighlightButton = null;
            //}
        }

        private void processButtonAction(Button button)
        {
            if (button == mButtonRed)
            {
            }

            if (button == mButtonGreen)
            {
            }

            if (button == mButtonBlue)
            {
            }

        }
 

    }
}
