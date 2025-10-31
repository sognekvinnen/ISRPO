using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Matrix
{
    public partial class Form1 : Form
    {
        private MatrixCalculator calculator;

        public Form1()
        {
            InitializeComponent();
            calculator = new MatrixCalculator();
            InitializeDataGridViews();

            dataGridViewMatrix.EditingControlShowing += DataGridView_EditingControlShowing;
            dataGridViewVector.EditingControlShowing += DataGridView_EditingControlShowing;
            dataGridViewResult.ReadOnly = true;

            buttonSave.Enabled = false;
        }

        private void InitializeDataGridViews()
        {
            // Настройка DataGridView для матрицы
            dataGridViewMatrix.AutoGenerateColumns = false;
            dataGridViewMatrix.AllowUserToAddRows = false;
            dataGridViewMatrix.RowHeadersVisible = true;
            dataGridViewMatrix.ColumnHeadersVisible = true;

            // Настройка DataGridView для вектора
            dataGridViewVector.AutoGenerateColumns = false;
            dataGridViewVector.AllowUserToAddRows = false;
            dataGridViewVector.RowHeadersVisible = true;
            dataGridViewVector.ColumnHeadersVisible = true;
        }

        private void UpdateDataGridViews()
        {
            try
            {
                int m = (int)numericUpDownM.Value;
                int n = (int)numericUpDownN.Value;

                // Обновляем матрицу
                dataGridViewMatrix.RowCount = m;
                dataGridViewMatrix.ColumnCount = n;

                // Устанавливаем заголовки столбцов для матрицы
                for (int i = 0; i < n; i++)
                {
                    if (dataGridViewMatrix.Columns.Count > i)
                    {
                        dataGridViewMatrix.Columns[i].HeaderText = $"Столбец {i + 1}";
                    }
                    else
                    {
                        dataGridViewMatrix.Columns.Add($"Column{i}", $"Столбец {i + 1}");
                    }
                }

                // Обновляем вектор (автоматически размером n матрицы)
                dataGridViewVector.ColumnCount = 1;
                dataGridViewVector.RowCount = n;
                dataGridViewVector.Columns[0].HeaderText = "Вектор";

                // Создаем матрицу в калькуляторе
                calculator.ResizeMatrix(m, n);

                // Заполняем DataGridView начальными значениями
                FillDataGridViewsWithData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления таблиц: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillDataGridViewsWithData()
        {
            // Заполняем матрицу
            var matrix = calculator.GetMatrix();
            for (int i = 0; i < calculator.Rows; i++)
            {
                for (int j = 0; j < calculator.Cols; j++)
                {
                    dataGridViewMatrix.Rows[i].Cells[j].Value = matrix[i, j];
                }
            }

            // Заполняем вектор
            var vector = calculator.GetVector();
            for (int i = 0; i < calculator.Cols; i++)
            {
                dataGridViewVector.Rows[i].Cells[0].Value = vector[i];
            }
        }

        private void numericUpDownN_ValueChanged(object sender, EventArgs e)
        {
            UpdateDataGridViews();
        }

        private void numericUpDownM_ValueChanged(object sender, EventArgs e)
        {
            UpdateDataGridViews();
        }

        private void buttonUploadMatrix_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                openFileDialog.Title = "Загрузить матрицу";

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    // Загружаем матрицу в калькулятор (внутри ResizeMatrix устанавливаются Rows/Cols)
                    calculator.LoadMatrixFromFile(openFileDialog.FileName);

                    // Проверка ограничений
                    if (calculator.Rows <= 0 || calculator.Cols <= 0)
                        throw new Exception("Неверные размеры матрицы после загрузки.");
                    if (calculator.Rows > 10 || calculator.Cols > 10)
                        throw new Exception("Размер матрицы превышает 10x10.");

                    // Временно отключаем обработчики ValueChanged, чтобы они не перезаписали данные
                    numericUpDownM.ValueChanged -= numericUpDownM_ValueChanged;
                    numericUpDownN.ValueChanged -= numericUpDownN_ValueChanged;

                    try
                    {
                        // Обновляем numericUpDown значениями, полученными из файла
                        numericUpDownM.Value = calculator.Rows;
                        numericUpDownN.Value = calculator.Cols;
                    }
                    finally
                    {
                        // Восстанавливаем обработчики
                        numericUpDownM.ValueChanged += numericUpDownM_ValueChanged;
                        numericUpDownN.ValueChanged += numericUpDownN_ValueChanged;
                    }

                    // Подготовим DataGridView и заполним его данными из calculator
                    dataGridViewMatrix.Columns.Clear();
                    dataGridViewMatrix.Rows.Clear();

                    dataGridViewMatrix.ColumnCount = calculator.Cols;
                    dataGridViewMatrix.RowCount = calculator.Rows;

                    for (int j = 0; j < calculator.Cols; j++)
                    {
                        dataGridViewMatrix.Columns[j].HeaderText = $"Столбец {j + 1}";
                    }

                    // Заполнение значениями
                    var mat = calculator.GetMatrix();
                    for (int i = 0; i < calculator.Rows; i++)
                    {
                        for (int j = 0; j < calculator.Cols; j++)
                        {
                            dataGridViewMatrix.Rows[i].Cells[j].Value = mat[i, j];
                        }
                    }

                    // Подготовка DataGridView для вектора
                    dataGridViewVector.Columns.Clear();
                    dataGridViewVector.Rows.Clear();
                    dataGridViewVector.ColumnCount = 1;
                    dataGridViewVector.RowCount = calculator.Cols;
                    dataGridViewVector.Columns[0].HeaderText = "Вектор";

                    var vec = calculator.GetVector();
                    if (vec != null)
                    {
                        for (int i = 0; i < vec.Length && i < dataGridViewVector.RowCount; i++)
                            dataGridViewVector.Rows[i].Cells[0].Value = vec[i];
                    }

                    // Только после того, как всё отрисовано и заполнено — показываем MessageBox
                    MessageBox.Show("Матрица успешно загружена!", "Успех",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки матрицы: {ex.Message}", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonUploadVector_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                openFileDialog.Title = "Загрузить вектор";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        calculator.LoadVectorFromFile(openFileDialog.FileName);

                        // Обновляем DataGridView вектора
                        var vector = calculator.GetVector();
                        for (int i = 0; i < vector.Length; i++)
                        {
                            dataGridViewVector.Rows[i].Cells[0].Value = vector[i];
                        }

                        MessageBox.Show("Вектор успешно загружен!", "Успех",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка загрузки вектора",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void buttonCount_Click(object sender, EventArgs e)
        {
            try
            {
                // Сначала обновляем данные из DataGridView
                UpdateDataFromGridViews();

                // Выполняем умножение
                calculator.Multiply();

                // Выводим результат
                DisplayResult();

                UpdateSaveButtonState();

                MessageBox.Show("Умножение выполнено успешно!", "Успех",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка умножения",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDataFromGridViews()
        {
            try
            {
                calculator.SetMatrixFromDataGridView(dataGridViewMatrix);

                calculator.SetVectorFromDataGridView(dataGridViewVector);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка данных в таблицах: {ex.Message}");
            }
        }

        private void DisplayResult()
        {
            var result = calculator.GetResult();

            // Очищаем и настраиваем DataGridView для результата
            dataGridViewResult.Rows.Clear();
            dataGridViewResult.Columns.Clear();

            dataGridViewResult.RowCount = result.Length;
            dataGridViewResult.ColumnCount = 1;

            dataGridViewResult.Columns[0].HeaderText = "Результат";

            // Добавляем столбцы с результатами
            for (int i = 0; i < result.Length; i++)
            {
                dataGridViewResult.Rows[i].Cells[0].Value = result[i].ToString("G");
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (calculator.GetResult() == null || calculator.GetResult().Length == 0)
                {
                    MessageBox.Show("Нет данных для сохранения. Сначала выполните умножение.",
                                  "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                calculator.SaveResultToFile();
                MessageBox.Show("Результат успешно сохранен!", "Успех",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSaveButtonState()
        {
            // Кнопка сохранить активна только когда таблица результата нарисована и заполнена
            bool hasResultTable = dataGridViewResult.Rows.Count > 0 && dataGridViewResult.Columns.Count > 0 && (int)numericUpDownM.Value > 0 && (int)numericUpDownN.Value > 0;

            buttonSave.Enabled = hasResultTable;
        }

        private void DataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox textBox)
            {
                // Удаляем предыдущий обработчик чтобы избежать дублирования
                textBox.KeyPress -= TextBox_KeyPress;
                textBox.KeyPress += TextBox_KeyPress;
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем: цифры, запятая, минус, Backspace
            if (char.IsDigit(e.KeyChar) || e.KeyChar == ',' || e.KeyChar == '-' || e.KeyChar == (char)Keys.Back)
            {
                // Дополнительная проверка для минуса (только в начале)
                if (e.KeyChar == '-' && ((TextBox)sender).SelectionStart != 0)
                {
                    e.Handled = true;
                    return;
                }

                // Дополнительная проверка для запятой (только одна)
                if (e.KeyChar == ',' && ((TextBox)sender).Text.Contains(','))
                {
                    e.Handled = true;
                    return;
                }

                e.Handled = false;
            }
            else
            {
                // Запрещаем все остальные символы, включая точку
                e.Handled = true;
            }
        }
    }
}