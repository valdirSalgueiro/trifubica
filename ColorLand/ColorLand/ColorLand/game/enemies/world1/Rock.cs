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
        private Texture2D[] texture;
        public Vector2 pos;
        private Boolean isActive = true;
        private Boolean collided = false;
        public float alpha = 1;
        public Vector2 vel;

        public Color type;

        float dy;
        float ay = 9.8f;

        public Rectangle collisionRect;

        int curFrame = 0;

        string[] sequenceBlue = new string[]{
                        "enemies\\Lizardo\\shoots\\blue\\blue0001",
                        "enemies\\Lizardo\\shoots\\blue\\blue0002",
                        "enemies\\Lizardo\\shoots\\blue\\blue0003",
                        "enemies\\Lizardo\\shoots\\blue\\blue0004",
                        "enemies\\Lizardo\\shoots\\blue\\blue0005",
                        "enemies\\Lizardo\\shoots\\blue\\blue0006",
                        "enemies\\Lizardo\\shoots\\blue\\blue0007",
                        "enemies\\Lizardo\\shoots\\blue\\blue0008",
                        "enemies\\Lizardo\\shoots\\blue\\blue0009",
                        "enemies\\Lizardo\\shoots\\blue\\blue0010",
                        "enemies\\Lizardo\\shoots\\blue\\blue0011",
                        "enemies\\Lizardo\\shoots\\blue\\blue0012",
                        "enemies\\Lizardo\\shoots\\blue\\blue0013",
                        "enemies\\Lizardo\\shoots\\blue\\blue0014",
        };

        string[] sequenceRed = new string[]{
                        "enemies\\Lizardo\\shoots\\red\\red0001",
                        "enemies\\Lizardo\\shoots\\red\\red0002",
                        "enemies\\Lizardo\\shoots\\red\\red0003",
                        "enemies\\Lizardo\\shoots\\red\\red0004",
                        "enemies\\Lizardo\\shoots\\red\\red0005",
                        "enemies\\Lizardo\\shoots\\red\\red0006",
                        "enemies\\Lizardo\\shoots\\red\\red0007",
                        "enemies\\Lizardo\\shoots\\red\\red0008",
                        "enemies\\Lizardo\\shoots\\red\\red0009",
                        "enemies\\Lizardo\\shoots\\red\\red0010",
                        "enemies\\Lizardo\\shoots\\red\\red0011",
                        "enemies\\Lizardo\\shoots\\red\\red0012",
                        "enemies\\Lizardo\\shoots\\red\\red0013",
                        "enemies\\Lizardo\\shoots\\red\\red0014",
        };

        string[] sequenceGreen = new string[]{
                        "enemies\\Lizardo\\shoots\\green\\green0001",
                        "enemies\\Lizardo\\shoots\\green\\green0002",
                        "enemies\\Lizardo\\shoots\\green\\green0003",
                        "enemies\\Lizardo\\shoots\\green\\green0004",
                        "enemies\\Lizardo\\shoots\\green\\green0005",
                        "enemies\\Lizardo\\shoots\\green\\green0006",
                        "enemies\\Lizardo\\shoots\\green\\green0007",
                        "enemies\\Lizardo\\shoots\\green\\green0008",
                        "enemies\\Lizardo\\shoots\\green\\green0009",
                        "enemies\\Lizardo\\shoots\\green\\green0010",
                        "enemies\\Lizardo\\shoots\\green\\green0011",
                        "enemies\\Lizardo\\shoots\\green\\green0012",
                        "enemies\\Lizardo\\shoots\\green\\green0013",
                        "enemies\\Lizardo\\shoots\\green\\green0014",
        };

        public Rock(Vector2 vel_, Color type_)
        {
            type = type_;
            vel = vel_;
            dy = vel.Y;
        }

        public void loadContent(ContentManager content)
        {
            if (type == Color.Red)
            {
                texture = new Texture2D[sequenceRed.Count()];
                for (int i = 0; i < sequenceRed.Count(); i++)
                {
                    texture[i] = content.Load<Texture2D>(sequenceRed[i]);
                }
            }
            if (type == Color.Blue)
            {
                texture = new Texture2D[sequenceBlue.Count()];
                for (int i = 0; i < sequenceBlue.Count(); i++)
                {
                    texture[i] = content.Load<Texture2D>(sequenceBlue[i]);
                }
            }
            else if (type == Color.Green)
            {
                texture = new Texture2D[sequenceGreen.Count()];
                for (int i = 0; i < sequenceGreen.Count(); i++)
                {
                    texture[i] = content.Load<Texture2D>(sequenceGreen[i]);                    
                }
            }
            else
            {
                texture = new Texture2D[] { content.Load<Texture2D>("enemies\\rocker\\rock\\Pedra") };                    
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture[curFrame], collisionRect, Color.White * alpha);
            curFrame++;
            if (curFrame > texture.Count()-1)
                curFrame = 0;
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

