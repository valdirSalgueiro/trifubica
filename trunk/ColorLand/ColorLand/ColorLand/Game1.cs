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
using Microsoft.Research.Kinect;
using Microsoft.Research.Kinect.Nui;



namespace ColorLand
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        
        public static int sSCREEN_RESOLUTION_WIDTH = 640;
        public static int sSCREEN_RESOLUTION_HEIGHT = 480;

        public static bool sKINECT_BASED = false;

        //Original structure
        private static Game1 instance;
        ScreenManager mScreenManager;

        //For - Resolution640x480
        private const float SkeletonMaxX = 0.6f;//0.8f;
        private const float SkeletonMaxY = 0.4f;//0.4f;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Runtime kinectSensor;
        Texture2D kinectRGBVideo;

        public static bool NEED_SHOULDERS = true;

        Texture2D img;

        //JOINTS
        private Vector2 mLeftHandCoord = new Vector2();
        private Vector2 mRightHandCoord = new Vector2();
        private float handX = 100;
        private float handY = 100;

        private Vector2 mHipCenterCoord = new Vector2();
        private Vector2 mCenterShouldersCoord = new Vector2();

        public static Vector2 sMAIN_HAND_COORD = new Vector2();
        public static Vector2 sSECONDARY_HAND_COORD = new Vector2();

        public static bool sMAIN_HAND_RIGHT = true;

        Joint fuckJoint;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            //* Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            graphics.IsFullScreen = false;

            mScreenManager = new ScreenManager(this);

            Components.Add(mScreenManager);

            instance = this;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            if (sKINECT_BASED)
            {
                kinectSensor = Runtime.Kinects[0];
                //kinectSensor.Initialize(RuntimeOptions.UseColor);
                kinectSensor.Initialize(RuntimeOptions.UseSkeletalTracking);
            }

            Console.WriteLine("INIT");

            graphics.PreferredBackBufferWidth = sSCREEN_RESOLUTION_WIDTH;
            graphics.PreferredBackBufferHeight = sSCREEN_RESOLUTION_HEIGHT;
            graphics.ApplyChanges();


            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //kinectRGBVideo = new Texture2D(GraphicsDevice, 640, 480);
            /*
            
            
            img = this.Content.Load<Texture2D>("lol");
            * */
            //kinectSensor.VideoStream.Open(ImageStreamType.Video,2, ImageResolution.Resolution640x480,ImageType.Color);
            if (sKINECT_BASED)
            {
                kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinectRuntime_SkeletonFrameReady);
            }
            //kinectSensor.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(kinectSensor_VideoFrameReady);

        }

        public static Game1 getInstance()
        {
            return instance;
        }

        public ScreenManager getScreenManager()
        {
            return this.mScreenManager;
        }


        void kinectSensor_VideoFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            PlanarImage p = e.ImageFrame.Image;

            Color[] color = new Color[p.Height * p.Width];
            kinectRGBVideo = new Texture2D(graphics.GraphicsDevice, p.Width, p.Height);

            int index = 0;
            for (int y = 0; y < p.Height; y++)
            {
                for (int x = 0; x < p.Width; x++, index += 4)
                {
                    color[y * p.Width + x] =
                                  new Color(p.Bits[index + 2], p.Bits[index + 1], p.Bits[index + 0]);
                }
            }

            kinectRGBVideo.SetData(color);
        }


        void kinectRuntime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonFrame = e.SkeletonFrame;

            foreach (SkeletonData data in skeletonFrame.Skeletons)
            {
                if (data.TrackingState == SkeletonTrackingState.Tracked)
                {
                    /*foreach (Joint joint in data.Joints)
                    {
                        if (joint.Position.W < 0.8f)
                            continue;
                        switch (joint.ID)
                        {
                            // add a graphic to the head
                            case JointID.Head:
                                //…  // add code for placing bobble head here.
                                Console.WriteLine("HEAAD");
                                break;
                        }
                    }*/

                    if (data.Joints[JointID.HandLeft].TrackingState == JointTrackingState.Tracked)
                    {
                        mLeftHandCoord = AdjustToScreen(data.Joints[JointID.HandLeft], sSCREEN_RESOLUTION_WIDTH, sSCREEN_RESOLUTION_HEIGHT);
                    }

                    if (data.Joints[JointID.HandRight].TrackingState == JointTrackingState.Tracked)
                    {
                        // Console.WriteLine("("+data.Joints[JointID.HandLeft].Position.X +","+data.Joints[JointID.HandLeft].Position.X+")");
                        //handX = data.Joints[JointID.HandLeft].Position.X;
                        //handY = data.Joints[JointID.HandLeft].Position.Y;

                        Vector2 p = AdjustToScreen(data.Joints[JointID.HandRight], sSCREEN_RESOLUTION_WIDTH, sSCREEN_RESOLUTION_HEIGHT);

                        //getDisplayPosition(data.Joints[JointID.HandLeft]);

                        //Console.WriteLine("LOCATION: " + p.X + "," + p.Y);
                        //handX = p.X;
                        //handY = p.Y;
                        mRightHandCoord.X = p.X;
                        mRightHandCoord.Y = p.Y;

                        //Console.WriteLine("(" + data.Joints[JointID.HandLeft].Position.X + "," + data.Joints[JointID.HandLeft].Position.Y + ")");
                    }

                    if (sMAIN_HAND_RIGHT)
                    {
                        sMAIN_HAND_COORD = mRightHandCoord;
                        sSECONDARY_HAND_COORD = mLeftHandCoord;
                    }
                    else
                    {
                        sMAIN_HAND_COORD = mLeftHandCoord;
                        sSECONDARY_HAND_COORD = mRightHandCoord;
                    }

                    if (data.Joints[JointID.HipCenter].TrackingState == JointTrackingState.Tracked)
                    {
                        mHipCenterCoord = AdjustToScreen(data.Joints[JointID.HipCenter], sSCREEN_RESOLUTION_WIDTH, sSCREEN_RESOLUTION_HEIGHT);
                    }

                    if (NEED_SHOULDERS)
                    {
                        if (data.Joints[JointID.ShoulderCenter].TrackingState == JointTrackingState.Tracked)
                        {
                            mCenterShouldersCoord = AdjustToScreen(data.Joints[JointID.ShoulderCenter], sSCREEN_RESOLUTION_WIDTH, sSCREEN_RESOLUTION_HEIGHT);
                        }
                    }

                }

            }
        }

        private Point getDisplayPosition(Joint joint)
        {
            float depthX, depthY;

            kinectSensor.SkeletonEngine.SkeletonToDepthImage(joint.Position, out depthX, out depthY);
            depthX = Math.Max(0, Math.Min(depthX * sSCREEN_RESOLUTION_WIDTH / 2, sSCREEN_RESOLUTION_WIDTH/2));
            depthY = Math.Max(0, Math.Min(depthY * sSCREEN_RESOLUTION_HEIGHT / 2, sSCREEN_RESOLUTION_HEIGHT));
            int colorX, colorY;
            ImageViewArea iv = new ImageViewArea();
            //kinectSensor.NuiCamera.GetColorPixelCoordinatesFromDepthPixel(ImageResolution.Resolution640x480, iv,
            kinectSensor.NuiCamera.GetColorPixelCoordinatesFromDepthPixel(ImageResolution.Resolution640x480, iv,
                (int)depthX, (int)depthY, (short)0, out colorX, out colorY);
            return new Point((int)(1337 * colorX / sSCREEN_RESOLUTION_WIDTH), (int)(1337 * colorY / sSCREEN_RESOLUTION_HEIGHT));
        }


        private static float Adjust(int primaryScreenResolution,
      float maxJointPosition, float jointPosition)
        {

            var value = (((((float)primaryScreenResolution) / maxJointPosition) / 2f) *
                jointPosition)
                + (primaryScreenResolution / 2);


            if (value > primaryScreenResolution || value < 0f) return 0f;


            return value;

        }


        /// <summary>
        /// Get the current Joint position and Adjust the Skeleton 
        /// joint position to the current Screen resolution.

        /// </summary>
        /// <param name="joint">Joint to Adjust</param>

        /// <returns></returns>
        public static Vector2 AdjustToScreen(Joint joint, int screenWidth, int screenHeight)
        {

            var newVector = new Vector2
            {

                X = Adjust(screenWidth, SkeletonMaxX, joint.Position.X),
                Y = Adjust(screenHeight, SkeletonMaxY, -joint.Position.Y),

                //Z = joint.Position.Z,
                //W = joint.Position.W

            };


            return newVector;
        }




        protected override void UnloadContent()
        {
            /*  kinectSensor.Uninitialize();  */
        }




        /*protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }*/

        public Vector2 getRightHandPosition()
        {
            return mRightHandCoord;
        }

        public Vector2 getLeftHandPosition()
        {
            return mLeftHandCoord;
        }

        public Vector2 getHipCenterPosition()
        {
            return mHipCenterCoord;
        }

        public Vector2 getCenterShouldersPosition()
        {
            return mCenterShouldersCoord;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code her

            /*spriteBatch.Begin();
            //spriteBatch.Draw(kinectRGBVideo, new Rectangle(0, 0, 640, 480), Color.White);
            //spriteBatch.Draw(overlay, new Rectangle(0, 0, 640, 480), Color.White);

            spriteBatch.Draw(img, new Rectangle((int)handX, (int)handY, 30, 30), Color.White);

            spriteBatch.End();
            */
            graphics.GraphicsDevice.Clear(Color.White);
            base.Draw(gameTime);
        }

        public static void print(String message)
        {
            //System.Diagnostics.Debug.WriteLine(message);
            Console.WriteLine(message);
        }

    }



}
