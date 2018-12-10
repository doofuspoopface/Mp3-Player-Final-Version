using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public class song
    {
        public string path;
        song next;
        song prev;
        public int index; // the index of the song, use it for deleting from the text file

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwdCallBack);

        public song(string p)
        {
            path = p;
        }
        public override string ToString()
        {
            return System.IO.Path.GetFileName(path);
        }
        public void addnext(song s)
        {
            if (this.next == null)
            {
                this.next = s;
                this.next.prev = this;
            }
            else this.next.addnext(s);
        }
        //mp3 related stuff (playing, pausing, etc)
        

        public bool dec = false;

        public void play()
        {
            string command = "play MediaFile";
            mciSendString(command, null, 0, 0);
            dec = true;
        }
        public void open(string File)
        {
            delete();
            string Format = @"open ""{0}"" type MPEGVideo alias MediaFile";
            string command = string.Format(Format, path);
            mciSendString(command, null, 0, 0);
            play();
        }
        public void stop()
        {
            string command = "stop MediaFile";
            mciSendString(command, null, 0, 0);
            dec = false;
        }
        public void resume()
        {
            string command = " resume MediaFile";
            mciSendString(command, null, 0, 0);
            dec = true;
        }
        public void delete()
        {
            string command = " close MediaFile";
            mciSendString(command, null, 0, 0);
            dec = false;
        }
        // add here a void that removes a song from the linked list only
    }
    class playlist //add here a function that deletes the song's path from the text file using, then subtract the indexes of the songs after it by 1 (song.next recursion) and then call the function that deletes it from the linked list
    {
        song start;
        string path;
        public int count = 0;

        public playlist()
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "playlist.txt");
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }

        public void addsong(song s)
        {
            if (start == null)
                start = s;
            else
                start.addnext(s);
            s.index = count;
            count++;
            WriteToFile(s.path);
        }

        void WriteToFile(string p)
        {

            string[] t = new string[] { p };
            File.AppendAllLines(path, t);
        }

        public string[] Read()
        {
            string[] paths = File.ReadAllLines(path);
            return paths;
        }
    }
}
