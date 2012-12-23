using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;

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

        private float oldX = 0.0f;

        Dictionary<Texture2D, Color[]> textures = new Dictionary<Texture2D, Color[]>();
        private BackgroundWorker bw = new BackgroundWorker();
        private bool saturated;

        private float saturation;

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!saturated)
            {
                saturated = true;
                foreach (var pair in textures)
                {
                    saturate(saturation, pair.Key);
                }
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            saturated = false;
        }

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

        public Sprite getPart(int index)
        {
            return mListParts.ElementAt(index);
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
            saturated = false;
            if (mImagePath != null)
            {
                mImage = content.Load<Texture2D>(mImagePath);
                mWidth = mImage.Width;
                mHeight = mImage.Height;

                Color[] color = new Color[mImage.Width * mImage.Height];
                mImage.GetData<Color>(color);
                textures[mImage] = color;
            }
            foreach (Sprite s in mListParts)
            {
                s.loadContent(content);
                Texture2D tex = s.getCurrentTexture2D();
                Color[] color = new Color[tex.Width * tex.Height];
                tex.GetData<Color>(color);
                textures[tex] = color;
            }

        }

        public void saturateAll(float x)
        {
            if (!saturated)
            {
                saturation = x;
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                bw.RunWorkerAsync();
            }

        }

        public void saturate(float x, Texture2D tex_)
        {
            x /= 100.0f;
            saturateImpl(x, textures[tex_], ref tex_);
        }

        private void saturateImpl(float x, Color[] color_, ref Texture2D image)
        {
            Color[] newColor = new Color[image.Width * image.Height];
            for (int i = 0; i < image.Width * image.Height; i++)
            {
                byte r = color_[i].R;
                byte g = color_[i].G;
                byte b = color_[i].B;
                byte a = color_[i].A;
                if (a == 0) continue;

                var avg = (byte)((0.3 * color_[i].R + 0.59 * color_[i].G + 0.11 * color_[i].B));

                newColor[i].R = (byte)((avg + x * (r - avg)));
                newColor[i].G = (byte)((avg + x * (g - avg)));
                newColor[i].B = (byte)((avg + x * (b - avg)));
                newColor[i].A = a;
            }
            try
            {
                image.SetData<Color>(newColor);
            }
            catch (Exception e)
            {
            }
        }

        public void update()
        {
            foreach (Sprite s in mListParts)
            {
                s.update();
            }
        }

        public void draw(SpriteBatch spritebatch)
        {
            //o draw que ta sendo chamado eh esse, Valdir
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
                spritebatch.Draw(mImage, mLocationVector, color);
            }

            foreach (Sprite s in mListParts)
            {
                s.draw(spritebatch, color);
            }

        }

        public void addXToAllParts(float x)
        {
            foreach (Sprite s in mListParts)
            {
                s.addX(x);
            }
        }

        public void reduceXToAllParts(float x)
        {
            foreach (Sprite s in mListParts)
            {
                s.reduceX(x);
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
