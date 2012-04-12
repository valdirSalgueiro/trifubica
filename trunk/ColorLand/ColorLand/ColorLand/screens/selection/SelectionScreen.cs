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
    public class SelectionScreen : BaseScreen
    {
        private const String cSOUND_HIGHLIGHT = "sound\\fx\\highlight8bit";
        private SpriteBatch mSpriteBatch;

        //Lista dos backgrounds
        private List<Background> mList = new List<Background>();

        private bool mMousePressing;

        private Texture2D mTextureChooseTitle;
        private SelectableCharacter mSelectableCharacterRed;
        private SelectableCharacter mSelectableCharacterGreen;
        private SelectableCharacter mSelectableCharacterBlue;

        private SelectableCharacter mCurrentSelectableCharacter;

        private Background mBackgroundImage;
        private Background mCurrentBackground;

        MouseState oldStateMouse;

        private Color mColorForAlphaEffect;
        private float mAlpha = 1f;
        private bool mReduceAlpha;

        private Rectangle mRect1;
        private Rectangle mRect2;
        private Rectangle mRect3;


        //fade
        private Fade mFade;
        private Fade mCurrentFade;

        private KeyboardState oldState;

        public SelectionScreen()
        {

            if (!SoundManager.isPlaying())
            {
                SoundManager.PlayMusic("sound\\music\\theme");
            }
            
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundImage = new Background("gameplay\\selection\\Nuvens_pos");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

            mList.Add(mBackgroundImage);

            mCurrentBackground = mList.ElementAt(0);

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

            mTextureChooseTitle = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\pausescreen\\paused_title");

            mRect1 = new Rectangle(102, 223,150, 155);
            mRect2 = new Rectangle(327, 223, 150, 155);
            mRect3 = new Rectangle(565, 223, 150, 155);

            //Game1.print("LOC: "  + mGroupButtons.getGameObject(2).getLocation());

            mSelectableCharacterRed = new SelectableCharacter(new Vector2(160, 310), Color.Red);
            mSelectableCharacterRed.loadContent(Game1.getInstance().getScreenManager().getContent());

            mSelectableCharacterGreen = new SelectableCharacter(new Vector2(382 + 15, 310 + 6), Color.Green);
            mSelectableCharacterGreen.loadContent(Game1.getInstance().getScreenManager().getContent());

            mSelectableCharacterBlue = new SelectableCharacter(new Vector2(625 - 5, 308 + 6), Color.Blue);
            mSelectableCharacterBlue.loadContent(Game1.getInstance().getScreenManager().getContent());

            mColorForAlphaEffect = Color.White;

            SoundManager.LoadSound(cSOUND_HIGHLIGHT);

        }


        public override void update(GameTime gameTime)
        {
            //checkCollisions();
            mCurrentBackground.update();

            mSelectableCharacterRed.update(gameTime);
            mSelectableCharacterGreen.update(gameTime);
            mSelectableCharacterBlue.update(gameTime);

            mCursor.update(gameTime);
            updateMouseInput();
            checkCollisions();

            


            KeyboardState kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.A))
            {

                if (!oldState.IsKeyDown(Keys.A))
                {
                    mSelectableCharacterRed.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                    //mSelectableCharacterGreen.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                    //mSelectableCharacterBlue.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                }

            }

            oldState = kState;

            if (mFade != null)
            {
                //mFade.update(gameTime);
            }
        }


        public override void draw(GameTime gameTime)
        {

            mSpriteBatch.Begin();
            //mCurrentBackground.draw(mSpriteBatch);

            mBackgroundImage.draw(mSpriteBatch);

            if (mReduceAlpha)
            {
                if (mAlpha > 0)
                {
                    mAlpha -= 0.05f;
                }
                else
                {
                    mAlpha = 0;
                }

                //só pra garantir o anti-pau
                if (mCurrentSelectableCharacter != null )
                {
                    if (mCurrentSelectableCharacter == mSelectableCharacterRed)
                    {
                        mSelectableCharacterRed.draw(mSpriteBatch);
                        mSelectableCharacterGreen.draw(mSpriteBatch, Color.White * mAlpha);//new Color(255 * mAlpha, 255 * mAlpha, 255 * mAlpha, mAlpha) * mAlpha);
                        mSelectableCharacterBlue.draw(mSpriteBatch, Color.White * mAlpha);
                    }
                    if (mCurrentSelectableCharacter == mSelectableCharacterGreen)
                    {
                        mSelectableCharacterRed.draw(mSpriteBatch, Color.White * mAlpha);
                        mSelectableCharacterGreen.draw(mSpriteBatch);
                        mSelectableCharacterBlue.draw(mSpriteBatch, Color.White * mAlpha);
                    }
                    if (mCurrentSelectableCharacter == mSelectableCharacterBlue)
                    {
                        mSelectableCharacterRed.draw(mSpriteBatch, Color.White * mAlpha);
                        mSelectableCharacterGreen.draw(mSpriteBatch, Color.White * mAlpha);
                        mSelectableCharacterBlue.draw(mSpriteBatch);
                    }
                }

            }
            else
            {
                mSelectableCharacterRed.draw(mSpriteBatch);
                mSelectableCharacterGreen.draw(mSpriteBatch);
                mSelectableCharacterBlue.draw(mSpriteBatch); 
            }
                       
            
            mCursor.draw(mSpriteBatch);

            /*if (mFade != null)
            {
                mFade.draw(mSpriteBatch);
            }*/

            mSpriteBatch.End();

        }

        private void updateMouseInput()
        {

            MouseState ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (oldStateMouse.LeftButton != ButtonState.Pressed)
                {
                    
                    if (mCurrentSelectableCharacter != null)
                    {
                        Game1.print("click");
                        mCurrentSelectableCharacter.changeState(SelectableCharacter.sSTATE_SELECTED);
                        mReduceAlpha = true;
                    }
                }
            }

            oldStateMouse = ms;

        }

        private void checkCollisions()
        {
            if (mSelectableCharacterRed.getState() != SelectableCharacter.sSTATE_SELECTED && mSelectableCharacterRed.getState() != SelectableCharacter.sSTATE_EXPLOSION &&
                mSelectableCharacterGreen.getState() != SelectableCharacter.sSTATE_SELECTED && mSelectableCharacterGreen.getState() != SelectableCharacter.sSTATE_EXPLOSION &&
                mSelectableCharacterBlue.getState() != SelectableCharacter.sSTATE_SELECTED && mSelectableCharacterBlue.getState() != SelectableCharacter.sSTATE_EXPLOSION)
            {
                mCurrentSelectableCharacter = null;
                if (mCursor.collidesWith(mRect1))
                {
                    mCurrentSelectableCharacter = mSelectableCharacterRed;

                    mSelectableCharacterGreen.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                    mSelectableCharacterBlue.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                }
                else
                {
                    mSelectableCharacterRed.changeState(SelectableCharacter.sSTATE_UNSELECTED);

                    if (mCursor.collidesWith(mRect2))
                    {
                        mCurrentSelectableCharacter = mSelectableCharacterGreen;

                        mSelectableCharacterRed.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                        mSelectableCharacterBlue.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                    }
                    else
                    {

                        mSelectableCharacterGreen.changeState(SelectableCharacter.sSTATE_UNSELECTED);

                        if (mCursor.collidesWith(mRect3))
                        {
                            mCurrentSelectableCharacter = mSelectableCharacterBlue;

                            mSelectableCharacterRed.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                            mSelectableCharacterGreen.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                        }
                        else
                        {
                            mSelectableCharacterBlue.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                        }
                    }

                }

                if (mCurrentSelectableCharacter != null && mCurrentSelectableCharacter.getState() == SelectableCharacter.sSTATE_UNSELECTED)
                {
                      mCurrentSelectableCharacter.changeState(SelectableCharacter.sSTATE_HIGHLIGHTED);
                }
                else
                {

                }
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
