using System;
using Tobii.Interaction;

namespace Interaction_VirtualWindows_101
{
    /// <summary>
    /// Sometimes you want to add some gaze based behavior to the
    /// window you do not own or you want to make some stuff
    /// outside of window boundaries. For all such or similar
    /// cases you can create virtual windows.
    /// 
    /// Below is just how to start with virtual windows in just few lines of code.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Everything starts with initializing Host, which manages connection to the 
            // Tobii Engine and provides all Tobii Core SDK functionality.
            // NOTE: Make sure that Tobii.EyeX.exe is running
            var host = new Host();

            // we will create virtual window covering whole screen
            // so we will get screenbounds using States.
            var screenBoundsState = host.States.GetScreenBoundsAsync().Result;
            var screenBounds = screenBoundsState.IsValid
                ? screenBoundsState.Value
                : new Rectangle(0d, 0d, 1000d, 1000d);

            var virtualWindowsAgent = host.InitializeVirtualWindowsAgent();
            var virtualWindow = virtualWindowsAgent.CreateFreeFloatingVirtualWindowAsync("MyVirtualWindow", screenBounds).Result;
            var unboundInteractorsAgent = host.InitializeVirtualInteractorAgent(virtualWindow.Id);

            unboundInteractorsAgent
                .AddInteractorFor(screenBounds)
                .WithGazeAware()
                .HasGaze(() => Console.WriteLine("You are looking at the screen."))
                .LostGaze(() => Console.WriteLine("You are looking outside the screen"));

            Console.ReadKey(true);

            // we will close the coonection to the Tobii Engine before exit.
            host.DisableConnection();
        }
    }
}
