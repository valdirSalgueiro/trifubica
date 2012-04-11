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
            
            //Game1.print("LOC: "  + mGroupButtons.getGameObject(2).getLocation());

            mSelectableCharacterRed = new SelectableCharacter(new Vector2(160, 310), Color.Red);
            mSelectableCharacterRed.loadContent(Game1.getInstance().getScreenManager().getContent());

            mSelectableCharacterGreen = new SelectableCharacter(new Vector2(382, 310), Color.Green);
            mSelectableCharacterGreen.loadContent(Game1.getInstance().getScreenManager().getContent());

            mSelectableCharacterBlue = new SelectableCharacter(new Vector2(625, 308), Color.Blue);
            mSelectableCharacterBlue.loadContent(Game1.getInstance().getScreenManager().getContent());

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

            if (kState.IsKeyDown(Keys.S))
            {

                if (!oldState.IsKeyDown(Keys.S))
                {
                    mSelectableCharacterRed.changeState(SelectableCharacter.sSTATE_HIGHLIGHTED);
                    //mSelectableCharacterGreen.changeState(SelectableCharacter.sSTATE_HIGHLIGHTED);
                    //mSelectableCharacterBlue.changeState(SelectableCharacter.sSTATE_HIGHLIGHTED);
                }

            }

            if (kState.IsKeyDown(Keys.D))
            {

                if (!oldState.IsKeyDown(Keys.D))
                {
                    mSelectableCharacterRed.changeState(SelectableCharacter.sSTATE_SELECTED);
                    //mSelectableCharacterGreen.changeState(SelectableCharacter.sSTATE_SELECTED);
                    //mSelectableCharacterBlue.changeState(SelectableCharacter.sSTATE_SELECTED);
                }

            }

            if (kState.IsKeyDown(Keys.F))
            {

                if (!oldState.IsKeyDown(Keys.F))
                {
                    mSelectableCharacterRed.changeState(SelectableCharacter.sSTATE_EXPLOSION);
                    //mSelectableCharacterGreen.changeState(SelectableCharacter.sSTATE_EXPLOSION);
                    //mSelectableCharacterBlue.changeState(SelectableCharacter.sSTATE_EXPLOSION);
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

            mSelectableCharacterRed.draw(mSpriteBatch);
            mSelectableCharacterGreen.draw(mSpriteBatch);
            mSelectableCharacterBlue.draw(mSpriteBatch);

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
                mMousePressing = true;
            }
            else
            {
                /*if (mCurrentHighlightButton != null)
                {

                    if (mMousePressing)
                    {
                        processButtonAction(mCurrentHighlightButton);
                    }

                }*/

                mMousePressing = false;
            }


        }

        private void checkCollisions()
        {
            if (mCursor.collidesWith(mSelectableCharacterRed))
            {
                mCurrentSelectableCharacter = mSelectableCharacterRed;
            }
            else
            if (mCursor.collidesWith(mSelectableCharacterGreen)){
                mCurrentSelectableCharacter = mSelectableCharacterRed;
            }else
            if (mCursor.collidesWith(mSelectableCharacterBlue)){
                mCurrentSelectableCharacter = mSelectableCharacterRed;
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
