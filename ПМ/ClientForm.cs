using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace ПМ
{
    public partial class ClientForm : Form
    {
        PostgresConnector bd = new PostgresConnector();

        public ClientForm()
        {
            InitializeComponent();
            LoadData();

            labelDate.Text = DateTime.Now.ToString("dd.MM.yyyy");
            labelTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void LoadData()
        {
            bd.openConnection();
            string queryString = "SELECT client_fio, client_telephone_number, problem, date_of_the_end, phone_name, cost FROM client_info";
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(queryString, bd.getConnection());
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;

            dataGridView1.Columns["client_fio"].HeaderText = "ФИО клиента";
            dataGridView1.Columns["client_telephone_number"].HeaderText = "Телефон клиента";
            dataGridView1.Columns["problem"].HeaderText = "Проблема";
            dataGridView1.Columns["date_of_the_end"].HeaderText = "Дата окончания";
            dataGridView1.Columns["phone_name"].HeaderText = "Название телефона";
            dataGridView1.Columns["cost"].HeaderText = "Стоимость";

            bd.closeConnection();
        }

        private void buttonPrintReceipt_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string clientName = selectedRow.Cells["client_fio"].Value.ToString();
                string phoneName = selectedRow.Cells["phone_name"].Value.ToString();
                string problem = selectedRow.Cells["problem"].Value.ToString();
                string cost = selectedRow.Cells["cost"].Value.ToString();
                string receiptText = $"Клиент: {clientName}\nТелефон: {phoneName}\nПроблема: {problem}\nСтоимость: {cost}";

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;

                    File.WriteAllText(fileName, receiptText);

                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += (s, ev) =>
                    {
                        ev.Graphics.DrawString(receiptText, new Font("Arial", 12), Brushes.Black, 10, 10);
                    };
                    pd.Print();
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для печати чека.");
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
                LoadData();
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

            LoadData();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.Rows.Count - 2; // Индекс последней строки с данными

            string client_fio = dataGridView1.Rows[rowIndex].Cells["client_fio"].Value.ToString();
            string client_telephone_number = dataGridView1.Rows[rowIndex].Cells["client_telephone_number"].Value.ToString();
            string problem = dataGridView1.Rows[rowIndex].Cells["problem"].Value.ToString();
            string date_of_the_end = dataGridView1.Rows[rowIndex].Cells["date_of_the_end"].Value.ToString();
            string phone_name = dataGridView1.Rows[rowIndex].Cells["phone_name"].Value.ToString();
            string cost = dataGridView1.Rows[rowIndex].Cells["cost"].Value.ToString();

            bd.openConnection();
            string insertQuery = $"INSERT INTO client_info (client_fio, client_telephone_number, problem, date_of_the_end, phone_name, cost) " +
                $"VALUES ('{client_fio}', '{client_telephone_number}', '{problem}', '{date_of_the_end}', '{phone_name}', '{cost}')";
            NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, bd.getConnection());
            cmd.ExecuteNonQuery();
            bd.closeConnection();

            LoadData();
        }
    }
}