using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace ColorLand
{
    public class BalloonsManager
    {

        private List<Balloon> mList = new List<Balloon>();



        public BalloonsManager(List<Balloon> list)
        {
            mList = list;
        }

        public BalloonsManager(){

        }

        /**
         * ADDS AND LOAD CONTENT
         * */
        public void add(Balloon balloonObject, ContentManager content)
        {
            balloonObject.loadContent(content);
            mList.Add(balloonObject);
        }

        public void addBalloon(Color color, Balloon.BTYPE type, Vector2 location, ContentManager content)
        {
            Balloon balloon = new Balloon(color, location, type);
            balloon.loadContent(content);
            mList.Add(balloon);
            balloon = null;
        }

        /*public void addExplosion(int howMany, Color color, ContentManager content)
        {
            for (int x = 0; x < howMany; x++)
            {
                Explosion explosion = new Explosion(color);
                explosion.loadContent(content);
                mList.Add(explosion);
            }
        }*/

        public void update(GameTime gameTime)
        {
            for (int x = 0; x < mList.Count; x++)
            {
                //if (mList.ElementAt(x) is Airplane)
                mList.ElementAt(x).update(gameTime);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < mList.Count; x++)
            {
                mList.ElementAt(x).draw(spriteBatch);
            }
        }

        public Balloon getNextOfColor(Color color)
        {

            //if color == red
            foreach (Balloon e in mList)
            {

                if (e.getColor() == color)
                {
                    /*if (e.())
                    {
                        return e;
                    }*/
                }
            }
            
            return mList.ElementAt(0);
        }
    }
}
