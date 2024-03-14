using System;
using System.Windows.Forms;
using Npgsql;

namespace ПМ
{
    public partial class LoginForm : Form
    {
        PostgresConnector bd = new PostgresConnector();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            string login = textBoxLog.Text;
            string password = textBoxPass.Text;

            NpgsqlCommand cmd_ex = new NpgsqlCommand($"SELECT COUNT(*) FROM users " +
                $"WHERE login = '{login}' and " +
                $"password = '{password}'", bd.getConnection());

            bd.openConnection();

            int user = Convert.ToInt32(cmd_ex.ExecuteScalar());

            bd.closeConnection();

            if (user > 0)
            {
                this.Hide();
                Menu f1 = new Menu();
                f1.ShowDialog();
            }
            else MessageBox.Show("Неправильно введён логин или пароль.\nПопробуйте ещё раз или обратитесь к системному администратору.");
        }

        private bool showPassword = false;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            showPassword = !showPassword;
            textBoxPass.PasswordChar = showPassword ? '\0' : '•';
            pictureBoxEyePass.Image = showPassword ? Properties.Resources.Image2 : Properties.Resources.Image1;
        }
    }
}