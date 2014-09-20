using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Media;

namespace StickFight.Audio
{
    public class BasicSound
    {
        #region Fields
        protected string Sound_Name { get; set; }
        protected UnmanagedMemoryStream Sound_Stream { get; set; }
        #endregion

        /// <summary>
        /// The basic class for a sound effect
        /// </summary>
        /// <param name="soundStream"></param>
        public BasicSound(UnmanagedMemoryStream soundStream)
        {
            this.Sound_Stream = soundStream;
        }

        /// <summary>
        /// The basic class for background music
        /// </summary>
        /// <param name="soundStream"></param>
        public BasicSound(string soundName)
        {
            this.Sound_Name = soundName;
        }
    }
}
