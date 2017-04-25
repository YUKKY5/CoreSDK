using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Tobii.Interaction;

namespace Interaction_Interactors_101
{
    /// <summary>
    /// Interactor is a region on the screen for which some gaze based behavior is defined.
    /// Most common behaviors the Tobii Core SDK is exposing for interactors is gaze aware
    /// (simply a notion of whether somebody is looking at the region or not), activatable
    /// (which means region has some action associated with it, which can be triggered, but
    /// only if someone is looking at it at the same time), pannable (provides panning like
    /// behavior with associated actions, which can be triggered, but again only if someone
    /// is looking at it at the same time). 
    /// 
    /// To help you manage interactors, the Tobii Core SDK provides another concept - InteractorAgents.
    /// When you do not work with WPF or WindowsForms, the Tobii Core SDK has UnboundInteractorAgent,
    /// which you can use to control everything related to interactors.
    /// 
    /// Gaze aware is the most basic behavior we could think of, so let's see how its easy to
    /// define 'you are looking at it' interactor with the Tobii Core SDK.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Everything starts with initializing Host, which manages the connection to the 
            // Tobii Engine and provides all the Tobii Core SDK functionality.
            // NOTE: Make sure that Tobii.EyeX.exe is running
            var host = new Host();

            PrintSampleIntroText();

            // InteractorAgents are defined per window, so we need a handle to it.
            var currentWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
            // Let's also obtain its bounds using Windows API calls (hidden in a helper method below).
            var currentWindowBounds = GetWindowBounds(currentWindowHandle);
            // Let's create the InteractorAgent.
            var interactorAgent = host.InitializeUnboundAgent(currentWindowHandle, "ConsoleWindowAgent");

            // Next we are going to create an interactor, which we will define with the gaze aware behavior.
            // Gaze aware behavior simply tells you whether somebody is looking at the interactor or not.
            interactorAgent
                .AddInteractorFor(currentWindowBounds)
                .WithGazeAware()
                .HasGaze(() => Console.WriteLine("Hey there!"))
                .LostGaze(() => Console.WriteLine("Bye..."));

            Console.ReadKey(true);

            // we will close the coonection to the Tobii Engine before exit.
            host.DisableConnection();
        }

        #region Helpers 

        private static void PrintSampleIntroText()
        {
            Console.Clear();
            Console.WriteLine("============================================================");
            Console.WriteLine("|           Tobii Core SDK: Interactors                    |");
            Console.WriteLine("============================================================");

            Console.WriteLine();
            Console.WriteLine("This sample will demonstrate you the usage of GazeAware interactors.");
            Console.WriteLine("Look at the window to trigger HasGaze event and look away to trigger\n" +
                              "LostGaze event.");

            Console.WriteLine();
        }

        private static Rectangle GetWindowBounds(IntPtr windowHandle)
        {
            NativeRect nativeNativeRect;
            if (GetWindowRect(windowHandle, out nativeNativeRect))
                return new Rectangle
                {
                    X = nativeNativeRect.Left,
                    Y = nativeNativeRect.Top,
                    Width = nativeNativeRect.Right,
                    Height = nativeNativeRect.Bottom
                };

            return new Rectangle(0d, 0d, 1000d, 1000d);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out NativeRect nativeRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeRect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion
    }
}
