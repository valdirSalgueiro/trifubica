using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    public class PaperObject : GameObject
    {

        private bool mSelecionMode;

        private int mXAfterFadingHorizontal;

        private String mName;

        /***********
         * FADE OUT
         ***********/
        //fade out left
        private bool mFadingOutLeft;
        private int mFadingOutLeftToX;

        //fade out right
        private bool mFadingOutRight;
        private int mFadingOutRightToX;

        //fade out top
        private bool mFadingOutTop;
        private int mFadingOutTopY;

        //fade out bottom
        private bool mFadingOutBottom;
        private int mFadingOutBottomY;

        public int FADE_SPEED = 40;


        /***********
         * FADE IN
        ***********/
        //fade int from bottom
        private bool mFadingInBottomToTop;
        private int mFadingInBottomToTopY;

        private bool mFadingInTopToBottom;
        private int mFadingInTopToBottomY;

        private bool mFadingInLeftToRight;
        private int  mFadingInLeftToRightX;

        private bool mFadingInRightToLeft;
        private int  mFadingInRightToLeftX;

        public PaperObject()
        {

        }

        public void allowSelectionMode(bool selectionAllowed)
        {
            this.mSelecionMode = true;
        }

        public bool isSelectionMode()
        {
            return this.mSelecionMode;
        }


        public void fadeOutLeft()
        {
            setActive(false);
            mFadingOutLeft = true;
            mFadingOutLeftToX = -300;
        }

        public void fadeOutRight(int finalPosition)
        {

            setActive(false);
            mFadingOutRight = true;
            mFadingOutRightToX = 900;
            mXAfterFadingHorizontal = finalPosition;
        }

        public void fadeOutTop()
        {
            setActive(false);
            mFadingOutTop = true;
            mFadingOutTopY = -200;
        }

        public void fadeOutBottom()
        {
            setActive(false);
            mFadingOutBottom = true;
            mFadingOutBottomY = -200;
        }

        public void fadeInFromLeftToRight(int toX)
        {
            setActive(false);
            mFadingInLeftToRight = true;
            mFadingInLeftToRightX = toX;
        }

        public void fadeInFromRightToleft(int toX)
        {
            setActive(false);
            mFadingInRightToLeft = true;
            mFadingInRightToLeftX = toX;

        }

        public void fadeInFromBottomToTop(int toY)
        {
            setActive(false);
            mFadingInBottomToTop = true;
            mFadingInBottomToTopY = toY;

        }

        public void fadeInFromTopToBottom(int toY)
        {
            setActive(false);
            mFadingInTopToBottom = true;
            mFadingInTopToBottomY = toY;

        }

        public virtual void update(GameTime gameTime)
        {
            base.update(gameTime);
            
            if (mFadingOutLeft)
            {
                if (mX > mFadingOutLeftToX)
                {
                    mX -= FADE_SPEED;
                }
                else
                {
                    setActive(true);
                    mFadingOutLeft = false;
                }
            }
            else
            if (mFadingOutRight)
            {
                if (mX < mFadingOutRightToX)
                {
                    mX += FADE_SPEED;
                }
                else
                {
                    setActive(true);
                    mFadingOutRight = false;
                    mX = mXAfterFadingHorizontal;
                }
            }
            else
            if (mFadingOutTop)
            {
                if (mY > mFadingOutTopY)
                {
                    mY -= FADE_SPEED;
                }
                else
                {
                    setActive(true);
                    mFadingOutTop = false;
                }
            }
            else
            if (mFadingOutTop)
            {
                if (mY > mFadingOutTopY)
                {
                    mY -= FADE_SPEED;
                }
                else
                {
                    setActive(true);
                    mFadingOutTop = false;
                }
            }
            else
            if (mFadingOutBottom)
            {
                if (mY < mFadingOutBottomY)
                {
                    mY += FADE_SPEED;
                }
                else
                {
                    setActive(true);
                    mFadingOutBottom = false;
                }
            }
            else
                if (mFadingInLeftToRight)
            {
                if (mX < mFadingInLeftToRightX)
                {
                    mX += FADE_SPEED;
                }
                else
                {
                    setActive(true);
                    mFadingInLeftToRight = false;
                }
            }
            else
            if (mFadingInRightToLeft)
            {
                if (mX > mFadingInRightToLeftX)
                {
                    mX -= FADE_SPEED;
                }
                else
                {
                    setActive(true);
                    mFadingInRightToLeft = false;
                }
            }
            else
            if (mFadingInBottomToTop)
            {
                if (mY > mFadingInBottomToTopY)
                {
                    mY -= FADE_SPEED;
                }
                else
                {
                    setActive(true);
                    mFadingInBottomToTop = false;
                }
            }
            else
            if (mFadingInTopToBottom)
            {
                if (mY < mFadingInTopToBottomY)
                {
                    mY += FADE_SPEED;
                }
                else
                {
                    setActive(true);
                    mFadingInTopToBottom = false;
                }
            
            }

        }

        public void setNome(String name)
        {
            this.mName = name;
        }

        public String getNome()
        {
            return this.mName;
        }
        
        /*public void handleGestureDEBUG(GestureSample gesture)
        {

            if (isActive())
            {

                if (gesture.GestureType == GestureType.FreeDrag)
                {

                    if (!isHold() && touchedMe((int)gesture.Position.X, (int)gesture.Position.Y))
                    {
                        setHold(true);
                        mDragInitialX = (int)gesture.Position.X - getCollisionRect().Left;
                        mDragInitialY = (int)gesture.Position.Y - getCollisionRect().Top;
                    }

                    //updateHold((int)gesture.Position.X, (int)gesture.Position.Y);
                    //Game1.print("Eh pra ir pra: " + (int)gesture.Position.X + " - " + (int)gesture.Position.Y);

                    if (isHold())
                    {
                        setLocation((int)gesture.Position.X - mDragInitialX,
                            (int)gesture.Position.Y - mDragInitialY);
                    }

                }
                if (gesture.GestureType == GestureType.DragComplete ||
                    gesture.GestureType == GestureType.None)
                {

                    setHold(false);

                }

            }

        }
        */
        /* public virtual void handleGesture(GestureSample gesture) {}

         public virtual void handleTouch(TouchLocation touch) { }
        */
        /*
         public void handleGestureDEBUG(GestureSample gesture)
         {
            
             if (isActive())
             {

                 if (gesture.GestureType == GestureType.FreeDrag)
                 {
                    
                     if (!isHold() && touchedMe((int)gesture.Position.X, (int)gesture.Position.Y))
                     {
                         setHold(true);
                         mDragInitialX = (int)gesture.Position.X - getCollisionRect().Left;
                         mDragInitialY = (int)gesture.Position.Y - getCollisionRect().Top;
                     }

                     //updateHold((int)gesture.Position.X, (int)gesture.Position.Y);
                     //Game1.print("Eh pra ir pra: " + (int)gesture.Position.X + " - " + (int)gesture.Position.Y);

                     if (isHold())
                     {
                         setLocation((int)gesture.Position.X - mDragInitialX,
                             (int)gesture.Position.Y - mDragInitialY);
                     }

                 }
                 if (gesture.GestureType == GestureType.DragComplete ||
                     gesture.GestureType == GestureType.None)
                 {

                     setHold(false);

                 }

             }
            
         }*/

    }
}
