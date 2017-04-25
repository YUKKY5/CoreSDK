using System;
using System.Threading.Tasks;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace Interaction_States_101
{
    /// <summary>
    /// Tobii Engine holds a StateContainer which can be used for getting various information.
    /// The Tobii Core SDK provides you two mechanisms to obtain these states values. 
    /// The state value could be obtained with asynchronous calls using the provided Get...Async methods
    /// from the States property accessible via the Host instance or by creating a state observer. 
    /// 
    /// This sample will demonstrate how to get information about user's presence from the eye-tracker.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Everything starts with initializing Host, which manages the connection to the 
            // Tobii Engine and provides all the Tobii Core SDK functionality.
            // NOTE: Make sure that Tobii.EyeX.exe is running
            var host = new Host();

            // We will create task to await our async state getters.
            Task.Run(async () =>
            {
                var userPresenceState = await host.States.GetUserPresenceAsync();

                if (!userPresenceState.IsValid)
                    return;

                switch (userPresenceState.Value)
                {
                    case UserPresence.Present:
                        Console.WriteLine("User is present");
                        break;

                    case UserPresence.NotPresent:
                        Console.WriteLine("User is not present");
                        break;

                    case UserPresence.Unknown:
                        Console.WriteLine("It is imposible to know whether user is present or not");
                        break;

                    default:
                        throw new InvalidOperationException("Unknown user presence state value, which doesn't have explicit handling.");
                }
            });

            Console.ReadLine();

            // we will close the coonection to the Tobii Engine before exit.
            host.DisableConnection();
        }
    }
}
