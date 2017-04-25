Unified foundation for WPF
--------------------------

Below you can find some insight into current sample and how its easy to add some gaze based behaviors to your WPF application.

To get started we need to instantiate `Host`, which handles connection to the **Tobii Engine**, and provides
access to different features of Tobii Core SDK. The best place to instantiate Host would be `App.xaml.cs`
file. In current sample we have overridden the `OnStartup` method of `App` class:

~~~~
// some usings here

public partial class App : Application
{
    private Host _host;
    private WpfInteractorAgent _agent;

    protected override void OnStartup(StartupEventArgs e)
    {
        _host = new Host();
        _agent = _host.InitializeWpfAgent();
    }
}
~~~~

You have noticed that we actually made one more thing - initialized `WpfInteractorAgent`. **InteractorAgents** designed
to help you work with **Interactors** which are basically regions in your application with some gaze based 
behaviors. Let's see how such behavior has been defined in the sample.

If you take a look at the `MainWindow.xaml` you can notice that we have added `wpf:Behaviors.IsGazeAware` to the 
Rectangle element:

~~~~
<Rectangle wpf:Behaviors.IsGazeAware="True" 
	   Style="{StaticResource RectangleWithGazeAwareAnimation}" />
~~~~

This will instruct Tobii Engine to treat this rectangle as an Interactor with GazeAware behavior. It means that
whenever somebody will be looking at the rectangle, another attached property `wpf:Behaviors.HasGaze` will be set 
to `true` and when nobody is looking at it, property will be `false`.

Taking a look at the Style we have defined for the rectangle, you can see how we used this property in the `Trigger`.

~~~~
<Style x:Key="RectangleWithGazeAwareAnimation" TargetType="Rectangle">
    <Setter Property="Fill" Value="White"></Setter>
    <Style.Triggers>
        <Trigger Property="wpf:Behaviors.HasGaze" Value="True">
            <Trigger.EnterActions>
			     <!-- Enter animation -->
            </Trigger.EnterActions>
            <Trigger.ExitActions>
			     <!-- Exit animation -->
            </Trigger.ExitActions>
       </Trigger>
    </Style.Triggers>
</Style>
~~~~








