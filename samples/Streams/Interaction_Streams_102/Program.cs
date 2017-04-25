using System;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace Interaction_Streams_102
{
    /// <summary>
    /// The data streams provide nicely filtered eye-gaze data from the eye tracker 
    /// transformed to a convenient coordinate system. The point on the screen where 
    /// your eyes are looking (gaze point), and the points on the screen where your 
    /// eyes linger to focus on something (fixations) are given as pixel coordinates 
    /// on the screen. The positions of your eyeballs (eye positions) are given in 
    /// space coordinates in millimeters relative to the center of the screen.
    /// 
    /// The Fixation data stream provides information about when the user is fixating
    /// his/her eyes at a single location. This data stream can be used to get an 
    /// understanding of where the user’s attention is. In most cases, when a person
    /// is fixating at something for a long time, this means that the person’s brain 
    /// is processing the information at the fixation point.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Everything starts with initializing Host, which manages the connection to the 
            // Tobii Engine and provides all the Tobii Core SDK functionality.
            // NOTE: Make sure that Tobii.EyeX.exe is running
            var host = new Host();

            // Initialize Fixation data stream.
            var fixationDataStream = host.Streams.CreateFixationDataStream();

            // Because timestamp of fixation events is relative to the previous ones
            // only, we will store them in this variable.
            var fixationBeginTime = 0d;

            fixationDataStream.Next += (o, fixation) =>
            {
                // On the Next event, data comes as FixationData objects, wrapped in a StreamData<T> object.
                var fixationPointX = fixation.Data.X;
                var fixationPointY = fixation.Data.Y;

                switch (fixation.Data.EventType)
                {
                    case FixationDataEventType.Begin:
                        fixationBeginTime = fixation.Data.Timestamp;
                        Console.WriteLine("Begin fixation at X: {0}, Y: {1}", fixationPointX, fixationPointY);
                        break;

                    case FixationDataEventType.Data:
                        Console.WriteLine("During fixation, currently at X: {0}, Y: {1}", fixationPointX, fixationPointY);
                        break;

                    case FixationDataEventType.End:
                        Console.WriteLine("End fixation at X: {0}, Y: {1}", fixationPointX, fixationPointY);
                        Console.WriteLine("Fixation duration: {0}",
                            fixationBeginTime > 0
                                ? TimeSpan.FromMilliseconds(fixation.Data.Timestamp - fixationBeginTime)
                                : TimeSpan.Zero);
                        Console.WriteLine();
                        break;

                    default:
                        throw new InvalidOperationException("Unknown fixation event type, which doesn't have explicit handling.");
                }
            };

            Console.ReadKey();

            // we will close the coonection to the Tobii Engine before exit.
            host.DisableConnection();
        }
    }
}
