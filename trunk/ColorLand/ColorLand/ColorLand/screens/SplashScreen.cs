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
    class SplashScreen : BaseScreen
    {
        private const String cSOUND_HIGHLIGHT = "sound\\fx\\highlight8bit";
        private SpriteBatch mSpriteBatch;

        //Lista dos backgrounds
        private Background mBackgroundImage;

        private Background mCurrentBackground;

        private List<Background> mList = new List<Background>();

        private const int cMAX_BG_COUNTER = 3;
        private int mBackgroundCounter;

        private Button mCurrentHighlightButton;
        private bool mMousePressing;

        
        private float mAlpha = 0f;
        private bool mIncreaseAlpha;
        private bool mShowBlackBackground = true;
        private Texture2D mBlackBackground;

        private Texture2D mGamelogo;
        private Texture2D mTextureClickToStart;
        private bool mShowTextClickToStart;

        private bool mClicked; //ready to go to main menu

        //FONT

        //private SpriteFont mFont;

        //fade
        private Fade mFade;
        private Fade mCurrentFade;

        private MTimer mTimer;
        private MTimer mTimerBlinkText;

        private MouseState oldStateMouse;

        public SplashScreen()
        {

            if (!SoundManager.isPlaying())
            {
                SoundManager.PlayMusic("sound\\music\\theme");
            }
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundImage = new Background("mainmenu\\MainMenu_bg");
            mBackgroundImage.addPart(new String[1] { "mainmenu\\MainMenu_barco" },  1, 125, 110, 80, 600 - 211);
            mBackgroundImage.addPart(new String[1] { "mainmenu\\MainMenu_areia" },  1, 801, 201, 0, 600 - 201);
            mBackgroundImage.addPart(new String[1] { "mainmenu\\MainMenu_mato01" }, 1, 232, 111, 0, 600 - 111);
            mBackgroundImage.addPart(new String[1] { "mainmenu\\MainMenu_mato02" }, 1, 345, 135, 800-345, 600 - 95);

            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

            mList.Add(mBackgroundImage);

            mCurrentBackground = mList.ElementAt(0);

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

            mGamelogo = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\logo");
            mBlackBackground = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("fades\\blackfade");

            mTextureClickToStart = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("mainmenu\\clicktostart");
            
            mFade = new Fade(this, "fades\\blackfade", Fade.SPEED.FAST);
            
            SoundManager.LoadSound(cSOUND_HIGHLIGHT);

            mTimer = new MTimer(true);
            
        }


        public override void update(GameTime gameTime)
        {
            //checkCollisions();
            mCurrentBackground.update();
            mCursor.update(gameTime);
            updateMouseInput();
            updateTimer(gameTime);
            updateTimerBlinkText(gameTime);

            if (mFade != null)
            {
                mFade.update(gameTime);
            }
        }


        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();

            if (mShowBlackBackground)
            {
            //    mSpriteBatch.Draw(mBlackBackground, new Rectangle(0, 0, 800, 600), Color.Black);
            }
            else
            {
                mCurrentBackground.draw(mSpriteBatch);
            }
           

            if (mFade != null)
            {
                mFade.draw(mSpriteBatch);
            }

            if (mIncreaseAlpha)
            {
                if (mAlpha < 1)
                {
                    mAlpha += 0.05f;
                }
                else
                {
                    mAlpha = 1;
                }

            }

            //se estiver saindo do splash pro main menu...
            if (mClicked)
            {
                mAlpha -= 0.25f;
            }

            mSpriteBatch.Draw(mGamelogo, new Rectangle(200, 50,382, 349), Color.White * mAlpha);


            if (mShowTextClickToStart && !mClicked)
            {
                mSpriteBatch.Draw(mTextureClickToStart, new Vector2(240, 392), Color.White);
            }

            mCursor.draw(mSpriteBatch);

            mSpriteBatch.End();

        }

        private void updateMouseInput()
        {

            MouseState mouseState = Mouse.GetState();


            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (oldStateMouse.LeftButton != ButtonState.Pressed)
                {
                    mFade = new Fade(this, "fades\\blackfade", Fade.SPEED.FAST);
                    mClicked = true;
                    executeFade(mFade, Fade.sFADE_OUT_EFFECT_GRADATIVE);
                }
            }

            oldStateMouse = mouseState;

        }

        public override void executeFade(Fade fadeObject, int effect)
        {
            base.executeFade(fadeObject, effect);

            mCurrentFade = fadeObject;
            fadeObject.execute(effect);
        }

        ///////prototipo
        public override void fadeFinished(Fade fadeObject)
        {
            if (fadeObject.getEffect() == Fade.sFADE_IN_EFFECT_GRADATIVE)
            {
                
            }
            else
            if (fadeObject.getEffect() == Fade.sFADE_OUT_EFFECT_GRADATIVE)
            {
                Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_MAIN_MENU, true);
            }

        }

        private void updateTimer(GameTime gameTime)
        {
            if (mTimer != null)
            {
                mTimer.update(gameTime);

                if (mTimer.getTimeAndLock(1))
                {
                    mIncreaseAlpha = true;
                }
                if (mTimer.getTimeAndLock(3))
                {
                    mShowBlackBackground = false;
                    executeFade(mFade, Fade.sFADE_IN_EFFECT_GRADATIVE);
                }
                if (mTimer.getTimeAndLock(4))
                {
                    mTimerBlinkText = new MTimer(true);
                }
                
            }
        }

        private void updateTimerBlinkText(GameTime gameTime)
        {
            if (mTimerBlinkText != null)
            {
                mTimerBlinkText.update(gameTime);

                if (mTimerBlinkText.getTimeAndLock(0.2f))
                {
                    mShowTextClickToStart = true;
                }
                if (mTimerBlinkText.getTimeAndLock(0.4f))
                {
                    mShowTextClickToStart = false;
                    mTimerBlinkText.start();
                }
            }
        }


    }
}
