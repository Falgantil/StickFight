using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Media;

namespace StickFight.Audio
{
    public class Effect : BasicSound
    {
        #region Fields
        private SoundPlayer soundEffect;
        #endregion

        /// <summary>
        /// Creates a basic sound effect
        /// </summary>
        /// <param name="soundStream">The sound effect stream</param>
        public Effect(UnmanagedMemoryStream soundStream)
            : base(soundStream)
        {
            soundStream.Position = 0;
            soundEffect = new SoundPlayer(soundStream);
        }

        /// <summary>
        /// Starts playing the sound effect
        /// </summary>
        public void Play()
        {
            if (Program.SoundEnabled)
                soundEffect.Play();
        }

        /// <summary>
        /// Stops playing the sound effect
        /// </summary>
        public void Stop()
        {
            soundEffect.Stop();
        }
    }
}
