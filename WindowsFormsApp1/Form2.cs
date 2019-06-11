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
    public partial class Form2 : Form
    {
        int price=1, max_park=2;
       
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {   if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("가격을 입력해 주세요.");
            }
            else if (textBox2.Text.Equals(""))
            {
                MessageBox.Show("주차 할 수 있는 최대 자리수를 입력해주세요");
            }
            else
            {
                price = int.Parse(textBox1.Text);
                max_park = int.Parse(textBox2.Text);
                this.Hide();
                Form1 fm1 = new Form1(price, max_park);
                fm1.Show();
            }
        }
    
    }
}


