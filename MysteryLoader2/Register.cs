using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeyAuth;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MysteryLoader2
{
    public partial class Register : Form
    {

        public static api KeyAuthApp = new api(
            name: "Seu Nome", // App name
            ownerid: "Seu ID", // Account ID
            version: "1.2" // Application version. Used for automatic downloads see video here https://www.youtube.com/watch?v=kW195PLCBKs
                           //path: @"Your_Path_Here" // (OPTIONAL) see tutorial here https://www.youtube.com/watch?v=I9rxt821gMk&t=1s
    );

        public Register()
        {
            InitializeComponent();
            KeyAuthApp.init();
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {
            Login loginform = new Login();
            this.Hide();
            loginform.Show();
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            KeyAuthApp.register(Username.Text, Password.Text, Key.Text, "");
            if (KeyAuthApp.response.success)
            {
                Main main = new Main();
                main.Show();
                this.Hide();
            }
            else
                MessageBox.Show("Status: " + KeyAuthApp.response.message);
        }

        private void Key_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

