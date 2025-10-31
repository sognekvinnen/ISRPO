using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gallery
{
    public partial class TovarovedForm : Form
    {
        private string connectionString;
        private SqlConnection connection;
        private string currentTable = "";
        private DataTable dataTable;

        public TovarovedForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            this.Text = "Галерея - Администратор";

            connection = new SqlConnection(connectionString);
            LoadTablesList();
        }

        private void LoadTablesList()
        {
            comboBoxTables.Items.AddRange(new string[] {
                "Экспонаты",
                "Экспонирование"
            });
            comboBoxTables.SelectedIndex = 0;
        }

        private void comboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTables.SelectedItem != null)
            {
                currentTable = comboBoxTables.SelectedItem.ToString();
                LoadCurrentTable();
            }
        }

        private void LoadCurrentTable()
        {
            if (string.IsNullOrEmpty(currentTable)) return;

            try
            {
                // Очищаем предыдущие данные
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query;
                    switch (currentTable)
                    {
                        case "Экспонаты":
                            query = "SELECT * FROM [dbo].[Экспонаты]";
                            break;
                        case "Экспонирование":
                            query = "SELECT * FROM [dbo].[Экспонирование]";
                            break;
                        default:
                            query = $"SELECT * FROM [dbo].[{currentTable}]";
                            break;
                    }

                    using (SqlCommand command = new SqlCommand(query, conn))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable = new DataTable();
                        dataTable.Load(reader);

                        // Назначаем DataSource
                        dataGridView1.DataSource = dataTable;
                    }
                }

                // Настраиваем внешний вид DataGridView
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.ReadOnly = true; // Только для просмотра

                MessageBox.Show($"Таблица '{currentTable}' загружена. Записей: {dataTable.Rows.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки таблицы '{currentTable}': {ex.Message}");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCurrentTable();
        }

        private void TovarovedForm_Load(object sender, EventArgs e)
        {
            LoadCurrentTable();
        }
    }
}