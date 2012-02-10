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

        private Texture2D mTexturePlayerHead;
        private Texture2D mTexturePlayerBarBackground;
        private Texture2D mTexturePlayerBarEnergy;

        //measures
        private Rectangle mRectHead;
        private Rectangle mRectPlayerBarEnergy;

        private static HUD instance;


        private HUD()
        {
            mRectHead = new Rectangle(100,
                                        Game1.sSCREEN_RESOLUTION_HEIGHT - 100,
                                        40,
                                        40);

            mRectPlayerBarEnergy = new Rectangle(200,
                                        Game1.sSCREEN_RESOLUTION_HEIGHT - 100,
                                        200,
                                        60); 
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

            mTexturePlayerHead           = contentManager.Load<Texture2D>("test\\head");
            mTexturePlayerBarBackground  = contentManager.Load<Texture2D>("test\\backbar");
            mTexturePlayerBarEnergy      = contentManager.Load<Texture2D>("test\\frontbar");
        }

        public void update(GameTime gameTime)
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexturePlayerHead, mRectHead, Color.White);
            spriteBatch.Draw(mTexturePlayerBarEnergy, mRectPlayerBarEnergy, Color.White);
        }

        public void setPlayerBarLevel(int value)
        {
            mRectPlayerBarEnergy.Width = value;
        }

    }
}
