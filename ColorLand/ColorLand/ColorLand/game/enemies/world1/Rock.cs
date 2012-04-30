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
        public Vector2 vel;

        public Color type;

        float dy;
        float ay = 9.8f;

        public Rectangle collisionRect;

        public Rock(Vector2 vel_)
        {
            type = Color.White;
            vel = vel_;
            dy = vel.Y;
        }

        public void loadContent(ContentManager content)
        {

            texture = content.Load<Texture2D>("enemies\\rocker\\rock\\Pedra");
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, collisionRect, type * alpha);
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

            if (pos.Y > GamePlayScreen.sGROUND_WORLD_1_1)
            {
                notifyCollision();
            }

            if (vel.X > 0)
                dy += ay;
            else
                dy += ay;
            pos.Y += (float)(dy * gameTime.ElapsedGameTime.TotalSeconds);
            pos.X += vel.X;
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

