using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    class BaseEnemy : PaperObject {

        //SpriteFont mFontDebug = Game1.getInstance().getScreenManager().getContent().Load<SpriteFont>("debug");

        public const int sTYPE_SIMPLE_FLYING_RED        = 0;
        public const int sTYPE_SIMPLE_FLYING_GREEN      = 1;
        public const int sTYPE_SIMPLE_FLYING_BLUE       = 2;

        public const int sTYPE_SIMPLE_GROUND_RED        = 3;
        public const int sTYPE_SIMPLE_GROUND_GREEN      = 4;
        public const int sTYPE_SIMPLE_GROUND_BLUE       = 5;
        

        //Specific
        private int mType;
        
        
        public BaseEnemy(int type)
        {

            this.mType = type;

        }

        public void loadContent(ContentManager content) {
            base.loadContent(content);
        }

        public override void update(GameTime gameTime) {

            base.update(gameTime);
        }

       
        public override void draw(SpriteBatch spriteBatch) {
            if (isActive())
            {
                base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);
               // spriteBatch.DrawString(mFontDebug, /*" ATE: " + mAlreadyAte + " ColEnabled: " + collisionEnabled() +*/" Rect: " + getCollisionRect(), new Vector2(0, 150), Color.Yellow);
            }
        }

        public void setType(int type)
        {
            this.mType = type;
        }

        public int getType()
        {
            return this.mType;
        }

    }

}
