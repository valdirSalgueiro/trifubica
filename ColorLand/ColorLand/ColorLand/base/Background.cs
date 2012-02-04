using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    public class Background
    {

        public String nome;

        private Texture2D mImage;

        public int R = 255;
        public int G = 255;
        public int B = 255;
        public int ALPHA = 255;

        String mImagePath;

        private float mX;
        private float mY;
        private Vector2 mLocationVector;

        public Background(String imagePath)
        {

            mImagePath = imagePath;

        }

        public void setLocation(int x, int y)
        {
            mX = x;
            mY = y;
            mLocationVector = new Vector2(x, y);
        }

        public Vector2 getLocation()
        {
            return mLocationVector;
        }

        public void loadContent(ContentManager content)
        {

            mImage = content.Load<Texture2D>(mImagePath);
            
        }

        public void update()
        {

        }

        public void draw(SpriteBatch spritebatch)
        {
            //se nÃ£o tiver aparecendo nada Ã© pq width e height nao foram setados
            spritebatch.Draw(mImage, mLocationVector, new Color(R * ALPHA / 255, G * ALPHA / 255, B * ALPHA / 255, ALPHA));
        }

        public bool touchedMe(int x, int y)
        {
            Point p = new Point(x, y);

            Rectangle r = new Rectangle((int)mX, (int)mY, mImage.Bounds.Width, mImage.Bounds.Height);

            if (r.Contains(p))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
