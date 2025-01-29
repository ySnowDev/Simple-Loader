using KeyAuth;

namespace MysteryLoader2
{
    public partial class Login : Form
    {

        public static api KeyAuthApp = new api(
           name: "Seu APP name", // App name
           ownerid: "Seu ID", // Account ID
           version: "1.2" // Application version. Used for automatic downloads see video here https://www.youtube.com/watch?v=kW195PLCBKs
                          //path: @"Your_Path_Here" // (OPTIONAL) see tutorial here https://www.youtube.com/watch?v=I9rxt821gMk&t=1s
       );
        public Login()
        {
            InitializeComponent();
            KeyAuthApp.init();
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2TextBox3_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            KeyAuthApp.login(Username.Text, Password.Text);
            if (KeyAuthApp.response.success)
            {
                Main main = new Main();
                main.Show();
                this.Hide();
            }
            else
                MessageBox.Show("Status: " + KeyAuthApp.response.message);
        }

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
