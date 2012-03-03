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
        
        /***
         * BUTTONS
         * */
        private Button mButtonPlay;
        private Button mButtonSettings;
        private Button mButtonCredits;

        public MainMenuScreen()
        {
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundImage = new Background("mainmenu\\mainmenubg");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

            mList.Add(mBackgroundImage);

            mCurrentBackground = mList.ElementAt(0);

            mCursor = new Cursor();
            mCursor.loadContent(Game1.getInstance().getScreenManager().getContent());

            mButtonPlay = new Button("test\\m", "test\\e", new Rectangle(100, 100, 200, 100));

            mButtonPlay.loadContent(Game1.getInstance().getScreenManager().getContent());

            mButtonPlay.setLocation(50, 300);

        }


        public override void update(GameTime gameTime)
        {
            mCurrentBackground.update();
            mButtonPlay.update(gameTime);
            mCursor.update(gameTime);
        }

        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();

            mCurrentBackground.draw(mSpriteBatch);
            mButtonPlay.draw(mSpriteBatch);
            mCursor.draw(mSpriteBatch);

            mSpriteBatch.End();
        }

        private void checkCollisions()
        {

            if (mCursor.collidesWith(mButtonPlay))
            {
                Game1.print("play");
            }

        }
        

        //apenas limpar quando for para o menu principal
        private void clearAllBackgrounds()
        {

        }

    }
}
