using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#if WINDOWS
using Microsoft.Research.Kinect;
using Microsoft.Research.Kinect.Nui;
#endif

namespace ColorLand
{
    public class KinectManager
    {
#if WINDOWS
        private static KinectManager instance;

        //For - Resolution640x480
        private const float SkeletonMaxX = 0.8f;
        private const float SkeletonMaxY = 0.4f;

        Runtime kinectSensor;

        private Vector2 mLeftHandCoord = new Vector2();
        private Vector2 mRightHandCoord = new Vector2();
      
        private Vector2 mHipCenterCoord = new Vector2();
        private Vector2 mCenterShouldersCoord = new Vector2();

        public static Vector2 sMAIN_HAND_COORD = new Vector2();
        public static Vector2 sSECONDARY_HAND_COORD = new Vector2();

        public static Vector2 sCenter_Position = new Vector2();

        private KinectManager(){

        }

        public static KinectManager getInstance()
        {
            if (instance == null)
            {
                instance = new KinectManager();
            }
            return instance;
        }

        public void init()
        {
            kinectSensor = Runtime.Kinects[0];
            kinectSensor.Initialize(RuntimeOptions.UseSkeletalTracking);

            kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinectRuntime_SkeletonFrameReady);
           
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
                        mLeftHandCoord = AdjustToScreen(data.Joints[JointID.HandLeft], Game1.sSCREEN_RESOLUTION_WIDTH, Game1.sSCREEN_RESOLUTION_HEIGHT);
                    }

                    if (data.Joints[JointID.HandRight].TrackingState == JointTrackingState.Tracked)
                    {
                        // Console.WriteLine("("+data.Joints[JointID.HandLeft].Position.X +","+data.Joints[JointID.HandLeft].Position.X+")");
                        //handX = data.Joints[JointID.HandLeft].Position.X;
                        //handY = data.Joints[JointID.HandLeft].Position.Y;

                        Vector2 p = AdjustToScreen(data.Joints[JointID.HandRight], Game1.sSCREEN_RESOLUTION_WIDTH, Game1.sSCREEN_RESOLUTION_HEIGHT);

                        //getDisplayPosition(data.Joints[JointID.HandLeft]);

                        //Console.WriteLine("LOCATION: " + p.X + "," + p.Y);
                        //handX = p.X;
                        //handY = p.Y;
                        mRightHandCoord.X = p.X;
                        mRightHandCoord.Y = p.Y;
                        sMAIN_HAND_COORD = p;
                        //Console.WriteLine("(" + data.Joints[JointID.HandLeft].Position.X + "," + data.Joints[JointID.HandLeft].Position.Y + ")");
                    }

                    if (data.Joints[JointID.HipCenter].TrackingState == JointTrackingState.Tracked)
                    {
                        mHipCenterCoord = AdjustToScreen(data.Joints[JointID.HipCenter], Game1.sSCREEN_RESOLUTION_WIDTH, Game1.sSCREEN_RESOLUTION_HEIGHT);
                    }

                  
                }

            }
        }

        private Point getDisplayPosition(Joint joint)
        {
            float depthX, depthY;

            kinectSensor.SkeletonEngine.SkeletonToDepthImage(joint.Position, out depthX, out depthY);
            depthX = Math.Max(0, Math.Min(depthX * Game1.sSCREEN_RESOLUTION_WIDTH / 2, Game1.sSCREEN_RESOLUTION_WIDTH / 2));
            depthY = Math.Max(0, Math.Min(depthY * Game1.sSCREEN_RESOLUTION_HEIGHT / 2, Game1.sSCREEN_RESOLUTION_HEIGHT));
            int colorX, colorY;
            ImageViewArea iv = new ImageViewArea();
            //kinectSensor.NuiCamera.GetColorPixelCoordinatesFromDepthPixel(ImageResolution.Resolution640x480, iv,
            kinectSensor.NuiCamera.GetColorPixelCoordinatesFromDepthPixel(ImageResolution.Resolution1280x1024, iv,
                (int)depthX, (int)depthY, (short)0, out colorX, out colorY);
            return new Point((int)(1337 * colorX / Game1.sSCREEN_RESOLUTION_WIDTH), (int)(1337 * colorY / Game1.sSCREEN_RESOLUTION_HEIGHT));
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




        protected void UnloadContent()
        {
            /*  kinectSensor.Uninitialize();  */
            kinectSensor.Uninitialize();
        }

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
#endif
    }
}
