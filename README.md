# Welcome to Tobii Core SDK samples for .NET!

  This repository contains samples and [documentation](https://tobii.github.io/CoreSDK/articles/intro.html) to show developers how to use our API to build interactive _eye tracking_ enabled games and applications on the Microsoft .NET Framework.

  These samples are minimal samples designed to showcase the basic use cases.

  Note that Tobii offers several SDK packages targeted at different programming languages and frameworks, so be sure to pick the one that fits your needs best. More developer related information can be found on [Tobii Developer Zone](http://developer.tobii.com/).

# Contact

  If you have problems, questions, ideas, or suggestions, please use the forums
  on the Tobii Developer Zone (link below). That's what they are for!

  Visit the Tobii Developer Zone web site for the latest news and downloads:

  http://developer.tobii.com/

# Compatibility

  This version of the Tobii Core SDK requires Tobii Interaction Engine version 1.X or later and a Tobii Eye Tracker.
  Specific features will require newer versions of the Tobii Interaction Engine as listed in the revision history below.

# Dependencies
  https://www.nuget.org/packages/Tobii.Interaction/

# License

  The sample source code in this project is licensed under the MIT License - please see the LICENSE file for details. Samples requires Tobii propriaritary binares to access the eye tracker (available on www.nuget.org). Note that these referenced libraries are covered by a [separate license agreement](https://developer.tobii.com/license-agreement/).

# Revision history

Change log below:

What's new in version 2.1.1:

	* Fix bug where WPF interactors did not get deleted properly when hiding the FrameworkElement


What's new in version 2.1.0:

	* Added Head Pose stream
	* Fix bug where WPF interactors did not get deleted properly when removing the FrameworkElement


What's new in version 2.0.5:

	* Fix for deleting WPF-interactors which lost behaviors
	* Fix bug for deleting virtual windows


What's new in version 2.0.4:

	* Fix for marshalling strings when handling profile commands


What's new in version 2.0.3:

	* Fix for broken interactors


What's new in version 2.0.2:

	* API changes


What's new in version 2.0.1:

	* Fixed issue which cased NullReferenceException while using WPF Behaviors during design time preview in VS
	* Fixed issue when unsubscribing from the OnNext event from Streams didn't really unsubscribed.

