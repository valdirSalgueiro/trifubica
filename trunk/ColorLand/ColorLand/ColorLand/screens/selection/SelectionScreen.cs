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
        
        //private const String cSOUND_HIGHLIGHT = "sound\\fx\\highlight8bit";
        private SpriteBatch mSpriteBatch;

        //Lista dos backgrounds
        private List<Background> mList = new List<Background>();

        private bool mMousePressing;

        private Texture2D mTextureChooseTitle;
        private Texture2D mTextureRedNameUnselected;
        private Texture2D mTextureRedNameSelected;
        private Texture2D mTextureGreenNameUnselected;
        private Texture2D mTextureGreenNameSelected;
        private Texture2D mTextureBlueNameUnselected;
        private Texture2D mTextureBlueNameSelected;
                       
        private SelectableCharacter mSelectableCharacterRed;
        private SelectableCharacter mSelectableCharacterGreen;
        private SelectableCharacter mSelectableCharacterBlue;

        private SelectableCharacter mCurrentSelectableCharacter;

        private Background mBackgroundImage;
        private Background mCurrentBackground;

        MouseState oldStateMouse;

        private float mAlpha = 1f;
        private bool mReduceAlpha;

        private Rectangle mRect1;
        private Rectangle mRect2;
        private Rectangle mRect3;

        private MTimer mTimerAfterSelection;

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

            mTextureChooseTitle = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\selection\\hero_screen");

            mTextureRedNameUnselected   = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\selection\\RedName_unselected");
            mTextureRedNameSelected     = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\selection\\Redname");
            mTextureGreenNameUnselected = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\selection\\GreenName_unselected");
            mTextureGreenNameSelected   = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\selection\\GreenName");
            mTextureBlueNameUnselected  = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\selection\\BlueName_unselected");
            mTextureBlueNameSelected    = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("gameplay\\selection\\Bluename"); 
            

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

            mFade = new Fade(this, "fades\\blackfade", Fade.SPEED.FAST);
            executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);

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

            updateTimers(gameTime);

            /*
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
            */
              
            if (mFade != null)
            {
                mFade.update(gameTime);
            }
        }


        public override void draw(GameTime gameTime)
        {

            mSpriteBatch.Begin();
            //mCurrentBackground.draw(mSpriteBatch);

            mBackgroundImage.draw(mSpriteBatch);

            mSpriteBatch.Draw(mTextureChooseTitle, new Rectangle(188, 50, mTextureChooseTitle.Width, mTextureChooseTitle.Height), Color.White);

            if (mCurrentSelectableCharacter == mSelectableCharacterRed)
            {
                mSpriteBatch.Draw(mTextureRedNameSelected, new Rectangle(120, 430, mTextureRedNameSelected.Width, mTextureRedNameSelected.Height), Color.White * mAlpha);
                mSpriteBatch.Draw(mTextureGreenNameUnselected, new Rectangle(354, 435, mTextureGreenNameUnselected.Width, mTextureGreenNameUnselected.Height), Color.White * mAlpha);
                mSpriteBatch.Draw(mTextureBlueNameUnselected, new Rectangle(580, 430, mTextureBlueNameUnselected.Width, mTextureBlueNameUnselected.Height), Color.White * mAlpha);
            }
            else
            if (mCurrentSelectableCharacter == mSelectableCharacterGreen)
            {
                mSpriteBatch.Draw(mTextureGreenNameSelected, new Rectangle(354, 435, mTextureGreenNameSelected.Width, mTextureGreenNameSelected.Height), Color.White * mAlpha);
                mSpriteBatch.Draw(mTextureRedNameUnselected, new Rectangle(120, 430, mTextureRedNameUnselected.Width, mTextureRedNameUnselected.Height), Color.White * mAlpha);
                mSpriteBatch.Draw(mTextureBlueNameUnselected, new Rectangle(580, 430, mTextureBlueNameUnselected.Width, mTextureBlueNameUnselected.Height), Color.White * mAlpha);
            }
            else
            if (mCurrentSelectableCharacter == mSelectableCharacterBlue)
            {
                mSpriteBatch.Draw(mTextureBlueNameSelected, new Rectangle(580, 430, mTextureBlueNameSelected.Width, mTextureBlueNameSelected.Height), Color.White * mAlpha);
                mSpriteBatch.Draw(mTextureGreenNameUnselected, new Rectangle(354, 435, mTextureGreenNameUnselected.Width, mTextureGreenNameUnselected.Height), Color.White * mAlpha);
                mSpriteBatch.Draw(mTextureRedNameUnselected, new Rectangle(120, 430, mTextureRedNameUnselected.Width, mTextureRedNameUnselected.Height), Color.White * mAlpha);
            }
            else //ninguem selecionado
            {
                mSpriteBatch.Draw(mTextureBlueNameUnselected, new Rectangle(580, 430, mTextureBlueNameUnselected.Width, mTextureBlueNameUnselected.Height), Color.White);
                mSpriteBatch.Draw(mTextureGreenNameUnselected, new Rectangle(354, 435, mTextureGreenNameUnselected.Width, mTextureGreenNameUnselected.Height), Color.White);
                mSpriteBatch.Draw(mTextureRedNameUnselected, new Rectangle(120, 430, mTextureRedNameUnselected.Width, mTextureRedNameUnselected.Height), Color.White);
            }

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
                if (oldStateMouse.LeftButton != ButtonState.Pressed)
                {
                    
                    if (mCurrentSelectableCharacter != null)
                    {

                        if (mCurrentSelectableCharacter == mSelectableCharacterRed)
                        {
                            ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setColor(ProgressObject.PlayerColor.RED));
                        }else
                        if (mCurrentSelectableCharacter == mSelectableCharacterGreen)
                        {
                            ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setColor(ProgressObject.PlayerColor.GREEN));
                        }else
                        if (mCurrentSelectableCharacter == mSelectableCharacterBlue)
                        {
                            ObjectSerialization.Save<ProgressObject>(Game1.sPROGRESS_FILE_NAME, Game1.progressObject.setColor(ProgressObject.PlayerColor.BLUE));
                        }

                        mCurrentSelectableCharacter.changeState(SelectableCharacter.sSTATE_SELECTED);
                        mReduceAlpha = true;

                        mTimerAfterSelection = new MTimer(true);
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
                    //SoundManager.PlaySound(cSOUND_SELECT);
                    
                    mCurrentSelectableCharacter = mSelectableCharacterRed;

                    mSelectableCharacterGreen.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                    mSelectableCharacterBlue.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                }
                else
                {
                    mSelectableCharacterRed.changeState(SelectableCharacter.sSTATE_UNSELECTED);

                    if (mCursor.collidesWith(mRect2))
                    {
                        //SoundManager.PlaySound(cSOUND_SELECT);

                        mCurrentSelectableCharacter = mSelectableCharacterGreen;

                        mSelectableCharacterRed.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                        mSelectableCharacterBlue.changeState(SelectableCharacter.sSTATE_UNSELECTED);
                    }
                    else
                    {

                        mSelectableCharacterGreen.changeState(SelectableCharacter.sSTATE_UNSELECTED);

                        if (mCursor.collidesWith(mRect3))
                        {

                            //SoundManager.PlaySound(cSOUND_SELECT);
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


        private void updateTimers(GameTime gameTime)
        {
            if (mTimerAfterSelection != null)
            {
                mTimerAfterSelection.update(gameTime);

                if (mTimerAfterSelection.getTimeAndLock(5))
                {
                    executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                }

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
                //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_HISTORY, true);
                //Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, false);
                Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MACROMAP, false);
            }

        }


    }
}
