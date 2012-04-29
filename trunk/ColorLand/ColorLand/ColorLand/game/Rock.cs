using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ColorLand.game
{
    class Rock
    {

        //SPRITES
        private Texture2D texture;
        public Vector2 pos;
        private Boolean isActive = true;


        float dy;
        float ay = 9.8f;

        public Rectangle collisionRect;

        public Rock()
        {
        }

        public void loadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("enemies\\rocker\\rock\\Pedra");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, collisionRect, Color.White);
        }

        public Boolean update(GameTime gameTime)
        {
            dy += ay;
            pos.Y += (float)(dy*gameTime.ElapsedGameTime.TotalSeconds);
            collisionRect = new Rectangle((int)pos.X, (int)pos.Y, 44, 45);
            return isActive;
        }
    }
}

