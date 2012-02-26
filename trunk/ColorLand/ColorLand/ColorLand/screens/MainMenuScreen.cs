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
        

        public MainMenuScreen()
        {
            mSpriteBatch = Game1.getInstance().getScreenManager().getSpriteBatch();

            mBackgroundImage = new Background("gameplay\\backgrounds\\bgteste");
            mBackgroundImage.loadContent(Game1.getInstance().getScreenManager().getContent());

            //mList.Add(mBackgroundImage);

            mCurrentBackground = mList.ElementAt(0);

        }


        public override void update(GameTime gameTime)
        {
            mCurrentBackground.update();

        }

        public override void draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();

            mCurrentBackground.draw(mSpriteBatch);

            
            mSpriteBatch.End();
        }


        //apenas limpar quando for para o menu principal
        private void clearAllBackgrounds()
        {

        }

    }
}
