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
    public class ExplosionManager
    {

        private List<Explosion> mList = new List<Explosion>();



        public ExplosionManager(List<Explosion> list)
        {
            mList = list;
        }

        public ExplosionManager(){

        }

        /**
         * ADDS AND LOAD CONTENT
         * */
        public void add(Explosion explosionObject, ContentManager content)
        {
            explosionObject.loadContent(content);
            mList.Add(explosionObject);
        }

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

        public Explosion getNextOfColor(/*Color color*/)
        {

            //if color == red
            foreach (Explosion e in mList)
            {

                if (e.isAvailableToExplode())
                {

                    return e;
                }

            }
            
            return mList.ElementAt(0);
        }
    }
}
