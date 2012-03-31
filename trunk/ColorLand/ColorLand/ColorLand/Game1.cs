using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace ColorLand
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public static String sPROGRESS_FILE_NAME = "progress.lol";

        public static int sSCREEN_RESOLUTION_WIDTH = 800;//640;
        public static int sSCREEN_RESOLUTION_HEIGHT = 600;//480;

        public static int sHALF_SCREEN_RESOLUTION_WIDTH = sSCREEN_RESOLUTION_WIDTH / 2;
        public static int sHALF_SCREEN_RESOLUTION_HEIGHT = sSCREEN_RESOLUTION_HEIGHT / 2;

        public static bool sKINECT_BASED = false;

        //Original structure
        private static Game1 instance;
        ScreenManager mScreenManager;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        public Game1()
        {
            //int level = 6;
            //ObjectSerialization.Save<int>("level", level);            
            //int loadedLevel=ObjectSerialization.Load<int>("level");
            //Console.WriteLine(loadedLevel);

            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            //* Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            
            graphics.IsFullScreen = false;

            mScreenManager = new ScreenManager(this);

            Components.Add(mScreenManager);

            instance = this;
            SoundManager.Initialize(this);

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            if (sKINECT_BASED)
            {
               // KinectManager.getInstance().init();
            }
#if WINDOWS_PHONE
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 800; 
#else
            graphics.PreferredBackBufferWidth = sSCREEN_RESOLUTION_WIDTH;
            graphics.PreferredBackBufferHeight = sSCREEN_RESOLUTION_HEIGHT;
#endif
            graphics.ApplyChanges();
            graphics.GraphicsDevice.Clear(Color.Black);


            base.Initialize();

            

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public static Game1 getInstance()
        {
            return instance;
        }

        public ScreenManager getScreenManager()
        {
            return this.mScreenManager;
        }

        public void toggleFullscreen()
        {
            graphics.IsFullScreen = !graphics.IsFullScreen;
            graphics.ApplyChanges();
            graphics.GraphicsDevice.Clear(Color.Black);
        }

        public void setFullscreen(bool fullscreen)
        {
            graphics.IsFullScreen = fullscreen;
            graphics.ApplyChanges();
            graphics.GraphicsDevice.Clear(Color.Black);
        }

        protected override void UnloadContent()
        {
            /*  kinectSensor.Uninitialize();  */
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

          
           if (KeyboardManager.getInstance().pressed(Keys.A))
           {
                Exit();
           }
        }

        public static void print(String message)
        {
            //System.Diagnostics.Debug.WriteLine(message);
            Console.WriteLine(message);
        }

    }



}
