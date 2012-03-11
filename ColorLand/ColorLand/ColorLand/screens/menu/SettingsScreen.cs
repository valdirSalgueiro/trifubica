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
    class SettingsScreen : BaseScreen
    {
        /***DATA****/
        private float mSoundVolume = 1f; //deve servir pra FX e MUSIC
        private bool mFullScreen = false;



        private SpriteBatch mSpriteBatch;

        //Lista dos backgrounds
        private Background mBackgroundImage;

        private Background mCurrentBackground;

        private List<Background> mList = new List<Background>();

        private Cursor mCursor;



        private MenuGrade mMenu;


        /***
         * BUTTONS
         * */
        private Button mButtonPlay;


        public SettingsScreen()
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

            //mMenu = new MenuGrade();

        }


        public override void update(GameTime gameTime)
        {
            checkCollisions();
            mCurrentBackground.update();
            //mButtonPlay.update(gameTime);
            mCursor.update(gameTime);
        }

        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();

            mCurrentBackground.draw(mSpriteBatch,0.0f);
           // mButtonPlay.draw(mSpriteBatch);
            mCursor.draw(mSpriteBatch);

            mSpriteBatch.End();
        }

        private void checkCollisions()
        {

            if (mCursor.collidesWith(mButtonPlay))
            {
                
            }

        }
        

        //apenas limpar quando for para o menu principal
        private void clearAllBackgrounds()
        {

        }

    }
}
