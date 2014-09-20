using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using StickFight.Audio;

namespace StickFight
{
    public class Player
    {
        #region Fields

        private bool falling;
        private int health = 20;
        private bool jumping;
        private Rectangle newCollision;
        private Timer useTimer;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the state of which the player is in
        /// </summary>
        public Standing State { get; set; }

        /// <summary>
        ///     Gets or sets the name of the player
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the color of the player
        /// </summary>
        private ConsoleColor Color { get; set; }

        /// <summary>
        ///     Gets or sets the state of the player
        /// </summary>
        private Dictionary<Standing, List<string>> PlayerStanding { get; set; }

        /// <summary>
        ///     Gets or sets the collision and position of the player
        /// </summary>
        public Rectangle Collision { get; set; }

        /// <summary>
        ///     Gets or sets the position of the players health bar
        /// </summary>
        public int HpBarPosition { get; set; }

        /// <summary>
        ///     Gets or sets the health of the player.
        /// </summary>
        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                UpdateHealth();
            }
        }

        /// <summary>
        ///     Gets or sets the sound effects that can be played
        /// </summary>
        public static List<Effect> EffectPlayer { get; set; }

        #endregion

        /// <summary>
        ///     Creates a standard player with the specified name, color and initial position
        /// </summary>
        /// <param name="name">The name of the player</param>
        /// <param name="color">The color of the player</param>
        /// <param name="location">The initialize location of the player</param>
        public Player(string name, ConsoleColor color, Point location)
        {
            Name = name;
            Color = color;
            Collision = new Rectangle(location, new Size(5, 5));
            this.State = Standing.Stand;
            PlayerStanding = new Dictionary<Standing, List<string>>();

            PlayerStanding.Add(Standing.Stand, new List<string>
                {
                    ("  O  "),
                    ("'-|-'"),
                    ("  |  "),
                    (" / \\ "),
                    ("/   \\")
                });

            PlayerStanding.Add(Standing.RightHit, new List<string>
                {
                    ("  O  "),
                    ("'-|--"),
                    ("  |  "),
                    (" / \\ "),
                    ("/   \\")
                });

            PlayerStanding.Add(Standing.LeftHit, new List<string>
                {
                    ("  O  "),
                    ("--|-'"),
                    ("  |  "),
                    (" / \\ "),
                    ("/   \\")
                });

            PlayerStanding.Add(Standing.RightKick, new List<string>
                {
                    ("  O  "),
                    ("'-|-'"),
                    ("  |  "),
                    (" / \\_"),
                    ("/    ")
                });

            PlayerStanding.Add(Standing.LeftKick, new List<string>
                {
                    ("  O  "),
                    ("'-|-'"),
                    ("  |  "),
                    ("_/ \\ "),
                    ("    \\")
                });

            PlayerStanding.Add(Standing.LeftBlock, new List<string>
                {
                    ("| O  "),
                    ("|-|-'"),
                    ("| |  "),
                    ("|/ \\ "),
                    ("/   \\")
                });

            PlayerStanding.Add(Standing.RightBlock, new List<string>
                {
                    ("  O |"),
                    ("'-|-|"),
                    ("  | |"),
                    (" / \\|"),
                    ("/   \\")
                });

            newCollision = new Rectangle(location, new Size(5, 5));
        }

        /// <summary>
        ///     Gets the opposite player of the one currently being managed
        /// </summary>
        private Player GetOppositePlayer
        {
            get
            {
                if (this == Program.Player1)
                {
                    return Program.Player2;
                }
                else
                {
                    return Program.Player1;
                }
            }
        }

        /// <summary>
        ///     Performs a basic hit, doing 1 point of damage
        /// </summary>
        public void Hit()
        {
            if (State == Standing.Stand)
            {
                if (GetOppositePlayer.Collision.X > Collision.X)
                {
                    State = Standing.RightHit;
                }
                else
                {
                    State = Standing.LeftHit;
                }
                useTimer = new Timer(ResetStanding, null, 250, 250);
                UpdateAnim();

                bool breakout = false;
                for (int i = Collision.X - 1; i < Collision.X + Collision.Width + 1; i++)
                {
                    for (int u = Collision.Y - 1; u < Collision.Y + Collision.Height + 1; u++)
                    {
                        if (Nearby(new Point(i, u), GetOppositePlayer.Collision))
                        {
                            if (GetOppositePlayer.State != Standing.LeftBlock &&
                                GetOppositePlayer.State != Standing.RightBlock)
                            {
                                GetOppositePlayer.Health--;

                                EffectPlayer[new Random().Next(1, 4)].Play();
                            }
                            else
                            {
                                EffectPlayer[0].Play();
                            }
                            breakout = true;
                        }
                        if (breakout)
                            break;
                    }
                    if (breakout)
                        break;
                }
            }
        }

        /// <summary>
        ///     Performs a basic kick, doing 1 point of damage
        /// </summary>
        public void Kick()
        {
            if (State == Standing.Stand)
            {
                if (GetOppositePlayer.Collision.X > Collision.X)
                {
                    State = Standing.RightKick;
                }
                else
                {
                    State = Standing.LeftKick;
                }
                useTimer = new Timer(ResetStanding, null, 250, 250);
                UpdateAnim();

                bool breakout = false;
                for (int i = Collision.X - 1; i < Collision.X + Collision.Width + 1; i++)
                {
                    for (int u = Collision.Y - 1; u < Collision.Y + Collision.Height + 1; u++)
                    {
                        if (Nearby(new Point(i, u), GetOppositePlayer.Collision))
                        {
                            if (GetOppositePlayer.State != Standing.LeftBlock &&
                                GetOppositePlayer.State != Standing.RightBlock)
                            {
                                GetOppositePlayer.Health--;

                                EffectPlayer[new Random().Next(1, 4)].Play();
                            }
                            else
                            {
                                EffectPlayer[0].Play();
                            }
                            breakout = true;
                        }
                        if (breakout)
                            break;
                    }
                    if (breakout)
                        break;
                }
            }
        }

        /// <summary>
        ///     Performs a basic block, preventing all damage taken while it lasts
        /// </summary>
        public void Block()
        {
            if (State == Standing.Stand)
            {
                if (GetOppositePlayer.Collision.X > Collision.X)
                {
                    State = Standing.RightBlock;
                }
                else
                {
                    State = Standing.LeftBlock;
                }
                useTimer = new Timer(ResetStanding, null, 500, 500);
                UpdateAnim();
            }
        }

        /// <summary>
        ///     Resets the standing of the player
        /// </summary>
        private void ResetStanding(object obj)
        {
            if (useTimer != null)
            {
                useTimer.Dispose();
                useTimer = null;
            }

            State = Standing.Stand;
            UpdateAnim();
        }

        /// <summary>
        ///     Attemps to perform a basic jump, moving upwards 5 points
        /// </summary>
        public void Jump()
        {
            if (CanWalkThisWay(Direction.Up) && !jumping && !falling)
            {
                var bgWorker = new BackgroundWorker();
                bgWorker.DoWork += (sender, e) =>
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            if (CanWalkThisWay(Direction.Up))
                            {
                                UpdateMovement();
                                Move(Direction.Up);
                            }
                            else
                            {
                                UpdateMovement();
                                break;
                            }
                            Thread.Sleep(60);
                        }
                        Fall();
                        jumping = false;
                    };
                bgWorker.RunWorkerAsync();
                jumping = true;
                falling = true;
            }
        }

        /// <summary>
        ///     Starts the falling simulation on a seperate thread
        /// </summary>
        private void StartFall()
        {
            var simulateFall = new BackgroundWorker();
            simulateFall.DoWork += (sender, e) => Fall();
            simulateFall.RunWorkerAsync();
        }

        /// <summary>
        ///     Initializes the falling sequence
        /// </summary>
        private void Fall()
        {
            while (CanWalkThisWay(Direction.Down))
            {
                falling = true;
                if (newCollision.Y < 20 + Program.Offset && CanWalkThisWay(Direction.Down))
                {
                    UpdateMovement();
                    Move(Direction.Down);
                }
                else
                {
                    UpdateMovement();
                    break;
                }
                Thread.Sleep(60);
            }
            falling = false;
        }

        /// <summary>
        ///     Updates both the players health bars
        /// </summary>
        private static void UpdateHealth()
        {
            Console.SetCursorPosition(Program.Player1.HpBarPosition, 2);
            Console.ForegroundColor = Program.Player1.Color;
            for (int i = 0; i < 20; i++)
            {
                if (i >= Program.Player1.Health)
                {
                    Console.Out.Write(" ");
                }
                else
                {
                    Console.Out.Write("▓");
                }
            }

            Console.SetCursorPosition(Program.Player2.HpBarPosition, 2);
            Console.ForegroundColor = Program.Player2.Color;
            for (int i = 0; i < 20; i++)
            {
                if (i < 20 - Program.Player2.Health)
                {
                    Console.Out.Write(" ");
                }
                else
                {
                    Console.Out.Write("▓");
                }
            }
        }

        /// <summary>
        ///     Updates the animation to the appropriate standing
        /// </summary>
        public void UpdateAnim()
        {
            lock (this)
            {
                SetConsoleColor();
                try
                {
                    for (int i = Collision.Y; i < Collision.Y + Collision.Height; i++)
                    {
                        if (i - Collision.Y < PlayerStanding[State].Count)
                        {
                            Console.SetCursorPosition(Collision.X, i);
                            Console.Write(PlayerStanding[State][i - Collision.Y]);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        /// <summary>
        ///     Attempts to move in the specified direction
        /// </summary>
        /// <param name="direction">The direction to move</param>
        public void Move(Direction direction)
        {
            try
            {
                switch (direction)
                {
                    case Direction.Up:
                        if (newCollision.Y > 1 + Program.Offset && CanWalkThisWay(direction))
                        {
                            newCollision = new Rectangle(newCollision.X, newCollision.Y - 1, 5, 5);
                        }
                        break;
                    case Direction.Down:
                        if (newCollision.Y < 20 + Program.Offset && CanWalkThisWay(direction))
                        {
                            newCollision = new Rectangle(newCollision.X, newCollision.Y + 1, 5, 5);
                        }
                        break;
                    case Direction.Left:
                        if (newCollision.X > 1 && CanWalkThisWay(direction))
                        {
                            newCollision = new Rectangle(newCollision.X - 1, newCollision.Y, 5, 5);
                        }
                        break;
                    case Direction.Right:
                        if (newCollision.X < 74 && CanWalkThisWay(direction))
                        {
                            newCollision = new Rectangle(newCollision.X + 1, newCollision.Y, 5, 5);
                        }
                        break;
                    default:
                        break;
                }
                if (CanWalkThisWay(Direction.Down) && !falling && (newCollision.Y < 20 + Program.Offset))
                    StartFall();
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///     Moves the player from the old position, to the new
        /// </summary>
        public void UpdateMovement()
        {
            if (!(Collision.X == newCollision.X && Collision.Y == newCollision.Y))
            {
                lock (this)
                {
                    Console.MoveBufferArea(Collision.X, Collision.Y, Collision.Width, Collision.Height, newCollision.X,
                                           newCollision.Y, ' ', Color, ConsoleColor.Black);
                    Collision = new Rectangle(newCollision.X, newCollision.Y, newCollision.Width, newCollision.Height);
                    UpdateAnim();
                }
            }
        }

/*
        /// <summary>
        /// Returns a boolean representing if the point specified, is within the rectangle specified
        /// </summary>
        /// <param name="comparer">Point to check on</param>
        /// <param name="otherRectangle">Rectangle to compare with</param>
        /// <returns></returns>
        private bool Contains(Point comparer, Rectangle otherRectangle)
        {
            for (int j = otherRectangle.X - 1; j < otherRectangle.X + otherRectangle.Width + 1; j++)
                for (int t = otherRectangle.Y - 1; t < otherRectangle.Y + otherRectangle.Height + 1; t++)
                    if (otherRectangle.Contains(comparer))
                        return true;

            return false;
        }
*/

        /// <summary>
        ///     Returns a boolean representing if the point specified, is next to the rectangle specified
        /// </summary>
        /// <param name="comparer">Point to check on</param>
        /// <param name="otherRectangle">Rectangle to compare with</param>
        /// <returns></returns>
        public bool Nearby(Point comparer, Rectangle otherRectangle)
        {
            for (int j = otherRectangle.X - 2; j < otherRectangle.X + otherRectangle.Width + 2; j++)
                for (int t = otherRectangle.Y - 2; t < otherRectangle.Y + otherRectangle.Height + 2; t++)
                    if (otherRectangle.Contains(comparer))
                        return true;

            return false;
        }

        /// <summary>
        ///     Determines if the player can walk in the specified direction
        /// </summary>
        /// <param name="direction">The direction to check on</param>
        /// <returns></returns>
        public bool CanWalkThisWay(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    for (int i = 0; i < 5; i++)
                    {
                        foreach (Point collisionPosition in Map.MapCollisions)
                            if (newCollision.Contains(new Point(collisionPosition.X, collisionPosition.Y + 1)))
                                return false;
                        if (GetOppositePlayer.Collision.Contains(newCollision.X + i, newCollision.Y - 1))
                            return false;
                    }
                    break;
                case Direction.Down:
                    for (int i = 0; i < 5; i++)
                    {
                        foreach (Point collisionPosition in Map.MapCollisions)
                            if (newCollision.Contains(new Point(collisionPosition.X, collisionPosition.Y - 1)))
                                return false;
                        if (GetOppositePlayer.Collision.Contains(newCollision.X + i, newCollision.Y + 5))
                            return false;
                    }
                    break;
                case Direction.Left:
                    for (int i = 0; i < 5; i++)
                    {
                        foreach (Point collisionPosition in Map.MapCollisions)
                            if (newCollision.Contains(new Point(collisionPosition.X + 1, collisionPosition.Y)))
                                return false;
                        if (GetOppositePlayer.Collision.Contains(newCollision.X - 1, newCollision.Y + i))
                            return false;
                    }
                    break;
                case Direction.Right:
                    for (int i = 0; i < 5; i++)
                    {
                        foreach (Point collisionPosition in Map.MapCollisions)
                            if (newCollision.Contains(new Point(collisionPosition.X - 1, collisionPosition.Y)))
                                return false;
                        if (GetOppositePlayer.Collision.Contains(newCollision.X + 5, newCollision.Y + i))
                            return false;
                    }
                    break;
                default:
                    break;
            }

            return true;
        }

        /// <summary>
        ///     Sets the console color to the player color
        /// </summary>
        public void SetConsoleColor()
        {
            Console.ForegroundColor = Color;
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    public enum Standing
    {
        Stand,
        LeftKick,
        LeftHit,
        LeftBlock,
        RightKick,
        RightHit,
        RightBlock
    }
}