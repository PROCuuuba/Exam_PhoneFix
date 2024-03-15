using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace ПМ
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            pictureBox1.BackgroundImage = Properties.Resources._3_1_;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            PhonesRepair f3 = new PhonesRepair();
            f3.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            ClientForm f4 = new ClientForm();
            f4.ShowDialog();
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
