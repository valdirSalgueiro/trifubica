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

        public static int sPLAYER_LAYER = 0;

        public String nome;

        private Texture2D mImage;

        public int R = 255;
        public int G = 255;
        public int B = 255;
        public int ALPHA = 255;

        String mImagePath;

        private float mX;
        private float mY;
        private int mWidth;
        private int mHeight;
        private Vector2 mLocationVector;

        private List<Sprite> mListParts;
        private int mVolatileIndexForParts;

        private Color[] color;

        private float oldX=0.0f;

        public Background()
        {
            mListParts = new List<Sprite>();
        }

        public Background(String imagePath)
        {

            mImagePath = imagePath;

            mListParts = new List<Sprite>();
        }

        public void addPart(String[] images, int tickspeed, int width, int height, int x, int y)
        {

            Sprite s = new Sprite(images, null, tickspeed, width, height, false, false);

            s.setLocation(x, y);

            mListParts.Add(s);

        }

        public void setPlayerLayer()
        {
            mListParts.Add(null);
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

            if (mImagePath != null)
            {
                mImage = content.Load<Texture2D>(mImagePath);
                mWidth = mImage.Width;
                mHeight = mImage.Height;
            }
            foreach (Sprite s in mListParts)
            {
                s.loadContent(content);
            }
            color = new Color[mImage.Width * mImage.Height];
            mImage.GetData<Color>(color);
            saturate(0.0f);
        }

        private void saturate(float x)
        {
            x /= 100.0f;
            //x = 1.0f;
            Color[] newColor = new Color[mImage.Width * mImage.Height];
            for (int i = 0; i < mImage.Width * mImage.Height; i++)
            {
                var avg = (byte)((0.3 * color[i].R + 0.59 * color[i].G + 0.11 * color[i].B));


                byte r = color[i].R;
                byte g = color[i].G;
                byte b = color[i].B;

                newColor[i].R = (byte)(avg + x * (r - avg));
                newColor[i].G = (byte)(avg + x * (g - avg));
                newColor[i].B = (byte)(avg + x * (b - avg));
            }
            mImage.SetData<Color>(newColor);
        }

        public void update()
        {
            foreach (Sprite s in mListParts)
            {
               s.update();
            }
        }

        public void draw(SpriteBatch spritebatch, float x)
        {
            if (x != oldX) {
                saturate(x);
                oldX = x;
            }

            //o draw que ta sendo chamado eh esse, Valdir
            //new Color(0.3f, 0.59f, 0.11f, ALPHA)
            if (mImagePath != null)
            {
                spritebatch.Draw(mImage, mLocationVector, Color.White);
            }

            foreach (Sprite s in mListParts)
            {
                s.draw(spritebatch);
            }

        }

        //mas vc pode chamar esse daqui la em gameplay screen, que eh o mais recomendavel
        public void draw(SpriteBatch spritebatch, Color color)
        {            
            if (mImagePath != null)
            {
                spritebatch.Draw(mImage, mLocationVector, new Color(R * ALPHA / 255, G * ALPHA / 255, B * ALPHA / 255, ALPHA));
            }

            foreach (Sprite s in mListParts)
            {
                s.draw(spritebatch,color);
            }

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

        public Texture2D getTexture()
        {
            return this.mImage;
        }

        public Rectangle getRectangle()
        {
            return new Rectangle((int)mX, (int)mY, mWidth, mHeight);
        }

        public int getTotalOfParts()
        {
            return mListParts.Count;
        }
    }
}
