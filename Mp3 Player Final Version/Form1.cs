using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib;

namespace Mp3_Player_Final_Version
{
    public partial class Form1 : Form
    {
        // A couple of global variables to save mp3 details in
        string songL, Length, songN;

        int f = 0;// Initialize a variable 34an hnst5dmoh fe elplay w elpause
        /*volume*/
        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
        private MediaSound mp3Player = new MediaSound();
        /*volume*/

        public Form1()
        {
            InitializeComponent();

            //start volume bar
            uint CurrVol = 0;            
            waveOutGetVolume(IntPtr.Zero, out CurrVol);            
            ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);           
            trackBar1.Value = CalcVol / (ushort.MaxValue / 10);
            //end volume bar

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 100;
            trackBar1.TickStyle = TickStyle.BottomRight;
            trackBar1.TickFrequency = 10;
        }
        private void me_player_Click(object sender, EventArgs e)//jouer une pause
        {
            //to remember  
            if (f == 0)
            {
                mp3Player.stop();
                f = 1;
            }
            else
            {
                mp3Player.resume();
                f = 0;
            }
            
            // Both of the next if conditions are for bug avoiding ( To stop the timer correctly when the user hit pause)
            if (mp3Player.dec == true)
            {                
                timer1.Start();
            }

            if (mp3Player.dec == false)
            {
                timer1.Stop();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog m = new OpenFileDialog()) 
            {
                m.Filter = "Mp3 Files|*.mp3";
                if (m.ShowDialog() == DialogResult.OK)
                {
                    mp3Player.open(m.FileName);

                    // Starting up the timer and setting it's tick to 1 second
                    timer1.Start();
                    timer1.Interval = 1000;

                    // Creating a file from the taglib DLL
                    TagLib.File a = TagLib.File.Create(m.FileName);

                    // Saving the sond duration in the string songL
                    songL = a.Properties.Duration.ToString();

                    // Saving the right format of the song duration in another string
                    Length = songL[3].ToString() + songL[4] + songL[5] + songL[6] + songL[7];

                    // Printing the song duration
                    label3.Text = Length;

                    if (a.Tag.Title != null)
                    {
                        // Saving song name in variable
                        songN = a.Tag.Title;

                        // Printing song name
                        label1.Text = songN;
                    }

                    // Algorithm to take the song name from the file path if there was no song title
                    else
                    {

                        int holder = 0, symCounter = 0;
                        string lFormat = "", rFormat = m.FileName;

                        for (int i = 0; i < m.FileName.Length; i++)
                        {
                            if (m.FileName[i].Equals('\\'))
                            {
                                symCounter++;
                            }                            
                        }

                        for (int i = 0; i < m.FileName.Length; i++)
                        {
                            if (m.FileName[i].Equals('\\'))
                            {
                                holder++;
                            }

                            if (holder == symCounter && m.FileName[i] != '\\')
                            {
                                lFormat += rFormat[i];
                            }
                        }
                        label1.Text = lFormat;
                    }

                    // Showing up the album art
                    if (a.Tag.Pictures.Length >= 1)
                    {
                        // Saving the album art in the variable bin
                        var bin = (byte[])(a.Tag.Pictures[0].Data.Data);

                        // This is taken from online microsoft document about TAGLIB DLL and IPicture, And I edited the image values to fit the box
                        pictureBox1.Image = Image.FromStream(new MemoryStream(bin)).GetThumbnailImage(343, 269, null, IntPtr.Zero);
                    }
                    se = 0;
                    mi = 0;

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mp3Player.delete();

            // Stopping the timer when the user stops or deletes the song
            timer1.Stop();

            // Setting the variables and the labels' texts to zeros
            se = 0;
            mi = 0;
            label3.Text = "00:00";
            label4.Text = "00:00";
            label1.Text = "Dummy song name";
            pictureBox1.Image = null;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void vol_Click(object sender, EventArgs e)
        {

        }

        // Couple of global variables to show the timer
        int se = 0, mi = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            // If the user stopped the song
            if (mp3Player.dec == false)
            {
                timer1.Stop();
            }
            // If the song is playing: Increment the seconds (se) variable by 1; When it hits 60 wrap it around and increment the minutes (mi) variable by one
            se++;
            if (se > 59)
            {
                mi++;
                se = 0;
            }
            // For showing the song timer
            label4.Text = "0" + mi.ToString() + ":" + se.ToString();

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int nVolume = ((ushort.MaxValue / 10) * trackBar1.Value);
            uint nVolumeForChan = ((uint)nVolume & 0x0000ffff | ((uint)nVolume << 16));
            vol.Text = trackBar1.Value.ToString();
            waveOutSetVolume(IntPtr.Zero, nVolumeForChan);
        }

        private void close_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
