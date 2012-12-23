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

using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Resources;
using System.Windows;

namespace ColorLand
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public static String sPROGRESS_FILE_NAME = "progress.lol";

        public const int sSCREEN_RESOLUTION_WIDTH = 800;//640;
        public const int sSCREEN_RESOLUTION_HEIGHT = 480;//480;

        public const int sHALF_SCREEN_RESOLUTION_WIDTH = sSCREEN_RESOLUTION_WIDTH / 2;
        public const int sHALF_SCREEN_RESOLUTION_HEIGHT = sSCREEN_RESOLUTION_HEIGHT / 2;

        public static bool sKINECT_BASED = false;

        public static ProgressObject progressObject;

        //Original structure
        private static Game1 instance;
        ScreenManager mScreenManager;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Vector3 screenScalingFactor;
        public static Matrix globalTransformation;

        public Game1()
        {
            saveStuff();
            progressObject = ExtraFunctions.loadProgress();

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;            
            graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";

            //* Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);


            mScreenManager = new ScreenManager(this);

            Components.Add(mScreenManager);

            instance = this;
            SoundManager.Initialize(this);
        }

        private const string FileName1 = "ColorLand;component/Story.wmv";
        private const string FileName2 = "ColorLand;component/ending.wmv";
        private String video1 = "Story.wmv";
        private String video2 = "ending.wmv";

        private void saveStuff()
        {
            saveVideo(FileName1,video1);
            saveVideo(FileName2, video2);
        }

        private void saveVideo(string param1, string param2)
        {
            StreamResourceInfo streamResourceInfo = Application.GetResourceStream(new Uri(param1, UriKind.RelativeOrAbsolute));

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(param2))
                {
                    //myIsolatedStorage.DeleteFile(video1);
                    return;
                }

                using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(param2, FileMode.Create, myIsolatedStorage))
                {
                    using (BinaryWriter writer = new BinaryWriter(fileStream))
                    {
                        Stream resourceStream = streamResourceInfo.Stream;
                        long length = resourceStream.Length;
                        byte[] buffer = new byte[32];
                        int readCount = 0;
                        using (BinaryReader reader = new BinaryReader(streamResourceInfo.Stream))
                        {
                            // read file in chunks in order to reduce memory consumption and increase performance
                            while (readCount < length)
                            {
                                int actual = reader.Read(buffer, 0, buffer.Length);
                                readCount += actual;
                                writer.Write(buffer, 0, actual);
                            }
                        }
                    }
                }
            }
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            if (sKINECT_BASED)
            {
               // KinectManager.getInstance().init();
            }


            base.Initialize();

            float horScaling = (float)graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 800;
            float verScaling = (float)graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 600;
            screenScalingFactor = new Vector3(horScaling, verScaling, 1);
            globalTransformation = Matrix.CreateScale(screenScalingFactor);

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
