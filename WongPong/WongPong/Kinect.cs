using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectTracking
{

    /// <summary>
    /// 
    /// Simple Kinect Interface
    ///     
    /// 
    /// </summary>
    class Kinect
    {
        // kinect variables
        KinectSensor kinectSensor = null;

        public Skeleton player1;
        public Skeleton player2;
        public int p1Index, p2Index;
        public bool p1tracked, trackedEach;

        Skeleton[] playerData;

        // USE to see if the kinect is enabled
        public bool enabled
        {
            get
            {
                return kinectSensor != null;
            }
        }
        public Kinect()
        {
        }
        ~Kinect()
        {
            if(enabled)
            kinectSensor.Stop();
        }

        public void pause()
        {
            kinectSensor.Stop();
        }
        
        public void start()
        {
            kinectSensor.Start();
        }
        
        public void initialize( int elevationAngle = 0 )
        {
            Console.WriteLine("Here.");
            while (kinectSensor == null)
            {
                kinectSensor = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected);
                if (kinectSensor == null)
                {
                    Console.WriteLine("Kinect not found. Please connect your Kinect to the computer and press enter.");
                    if (Console.ReadLine() == "q")
                    {
                        Environment.Exit(0);
                    }
                }
            }

            // limits elevation angle to keep the motors from trying too extreme an angle
            if (elevationAngle >= 26 )
            {
                elevationAngle = 26;
            }
            else if (elevationAngle <= -26)
            {
                elevationAngle = -26;
            }
            // set a call back function to process skeleton data
            kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinectSkeletonFrameReadyCallback);
            
            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.5f,
                Correction = 0.1f,
                Prediction = 0.5f,
                JitterRadius = 0.1f,
                MaxDeviationRadius = 0.1f
            };

            kinectSensor.SkeletonStream.Enable(parameters);
            kinectSensor.Start();
            kinectSensor.ElevationAngle = elevationAngle;

        }

        // Process skeleton data
        void kinectSkeletonFrameReadyCallback(object sender, SkeletonFrameReadyEventArgs skeletonFrames)
        {
            // Skeleton FRAME:  Open the skeleton
            using (SkeletonFrame skeleton = skeletonFrames.OpenSkeletonFrame())
            {

                // ensure that there is a skeleton
                if (skeleton != null)
                {
                    // if there are no players or a new player has entered or left
                    // resize playerdata to fit exactly all the players
                    if (playerData == null || this.playerData.Length != skeleton.SkeletonArrayLength)
                    {
                        this.playerData = new Skeleton[skeleton.SkeletonArrayLength];
                    }

                    // store info on all players
                    skeleton.CopySkeletonDataTo(playerData);
                }
            }

            if (playerData != null)
            {
                bool p2tracked = false;
                try
                {
                    if(playerData.ElementAt(p1Index).TrackingState == SkeletonTrackingState.Tracked
                        &&
                       playerData.ElementAt(p2Index).TrackingState == SkeletonTrackingState.Tracked)
                    {
                        p2tracked = true;
                    }
                }
                catch(Exception e){}

                if(!p2tracked)
                {
                    foreach (Skeleton skeleton in playerData)
                    {
                        if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            if (!p1tracked && skeleton.Joints[JointType.HipCenter].Position.Z < 2) { player1 = skeleton; p1tracked = true; } 
                            else if (!p2tracked && skeleton.Joints[JointType.HipCenter].Position.Z < 2)
                            { 
                                player2 = skeleton; p2tracked = true;

                                //Swap the players if player 1 was picked on the right side
                                if (player1.Joints[JointType.HipCenter].Position.X >
                                    player2.Joints[JointType.HipCenter].Position.X)
                                {
                                    Skeleton temp = player2;
                                    player2 = player1;
                                    player1 = temp;
                                }
                            }
                        }
                    }
                }
                if (p2tracked)
                {

                }
            }
        }
    }
}
