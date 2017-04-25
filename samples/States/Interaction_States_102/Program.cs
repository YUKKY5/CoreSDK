using System;
using Tobii.Interaction;

namespace Interaction_States_102
{
    /// <summary>
    /// Tobii Engine holds a StateContainer which can be used for getting various information.
    /// The Tobii Core SDK provides two mechanisms to obtain some of those states value. 
    /// The state value could be obtain with asynchronous calls using the provided Get...Async methods
    /// from States property accessible from Host instance or by creating a state observer. 
    /// 
    /// In this sample we will try to do some interactive stuff by subscribing to the state observers.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Everything starts with initializing Host, which manages connection to the 
            // Tobii Engine and provides all the Tobii Core SDK functionality.
            // NOTE: Make sure that Tobii.EyeX.exe is running
            var host = new Host();

            // We will subscribe to the user presence state first.
            var userPresenceStateObserver = host.States.CreateUserPresenceObserver();

            // After that we can either use our extension method.
            userPresenceStateObserver.WhenChanged(userPresenceState =>
            {
                if(userPresenceState.IsValid)
                    Console.WriteLine("User presence is: {0}", userPresenceState.Value);
            });

            // NOTE: Try to cover eye-tracker with your hand while running this sample,
            //       and you should see how user presence is changing. Equally you can 
            //       just move yourself so eye-tracker do not see you and then back. 

            // Next we going to look at another option, by providing event handler.
            var gazeTrackingStateObserver = host.States.CreateGazeTrackingObserver();
            gazeTrackingStateObserver.Changed += OnGazeTrackingStateChanged;

            Console.ReadLine();

            // we will close the coonection to the Tobii Engine before exit.
            host.DisableConnection();
        }

        private static void OnGazeTrackingStateChanged(object sender, EngineStateValue<Tobii.Interaction.Framework.GazeTracking> state)
        {
            if(state.IsValid)
                Console.WriteLine("Gaze tracking state is: {0}", state.Value);

            // Note: To play around with this state, click on Tobii Eye Tracking tray icon,
            //       and in the menu which is appeared try to turn eyetracking on/off again.
        }
    }
}
