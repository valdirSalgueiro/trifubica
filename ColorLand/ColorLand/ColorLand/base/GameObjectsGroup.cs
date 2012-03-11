using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    public class GameObjectsGroup <T> where T : GameObject {

        private List<T> mList;

        private T mCollidedObject;
        private T mCollidedPassiveObject;

        private int mManualIndex; //only for the getNext() method. Do not touch this 
        

        public GameObjectsGroup() {
            mList = new List<T>();
        }

        public void addGameObject(T item) {
            mList.Add(item);
        }


        public void remove(int index) {
            mList.RemoveAt(index);
        }

        public void remove(T gameObject)
        {
            mList.Remove(gameObject);
        }

        public T getGameObject(int pos) {
            return mList.ElementAt(pos);
        }

        public int getSize() {
            return mList.Count;
        }

        public void loadContent(ContentManager contentManager) {
            for (int x = 0; x < mList.Count; x++) {
                mList.ElementAt(x).loadContent(contentManager);
            }
        }

        //TODO getGameObject por referencia

        //TODO o resto dos metodos

        public void update(GameTime gameTime) {
            for (int x = 0; x < mList.Count; x++) {
                //if (mList.ElementAt(x) is Airplane)
               mList.ElementAt(x).update(gameTime);
            }
        }

        public void draw(SpriteBatch spriteBatch) {
            for (int x = 0; x < mList.Count; x++) {
                mList.ElementAt(x).draw(spriteBatch);
            }
        }

        public bool checkCollisionWith(GameObject gameObject)
        {
            for (int x = 0; x < mList.Count; x++)
            {
               //  Game1.print("X,Y ELEMENT: " + mList.ElementAt(x).getX() + mList.ElementAt(x).getY() + "--- X,Y BUCCET: " + gameObject.getX() + gameObject.getY());
                if (mList.ElementAt(x).collidesWith(gameObject))
                {
                    mCollidedObject = mList.ElementAt(x);
                    return true;
                }
            }

            mCollidedObject = null;
            return false;
        }

        public bool checkAttackCollisionWith(GameObject gameObject)
        {
            for (int x = 0; x < mList.Count; x++)
            {
                //  Game1.print("X,Y ELEMENT: " + mList.ElementAt(x).getX() + mList.ElementAt(x).getY() + "--- X,Y BUCCET: " + gameObject.getX() + gameObject.getY());
                if (mList.ElementAt(x).attackRecCollidesWith(gameObject))
                {
                    mCollidedObject = mList.ElementAt(x);
                    return true;
                }
            }

            mCollidedObject = null;
            return false;
        }

        /*public bool checkCollisionWith<T>(GameObjectsGroup<T> gameObjects) where T : PaperObject
        {
            for (int x = 0; x < mList.Count; x++)
            {

                for (int k = 0; k < (gameObjects.getSize()); k++)
                {
                    if (mList.ElementAt(x).collidesWith(gameObjects.getGameObject(k)))
                    {
                        mCollidedObject = mList.ElementAt(x);
                        mCollidedPassiveObject = gameObjects.getGameObject(k);
                        return true;
                    }
                }
                //  Game1.print("X,Y ELEMENT: " + mList.ElementAt(x).getX() + mList.ElementAt(x).getY() + "--- X,Y BUCCET: " + gameObject.getX() + gameObject.getY());
                
            }

            mCollidedObject = null;
            mCollidedPassiveObject = null;
            return false;
        }*/

        public T getCollidedObject()
        {
            return mCollidedObject;
        }

        public T getCollidedPassiveObject()
        {
            return mCollidedPassiveObject;
        }

        public void clear()
        {
            mList.Clear();
            mCollidedObject = null;
            mCollidedPassiveObject = null;
        }

        public bool contains(GameObject gameObject)
        {

            for (int x = 0; x < mList.Count; x++)
            {
                if(mList.ElementAt(x) == gameObject)
                {
                    return true;
                }
            }

            return false;
        }

        public void deactivateGameObject(int index)
        {
            for (int x = 0; x < mList.Count; x++)
            {
                if (x == index)
                {
                    mList.ElementAt(x).setActive(false);
                }
            }

        }

        public T getNext()
        {
            T next = mList.ElementAt(mManualIndex);
            mManualIndex++;
            return next;
        }

        public void deactivateGameObject(GameObject gameObject)
        {
            for (int x = 0; x < mList.Count; x++)
            {
                if (mList.ElementAt(x) == gameObject)
                {
                    mList.ElementAt(x).setActive(false);
                }
            }
        }

        public void deactivateAllGameObjects()
        {
            for (int x = 0; x < mList.Count; x++)
            {
                mList.ElementAt(x).setActive(false);
            }
        }

    }
}
