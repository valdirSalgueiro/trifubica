using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Input.Touch;

namespace ColorLand
{
    public abstract class GameObject
    {

        public bool isHoldable = true;

        public String nome;

        //utilizado pra calcular a diferenca na hora do drag
        public int mDragInitialX;
        public int mDragInitialY;

        private bool mHold;


        public float mX;
        public float mY;

        private Rectangle mCollisionRect;
        private bool mCollisionEnabled = true;

        private bool mIsActive = true;
        private bool mIsVisible = true;


        private List<Sprite> mSpritesList;

        private Sprite mCurrentSprite;
        private int mCurrentSpriteIndex;

        private int mState;

        private ContentManager mContent;

        private double angle = 0;

        /////PERMISSIONS
        private bool mAllowMovement;

        public GameObject()
        {
            init();
        }

        public virtual void loadContent(ContentManager content)
        {
            this.mContent = content;

            for (int x = 0; x < mSpritesList.Count; x++)
            {
                mSpritesList.ElementAt<Sprite>(x).loadContent(content);
            }

        }

        private void init()
        {
            mSpritesList = new List<Sprite>();
            mCollisionRect = new Rectangle();
            setCollisionRect((int)mX, (int)mY, 150, 150);
            //setLocation(10, 10);
            setMovementAllowed(true);
            setActive(true);
        }

        protected void addSprite(Sprite sprite, int index)
        {
            mSpritesList.Insert(index, sprite);
            //mCurrentSprite = mSpritesList.ElementAt(0);
        }

        public Sprite getCurrentSprite()
        {
            return this.mCurrentSprite;
        }


        public int getCurrentSpriteIndex()
        {
            return this.mCurrentSpriteIndex;
        }

        public void changeToSprite(int index)
        {
            this.mCurrentSpriteIndex = index;
            mCurrentSprite = mSpritesList.ElementAt<Sprite>(index);
            mCurrentSprite.setLocation(mX, mY);
        }

        public void setCollisionRect(int x, int y, int width, int height)
        {
            if (mCollisionRect != null)
            {
                mCollisionRect.X = x;
                mCollisionRect.Y = y;
                mCollisionRect.Width = width;
                mCollisionRect.Height = height;
            }
        }

        public void setCollisionRect(int width, int height)
        {
            if (mCollisionRect != null)
            {
                mCollisionRect.X = (int)mX;
                mCollisionRect.Y = (int)mY;
                mCollisionRect.Width = width;
                mCollisionRect.Height = height;
            }
        }



        private void updateCollisionRectLocation(int x, int y)
        {
            if (mCollisionRect != null)
            {
                mCollisionRect.X = x;
                mCollisionRect.Y = y;
            }
        }

        public void updateCollisionRectLocation()
        {
            updateCollisionRectLocation((int)mX, (int)mY);
        }

        public void updateHold(int x, int y)
        {

            setLocation(x, y);

        }

        public virtual void update(GameTime gameTime)
        {
            //mCurrentSprite.setLocation(mX, mY);
            //if (mIsActive) {
            if (mIsActive)
            {
                mCurrentSprite.update();
                mCurrentSprite.setX(mX);
                mCurrentSprite.setY(mY);
                updateCollisionRectLocation((int)mX, (int)mY);
            }
            //}
        }

        public void setCenter(float x, float y)
        {
            this.mX = x - getCurrentSprite().getWidth() / 2;
            this.mY = y - getCurrentSprite().getHeight() / 2;
        }

        public void setLocation(float x, float y)
        {
            this.mX = x;
            this.mY = y;
        }

        public void setX(float x)
        {
            this.mX = x;
        }

        public void setY(float y)
        {
            this.mY = y;
        }
        
        public void setLocation(Vector2 location)
        {
            this.mX = location.X;
            this.mY = location.Y;
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            if (mIsActive)
            {
                if (mIsVisible)
                {
                    //mCurrentSprite.draw(spriteBatch, angle);
                    mCurrentSprite.draw(spriteBatch);
                }
                else
                {
                    //mIsActive = false;
                }
            }
        }

        public void setHold(bool holded)
        {

            this.mHold = holded;

        }

        public bool isHold()
        {
            return this.mHold;
        }

        public void setRGB(int R, int G, int B)
        {
            for (int x = 0; x < mSpritesList.Count; x++)
            {
                mSpritesList.ElementAt<Sprite>(x).R = R;
                mSpritesList.ElementAt<Sprite>(x).G = G;
                mSpritesList.ElementAt<Sprite>(x).B = B;
            }

        }

        public void stopMovement()
        {
            //TODO
        }

        public Rectangle getCollisionRect()
        {
            return this.mCollisionRect;
        }

        public bool collidesWith(GameObject gameObject)
        {
            if (mCollisionEnabled)
            {
                if (mCollisionRect.Intersects(gameObject.getCollisionRect()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }


        public bool collidesWith(Rectangle rectangle)
        {
            if (mCollisionEnabled)
            {
                if (mCollisionRect.Intersects(rectangle))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public bool touchedMe(int x, int y)
        {

            Point p = new Point(x, y);
           
            if(mCollisionRect.Contains(p)){
                return true;
            }else{
                return false;
            }

        }

        public Vector2 getLocation()
        {
            return new Vector2(mX, mY);
        }

        public float getX()
        {
            return this.mX;
        }

        public float getY()
        {
            return this.mY;
        }

        //permission method
        public void setMovementAllowed(bool updateMovement)
        {
            this.mAllowMovement = updateMovement;
        }

        public int getState()
        {
            return this.mState;
        }

        public void setState(int state)
        {
            this.mState = state;
        }

        public void destroy()
        {
            //mCollisionRect = new Rectangle(0,0,0,0);
            this.mIsActive = false;
        }

        public bool isActive()
        {
            return this.mIsActive;
        }

        public void setActive(bool active)
        {
            this.mIsActive = active;
        }

        public void setVisible(bool visible)
        {
            this.mIsVisible = visible;
        }

        public bool isVisible()
        {
            return this.mIsVisible;
        }

        public void setAngle(double angle)
        {
            this.angle = angle;
        }

        public void enableCollision(bool isCollisionEnabled)
        {
            this.mCollisionEnabled = isCollisionEnabled;
        }

        public bool collisionEnabled(){
            return this.mCollisionEnabled;
        }


        /*public enum enumMoveDirection
        {
            STOPPED,
            LEFT,
            RIGHT,
            UP,
            DOWN,
            UP_LEFT,
            UP_RIGHT,
            DOWN_LEFT,
            DOWN_RIGHT
        }*/

        /*public virtual void move(enumMoveDirection direction)
        {

        }*/

    }
}
