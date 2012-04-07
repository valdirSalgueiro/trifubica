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

        /// SPECIFIC FOR ENEMY MANAGER
        private bool mReady;
        // /

        //center hotspot

        private Vector2 mCenterHotspot;
        private float mXCenterDiff;
        private float mYCenterDiff;

        //

        private Direction mDirection;
        private float mSpeed;

        public bool isHoldable = true;

        public String nome;

        //utilizado pra calcular a diferenca na hora do drag
        public int mDragInitialX;
        public int mDragInitialY;

        private bool mHold;

        public float mX;
        public float mY;

        private Rectangle mAttackCollisionRect;
        private bool mAttackCollisionEnabled = true;

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

        private FlipDirection mFlipDirection;

        /////PERMISSIONS
        private bool mAllowMovement;


        private int mInitialCollisionRectX;
        private int mInitialCollisionRectY;

        private int mInitialAttackCollisionRectX;
        private int mInitialAttackCollisionRectY;

        //extra
        private Texture2D mCollisionTexture;

        public enum FlipDirection
        {
            Left,
            Right
        }


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

        //provisorio para este projeto
        public Vector2 getPlayerPosition()
        {
            BaseScreen currentScreen = Game1.getInstance().getScreenManager().getCurrentScreen();
            if (currentScreen is GamePlayScreen)
            {
                return ((GamePlayScreen)currentScreen).getPlayerLocation();
            }

            return new Vector2();
        }
        //// 

        private void init()
        {
            mSpritesList = new List<Sprite>();
            mCollisionRect = new Rectangle();
            setCollisionRect((int)mX, (int)mY, 150, 150);
            //setLocation(10, 10);
            setMovementAllowed(true);
            setActive(true);

            showCollisionRect();
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

                mInitialCollisionRectX = x;
                mInitialCollisionRectY = y;
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

                mInitialCollisionRectX = mCollisionRect.X;
                mInitialCollisionRectY = mCollisionRect.Y;
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

        public void updateAttackCollisionRectLocation()
        {
            if (mAttackCollisionRect != null)
            {
                mAttackCollisionRect.X = (int)mX + mInitialAttackCollisionRectX;
                mAttackCollisionRect.Y = (int)mY + mInitialAttackCollisionRectY;
            }
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
                updateCollisionRectLocation((int)mX + mInitialCollisionRectX, (int)mY + mInitialCollisionRectY);
                updateAttackCollisionRectLocation();
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

            mCurrentSprite.update();
            mCurrentSprite.setX(mX);
            mCurrentSprite.setY(mY);
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

                    if (mCollisionTexture != null)
                    {
                        spriteBatch.Draw(mCollisionTexture, getCollisionRect(), new Color(0, 0, 0, 0.5f));

                        spriteBatch.Draw(mCollisionTexture, getAttackRectangle(), new Color(0, 0, 0, 0.9f));
                    }
                }
                else
                {
                    //mIsActive = false;
                }
            }
        }

        public virtual void draw(SpriteBatch spriteBatch, Color color)
        {
            if (mIsActive)
            {
                if (mIsVisible)
                {
                    //mCurrentSprite.draw(spriteBatch, angle);
                    mCurrentSprite.draw(spriteBatch,color);

                    if (mCollisionTexture != null)
                    {
                        spriteBatch.Draw(mCollisionTexture, getCollisionRect(), new Color(0, 0, 0, 0.5f));
                        spriteBatch.Draw(mCollisionTexture, getAttackRectangle(), new Color(0, 0, 0, 0.9f));
                    }
                }
                else
                {
                    //mIsActive = false;
                }
            }
        }

        public virtual void draw(SpriteBatch spriteBatch, float rotationAngle)
        {
            if (mIsActive)
            {
                if (mIsVisible)
                {
                    //mCurrentSprite.draw(spriteBatch, angle);
                    mCurrentSprite.draw(spriteBatch, rotationAngle);

                    if (mCollisionTexture != null)
                    {
                        spriteBatch.Draw(mCollisionTexture, getCollisionRect(), new Color(0, 0, 0, 0.5f));
                        spriteBatch.Draw(mCollisionTexture, getAttackRectangle(), new Color(0, 0, 0, 0.9f));
                    }
                }
            }
        }

        public virtual void draw(SpriteBatch spriteBatch, float rotationAngle, float zoomScale)
        {
            if (mIsActive)
            {
                if (mIsVisible)
                {
                    //mCurrentSprite.draw(spriteBatch, angle);
                    mCurrentSprite.draw(spriteBatch, rotationAngle, zoomScale);

                    if (mCollisionTexture != null)
                    {
                        spriteBatch.Draw(mCollisionTexture, getCollisionRect(), new Color(0, 0, 0, 0.5f));
                        spriteBatch.Draw(mCollisionTexture, getAttackRectangle(), new Color(0, 0, 0, 0.9f));
                    }
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

        public bool attackRecCollidesWith(GameObject gameObject)
        {
            if (mAttackCollisionEnabled)
            {
                if (mAttackCollisionRect.Intersects(gameObject.getCollisionRect()))
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
            //Game1.print("FUCK GET: " + mState);
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
            enableCollision(false);
            //setLocation(1000, 1000);
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

        public void moveLeft(int pixels)
        {
            mX -= pixels;
        }

        public void moveRight(int pixels)
        {
            mX += pixels;
        }

        public void setDirection(Direction direction)
        {
            mDirection = direction;
        }

        public Direction getDirection()
        {
            return this.mDirection;
        }

        public void setSpeed(float speed)
        {
            mSpeed = speed;
        }

        public float getSpeed()
        {
            return this.mSpeed;
        }

        public void setFlipDirection(FlipDirection direction)
        {
            mFlipDirection = direction;
        }

        public FlipDirection getFlipDirection()
        {
            return this.mFlipDirection;
        }

        public void setAttackRectangle(int x, int y, int width, int height)
        {
            mAttackCollisionRect = new Rectangle(x, y, width, height);
            mInitialAttackCollisionRectX = x;
            mInitialAttackCollisionRectY = y;
        }

        public void setAttackRectangle(Rectangle r)
        {
            mAttackCollisionRect = r;
        }

        public Rectangle getAttackRectangle()
        {
            return mAttackCollisionRect;
        }

        public void setReady(bool ready)
        {
            this.mReady = ready;
        }

        public bool isReady()
        {
            return this.mReady;
        }

        public float getCenterX()
        {
            return mX + (getCurrentSprite().getWidth() / 2);
        }

        public float getCenterY()
        {
            return mY + (getCurrentSprite().getHeight() / 2);
        }

        public Vector2 getCenter()
        {
            return new Vector2(getCenterX(), getCenterY());
        }


        //not relative to the image, but relative to the enemy
        public Vector2 getCenterHotspot()
        {
            return mCenterHotspot;
        }

        public void setCenterHotspot(Vector2 center)
        {
            mCenterHotspot = center;
            mXCenterDiff = mCenterHotspot.X;
            mYCenterDiff = mCenterHotspot.Y;
        }

        public void updateCenterHotspot()
        {
            if (mCenterHotspot != null)
            {
                mCenterHotspot.X = mX + mXCenterDiff;
                mCenterHotspot.Y = mY + mYCenterDiff;
            }
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

        public enum Direction
        {
            NONE,
            LEFT,
            RIGHT,
            UP,
            DOWN
        }

        public void showCollisionRect()
        {
            mCollisionTexture = Game1.getInstance().getScreenManager().getContent().Load<Texture2D>("fades\\blackfade");
        }

        public Vector2 getInitialCollisionDimension()
        {
            return new Vector2(mInitialCollisionRectX,mInitialCollisionRectY);
        }

    }
}
