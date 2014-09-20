using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Media;
using System.Collections.Generic;

namespace System
{
    public class ConsoleOptions
    {
        #region Fields
        private static SoundPlayer changeAudioPlayer;
        private static SoundPlayer selectAudioPlayer;
        #endregion

        /// <summary>
        /// Gets or sets the audio effect being played when changing option in the currently displayed menu
        /// </summary>
        public static Stream ChangeAudio { get; set; }

        /// <summary>
        /// Gets or sets the audio effect being played when selecting an option in the currently displayed menu
        /// </summary>
        public static Stream SelectAudio { get; set; }

        /// <summary>
        /// Sends the choices to the player, prompting the player to choose an option
        /// </summary>
        /// <param name="Header">The title being display in the top of the menu</param>
        /// <param name="options">The collection of options for the player to select from</param>
        /// <returns>Returns the selected string choice</returns>
        public static string SendChoices(string Header, List<string> options)
        {
            return SendChoices(Header, options, null, 0);
        }

        /// <summary>
        /// Sends the choices to the player, prompting the player to choose an option, with the pointer on the specified position
        /// </summary>
        /// <param name="Header">The title being display in the top of the menu</param>
        /// <param name="options">The collection of options for the player to select from</param>
        /// <param name="cursorStartPosition">The position of where the pointer should start</param>
        /// <returns>Returns the selected string choice</returns>
        public static string SendChoices(string Header, List<string> options, int cursorStartPosition)
        {
            return SendChoices(Header, options, null, cursorStartPosition);
        }

        /// <summary>
        /// Sends the choices to the player, prompting the player to choose an option, coloring each option according to the color in the position in the color collection
        /// </summary>
        /// <param name="Header">The title being display in the top of the menu</param>
        /// <param name="options">The collection of options for the player to select from</param>
        /// <param name="colors">The collection of colors to paint the options</param>
        /// <returns>Returns the selected string choice</returns>
        public static string SendChoices(string Header, List<string> options, List<ConsoleColor> colors)
        {
            return SendChoices(Header, options, colors, 0);
        }

        /// <summary>
        /// Sends the choices to the player, prompting the player to choose an option, with the pointer on the specified position, and coloring each option according to the color in the position in the color collection
        /// </summary>
        /// <param name="Header">The title being display in the top of the menu</param>
        /// <param name="options">The collection of options for the player to select from</param>
        /// <param name="colors">The collection of colors to paint the options</param>
        /// <param name="cursorStartPosition">The position of where the pointer should start</param>
        /// <returns>Returns the selected string choice</returns>
        public static string SendChoices(string Header, List<string> options, List<ConsoleColor> colors, int cursorStartPosition)
        {
            if (ChangeAudio != null)
            {
                ChangeAudio.Position = 0;
                changeAudioPlayer = new SoundPlayer(ChangeAudio);
            }
            if (SelectAudio != null)
            {
                SelectAudio.Position = 0;
                selectAudioPlayer = new SoundPlayer(SelectAudio);
            }
            Console.WriteLine();

            int topCurPosition, leftCurPosition;
            topCurPosition = Console.CursorTop;
            leftCurPosition = Console.CursorLeft + 1;

            int startPosition = 40 - (Header.Length / 2);
            Console.SetCursorPosition(startPosition, Console.CursorTop);
            Console.Write(Header);
            Console.WriteLine();
            bool cursorState = Console.CursorVisible;
            Console.CursorVisible = false;
            for (int i = 0; i < options.Count; i++)
            {
                string toWrite = "      " + options[i] + "\n";

                ConsoleColor currentColor = Console.ForegroundColor;
                if (colors != null)
                {
                    Console.ForegroundColor = colors[i];
                }
                Console.Write(toWrite);
                if (colors != null)
                {
                    Console.ForegroundColor = currentColor;
                }
            }

            int Location = 1 + cursorStartPosition;

            Console.SetCursorPosition(leftCurPosition, topCurPosition + Location);

            Console.Write("--→");

            ConsoleKey keyPress = Console.ReadKey(true).Key;

            while (keyPress != ConsoleKey.Enter)
            {
                if (keyPress == ConsoleKey.DownArrow && Location < options.Count)
                {
                    Console.MoveBufferArea(leftCurPosition, Location + topCurPosition, 3, 1, leftCurPosition, Location + topCurPosition + 1);
                    Location++;
                    if (ChangeAudio != null)
                        changeAudioPlayer.Play();
                }
                else if (keyPress == ConsoleKey.UpArrow && Location > 1)
                {
                    Console.MoveBufferArea(leftCurPosition, Location + topCurPosition, 3, 1, leftCurPosition, Location + topCurPosition - 1);
                    Location--;
                    if (ChangeAudio != null)
                        changeAudioPlayer.Play();
                }

                keyPress = Console.ReadKey(true).Key;
            }

            if (SelectAudio != null)
            {
                selectAudioPlayer.Play();
            }

            Console.Clear();
            Console.CursorVisible = cursorState;
            return options[Location - 1];
        }
    }
}
