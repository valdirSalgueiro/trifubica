using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using System.Timers;
using Microsoft.Phone.Tasks;

namespace ColorLand
{
    public class StoryScreen : BaseScreen
    {
        private MTimer mTimer;

        public StoryScreen()
        {
            MediaPlayerLauncher mediaPlayerLauncher = new MediaPlayerLauncher();
            mediaPlayerLauncher.Media = new Uri("Story.wmv", UriKind.RelativeOrAbsolute);
            mediaPlayerLauncher.Location = MediaLocationType.Data;
            mediaPlayerLauncher.Controls = MediaPlaybackControls.All;
            mediaPlayerLauncher.Show();

            Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_CHAR_SELECTION, true, false);
        }       

        public override void update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Game1.getInstance().getScreenManager().changeScreen(ScreenManager.SCREEN_ID_CHAR_SELECTION, true, false);
            }
        }

        public override void draw(GameTime gameTime)
        {
            return;
        }

    }
}
