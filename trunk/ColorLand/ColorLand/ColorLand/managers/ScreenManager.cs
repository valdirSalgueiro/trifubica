using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace ColorLand
{
    public class ScreenManager : DrawableGameComponent
    {
        private ContentManager mContentManager;

        InputState input = new InputState();

        private BaseScreen mCurrentScreen;

        private SpriteBatch mSpriteBatch;
        
        private SpriteFont mFont;

        bool isInitialized;


        //indexes
        public const int SCREEN_ID_GAMEPLAY        = 0;
        //public const int SCREEN_ID_MAIN_MENU     = 1;
        //public const int SCREEN_ID_HISTORY       = 2;
        

        public ScreenManager(Game game)
            : base(game) {
            
        }

        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }

        protected override void LoadContent() {

            mContentManager = Game.Content;

            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            changeScreen(SCREEN_ID_GAMEPLAY, false);
            
        }

        public void UnloadContent()
        {
            mContentManager.Unload();
            mContentManager = Game.Content;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            
            /*KeyboardManager.getInstance().update();
            JoystickManager.getInstance(PlayerIndex.One).update();
            if (mCurrentScreen.hasRequestedScreenChange()) {
                changeScreen(mCurrentScreen.getNextScreenId(),mCurrentScreen.shouldReleaseMe());
            }
            */
            input.Update();

            mCurrentScreen.handleInput(input);
            mCurrentScreen.update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
            mCurrentScreen.draw(gameTime);
        }

        public void changeScreen(int id, bool releaseCurrentScreen) {

            /*if (releaseCurrentScreen) {
                //mCurrentScreen.releaseScreenResources();
                mContentManager = new ContentManager(Game.Services, "Content");
            }*/

            switch (id) {
                case SCREEN_ID_GAMEPLAY:
                   mCurrentScreen = new GamePlayScreen();
                   break;
            }

        }

        //TODO deixar isso generico futuramente
        public void changeToLetteringScreen(int param)
        {
            //mCurrentScreen = new ScreenLettering(param);
        }

        public SpriteBatch getSpriteBatch() {

            return mSpriteBatch;
        }


        public ContentManager getContent() {
            return this.mContentManager;
        }


        public BaseScreen getCurrentScreen() {
            return this.mCurrentScreen;
        }

    }

}