using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace StickFight
{
    public class Map
    {
        #region Properties
        /// <summary>
        /// Gets all of the points on the map where there is a collision
        /// </summary>
        public static List<Point> MapCollisions { get; private set; }
        #endregion

        /// <summary>
        /// Creates a standard map, using the content of the chosen map
        /// </summary>
        /// <param name="chosenMap"></param>
        public static void CreateMap(string chosenMap)
        {
            MapCollisions = new List<Point>();

            Console.WindowHeight = 33;

            Console.Clear();

            #region Draws player health bars
            Console.SetCursorPosition(Program.Player1.HpBarPosition, 2);
            Program.Player1.SetConsoleColor();
            for (int i = 0; i < 20; i++)
            {
                Console.Out.Write("▓");
            }
            WritePlayerName(Program.Player1.Name, 11);

            Console.SetCursorPosition(Program.Player2.HpBarPosition, 2);
            Program.Player2.SetConsoleColor();
            for (int i = 0; i < 20; i++)
            {
                Console.Out.Write("▓");
            }
            WritePlayerName(Program.Player2.Name, 69);
            #endregion

            Console.ForegroundColor = ConsoleColor.Gray;

            #region Draws the map
            Console.SetCursorPosition(0, Program.Offset);

            string[] completeMap = System.IO.File.ReadAllLines(chosenMap);

            Console.Write(completeMap[0]);
            for (int i = 1; i < completeMap.Length - 1; i++)
            {
                Console.Write(completeMap[i][0]);
                for (int u = 1; u < 79; u++)
                {
                    Console.Write(completeMap[i][u]);
                    if (completeMap[i][u] != ' ')
                    {
                        MapCollisions.Add(new System.Drawing.Point(u, i + Program.Offset));
                    }
                }
                Console.Write(completeMap[i][completeMap[i].Length - 1]);
            }
            Console.Write(completeMap[completeMap.Length - 1]);
            #endregion
        }

        /// <summary>
        /// Writes the player name on the specified position
        /// </summary>
        /// <param name="message">The player name</param>
        /// <param name="startPosition">The center position to draw it.</param>
        private static void WritePlayerName(string message, int startPosition)
        {
            Console.SetCursorPosition(startPosition - (message.Length / 2), 3);
            Console.Out.Write(message);
            Console.SetCursorPosition(0, 26 + Program.Offset);
        }
    }
}
