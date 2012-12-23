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
	public class MenuGrade
	{

        private const int cSPACE_BETWEEN_OPTIONS = 80;

        private int mCurrentIndex;

        private List<MenuOption> mListOption;

        private Vector2 mLocation;
        private int mSpacementY;

        public MenuGrade(Vector2 location)
        {
            mListOption = new List<MenuOption>();
            mLocation = location;
            mSpacementY = (int)location.Y;
        }

        public void addOption(String name)
        {
            mListOption.Add(new MenuOption(name,new Vector2(mLocation.X, mSpacementY += 100))); 
        }

        public void draw(SpriteBatch spritebatch)
        {

        }

        public void update(GameTime gameTime)
        {

        }

	}

    public class MenuOption {

        private String mText;

        private SpriteFont mFont;

        private Vector2 mLocation;

        public MenuOption(String text, Vector2 location)
        {
            mText = text;
            mLocation = location;
        }

        public void draw(SpriteBatch batch)
        {
            batch.DrawString(mFont, mText, mLocation, Color.Black);
        }

    }
}
