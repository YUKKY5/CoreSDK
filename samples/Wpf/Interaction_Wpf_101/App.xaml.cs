using System.Windows;
using Tobii.Interaction;
using Tobii.Interaction.Wpf;

namespace Interaction_Wpf_101
{
    public partial class App : Application
    {
        private Host _host;
        private WpfInteractorAgent _agent;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Everything starts with initializing Host, which manages connection to the 
            // Tobii Engine and provides all Tobii Core SDK functionality.
            // NOTE: Make sure that Tobii.EyeX.exe is running
            _host = new Host();

            // We need to instantiate InteractorAgent so it could control lifetime of the interactors.
            _agent = _host.InitializeWpfAgent();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // we will close the coonection to the Tobii Engine before exit.
            _host.DisableConnection();

            base.OnExit(e);
        }
    }
}
