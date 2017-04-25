using System;
using Tobii.Interaction;
using Tobii.Interaction.Framework;

namespace Interaction_Streams_103
{
    /// <summary>
    /// The data streams provide nicely filtered eye-gaze data from the eye tracker 
    /// transformed to a convenient coordinate system. The point on the screen where 
    /// your eyes are looking (gaze point), and the points on the screen where your 
    /// eyes linger to focus on something (fixations) are given as pixel coordinates 
    /// on the screen. The positions of your eyeballs (eye positions) are given in 
    /// space coordinates in millimeters relative to the center of the screen. The 
    /// position of your head (head pose) is given by two coordinates, one for head 
    /// position and the other for head rotation.
    /// 
    /// This sample allows you to try all four streams, and play around with them,
    /// so we recommend to just run it first.
    /// </summary>
    public class Program
    {
        private static Host _host;
        private static GazePointDataStream _gazePointDataStream;
        private static FixationDataStream _fixationDataStream;
        private static EyePositionStream _eyePositionDataStream;
        private static HeadPoseStream _headPoseStream;
        private static DateTime _fixationBeginTime = default(DateTime);
        private static double _fixationBeginTimestamp = Double.NaN;

        // Inside this region is some core logic for this application,
        // mostly console menus and the like, and mostly unrelated to
        // the Tobii Core SDK.
        #region Nothing related to Tobii Core SDK

        private static SamplePage _currentPage;
        private static ConsoleColor _defaultForegroundColor;

        public static void Main(string[] args)
        {
            InitializeHost();

            _defaultForegroundColor = Console.ForegroundColor;
            ConstructMainMenu();
            _currentPage = SamplePage.Main;

            var userResponce = Console.ReadKey(true);
            while (!ReadyToExit(userResponce))
            {
                switch (_currentPage)
                {
                    case SamplePage.Main:
                        ResolveUserRequestOnMainPage(userResponce.Key);
                        break;

                    case SamplePage.GazePointMenu:
                        ResolveUserRequestOnGazePointPage(userResponce.Key);
                        break;

                    case SamplePage.FixationMenu:
                        ResolveUserRequestOnFixationPage(userResponce.Key);
                        break;

                    case SamplePage.EyePositionMenu:
                        ResolveUserRequestOnEyePositionPage(userResponce.Key);
                        break;

                    case SamplePage.HeadPoseMenu:
                        ResolveUserRequestOnHeadPosePage(userResponce.Key);
                        break;

                }

                userResponce = Console.ReadKey(true);
            }

            DisableConnectionWithTobiiEngine();
        }

        private static void ResolveUserRequestOnMainPage(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D1:
                    ConstructGazePointStreamPage();
                    _currentPage = SamplePage.GazePointMenu;
                    break;
                case ConsoleKey.D2:
                    ConstructFixationStreamPage();
                    _currentPage = SamplePage.FixationMenu;
                    break;

                case ConsoleKey.D3:
                    ConstructEyePositionStreamPage();
                    _currentPage = SamplePage.EyePositionMenu;
                    break;

                case ConsoleKey.D4:
                    ConstructHeadPoseStreamPage();
                    _currentPage = SamplePage.HeadPoseMenu;
                    break;
            }
        }

        private static void ResolveUserRequestOnGazePointPage(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D0:
                    ConstructMainMenu();
                    _currentPage = SamplePage.Main;
                    break;

                case ConsoleKey.D1:
                    CreateAndVisualizeLightlyFilteredGazePointStream();
                    break;

                case ConsoleKey.D2:
                    CreateAndVisualizeUnfilteredGazePointStream();
                    break;

                default:
                    ToggleGazePointDataStream();
                    Console.WriteLine();
                    PrintGazePointStreamDescription();
                    PrintGazePointStreamPageActions();
                    break;
            }
        }

        private static void ResolveUserRequestOnFixationPage(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D0:
                    ConstructMainMenu();
                    _currentPage = SamplePage.Main;
                    break;

                case ConsoleKey.D1:
                    CreateAndVisualizeSensitiveFilteredFixationsStream();
                    break;

                case ConsoleKey.D2:
                    CreateAndVisualizeSlowFilteredFixationsStream();
                    break;

                default:
                    ToggleFixationDataStream();
                    Console.WriteLine();
                    PrintFixationStreamDescription();
                    PrintFixationStreamPageActions();
                    break;
            }
        }

        private static void ResolveUserRequestOnEyePositionPage(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D0:
                    ConstructMainMenu();
                    _currentPage = SamplePage.Main;
                    break;

                case ConsoleKey.D1:
                    CreateAndVisualizeEyePositionDataStream();
                    break;

                default:
                    ToggleEyePositionDataStream();
                    Console.WriteLine();
                    PrintEyePositionStreamDescription();
                    PrintEyePositionStreamPageActions();
                    break;
            }
        }

        private static void ResolveUserRequestOnHeadPosePage(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D0:
                    ConstructMainMenu();
                    _currentPage = SamplePage.Main;
                    break;

                case ConsoleKey.D1:
                    CreateAndVisualizeHeadPoseStream();
                    break;

                case ConsoleKey.D2:
                    ToggleHeadPoseStream();
                    break;

                default:
                    Console.WriteLine();
                    _headPoseStream.IsEnabled = false;
                    PrintHeadPoseStreamDescription();
                    PrintHeadPoseStreamPageActions();
                    break;
            }
        }

        private static bool ReadyToExit(ConsoleKeyInfo readKey)
        {
            return _currentPage == SamplePage.Main && readKey.Key == ConsoleKey.D0;
        }

        private static void ConstructMainMenu()
        {
            Console.Clear();
            Console.WriteLine("============================================================");
            Console.WriteLine("|           Tobii Core SDK: Streams                        |");
            Console.WriteLine("============================================================");

            Console.WriteLine();
            Console.WriteLine("Tobii Core SDK provides you with access to four kinds of\n" +
                              "streams: gaze point, fixations, eye position, head pose");

            Console.WriteLine("Press the number of the stream you are interested to play with:");
            Console.WriteLine("'1' - Gaze point stream.");
            Console.WriteLine("'2' - Fixations stream.");
            Console.WriteLine("'3' - Eye position stream.");
            Console.WriteLine("'4' - Head pose stream.");
            Console.WriteLine();
            Console.WriteLine("0\tExit.");
        }

        private static void ConstructGazePointStreamPage()
        {
            Console.Clear();
            PrintGazePointStreamDescription();
            PrintGazePointStreamPageActions();
        }

        private static void PrintGazePointStreamDescription()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("|      Tobii Core SDK: Gaze Points stream                  |");
            Console.WriteLine("============================================================");

            Console.WriteLine();
            Console.WriteLine("Gaze points stream is basically a stream of points on the\n" +
                              "screen where your eyes looking at. You can obtain it in two\n" +
                              "modes - lightly filtered and unfiltered.");
            Console.WriteLine();
            Console.Write("Lightly filtered");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" (default) ");
            Console.ForegroundColor = _defaultForegroundColor;
            Console.Write("- an adaptive filter which is weighted\n" +
                          "based on both the age of the data points and the velocity of the\n" +
                          "eye movements. This filter is designed to remove noise while at the\n" +
                          "same time being responsive to quick eye movements.\n");
            Console.WriteLine();
            Console.WriteLine("Unfiltered - no filtering performed by Engine, except removing\n" +
                              "invalid data and averaging of the gaze points of both eyes.");
        }

        private static void PrintGazePointStreamPageActions()
        {
            Console.WriteLine();
            Console.WriteLine("'1' - to show lightly filtered gaze data");
            Console.WriteLine("'2' - to show unfiltered gaze data");
            Console.WriteLine("'0' - to jump back to main menu");
            Console.WriteLine("");
            Console.WriteLine("You can use any other key to pause the running stream: ");
        }

        private static void ConstructFixationStreamPage()
        {
            Console.Clear();

            PrintFixationStreamDescription();
            PrintFixationStreamPageActions();
        }

        private static void PrintFixationStreamDescription()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("|        Tobii Core SDK: Fixations stream                  |");
            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine("Fixations stream is a stream of points on the screen where\n" +
                              "your eyes linger to focus on something. This data stream can\n" +
                              "be used to get an understanding of where the user’s attention\n" +
                              "is. In most cases, when a person is fixating at something for\n" +
                              "a long time, this means that the person’s brain is processing\n" +
                              "the information at the point of fixation.\n" +
                              "Each fixation corresponds to series of fixation events: begin,\n" +
                              "data, data, data, .., end");
            Console.WriteLine();
            Console.WriteLine("You can obtain fixation stream in two filter modes: slow and sensitive.");
            Console.Write("Sensitive");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" (default) ");
            Console.ForegroundColor = _defaultForegroundColor;
            Console.WriteLine("- will result in many fixations, sometimes very\n close and in quick succession.");
            Console.WriteLine();
            Console.WriteLine("Slow - will result in fairly stable fixations but may leave somewhat late.");
        }

        private static void PrintFixationStreamPageActions()
        {
            Console.WriteLine();
            Console.WriteLine("'1' - to show sensitive fixations data");
            Console.WriteLine("'2' - to show slow fixation data");
            Console.WriteLine("'0' - to jump back to main menu");
            Console.WriteLine("");
            Console.WriteLine("You can use any other key to pause the running stream: ");
        }

        private static void ConstructEyePositionStreamPage()
        {
            Console.Clear();

            PrintEyePositionStreamDescription();
            PrintEyePositionStreamPageActions();
        }

        private static void PrintEyePositionStreamDescription()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("|       Tobii Core SDK: Eye positions stream               |");
            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine("Eye Positions stream provides the user’s eye positions in \n" +
                              "three-dimensional space. This data stream produces a new value\n" +
                              "whenever the engine receives a valid sample from the eye tracker,\n" +
                              "and no statements are made about the frame rate, just as for the\n" +
                              "Gaze point data behavior. The eye positions are given for the\n" +
                              "left and right eyes individually, both in relation to the screen\n" +
                              "and normalized to the track box.");
        }

        private static void ConstructHeadPoseStreamPage()
        {
            Console.Clear();

            PrintHeadPoseStreamDescription();
            PrintHeadPoseStreamPageActions();
        }

        private static void PrintEyePositionStreamPageActions()
        {
            Console.WriteLine();
            Console.WriteLine("Tobii Core SDK doesn't offer any filtering for this stream.");
            Console.WriteLine();
            Console.WriteLine("'1' - to show eye position");
            Console.WriteLine("'0' - to jump back to main menu");
            Console.WriteLine();
            Console.WriteLine("You can use any other key to pause the running stream: ");
        }

        private static void PrintHeadPoseStreamDescription()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("|       UnifiedFoundation: Head pose stream                |");
            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine("Head pose stream provides the user´s head pose by two coordinates \n" +
                              "in the the three-dimensional space. The data stream contains one \n " +
                              "coordinate for the head positions and the other one \n " +
                              "for head rotation");
        }

        private static void PrintHeadPoseStreamPageActions()
        {
            Console.WriteLine();
            Console.WriteLine("");
            Console.WriteLine();
            Console.WriteLine("'1' - start head pose stream");
            Console.WriteLine("'2' - toggle the head pose stream ON/OFF");
            Console.WriteLine("'0' - to jump back to main menu");
            Console.WriteLine();
            Console.WriteLine("You can use any other key to pause the running stream: ");
        }

        private enum SamplePage
        {
            Main = 1,
            GazePointMenu = 100,
            FixationMenu = 200,
            EyePositionMenu = 300,
            HeadPoseMenu = 400,
        }

        #endregion

        private static void InitializeHost()
        {
            // Everything starts with initializing Host, which manages connection to the 
            // Tobii Engine and provides all the Tobii Core SDK functionality.
            // NOTE: Make sure that Tobii.EyeX.exe is running
            _host = new Host();
        }

        private static void DisableConnectionWithTobiiEngine()
        {
            // We should disable connection with TobiiEngine before exit the application.
            _host.DisableConnection();
        }

        private static void ToggleGazePointDataStream()
        {
            if (_gazePointDataStream != null)
                _gazePointDataStream.IsEnabled = !_gazePointDataStream.IsEnabled;
        }

        private static void ToggleFixationDataStream()
        {
            if (_fixationDataStream != null)
                _fixationDataStream.IsEnabled = !_fixationDataStream.IsEnabled;
        }

        private static void ToggleEyePositionDataStream()
        {
            if (_eyePositionDataStream != null)
                _eyePositionDataStream.IsEnabled = !_eyePositionDataStream.IsEnabled;
        }

        private static void ToggleHeadPoseStream()
        {
            if (_headPoseStream != null)
                _headPoseStream.IsEnabled = !_headPoseStream.IsEnabled;
        }

        private static void CreateAndVisualizeUnfilteredGazePointStream()
        {
            if (_gazePointDataStream != null)
                _gazePointDataStream.Next -= OnNextGazePoint;

            _gazePointDataStream = _host.Streams.CreateGazePointDataStream(GazePointDataMode.Unfiltered);
            _gazePointDataStream.Next += OnNextGazePoint;
        }

        private static void OnNextGazePoint(object sender, StreamData<GazePointData> gazePoint)
        {
            Console.WriteLine("Timestamp: {0}\tX:{1}, Y:{2}", gazePoint.Data.Timestamp, gazePoint.Data.X, gazePoint.Data.Y);
        }

        private static void CreateAndVisualizeLightlyFilteredGazePointStream()
        {
            _gazePointDataStream = _host.Streams.CreateGazePointDataStream();
            _gazePointDataStream.GazePoint((x, y, ts) =>
            {
                Console.WriteLine("Timestamp: {0}\tX:{1}, Y:{2}", ts, x, y);
            });
        }

        private static void CreateAndVisualizeSensitiveFilteredFixationsStream()
        {
            _fixationDataStream = _host.Streams.CreateFixationDataStream();
            _fixationDataStream
                .Begin((x, y, _) =>
                    {
                        Console.WriteLine("\n" +
                                          "Fixation started at X: {0}, Y: {1}", x, y);
                        _fixationBeginTime = DateTime.Now;
                    })
                .Data((x, y, _) =>
                    {
                        Console.WriteLine("During fixation, currently at: X: {0}, Y: {1}", x, y);
                    })
                .End((x, y, _) =>
                    {
                        Console.WriteLine("Fixation ended at X: {0}, Y: {1}", x, y);
                        if (_fixationBeginTime != default(DateTime))
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Fixation duration: {0}", DateTime.Now - _fixationBeginTime);
                            Console.ForegroundColor = _defaultForegroundColor;
                        }
                    });
        }

        private static void CreateAndVisualizeSlowFilteredFixationsStream()
        {
            if (_fixationDataStream != null)
                _fixationDataStream.Next -= OnNextFixation;
            _fixationDataStream = _host.Streams.CreateFixationDataStream(FixationDataMode.Slow);
            _fixationDataStream.Next += OnNextFixation;
        }

        private static void OnNextFixation(object sender, StreamData<FixationData> fixation)
        {
            switch (fixation.Data.EventType)
            {
                case FixationDataEventType.Begin:
                    Console.WriteLine("\n" +
                                      "Fixation started at X: {0}, Y: {1}", fixation.Data.X, fixation.Data.Y);
                    _fixationBeginTimestamp = fixation.Data.Timestamp;
                    break;

                case FixationDataEventType.Data:
                    Console.WriteLine("During fixation, currently at: X: {0}, Y: {1}", fixation.Data.X, fixation.Data.Y);
                    break;

                case FixationDataEventType.End:
                    Console.WriteLine("Fixation ended at X: {0}, Y: {1}", fixation.Data.X, fixation.Data.Y);
                    if (!Double.IsNaN(_fixationBeginTimestamp))
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Fixation duration: {0}", TimeSpan.FromMilliseconds(fixation.Data.Timestamp - _fixationBeginTimestamp));
                        Console.ForegroundColor = _defaultForegroundColor;
                    }
                    break;
            }
        }

        private static void CreateAndVisualizeEyePositionDataStream()
        {
            _eyePositionDataStream = _host.Streams.CreateEyePositionStream();
            _eyePositionDataStream.EyePosition(eyePosition =>
            {
                Console.WriteLine("Has Left eye position: {0}", eyePosition.HasLeftEyePosition);
                Console.WriteLine("Left eye position: X:{0} Y:{1} Z:{2}",
                    eyePosition.LeftEye.X, eyePosition.LeftEye.Y, eyePosition.LeftEye.Z);
                Console.WriteLine("Left eye position (normalized): X:{0} Y:{1} Z:{2}",
                    eyePosition.LeftEyeNormalized.X, eyePosition.LeftEyeNormalized.Y, eyePosition.LeftEyeNormalized.Z);

                Console.WriteLine("Has Right eye position: {0}", eyePosition.HasRightEyePosition);
                Console.WriteLine("Right eye position: X:{0} Y:{1} Z:{2}",
                    eyePosition.RightEye.X, eyePosition.RightEye.Y, eyePosition.RightEye.Z);
                Console.WriteLine("Right eye position (normalized): X:{0} Y:{1} Z:{2}",
                    eyePosition.RightEyeNormalized.X, eyePosition.RightEyeNormalized.Y, eyePosition.RightEyeNormalized.Z);
                Console.WriteLine();
            });
        }

        private static void CreateAndVisualizeHeadPoseStream()
        {
            _headPoseStream = _host.Streams.CreateHeadPoseStream();
            _headPoseStream.Next += OnNextHeadPose;
        }

        private static void OnNextHeadPose(object sender, StreamData<HeadPoseData> headPose)
        {
            var timestamp = headPose.Data.Timestamp;
            var hasHeadPosition = headPose.Data.HasHeadPosition;
            var headPosition = headPose.Data.HeadPosition;
            var hasRotation = headPose.Data.HasRotation;
            var headRotation = headPose.Data.HeadRotation;

            Console.WriteLine($"Head pose timestamp  : {timestamp}");
            Console.WriteLine($"Has head position    : {hasHeadPosition}");
            Console.WriteLine($"Has rotation  (X,Y,Z): ({hasRotation.HasRotationX},{hasRotation.HasRotationY},{hasRotation.HasRotationZ})");
            Console.WriteLine($"Head position (X,Y,Z): ({headPosition.X},{headPosition.Y},{headPosition.Z})");
            Console.WriteLine($"Head rotation (X,Y,Z): ({headRotation.X},{headRotation.Y},{headRotation.Z})");
            Console.WriteLine("-----------------------------------------------------------------");
        }
    }
}
