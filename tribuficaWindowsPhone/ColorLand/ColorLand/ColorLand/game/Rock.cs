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
        private Boolean collided = false;
        public float alpha = 1;


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
            spriteBatch.Draw(texture, collisionRect, Color.White*alpha);
        }

        public Boolean update(GameTime gameTime)
        {
            if (collided)
            {
                if (alpha > 0.0f)
                    alpha -= 0.03f;
                else
                    isActive = false;
                pos.X -= (float)(100 * gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (pos.Y > GamePlayScreen.sGROUND_WORLD_1_1) {
                notifyCollision();
            }

            dy += ay;
            pos.Y += (float)(dy * gameTime.ElapsedGameTime.TotalSeconds);
            collisionRect = new Rectangle((int)pos.X, (int)pos.Y, 44, 45);
            return isActive;
        }

        public void notifyCollision()
        {
            if (collided)
                return;
            collided = true;
            dy = -150;
        }
    }
}

