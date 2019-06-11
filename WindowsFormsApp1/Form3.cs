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

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        string filepath = "";
        public Form3()
        {
            InitializeComponent();
        }

        public Form3(String s)
        {
            InitializeComponent();
            FileInfo fi = new FileInfo(s);
            if (fi.Exists)
            {
                filepath = s;
                FileStream fs = new FileStream(s, FileMode.OpenOrCreate);
                StreamReader sr = new StreamReader(fs);
                while (sr.Peek() > -1)
                {
                    listBox1.Items.Add(sr.ReadLine());
                }
                sr.Close();
                fs.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filename = "";
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.InitialDirectory = @"C:\";
            savefile.Title = "Save Log File";
            savefile.DefaultExt = "txt";
            savefile.Filter = "Text 파일(*.txt)|*.txt";
            savefile.FilterIndex = 0;
            savefile.RestoreDirectory = true;
            if(savefile.ShowDialog()== DialogResult.OK)
            {
                filename = savefile.FileName.ToString();
                File.Copy(filepath, filename, true);
            }

        }
    }
}
