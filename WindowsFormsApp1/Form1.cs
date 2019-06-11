using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int max_parking = 0;
        int price =0;
        int parking = 0;
        string account = (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1];
        string date;
        string[] parking_now;
        bool now_parking = false;

        public Form1()
        {
            InitializeComponent();
        }
        public Form1(int price, int max_park)
        {
            InitializeComponent();
            max_parking = max_park;
            label7.Text = max_park + "";
            parking_now = new string[max_park];
            for(int i = 0; i<max_park; i++)
            {
                parking_now[i] = "";
            }
            Console.WriteLine(parking_now+"/"+parking_now.Length);
            this.price = price;
            GetSystemDate(out date);
            string filepath = string.Format("C:\\Users\\" + account + "\\AppData\\Local\\Parking_system" + "\\" + date + "_Log.txt");
            FileInfo fi = new FileInfo(filepath);
            if (fi.Exists)
            {
                FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate);
                StreamReader sr = new StreamReader(fs);
                while (sr.Peek() > -1)
                {
                    listBox1.Items.Add(sr.ReadLine());
                }
                sr.Close();
                fs.Close();
            }
     
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            if (textBox1.Text != null && textBox1.Text != "")
            {
                if (textBox1.Text.Length == 7)
                {
                    if (max_parking != 0)
                    {
                        //  Console.WriteLine(parking_now + "/" + parking_now.Length + " / " + textBox1.Text);
                        DateTime dtNow = DateTime.Now;
                        // listBox1.Items.Add(textBox1.Text + "/" + dtNow.ToLongDateString() + " " + dtNow.ToShortTimeString());
                        string s = textBox1.Text;
                        for (int i = 0; i < parking_now.Length; i++)
                        {
                            if (parking_now[i].Equals(s))
                            {
                                now_parking = true;
                                MessageBox.Show("이미 주차되있는 차량입니다.");
                                break;
                            }
                            else { now_parking = false; }
                        }
                        if (now_parking == false)
                        {
                            CrossThreadSetLogMessage(textBox1.Text + " /" + dtNow.ToLongDateString() + " " + dtNow.ToShortTimeString());
                            textBox1.Text = "";
                            parking++;
                            max_parking--;
                            label5.Text = parking + "";
                            label7.Text = max_parking.ToString();
                            for (int i = 0; i < parking_now.Length; i++)
                            {
                                if (parking_now[i].Equals(""))
                                {
                                    parking_now[i] = s;
                                    break;
                                }
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("주차 자리가 없습니다.");
                    }
                }
                else
                {
                    MessageBox.Show("차량번호 양식이 맞지않습니다.");
                }
            }else
            {
                MessageBox.Show("차량번호를 입력해주세요.");
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string outcar = textBox2.Text;
            bool parked = false;
            int parking_num = 0;
             
            for(int i = 0; i< max_parking; i++)
            {
                if (parking_now[i].Equals(outcar))
                {
                    parked = true;
                    parking_num = i;
                    MessageBox.Show("이 값은 " + i + "번째에 있습니다.");
                    break;
                }else { parked = false; }
            }

            if(parked)
            {
                MessageBox.Show("주차된 차가 있습니다.");
            }
            else
            {
                MessageBox.Show("주차된 차가 없습니다.");
            }
        
           
         
        }

        public string[] GetIniValue(string Section, string path)
        {
            byte[] ba = new byte[5000];
            uint Flag = GetPrivateProfileSection(Section, ba, 5000, path);
            return Encoding.Default.GetString(ba).Split(new char[1] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string GetIniValue(string Section, string Key, string path)
        {
            StringBuilder sb = new StringBuilder(500);
            int Flag = GetPrivateProfileString(Section, Key, "", sb, 500, path);
            return sb.ToString();
        }

        public bool SetIniValue(string Section, string Key, string value, string path)
        {
            return (WritePrivateProfileString(Section, Key, value, path));

        }

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string IpAppName, string IpKeyName, string IpDefault, StringBuilder IpReturnedString, int nSize, string IpFileName);
        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(string IpAppName, string IpKeyName, string IpString, string IpFileName);
        [DllImport("kernel32")]
        public static extern uint GetPrivateProfileInt(string IpAppName, string IpKeyName, int nDefault, string IpFileName);
        [DllImport("kernel32.dll")]
        public static extern uint GetPrivateProfileSection(string IpAppName, byte[] IpPairValues, uint nSize, string IpFileName);
        [DllImport("kernel32.dll")]
        public static extern uint GetPrivateProfileSectionNames(byte[] IpSections, uint nSize, string IpFileName);


       public void GetSystemDate(out string outTime)
        {
            outTime = string.Format(DateTime.Now.ToString("yyyy.MM.dd"));
        }

        public void GetSystemTime(out String outTime)
        {
            outTime = string.Format("[" + DateTime.Now.ToString("yyyy.MM.dd") + "_" + DateTime.Now.ToString("HH:mm:ss") + "] ");
            
        }

        delegate void CrossThreadSafetySetLogMessage(string inMessage);

        public void CrossThreadSetLogMessage(string inMessage)
        {
            this.Invoke(new CrossThreadSafetySetLogMessage(SetLogMessage), inMessage);
        }

        public void SetLogMessage(string inMessage)
        {
            string LogMessage = "";
            string strTime = "";
            GetSystemTime(out strTime);

            LogMessage = string.Format(strTime.ToString() + inMessage.ToString());
            listBox1.Items.Add(LogMessage);
            int index = listBox1.Items.Count;
            listBox1.SelectedIndex = index - 1;

            if(listBox1.Items.Count > 1000)
            {
                listBox1.Items.RemoveAt(0);
            }
            SaveLogFile(LogMessage);

        }

        public void SaveLogFile(string inLogMessage)
        {
            string strDate;
            GetSystemDate(out strDate);

            string FilePath = string.Format("C:\\Users\\" + account + "\\AppData\\Local\\Parking_system" + "\\" + strDate+"_Log.txt");
            FileInfo fi = new FileInfo(FilePath);

            DirectoryInfo dir = new DirectoryInfo("C:\\Users\\"+account+"\\AppData\\Local\\Parking_system");

            if(dir.Exists == false)
            {
                dir.Create();
            }

            try
            {
                if(fi.Exists != true)
                {
                    using(StreamWriter sw = new StreamWriter(FilePath))
                    {
                        sw.WriteLine(inLogMessage);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        sw.WriteLine(inLogMessage);
                        sw.Close();
                    }
                }
            }catch (Exception e)
            {
                CrossThreadSetLogMessage("로그 저장 실패");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String dt = dateTimePicker1.Value.ToShortDateString();
            String[] dt1 = dt.Split('-');
           
            string filepath = string.Format("C:\\Users\\" + account + "\\AppData\\Local\\Parking_system" + "\\" + dt1[0]+"." + dt1[1] + "." + dt1[2]+ "_Log.txt");
            FileInfo fi = new FileInfo(filepath);
            if (fi.Exists)
            {
                Form3 form3 = new Form3(filepath);
                form3.Show();

            }else
            {
                MessageBox.Show("해당날짜에 대한 로그가 없습니다.");
            }

        }

        public void cutting()
        {

        }
    }
}
