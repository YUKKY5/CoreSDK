using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace Interaction_Interactors_102
{
    /// <summary>
    /// Interactor is a region on screen for which some gaze based behavior is defined.
    /// Most common behaviors the Tobii Core SDK is exposing for interactors is gaze aware
    /// (simply a notion of whether somebody is looking at the region or not), activatable
    /// (which means the region has some action associated with it, which can be triggered, but
    /// only if someone is looking at it at the same time), pannable (provides panning like
    /// behavior with associated actions, which can be triggered, but again only if someone
    /// is looking at it at the same time). 
    /// 
    /// To help you manage interactors, the Tobii Core SDK provides another concept - InteractorAgents.
    /// When you do not work with WPF or WindowsForms, the Tobii Core SDK has UnboundInteractorAgent,
    /// which you can use to control everything related to interactors.
    /// 
    /// Activatable interactor usually has some action associated with it. This action can be
    /// invoked using Commands, exposed through Commands property of the Host instance.
    /// Activatable interactors can also have Tentative Focus and/or an ActivationFocus action
    /// associated with them. Tentative focus behaves similary to GazeAware behavior, except
    /// that only one activatable interactor could have focus at any given moment of time, while
    /// any number of GazeAware interactors will have gaze if you are looking at them.
    /// Before activatable interactors can be activated you need to send a special command -
    /// ActivationModeOn. When that occurrs, instead of reporting Tentative focus when
    /// someone is looking at the activatable interactor, it will report ActivationFocus.
    /// Again, only one activatable interactor could have ActivationFocus at any given moment in time.
    /// 
    /// This sample demonstrates how to define an activatable interactor, how to associate different
    /// actions with it's states and how to activate it using Commands.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Everything starts with initializing Host, which manages the connection to the 
            // Tobii Engine and provides all the Tobii Core SDK functionality.
            // NOTE: Make sure that Tobii.EyeX.exe is running
            var host = new Host();

            // InteractorAgents are defined per window, so we need a handle to it.
            var currentWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
            
            // Let's also obtain its bounds using Windows API calls (hidden in a helper method below).
            var currentWindowBounds = GetWindowBounds(currentWindowHandle);
            // Let's create the InteractorAgent.
            var interactorsAgent = host.InitializeUnboundAgent(currentWindowHandle, "ConsoleWindowAgent");

            interactorsAgent
                // we will leave default params for everything except bounds and id
                .AddInteractorFor(currentWindowBounds, Literals.RootId, 0, currentWindowHandle.ToString(), "MyFirstActivatable")
                // we define interactor to be an activatable one
                .WithActivatable()
                // we will provide some action which will be invoked when interactor will receive
                // gaze while Engine is in ActivationModeOn
                .HasActivationFocus(id => Console.WriteLine("Id: {0} got activation focus.", id))
                // Enabling tentative focus will also provide this interactor with capabilities
                // similar to gaze aware interactors
                .SetTentativeFocusEnabled(true)
                // we will provide some action which will be invoked when interactor will receive
                // gaze while Engine is in ActivationModeOff
                .HasTentativeFocus(id => Console.WriteLine("Id: {0} got tentative activation focus.", id))
                // Allows you to findout about changed focus and which focus exactly has been changed
                .WhenFocusChanged((id, hasTentativeFocus, hasActivationFocus) => { })
                // will be invoked when interactor loose any kind of focus
                .LostFocus(id => Console.WriteLine("Id: {0} lost focus.", id))
                // Interactor becomes activated when someone is looking at it and
                // activation command has been received.
                .WhenActivated(id => Console.WriteLine("Id: {0} activated", id));

            var currentKey = Console.ReadKey(true).Key;
            while (currentKey != ConsoleKey.Q)
            {
                // instructs TobiiEngine to switch on Activation mode,
                // while this mode is on, activatable interactors will report
                // receiving of activation focus instead of tentative focus.
                host.Commands.Input.SendActivationModeOn();

                // Sends an activation request to all activatable interactors.
                // The ones which are currently gazed at will be activated.
                host.Commands.Input.SendActivation();

                // instructs TobiiEngine to switch activation mode off.
                host.Commands.Input.SendActivationModeOff();

                currentKey = Console.ReadKey(true).Key;
            }

            // we will close the coonection to the Tobii Engine before exit.
            host.DisableConnection();
        }

        #region Helper 
        private static Rectangle GetWindowBounds(IntPtr windowHandle)
        {
            RECT nativeRect;
            if (GetWindowRect(windowHandle, out nativeRect))
                return new Rectangle()
                {
                    X = nativeRect.Left,
                    Y = nativeRect.Top,
                    Width = nativeRect.Right,
                    Height = nativeRect.Bottom
                };

            return new Rectangle(0d, 0d, 1000d, 1000d);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion
    }
}

