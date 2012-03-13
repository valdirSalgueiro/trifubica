using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ColorLand
{
    public class BaseEnemy : GameObject {

        public const int sTYPE_SIMPLE_FLYING_RED = 0;
        public const int sTYPE_SIMPLE_FLYING_GREEN = 1;
        public const int sTYPE_SIMPLE_FLYING_BLUE = 2;

        public const String cSOUND_EXPLOSION = "sound\\fx\\explosao8bit";
        private Color mColor;
        
        public BaseEnemy(Color color)
        {
            mColor = color;
        }

        public BaseEnemy(Color color, Vector2 origin)
        {
            mColor = color;
            setLocation(origin);
        }

        public override void loadContent(ContentManager content) {
            base.loadContent(content);
            SoundManager.LoadSound(cSOUND_EXPLOSION);
        }

        public override void update(GameTime gameTime) {

            base.update(gameTime);
            updateCenterHotspot();
        }

       
        public override void draw(SpriteBatch spriteBatch) {
            if (isActive())
            {
                base.draw(spriteBatch);//getCurrentSprite().draw(spriteBatch);
               // spriteBatch.DrawString(mFontDebug, /*" ATE: " + mAlreadyAte + " ColEnabled: " + collisionEnabled() +*/" Rect: " + getCollisionRect(), new Vector2(0, 150), Color.Yellow);
            }
        }

        public Vector2 getPlayerPosition()
        {
            BaseScreen currentScreen = Game1.getInstance().getScreenManager().getCurrentScreen();
            if (currentScreen is GamePlayScreen)
            {
                return ((GamePlayScreen)currentScreen).getPlayerLocation();
            }

            return new Vector2();
        }

        public float getPlayerCenter()
        {
            BaseScreen currentScreen = Game1.getInstance().getScreenManager().getCurrentScreen();
            if (currentScreen is GamePlayScreen)
            {
                return ((GamePlayScreen)currentScreen).getPlayerCenter();
            }

            return 0f;
        }

        public Color getColor()
        {
            return this.mColor;
        }

        public void destroy()
        {
            base.destroy();
            SoundManager.PlaySound(cSOUND_EXPLOSION);
        }

    }

}
