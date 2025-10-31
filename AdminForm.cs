using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Gallery
{
    public partial class AdminForm : Form
    {
        private string connectionString;
        private string currentTable = "";
        private DataTable dataTable;
        private DataView dataView;

        public AdminForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            this.Text = "Галерея - Администратор";
            LoadTablesList();
            SetupDataGridView();
        }

        private void LoadTablesList()
        {
            comboBoxTables.Items.AddRange(new string[] {
                "Авторы",
                "Экспонаты",
                "Экспонирование",
                "Продажи",
                "Бухгалтерия"
            });
            comboBoxTables.SelectedIndex = 0;
        }

        private void SetupDataGridView()
        {
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
        }

        private void comboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTables.SelectedItem != null)
            {
                currentTable = comboBoxTables.SelectedItem.ToString();
                LoadCurrentTable();
                SetupFilterComboBox();
            }
        }

        private void SetupFilterComboBox()
        {
            comboBoxFilterField.Items.Clear();
            if (dataTable != null)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    comboBoxFilterField.Items.Add(column.ColumnName);
                }
                if (comboBoxFilterField.Items.Count > 0)
                    comboBoxFilterField.SelectedIndex = 0;
            }
        }

        private void LoadCurrentTable()
        {
            if (string.IsNullOrEmpty(currentTable)) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"SELECT * FROM [dbo].[{currentTable}]";

                    dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        adapter.Fill(dataTable);
                    }

                    dataView = new DataView(dataTable);
                    dataGridView1.DataSource = dataView;
                }

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                lblRecordCount.Text = $"Записей: {dataTable.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки таблицы '{currentTable}': {ex.Message}");
            }
        }

        // 2. СОРТИРОВКА И ФИЛЬТРАЦИЯ
        private void btnSortAsc_Click(object sender, EventArgs e)
        {
            if (dataView != null && comboBoxFilterField.SelectedItem != null)
            {
                dataView.Sort = comboBoxFilterField.SelectedItem.ToString() + " ASC";
            }
        }

        private void btnSortDesc_Click(object sender, EventArgs e)
        {
            if (dataView != null && comboBoxFilterField.SelectedItem != null)
            {
                dataView.Sort = comboBoxFilterField.SelectedItem.ToString() + " DESC";
            }
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            if (dataView != null && comboBoxFilterField.SelectedItem != null)
            {
                string filterValue = txtFilterValue.Text.Trim();
                if (!string.IsNullOrEmpty(filterValue))
                {
                    string filterField = comboBoxFilterField.SelectedItem.ToString();
                    dataView.RowFilter = $"{filterField} LIKE '%{filterValue}%'";
                }
                else
                {
                    dataView.RowFilter = "";
                }
                lblRecordCount.Text = $"Записей: {dataView.Count}";
            }
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            if (dataView != null)
            {
                dataView.RowFilter = "";
                txtFilterValue.Text = "";
                lblRecordCount.Text = $"Записей: {dataTable.Rows.Count}";
            }
        }

        // 3. ПОИСК
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText) && dataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Selected = false;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText.ToLower()))
                        {
                            row.Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                    }
                }
            }
        }

        // 4. ДОБАВЛЕНИЕ, РЕДАКТИРОВАНИЕ, УДАЛЕНИЕ
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataTable != null)
                {
                    // Создаем новую строку
                    DataRow newRow = dataTable.NewRow();

                    // Для таблицы Авторы заполняем обязательное поле
                    if (currentTable == "Авторы")
                    {
                        newRow["Полное_Имя"] = "Новый автор"; // обязательное поле
                    }
                    // Для других таблиц заполняем обязательные поля по умолчанию
                    else if (currentTable == "Экспонаты")
                    {
                        newRow["Название"] = "Новый экспонат";
                        newRow["ID_Автора"] = 1; // минимальный существующий ID
                    }
                    else if (currentTable == "Экспонирование")
                    {
                        newRow["ID_Экспоната"] = 1;
                        newRow["Дата_Начала"] = DateTime.Today;
                    }
                    else if (currentTable == "Продажи")
                    {
                        newRow["ID_Экспоната"] = 1;
                        newRow["Имя_Покупателя"] = "Покупатель";
                        newRow["Дата_Продажи"] = DateTime.Today;
                        newRow["Цена"] = 0;
                    }
                    else if (currentTable == "Бухгалтерия")
                    {
                        newRow["Тип_Операции"] = "комиссия";
                        newRow["Сумма"] = 0;
                        newRow["Дата_Операции"] = DateTime.Today;
                    }

                    dataTable.Rows.Add(newRow);

                    // Сохраняем изменения
                    SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                // Завершаем любое активное редактирование
                dataGridView1.EndEdit();

                // Разрешаем редактирование
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;

                // Переводим всю строку в режим редактирования
                dataGridView1.CurrentRow.Selected = true;
                dataGridView1.BeginEdit(true);

                MessageBox.Show("Редактирование разрешено. Внесите изменения и нажмите кнопку 'Сохранить'.");
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.");
            }
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Автоматически сохраняем изменения при завершении редактирования ячейки
            SaveChanges();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                    SaveChanges();
                }
            }
        }

        private void SaveChanges()
        {
            try
            {
                // Важно: завершаем редактирование перед сохранением
                dataGridView1.EndEdit();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"SELECT * FROM [dbo].[{currentTable}]";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                        // Проверяем, есть ли изменения
                        if (dataTable.GetChanges() != null)
                        {
                            int rowsAffected = adapter.Update(dataTable);
                            MessageBox.Show($"Успешно сохранено изменений: {rowsAffected}");
                            LoadCurrentTable(); // Перезагружаем для обновления данных
                        }
                        else
                        {
                            MessageBox.Show("Нет изменений для сохранения.");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Ошибка базы данных: {sqlEx.Message}\n\nПроверьте обязательные поля и связи между таблицами.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
            finally
            {
                dataGridView1.ReadOnly = true;
            }
        }

        // 6. ГЕНЕРАЦИЯ ОТЧЕТОВ И ВЫГРУЗКА
        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Нет данных для экспорта.");
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV файлы (*.csv)|*.csv|Текстовые файлы (*.txt)|*.txt";
                saveDialog.Title = "Сохранить отчет";
                saveDialog.FileName = $"Отчет_{currentTable}_{DateTime.Now:yyyyMMdd}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveDialog.FileName;

                    // Всегда сохраняем как CSV - это откроется в Excel
                    if (!filePath.EndsWith(".csv"))
                    {
                        filePath += ".csv";
                    }

                    ExportToCSV(filePath);

                    // Пытаемся открыть файл
                    try
                    {
                        System.Diagnostics.Process.Start(filePath);
                        MessageBox.Show($"Отчет успешно сохранен и открыт:\n{filePath}");
                    }
                    catch
                    {
                        MessageBox.Show($"Отчет сохранен, но не удалось открыть автоматически:\n{filePath}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}");
            }
        }

        private void ExportToCSV(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Заголовки с кавычками для корректного отображения в Excel
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    writer.Write($"\"{dataTable.Columns[i].ColumnName}\"");
                    if (i < dataTable.Columns.Count - 1)
                        writer.Write(";");
                }
                writer.WriteLine();

                // Данные с кавычками
                foreach (DataRow row in dataTable.Rows)
                {
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        string value = row[i]?.ToString() ?? ""; // обработка NULL
                                                                 // Экранируем кавычки и запятые
                        value = value.Replace("\"", "\"\"");
                        writer.Write($"\"{value}\"");
                        if (i < dataTable.Columns.Count - 1)
                            writer.Write(";");
                    }
                    writer.WriteLine();
                }
            }
        }

        private void ExportToExcel(string filePath)
        {
            // Простой экспорт в CSV (можно подключить библиотеку EPPlus для настоящего Excel)
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Заголовки
                writer.WriteLine(string.Join(";", dataTable.Columns.Cast<DataColumn>().Select(col => col.ColumnName)));

                // Данные
                foreach (DataRow row in dataTable.Rows)
                {
                    writer.WriteLine(string.Join(";", row.ItemArray.Select(field => field.ToString())));
                }
            }
        }

        private void ExportToWord(string filePath)
        {
            // Простой экспорт в текстовый файл (можно подключить библиотеку DocX для настоящего Word)
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"ОТЧЕТ ПО ТАБЛИЦЕ: {currentTable}");
                writer.WriteLine($"Дата формирования: {DateTime.Now}");
                writer.WriteLine(new string('=', 50));

                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        writer.WriteLine($"{col.ColumnName}: {row[col]}");
                    }
                    writer.WriteLine(new string('-', 30));
                }
            }
        }

        // ОСНОВНЫЕ КНОПКИ
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCurrentTable();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            LoadCurrentTable();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }
    }
}