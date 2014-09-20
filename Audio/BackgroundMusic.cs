using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;
using System.Threading;

namespace StickFight.Audio
{
    public class BackgroundMusic : BasicSound
    {
        #region Fields
        private byte[] fileStream;
        private Thread musicPlayer;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets if the music should be replayed
        /// </summary>
        public bool Replay { get; set; }

        /// <summary>
        /// Returns true, if the music is currently playing
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return musicPlayer.IsAlive;
            }
        }
        #endregion

        /// <summary>
        /// Creates a basic background music effect. You can have multiple of these runnning at the same time.
        /// </summary>
        /// <param name="fileName">Name to call the file when it is saved to the desktop</param>
        /// <param name="fileStream">The array of bytes representing the song</param>
        /// <param name="replay">Defines whether to replay the song or not</param>
        public BackgroundMusic(string fileName, byte[] fileStream, bool replay)
            : base(fileName)
        {
            this.Replay = replay;
            this.fileStream = fileStream;
            musicPlayer = new Thread(new ThreadStart(priv_StartMusic));
            musicPlayer.SetApartmentState(ApartmentState.STA);
        }

        /// <summary>
        /// Starts playing the music
        /// </summary>
        public void StartMusic()
        {
            if (Program.SoundEnabled)
                musicPlayer.Start();
        }

        /// <summary>
        /// Starts playing the music on a seperate thread
        /// </summary>
        private void priv_StartMusic()
        {
            using (Stream file = File.OpenWrite(Sound_Name + ".mp3"))
            {
                byte[] entireFile = fileStream;
                file.Write(entireFile, 0, entireFile.Length);
            }

            MediaPlayer p1 = new MediaPlayer();
            p1.Open(new System.Uri(AppDomain.CurrentDomain.BaseDirectory + Sound_Name + ".mp3"));
            do
            {
                p1.Volume = 0.1;
                p1.Play();
                bool sleep = true;
                p1.MediaEnded += (sender, e) => sleep = false;
                while (sleep)
                {
                    Thread.Sleep(1000);
                }

            } while (Replay);
        }
    }
}
