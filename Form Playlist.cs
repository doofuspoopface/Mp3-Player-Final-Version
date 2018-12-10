using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1 
{
    public partial class Form1 : Form
    {
        playlist main = new playlist();
        
        public Form1()
        {
            InitializeComponent();
            load();
        }

        protected void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void addentry(song f) //adds a song to listbox, the listbox item date returns song data type
        {
            listBox1.Items.Add(f);
            main.addsong(f);
        }

        public void load() //loads items to listbox from file
        {
            string[] paths = main.Read();
            foreach (string p in paths)
            {
                listBox1.Items.Add(new song(p));
            }
        }

        public void refresh() //for deleting, remove the song path from the file and then refresh()
        {
            listBox1.Items.Clear();
            load();
        }
        // add here a delete button that when clicked, calls the delete function that you made in playlist (main.delete(song)), and then refresh()

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string p = System.IO.Path.GetFullPath(openFileDialog1.FileName);
                song s = new song(p);
                addentry(s);
            }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}