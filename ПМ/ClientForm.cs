using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace ПМ
{
    public partial class ClientForm : Form
    {
        PostgresConnector bd = new PostgresConnector();

        public ClientForm()
        {
            InitializeComponent();

            labelDate.Text = DateTime.Now.ToString("dd.MM.yyyy");
            labelTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            dataGridView1.CellMouseDown += dataGridView1_CellMouseDown;

            bd.openConnection();
            string queryString = "SELECT client_fio, client_telephone_number, problem, date_of_the_end, phone_name, cost FROM client_info";
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(queryString, bd.getConnection());
            DataTable dataTable = new DataTable();
            adapter.FillSchema(dataTable, SchemaType.Source);
            dataGridView1.DataSource = dataTable;

            dataGridView1.Columns["client_fio"].HeaderText = "ФИО клиента";
            dataGridView1.Columns["client_telephone_number"].HeaderText = "Телефон клиента";
            dataGridView1.Columns["problem"].HeaderText = "Проблема";
            dataGridView1.Columns["date_of_the_end"].HeaderText = "Дата окончания";
            dataGridView1.Columns["phone_name"].HeaderText = "Название телефона";
            dataGridView1.Columns["cost"].HeaderText = "Стоимость";

            bd.closeConnection();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            DataTable dataTable = (DataTable)dataGridView1.DataSource;

            string updateQueryString = "SELECT client_info_id, client_fio, client_telephone_number, problem, date_of_the_end, phone_name, cost FROM client_info";
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(updateQueryString, bd.getConnection());
            NpgsqlCommandBuilder builder = new NpgsqlCommandBuilder(adapter);
            adapter.Update(dataTable);

            string selectQueryString = "SELECT client_info_id, client_fio, client_telephone_number, problem, date_of_the_end, phone_name, cost FROM client_info";
            NpgsqlCommand cmd = new NpgsqlCommand(selectQueryString, bd.getConnection());
            bd.openConnection();
            NpgsqlDataReader reader = cmd.ExecuteReader();
            try
            {
                dataTable = new DataTable();
                dataTable.Load(reader);
                dataGridView1.DataSource = dataTable;
                dataGridView1.Columns["client_info_id"].Visible = false;

                dataGridView1.Columns["client_fio"].HeaderText = "ФИО клиента";
                dataGridView1.Columns["client_telephone_number"].HeaderText = "Телефон клиента";
                dataGridView1.Columns["problem"].HeaderText = "Проблема";
                dataGridView1.Columns["date_of_the_end"].HeaderText = "Дата окончания";
                dataGridView1.Columns["phone_name"].HeaderText = "Название телефона";
                dataGridView1.Columns["cost"].HeaderText = "Стоимость";
            }
            finally
            {
                reader.Close();
                bd.closeConnection();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Menu f1 = new Menu();
            f1.ShowDialog();
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bd.openConnection();
                e.SuppressKeyPress = true;
                contextMenuStrip1.Close();
                int clientId = (int)dataGridView1.CurrentRow.Cells["client_info_id"].Value;

                NpgsqlCommand cmd = new NpgsqlCommand($"UPDATE client_info SET {dataGridView1.CurrentCell.OwningColumn.HeaderText} = '{toolStripTextBox1.Text}'" +
                    $"WHERE client_info_id = {clientId}", bd.getConnection());
                cmd.ExecuteNonQuery();
                toolStripTextBox1.Clear();
                string queryString = "SELECT client_info_id, client_fio, client_telephone_number, problem, date_of_the_end, phone_name, cost FROM client_info";
                NpgsqlCommand cmd1 = new NpgsqlCommand(queryString, bd.getConnection());
                NpgsqlDataReader reader = cmd1.ExecuteReader();
                try
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Columns["client_info_id"].Visible = false;
                }
                finally
                {
                    reader.Close();
                    bd.closeConnection();
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bd.openConnection();
            int clientId = (int)dataGridView1.CurrentRow.Cells["client_info_id"].Value;
            string deleteString = $"DELETE FROM client_info WHERE client_info_id = {clientId}";
            NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteString, bd.getConnection());
            deleteCommand.ExecuteNonQuery();
            bd.closeConnection();
        }
    }
}