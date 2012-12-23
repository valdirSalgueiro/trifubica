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

        private const String cSOUND_COLOR = "sound\\fx\\colorswap8bit";
        
        private Texture2D mTextureHudBG;
        private Texture2D mTextureHudBGHead;       
        private Texture2D mTexturePlayerHead;

        private Texture2D mBarraBLeft;
        private Texture2D mBarraBMiddle;
        private Texture2D mBarraBRight;

        private Texture2D mBarraOLeft;
        private Texture2D mBarraOMiddle;
        private Texture2D mBarraORight;

        private Button mButtonRed;
        private Button mButtonGreen;
        private Button mButtonBlue;
        private Button mButtonPause;


        //measures
        private Rectangle mRectHead;
        private Rectangle mRectPlayerBarEnergy;
        private Rectangle mRectPlayerBar;
        private Rectangle mRectBarEnergy;
        private Rectangle mRectBar;

        //location das cores
        private Rectangle mRectColorSlot1;
        private Rectangle mRectColorSlot2;
        private Rectangle mRectColorSlot3;

        private static HUD instance;

        private MouseState oldStateMouse;

        private GameObjectsGroup<Button> mGroupButtons;
        Button mCurrentHighlightButton;

        float energy=1f;
        float level=0;

        private int mInstantInks;

        private const int cMAX_INSTANT_INKS = 3;

        private static GamePlayScreen sContext;


        private HUD()
        {
            mRectHead = new Rectangle(20,
                                           10,
                                           100,
                                           100);

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


            mRectColorSlot1 = new Rectangle(574, 535, 70, 70);
            mRectColorSlot2 = new Rectangle(634, 512, 70, 70);
            mRectColorSlot3 = new Rectangle(695, 535, 70, 66);
            
            /*mRectColorSlot1 = new Rectangle(574 - 260, 535, 70, 70);
            mRectColorSlot2 = new Rectangle(634 - 260, 512, 70, 70);
            mRectColorSlot3 = new Rectangle(695 - 260, 535, 70, 66);
            */
            mButtonRed = new Button("gameplay\\hud\\new\\balde_red", "gameplay\\hud\\new\\balde_red_selected", "gameplay\\hud\\new\\balde_red_selected", mRectColorSlot1);
            mButtonGreen = new Button("gameplay\\hud\\new\\balde_green", "gameplay\\hud\\new\\balde_green_selected", "gameplay\\hud\\new\\balde_green_selected", mRectColorSlot2);
            mButtonBlue = new Button("gameplay\\hud\\new\\balde_blue", "gameplay\\hud\\new\\balde_blue_selected", "gameplay\\hud\\new\\balde_blue_selected", mRectColorSlot3);

            //mButtonPause = new Button("gameplay\\hud\\new\\pause", "gameplay\\hud\\new\\pause_select", "gameplay\\hud\\new\\pause_selected", new Rectangle(727, 28, 54, 63));

            mButtonRed.setCollisionRect(19, 19, 33, 33);
            mButtonGreen.setCollisionRect(19, 19, 33, 33);
            mButtonBlue.setCollisionRect(19, 19, 33, 33);
            //mButtonPause.setCollisionRect(0, 0, 70, 70);

            SoundManager.LoadSound(cSOUND_COLOR);

            mGroupButtons = new GameObjectsGroup<Button>();
            mGroupButtons.addGameObject(mButtonRed);
            mGroupButtons.addGameObject(mButtonGreen);
            mGroupButtons.addGameObject(mButtonBlue);
            //mGroupButtons.addGameObject(mButtonPause);  
        }

        public void restart()
        {
            instance = null;
        }

        public static HUD getInstance(GamePlayScreen context)
        {
            if (instance == null)
            {
                instance = new HUD();
            }

            sContext = context;

            return instance;
        }


        public void loadContent(ContentManager contentManager)
        {
            if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.BLUE)
            {
                mTexturePlayerHead = contentManager.Load<Texture2D>("gameplay\\hud\\new\\blue");
            }
            if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.GREEN)
            {
                mTexturePlayerHead = contentManager.Load<Texture2D>("gameplay\\hud\\new\\green");
            }
            if (Game1.progressObject.getColor() == ProgressObject.PlayerColor.RED)
            {
                mTexturePlayerHead = contentManager.Load<Texture2D>("gameplay\\hud\\new\\red");
            }

            mTextureHudBGHead = contentManager.Load<Texture2D>("gameplay\\hud\\new\\hud_bg");
            mTextureHudBG = contentManager.Load<Texture2D>("gameplay\\hud\\new\\hud_bg_colors");

            mBarraBLeft = contentManager.Load<Texture2D>("gameplay\\hud\\new\\barra_bleft");
            mBarraBMiddle = contentManager.Load<Texture2D>("gameplay\\hud\\new\\barra_bmiddle");
            mBarraBRight = contentManager.Load<Texture2D>("gameplay\\hud\\new\\barra_bright");

            mBarraOLeft = contentManager.Load<Texture2D>("gameplay\\hud\\new\\barra_oleft");
            mBarraOMiddle = contentManager.Load<Texture2D>("gameplay\\hud\\new\\barra_omiddle");
            mBarraORight = contentManager.Load<Texture2D>("gameplay\\hud\\new\\barra_oright");

            mGroupButtons.loadContent(Game1.getInstance().getScreenManager().getContent());
                       
              
        }

        public void update(GameTime gameTime)
        {
            mGroupButtons.update(gameTime);
            //checkCollisions();

            if (GamePlayScreen.sCURRENT_STAGE_X_PROGRESSIVE < GamePlayScreen.sCURRENT_STAGE_X)
            {
                mButtonRed.addX(1);
                mButtonGreen.addX(1);
                mButtonBlue.addX(1);
             }         

        }

        /*
         * public void realignButtons (to be called manually)
         * */

        public void draw(SpriteBatch spriteBatch)
        {
            int complement = (int)GamePlayScreen.sCURRENT_STAGE_X_PROGRESSIVE;

            spriteBatch.Draw(mTextureHudBG, new Rectangle(550 + complement /*- 260*/, 504, 238, 96), Color.White);
            spriteBatch.Draw(mTextureHudBGHead, new Rectangle(42 + complement, 38, 285, 64), Color.White);

            if (energy > 0)
            {
                spriteBatch.Draw(mBarraBLeft, new Rectangle(115 + complement, 38 + 15, 7, 20), Color.White);
                spriteBatch.Draw(mBarraBMiddle, new Rectangle(115 + 7 + complement, 38 + 15, (int)(energy * 185), 20), Color.White);
                spriteBatch.Draw(mBarraBRight, new Rectangle(114 + 7 + (int)(energy * 185) + complement, 38 + 15, 7, 20), Color.White);
            }

            if (level > 0)
            {
                spriteBatch.Draw(mBarraOLeft, new Rectangle(114 + complement, 60 + 19, 7, 8), Color.White);
                spriteBatch.Draw(mBarraOMiddle, new Rectangle(114 + 7 + complement, 60 + 19, (int)(level * 147), 8), Color.White);
                spriteBatch.Draw(mBarraORight, new Rectangle(114 + 7 + (int)(level * 147) + complement, 60 + 19, 7, 8), Color.White);
            }
            mGroupButtons.draw(spriteBatch);

            mRectHead = new Rectangle(20 + complement, mRectHead.Y, mRectHead.Width, mRectHead.Height);
            spriteBatch.Draw(mTexturePlayerHead, mRectHead, Color.White);
        }
        
        public void setPlayerBarLevel(float value)
        {
            energy = value / 100.0f;
        }

        public void setBarLevel(float value)
        {
            level = value / 100.0f;
        }

        //the hud is an exception. THe checkcollision method must be called by GamePlayScreen class.
        public void checkCollisions(Cursor cursor, bool mousePressing)
        {
            if (mGroupButtons.checkCollisionWith(cursor))
            {
                mCurrentHighlightButton = (Button)mGroupButtons.getCollidedObject();

                //Game1.print(" XOE");

               /* if (mCurrentHighlightButton == mButtonRed)
                {
                    if (cursor.getColor() != Color.Red)
                    {
                        cursor.changeColor(Color.Red);
                        //play sound
                    }
                }
                else
                    if (mCurrentHighlightButton == mButtonGreen)
                    {
                        if (cursor.getColor() != Color.Green)
                        {
                            cursor.changeColor(Color.Green);
                        }
                    }
                    else
                        if (mCurrentHighlightButton == mButtonBlue)
                        {
                            if (cursor.getColor() != Color.Blue)
                            {
                                cursor.changeColor(Color.Blue);
                            }
                        }*/

                if (mousePressing)
                {
                    if (mCurrentHighlightButton == mButtonRed)
                    {
                        if (cursor.getColor() != Color.Red)
                        {
                            cursor.changeColor(Color.Red);
                            SoundManager.PlaySound(cSOUND_COLOR);
                        }
                    }else
                    if (mCurrentHighlightButton == mButtonGreen)
                    {
                        if (cursor.getColor() != Color.Green)
                        {
                            cursor.changeColor(Color.Green);
                            SoundManager.PlaySound(cSOUND_COLOR);
                        }
                    }else
                    if (mCurrentHighlightButton == mButtonBlue)
                    {
                        if (cursor.getColor() != Color.Blue)
                        {
                            cursor.changeColor(Color.Blue);
                            SoundManager.PlaySound(cSOUND_COLOR);
                        }
                    }
                    else
                        if (mCurrentHighlightButton == mButtonPause)
                        {
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

        public int getInstantInks()
        {
            return this.mInstantInks;
        }

        public void addInstantInk()
        {
            if (mInstantInks < cMAX_INSTANT_INKS)
            {
                this.mInstantInks++;
            }
        }

        public void removeInstantInk()
        {
            if (mInstantInks > 0)
            {
                this.mInstantInks--;
            }
        }

        public void setInstantInk(int amount)
        {
            this.mInstantInks = amount;
        }

    }
}
