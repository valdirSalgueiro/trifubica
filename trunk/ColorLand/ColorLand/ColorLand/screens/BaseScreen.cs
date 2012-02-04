using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace ColorLand
{
    public abstract class BaseScreen
    {

        //private GestureType mEnabledGestures;

        private bool mChangeMe = false;
        private bool mReleaseMe;
        private int mNextScreenId;
        
        public BaseScreen()
        {
            //mEnabledGestures = GestureType.None;
        }

        /*public void setGestures(GestureType value)
        {
            mEnabledGestures = value;
            TouchPanel.EnabledGestures = value;
        }

        public GestureType getGesture()
        {
            return mEnabledGestures;
        }*/

        public virtual void update(GameTime gameTime) { }
        public virtual void draw(GameTime gameTime) { }
        public virtual void handleInput(InputState input) { }

        public void releaseScreenResources() {
          //  this.mContentManager.Dispose();
        }

        //public ContentManager getContentManager() {
            //return mContentManager;
        //}

        public void setChangeMe(bool changeMe, int toScreenId, bool releaseMe) {
            this.mChangeMe = changeMe;
            this.mNextScreenId = toScreenId;
            this.mReleaseMe = releaseMe;
        }

        public bool hasRequestedScreenChange() {
            return this.mChangeMe;
        }

        public int getNextScreenId() {
            return this.mNextScreenId;
        }

        public bool shouldReleaseMe() {
            return this.mReleaseMe;
        }



    }
    
}