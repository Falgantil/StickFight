using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using StickFight.Audio;
using StickFight.Properties;

namespace StickFight
{
    internal class Program
    {
        #region Constants

        /// <summary>
        ///     The initial Height offset for the map
        /// </summary>
        public const int Offset = 5;

        #endregion

        #region Fields

        private static bool soundEnabled = true;

        #endregion

        #region Properties

        public static AiCombat AiCombat { get; set; }

        /// <summary>
        ///     Gets or sets Player 1
        /// </summary>
        public static Player Player1 { get; set; }

        /// <summary>
        ///     Gets or sets Player 2
        /// </summary>
        public static Player Player2 { get; set; }

        /// <summary>
        ///     Gets or sets Player 1 status bar
        /// </summary>
        //public static StatusBar P1Status { get; set; }

        /// <summary>
        ///     Gets or sets Player 2 status bar
        /// </summary>
        //public static StatusBar P2Status { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating if the game has been won
        /// </summary>
        private static bool GameWon { get; set; }

        /// <summary>
        ///     Gets or sets the background music controller
        /// </summary>
        public static BackgroundMusic Music { get; set; }

        /// <summary>
        ///     Gets or sets a boolean value representing if sound should be played or not.
        /// </summary>
        public static bool SoundEnabled
        {
            get { return soundEnabled; }
            set
            {
                soundEnabled = value;
                if (value)
                {
                    ConsoleOptions.ChangeAudio = Resources.ChangeChoice;
                    ConsoleOptions.SelectAudio = Resources.SelectChoice;
                }
                else
                {
                    ConsoleOptions.ChangeAudio = null;
                    ConsoleOptions.SelectAudio = null;
                }
            }
        }

        #endregion

        /// <summary>
        ///     Starting entry for the program. Brings up the initial menu.
        /// </summary>
        private static void Main()
        {
            ConsoleOptions.ChangeAudio = Resources.ChangeChoice;
            ConsoleOptions.SelectAudio = Resources.SelectChoice;

            Console.Title = "Stick Fight - By Keta";

            string userPick = "";
            while (!userPick.StartsWith("4"))
            {
                if (userPick.StartsWith("1"))
                {
                    Initialize_Game_PVAI();
                }
                else if (userPick.StartsWith("2"))
                {
                    Initialize_Game_PVP();
                }
                else if (userPick.StartsWith("3"))
                {
                    SettingsScreen();
                    Settings.Default.Save();
                }
                Console.Clear();
                Console.Out.Write(Settings.Default.EntryScreen); // Writes out the standard main menu screen.
                userPick = ConsoleOptions.SendChoices("", new List<string>
                    {
                        "1: Start Human vs. AI",
                        "2: Start Human vs. Human",
                        "3: Settings",
                        "4: Exit"
                    });
            }

            Environment.Exit(0);
        }

        /// <summary>
        ///     Displays the settings screen, containing the control settings for Player 1 and 2.
        /// </summary>
        private static void SettingsScreen()
        {
            string selected = "";
            while (!selected.StartsWith("4"))
            {
                int cursorPosition = 0;
                if (selected.StartsWith("1"))
                {
                    ChangePlayer1Settings();
                }
                else if (selected.StartsWith("2"))
                {
                    ChangePlayer2Settings();
                }
                else if (selected.StartsWith("3"))
                {
                    SoundEnabled = !SoundEnabled;
                    cursorPosition = 3;
                }
                Console.Clear();
                Console.WriteLine("\n  Player 1:");
                Console.WriteLine("\tLeft: " + (ConsoleKey) Settings.Default.Plr1Left);
                Console.WriteLine("\tRight: " + (ConsoleKey) Settings.Default.Plr1Right);
                Console.WriteLine("\tUp: " + (ConsoleKey) Settings.Default.Plr1Up);
                Console.WriteLine("\tDown: " + (ConsoleKey) Settings.Default.Plr1Down);
                Console.WriteLine("\tHit: " + (ConsoleKey) Settings.Default.Plr1Hit);
                Console.WriteLine("\tKick: " + (ConsoleKey) Settings.Default.Plr1Kick);
                Console.WriteLine("\tBlock: " + (ConsoleKey) Settings.Default.Plr1Block);
                Console.WriteLine("\n  Player 2:");
                Console.WriteLine("\tLeft: " + (ConsoleKey) Settings.Default.Plr2Left);
                Console.WriteLine("\tRight: " + (ConsoleKey) Settings.Default.Plr2Right);
                Console.WriteLine("\tUp: " + (ConsoleKey) Settings.Default.Plr2Up);
                Console.WriteLine("\tDown: " + (ConsoleKey) Settings.Default.Plr2Down);
                Console.WriteLine("\tHit: " + (ConsoleKey) Settings.Default.Plr2Hit);
                Console.WriteLine("\tKick: " + (ConsoleKey) Settings.Default.Plr2Kick);
                Console.WriteLine("\tBlock: " + (ConsoleKey) Settings.Default.Plr2Block);
                selected = ConsoleOptions.SendChoices("",
                                                      new List<string>
                                                          {
                                                              "1: Change player 1 controls",
                                                              "2: Change player 2 controls",
                                                              "3: Disable/Enable music",
                                                              "4: Back",
                                                          }, cursorPosition);
            }
        }

        /// <summary>
        ///     Displays the Player 1 settings screen, allowing the user to set keybindings for all controls.
        /// </summary>
        private static void ChangePlayer1Settings()
        {
            string selected = "0";
            while (!selected.StartsWith("8"))
            {
                if (selected.StartsWith("1"))
                {
                    string Header = "Press a key bind Left to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr1Left = (int) (Console.ReadKey(true).Key);
                }
                else if (selected.StartsWith("2"))
                {
                    string Header = "Press a key bind Right to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr1Right = (int) (Console.ReadKey(true).Key);
                }
                else if (selected.StartsWith("3"))
                {
                    string Header = "Press a key bind Up to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr1Up = (int) (Console.ReadKey(true).Key);
                }
                else if (selected.StartsWith("4"))
                {
                    string Header = "Press a key bind Down to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr1Down = (int) (Console.ReadKey(true).Key);
                }
                else if (selected.StartsWith("5"))
                {
                    string Header = "Press a key bind Hit to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr1Hit = (int) (Console.ReadKey(true).Key);
                }
                else if (selected.StartsWith("6"))
                {
                    string Header = "Press a key bind Kick to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr1Kick = (int) (Console.ReadKey(true).Key);
                }
                else if (selected.StartsWith("7"))
                {
                    string Header = "Press a key bind Block to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr1Block = (int) (Console.ReadKey(true).Key);
                }

                Console.Clear();
                Console.WriteLine("Select control to edit:");
                selected = ConsoleOptions.SendChoices("", new List<string>
                    {
                        "1: Left",
                        "2: Right",
                        "3: Up",
                        "4: Down",
                        "5: Hit",
                        "6: Kick",
                        "7: Block",
                        "8: Back",
                    }, Convert.ToInt32(selected.Substring(0, 1)));
            }
        }

        /// <summary>
        ///     Displays the Player 2 settings screen, allowing the user to set keybindings for all controls.
        /// </summary>
        private static void ChangePlayer2Settings()
        {
            string selected = "0";
            while (!selected.StartsWith("8"))
            {
                if (selected.StartsWith("1"))
                {
                    string Header = "Press a key bind Left to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr2Left = (int) (Console.ReadKey(true).Key);
                }
                else if (selected.StartsWith("2"))
                {
                    string Header = "Press a key bind Right to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr2Right = (int) (Console.ReadKey(true).Key);
                }
                else if (selected.StartsWith("3"))
                {
                    string Header = "Press a key bind Up to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr2Up = (int) (Console.ReadKey(true).Key);
                }
                else if (selected.StartsWith("4"))
                {
                    string Header = "Press a key bind Down to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr2Down = (int) (Console.ReadKey(true).Key);
                }
                else if (selected.StartsWith("5"))
                {
                    string Header = "Press a key bind Hit to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr2Hit = (int) (Console.ReadKey(true).Key);
                }
                else if (selected.StartsWith("6"))
                {
                    string Header = "Press a key bind Kick to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr2Kick = (int) (Console.ReadKey(true).Key);
                }
                else if (selected.StartsWith("7"))
                {
                    string Header = "Press a key bind Block to:";
                    SettingsHeaderMessage(Header);
                    Settings.Default.Plr2Block = (int) (Console.ReadKey(true).Key);
                }

                Console.Clear();
                Console.WriteLine("Select control to edit:");
                selected = ConsoleOptions.SendChoices("", new List<string>
                    {
                        "1: Left",
                        "2: Right",
                        "3: Up",
                        "4: Down",
                        "5: Hit",
                        "6: Kick",
                        "7: Block",
                        "8: Back",
                    }, Convert.ToInt32(selected.Substring(0, 1)));
            }
        }

        /// <summary>
        ///     Sets the header message in the keybinding menu.
        /// </summary>
        /// <param name="message">The message to display</param>
        private static void SettingsHeaderMessage(string message)
        {
            int startPosition = 40;

            Console.SetCursorPosition(startPosition - (message.Length/2), 2);
            Console.Out.Write(message);
            Console.WriteLine();
        }

        /// <summary>
        ///     Starts the game, prompting the user to enter player 1 and 2 names and select color.
        /// </summary>
        private static void Initialize_Game_PVP()
        {
            bool Replay = true;

            Console.Title = "Stick Fight - Player versus Player";

            Console.Out.WriteLine("Enter Player 1 name.");
            string plr1Name = Console.ReadLine();
            while (plr1Name == "")
            {
                Console.Out.WriteLine("You need a name.");
                plr1Name = Console.ReadLine();
            }

            Console.Out.WriteLine("Enter Player 2 name.");
            string plr2Name = Console.ReadLine();
            while (plr2Name == "" || plr2Name.ToLower() == plr1Name.ToLower())
            {
                Console.Out.WriteLine("You need a name.");
                plr2Name = Console.ReadLine();
            }

            #region Player 1 Color Selecting

            ConsoleColor Plr1Color;

            #region Player 1 color listing and selection

            var colorName1 = new List<string>();
            var colorConsole1 = new List<ConsoleColor>();

            foreach (ConsoleColor item in Enum.GetValues(typeof (ConsoleColor)))
            {
                string colorName = item.ToString().ToLower();
                if (!colorName.Contains("black"))
                {
                    colorName1.Add(item.ToString());
                    colorConsole1.Add(item);
                }
            }

            #endregion

            Console.Clear();
            if (
                !Enum.TryParse(
                    ConsoleOptions.SendChoices("Select " + plr1Name + "'s Color", colorName1, colorConsole1),
                    out Plr1Color))
                throw new Exception("Error on converting player 1's color");

            #endregion

            #region Player 2 Color Selecting

            ConsoleColor plr2Color;

            #region Player 2 color listing and selection

            var colorName2 = new List<string>();
            var colorConsole2 = new List<ConsoleColor>();

            foreach (ConsoleColor item in Enum.GetValues(typeof (ConsoleColor)))
            {
                string colorName = item.ToString().ToLower();
                if (!colorName.Contains("black") && item != Plr1Color)
                {
                    colorName2.Add(item.ToString());
                    colorConsole2.Add(item);
                }
            }

            #endregion

            Console.Clear();
            if (
                !Enum.TryParse(
                    ConsoleOptions.SendChoices("Select " + plr2Name + "'s Color", colorName2, colorConsole2),
                    out plr2Color))
                throw new Exception("Error on converting player 2's color");

            #endregion

            #region Handles Map Selection

            if (!Directory.Exists("Maps"))
                Directory.CreateDirectory("Maps");

            List<string> MapPaths = Directory.GetFiles("Maps").ToList();

            if (MapPaths.Count == 0)
            {
                File.WriteAllText(@"Maps\Map_00.txt", Settings.Default.StandardMap);
                MapPaths = Directory.GetFiles("Maps").ToList();
            }

            for (int i = 0; i < MapPaths.Count; i++)
            {
                MapPaths[i] = MapPaths[i].Replace(@"Maps\", "");
                MapPaths[i] = MapPaths[i].Replace(".txt", "");
            }

            string chosenMap = @"Maps\" + ConsoleOptions.SendChoices("Choose your Arena", MapPaths) + ".txt";

            #endregion

            Console.Out.WriteLine("Game Starting in 3 seconds");

            Thread.Sleep(3000);

            Player.EffectPlayer = new List<Effect>
                {
                    new Effect(Resources.Block),
                    new Effect(Resources.Punch_1),
                    new Effect(Resources.Punch_2),
                    new Effect(Resources.Punch_3)
                };

            Music = new BackgroundMusic("BackgroundMusic", Resources.Background_Theme, true);

            #region Controls the actual game process.

            while (Replay)
            {
                try
                {
                    Player1 = new Player(
                        plr1Name,
                        Plr1Color,
                        new Point(20, 20 + Offset))
                        {
                            HpBarPosition = 1
                        };

                    Player2 = new Player(
                        plr2Name,
                        plr2Color,
                        new Point(60, 20 + Offset))
                        {
                            HpBarPosition = 59
                        };

                    //P1Status = new StatusBar(new Point(1, 31), 30, true);
                    //P2Status = new StatusBar(new Point(79, 31), 30, false);

                    GameWon = false;

                    Console.CursorVisible = false;

                    Map.CreateMap(chosenMap);

                    Player1.UpdateAnim();
                    Player2.UpdateAnim();

                    //P1Status.DrawText("0123456789012345678901234567895234", new TimeSpan(0, 0, 3));
                    //P2Status.DrawText("012345678901234567890123456789", new TimeSpan(0, 0, 3));

                    #region Countdown to start.

                    Thread.Sleep(1000);
                    SetHeaderMessage("Ready");
                    //P1Status.DrawText("Blargh fish", new TimeSpan(0, 0, 10));
                    Thread.Sleep(1000);
                    //P2Status.DrawText("Fiskemad", new TimeSpan(0, 0, 4));
                    SetHeaderMessage("Set!");

                    if (!Music.IsPlaying)
                        Music.StartMusic();

                    Thread.Sleep(1000);
                    SetHeaderMessage("GO!");

                    #endregion

                    #region The actual main loop, checking for user input, controlling the players according to the buttons pressed.

                    int plr1Left = Settings.Default.Plr1Left;
                    int plr1Right = Settings.Default.Plr1Right;
                    int plr1Down = Settings.Default.Plr1Down;
                    int plr1Up = Settings.Default.Plr1Up;
                    int plr1Hit = Settings.Default.Plr1Hit;
                    int plr1Kick = Settings.Default.Plr1Kick;
                    int plr1Block = Settings.Default.Plr1Block;

                    int plr2Left = Settings.Default.Plr2Left;
                    int plr2Right = Settings.Default.Plr2Right;
                    int plr2Down = Settings.Default.Plr2Down;
                    int plr2Up = Settings.Default.Plr2Up;
                    int plr2Hit = Settings.Default.Plr2Hit;
                    int plr2Kick = Settings.Default.Plr2Kick;
                    int plr2Block = Settings.Default.Plr2Block;

                    while (!GameWon)
                    {
                        if (NativeKeyboard.IsKeyDown(plr1Left))
                            Player1.Move(Direction.Left);
                        else if (NativeKeyboard.IsKeyDown(plr1Right))
                            Player1.Move(Direction.Right);
                        else if (NativeKeyboard.IsKeyDown(plr1Down))
                            Player1.Move(Direction.Down);
                        if (NativeKeyboard.IsKeyDown(plr1Up))
                            Player1.Jump();
                        if (NativeKeyboard.IsKeyDown(plr1Hit))
                            Player1.Hit();
                        if (NativeKeyboard.IsKeyDown(plr1Kick))
                            Player1.Kick();
                        if (NativeKeyboard.IsKeyDown(plr1Block))
                            Player1.Block();
                        Player1.UpdateMovement();

                        if (NativeKeyboard.IsKeyDown(plr2Left))
                            Player2.Move(Direction.Left);
                        else if (NativeKeyboard.IsKeyDown(plr2Right))
                            Player2.Move(Direction.Right);
                        else if (NativeKeyboard.IsKeyDown(plr2Down))
                            Player2.Move(Direction.Down);
                        if (NativeKeyboard.IsKeyDown(plr2Up))
                            Player2.Jump();
                        if (NativeKeyboard.IsKeyDown(plr2Hit))
                            Player2.Hit();
                        if (NativeKeyboard.IsKeyDown(plr2Kick))
                            Player2.Kick();
                        if (NativeKeyboard.IsKeyDown(plr2Block))
                            Player2.Block();
                        Player2.UpdateMovement();

                        GameWon = CheckForWinner();
                        Thread.Sleep(30);
                    }

                    #endregion

                    #region After game ends, moves the player models back to default position.

                    for (int i = Player1.Collision.X; i < Player1.Collision.X + 5; i++)
                    {
                        for (int u = Player1.Collision.Y; u < Player1.Collision.Y + 5; u++)
                        {
                            Console.SetCursorPosition(i, u);
                            Console.Out.Write(" ");
                        }
                    }

                    for (int i = Player2.Collision.X; i < Player2.Collision.X + 5; i++)
                    {
                        for (int u = Player2.Collision.Y; u < Player2.Collision.Y + 5; u++)
                        {
                            Console.SetCursorPosition(i, u);
                            Console.Out.Write(" ");
                        }
                    }

                    Player1.Collision = new Rectangle(20, 20 + Offset, 5, 5);
                    Player2.Collision = new Rectangle(60, 20 + Offset, 5, 5);
                    Player1.UpdateAnim();
                    Player2.UpdateAnim();

                    #endregion

                    if (Player1.Health == 0 && Player2.Health == 0)
                    {
                        SetHeaderMessage("It is tied!");
                    }
                    else if (Player1.Health == 0)
                    {
                        SetHeaderMessage(Player2.Name + " has won the Game!");
                    }
                    else if (Player2.Health == 0)
                    {
                        SetHeaderMessage(Player1.Name + " has won the Game!");
                    }
                    else
                    {
                        SetHeaderMessage("Unknown Player has won the Game!");
                    }
                    Thread.Sleep(3000); // Sleeps to allow for the winning to be displayed.
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                SetHeaderMessage("Play Again? Y/N");

                while (true)
                {
                    if (NativeKeyboard.IsKeyDown(Convert.ToInt32('Y')))
                    {
                        Replay = true;
                        break;
                    }
                    else if (NativeKeyboard.IsKeyDown(Convert.ToInt32('N')))
                    {
                        Replay = false;
                        break;
                    }

                    Thread.Sleep(10);
                }
            }

            #endregion
        }

        /// <summary>
        ///     Starts the game, prompting the user to enter player 1 and 2 names and select color.
        /// </summary>
        private static void Initialize_Game_PVAI()
        {
            bool Replay = true;

            Console.Title = "Stick Fight - Player versus Artificial Intelligence";

            Console.Out.WriteLine("Enter Player 1 name.");
            string plr1Name = Console.ReadLine();
            while (plr1Name == "")
            {
                Console.Out.WriteLine("You need a name.");
                plr1Name = Console.ReadLine();
            }

            #region Player 1 Color Selecting

            ConsoleColor Plr1Color;

            #region Player 1 color listing and selection

            var colorName1 = new List<string>();
            var colorConsole1 = new List<ConsoleColor>();

            foreach (ConsoleColor item in Enum.GetValues(typeof(ConsoleColor)))
            {
                string colorName = item.ToString().ToLower();
                if (!colorName.Contains("black"))
                {
                    colorName1.Add(item.ToString());
                    colorConsole1.Add(item);
                }
            }

            #endregion

            Console.Clear();
            if (
                !Enum.TryParse(
                    ConsoleOptions.SendChoices("Select " + plr1Name + "'s Color", colorName1, colorConsole1),
                    out Plr1Color))
                throw new Exception("Error on converting player 1's color");

            #endregion

            #region Player 2 Color Selecting

            List<ConsoleColor> colors = Enum.GetValues(typeof (ConsoleColor)).Cast<ConsoleColor>().Where(item => item != Plr1Color && item != ConsoleColor.Black).ToList();

            ConsoleColor plr2Color = colors[new Random().Next(0, colors.Count)];

            Console.Clear();

            #endregion

            #region Handles Map Selection

            if (!Directory.Exists("Maps"))
                Directory.CreateDirectory("Maps");

            List<string> mapPaths = Directory.GetFiles("Maps").ToList();

            if (mapPaths.Count == 0)
            {
                File.WriteAllText(@"Maps\Map_00.txt", Settings.Default.StandardMap);
                mapPaths = Directory.GetFiles("Maps").ToList();
            }

            for (int i = 0; i < mapPaths.Count; i++)
            {
                mapPaths[i] = mapPaths[i].Replace(@"Maps\", "");
                mapPaths[i] = mapPaths[i].Replace(".txt", "");
            }

            string chosenMap = @"Maps\" + ConsoleOptions.SendChoices("Choose your Arena", mapPaths) + ".txt";

            #endregion

            Console.Out.WriteLine("Game Starting in 3 seconds");

            Thread.Sleep(3000);

            Player.EffectPlayer = new List<Effect>
                {
                    new Effect(Resources.Block),
                    new Effect(Resources.Punch_1),
                    new Effect(Resources.Punch_2),
                    new Effect(Resources.Punch_3)
                };

            Music = new BackgroundMusic("BackgroundMusic", Resources.Background_Theme, true);

            #region Controls the actual game process.

            while (Replay)
            {
                try
                {
                    Player1 = new Player(
                        plr1Name,
                        Plr1Color,
                        new Point(20, 20 + Offset))
                    {
                        HpBarPosition = 1
                    };

                    Player2 = new Player(
                        "Computer AI",
                        plr2Color,
                        new Point(60, 20 + Offset))
                    {
                        HpBarPosition = 59
                    };

                    AiCombat = new AiCombat(Player2);

                    //P1Status = new StatusBar(new Point(1, 31), 30, true);
                    //P2Status = new StatusBar(new Point(79, 31), 30, false);

                    GameWon = false;

                    Console.CursorVisible = false;

                    Map.CreateMap(chosenMap);

                    Player1.UpdateAnim();
                    Player2.UpdateAnim();

                    #region Countdown to start.

                    Thread.Sleep(1000);
                    SetHeaderMessage("Ready");
                    Thread.Sleep(1000);
                    SetHeaderMessage("Set!");

                    if (!Music.IsPlaying)
                        Music.StartMusic();

                    Thread.Sleep(1000);
                    SetHeaderMessage("GO!");

                    #endregion

                    #region The actual main loop, checking for user input, controlling the players according to the buttons pressed.

                    int plr1Left = Settings.Default.Plr1Left;
                    int plr1Right = Settings.Default.Plr1Right;
                    int plr1Down = Settings.Default.Plr1Down;
                    int plr1Up = Settings.Default.Plr1Up;
                    int plr1Hit = Settings.Default.Plr1Hit;
                    int plr1Kick = Settings.Default.Plr1Kick;
                    int plr1Block = Settings.Default.Plr1Block;

                    while (!GameWon)
                    {
                        if (NativeKeyboard.IsKeyDown(plr1Left))
                            Player1.Move(Direction.Left);
                        else if (NativeKeyboard.IsKeyDown(plr1Right))
                            Player1.Move(Direction.Right);
                        else if (NativeKeyboard.IsKeyDown(plr1Down))
                            Player1.Move(Direction.Down);
                        if (NativeKeyboard.IsKeyDown(plr1Up))
                            Player1.Jump();
                        if (NativeKeyboard.IsKeyDown(plr1Hit))
                            Player1.Hit();
                        if (NativeKeyboard.IsKeyDown(plr1Kick))
                            Player1.Kick();
                        if (NativeKeyboard.IsKeyDown(plr1Block))
                            Player1.Block();
                        Player1.UpdateMovement();

                        AiCombat.Update(Player1);
                        Player2.UpdateMovement();

                        GameWon = CheckForWinner();
                        Thread.Sleep(30);
                    }

                    #endregion

                    #region After game ends, moves the player models back to default position.

                    for (int i = Player1.Collision.X; i < Player1.Collision.X + 5; i++)
                    {
                        for (int u = Player1.Collision.Y; u < Player1.Collision.Y + 5; u++)
                        {
                            Console.SetCursorPosition(i, u);
                            Console.Out.Write(" ");
                        }
                    }

                    for (int i = Player2.Collision.X; i < Player2.Collision.X + 5; i++)
                    {
                        for (int u = Player2.Collision.Y; u < Player2.Collision.Y + 5; u++)
                        {
                            Console.SetCursorPosition(i, u);
                            Console.Out.Write(" ");
                        }
                    }

                    Player1.Collision = new Rectangle(20, 20 + Offset, 5, 5);
                    Player2.Collision = new Rectangle(60, 20 + Offset, 5, 5);
                    Player1.UpdateAnim();
                    Player2.UpdateAnim();

                    #endregion

                    if (Player1.Health == 0 && Player2.Health == 0)
                    {
                        SetHeaderMessage("It is tied!");
                    }
                    else if (Player1.Health == 0)
                    {
                        SetHeaderMessage(Player2.Name + " has won the Game!");
                    }
                    else if (Player2.Health == 0)
                    {
                        SetHeaderMessage(Player1.Name + " has won the Game!");
                    }
                    else
                    {
                        SetHeaderMessage("Unknown Player has won the Game!");
                    }
                    Thread.Sleep(3000); // Sleeps to allow for the winning to be displayed.
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                SetHeaderMessage("Play Again? Y/N");

                while (true)
                {
                    if (NativeKeyboard.IsKeyDown(Convert.ToInt32('Y')))
                    {
                        Replay = true;
                        break;
                    }
                    else if (NativeKeyboard.IsKeyDown(Convert.ToInt32('N')))
                    {
                        Replay = false;
                        break;
                    }

                    Thread.Sleep(10);
                }
            }

            #endregion
        }

        /// <summary>
        ///     Returns a value indication if player 1 or player 2 has 0 health.
        /// </summary>
        /// <returns></returns>
        public static bool CheckForWinner()
        {
            return (Player1.Health == 0 || Player2.Health == 0);
        }

        /// <summary>
        ///     Sets the header in the game's message to the specified string.
        /// </summary>
        /// <param name="Message">The header string to display</param>
        private static void SetHeaderMessage(string Message)
        {
            int startPosition = 40;

            Console.SetCursorPosition(21, 2);

            for (int i = 0; i < 38; i++)
            {
                Console.Out.Write(" ");
            }

            Console.SetCursorPosition(startPosition - (Message.Length/2), 2);
            Console.Out.Write(Message);
            Console.SetCursorPosition(0, 26 + Offset);
        }
    }
}