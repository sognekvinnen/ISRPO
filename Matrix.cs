using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Matrix
{
    public class MatrixCalculator
    {
        private double[,] matrix;
        private double[] vector;
        private double[] result;

        // Размеры матрицы
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        // Доступ к данным
        public double[,] GetMatrix() => matrix;
        public double[] GetVector() => vector;
        public double[] GetResult() => result;

        // Создание матрицы заданного размера
        public void ResizeMatrix(int newRows, int newCols)
        {
            // Создание пустой матрицы
            if (matrix == null)
            {
                matrix = new double[newRows, newCols];
                vector = new double[newCols];
                Rows = newRows;
                Cols = newCols;
                return;
            }

            // Изменение размеров заполненной матрицы
            double[,] newMatrix = new double[newRows, newCols];
            double[] newVector = new double[newCols];

            // Копирование данных
            int copyRows = Math.Min(Rows, newRows);
            int copyCols = Math.Min(Cols, newCols);

            for (int i = 0; i < copyRows; i++)
                for (int j = 0; j < copyCols; j++)
                    newMatrix[i, j] = matrix[i, j];

            for (int j = 0; j < Math.Min(vector.Length, newCols); j++)
                newVector[j] = vector[j];

            Rows = newRows;
            Cols = newCols;
            matrix = newMatrix;
            vector = newVector;
        }

        // Загрузка матрицы из файла
        public void LoadMatrixFromFile(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath)
                                   .Where(line => !string.IsNullOrWhiteSpace(line))
                                   .ToArray();

                if (lines.Length == 0)
                    throw new Exception("Файл пустой");

                // Определяем количество строк матрицы
                int rowCount = lines.Length;

                // Находим максимальное количество столбцов среди всех строк
                int colCount = 0;
                for (int i = 0; i < rowCount; i++)
                {
                    var elements = lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (elements.Length > colCount)
                        colCount = elements.Length;
                }

                if (rowCount > 10 || colCount > 10)
                    throw new Exception("Размеры матрицы превышают 10x10");

                // Создание матрицы
                ResizeMatrix(rowCount, colCount);

                // Заполнение матрицы
                for (int i = 0; i < Rows; i++)
                {
                    var rowData = lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < Cols; j++)
                    {
                        if (j < rowData.Length)
                        {
                            string numberString = rowData[j].Replace('.', ',');
                            if (!double.TryParse(numberString, out double value))
                                throw new Exception($"Некорректное число в позиции [{i + 1}, {j + 1}]");
                            matrix[i, j] = value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки матрицы: {ex.Message}");
            }
        }

        // Загрузка вектора из файла
        public void LoadVectorFromFile(string filePath)
        {
            try
            {
                string allText = File.ReadAllText(filePath);

                string[] numberStrings = allText.Split(new[] { ' ', '\t', '\r', '\n' },
                                                     StringSplitOptions.RemoveEmptyEntries);

                if (numberStrings.Length == 0)
                    throw new Exception("Файл пустой");

                // Проверка, что вектор соответствует размеру матрицы
                if (numberStrings.Length != Cols)
                    throw new Exception($"Размер вектора ({numberStrings.Length}) не соответствует количеству столбцов матрицы ({Cols})");

                vector = new double[Cols];

                for (int i = 0; i < Cols; i++)
                {
                    if (!double.TryParse(numberStrings[i], out double value))
                        throw new Exception($"Некорректное число в векторе на позиции {i + 1}");

                    vector[i] = value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки вектора: {ex.Message}");
            }
        }

        // Умножение матрицы на вектор
        public void Multiply()
        {
            if (matrix == null)
                throw new Exception("Матрица не загружена");
            if (vector == null)
                throw new Exception("Вектор не загружен");
            if (Cols != vector.Length)
                throw new Exception($"Несовместимые размеры: матрица {Rows}x{Cols}, вектор {vector.Length}x1");

            result = new double[Rows];

            for (int i = 0; i < Rows; i++)
            {
                result[i] = 0;
                for (int j = 0; j < Cols; j++)
                {
                    result[i] += matrix[i, j] * vector[j];
                }
            }
        }

        // Сохранение результата в файл через SaveFileDialog
        public void SaveResultToFile()
        {
            if (result == null || result.Length == 0)
                throw new Exception("Нет данных для сохранения");

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveDialog.Title = "Сохранить результат";
                saveDialog.FileName = "resultvector.txt";

                if (saveDialog.ShowDialog() == DialogResult.OK) // Проверяем OK
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                        {
                            for (int i = 0; i < result.Length; i++)
                            {
                                writer.WriteLine(result[i].ToString("G"));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Ошибка сохранения файла: {ex.Message}");
                    }
                }
                else
                {
                    // Пользователь нажал Отмена - исключение
                    throw new Exception("Сохранение отменено пользователем");
                }
            }
        }

        // Установка матрицы из DataGridView
        public void SetMatrixFromDataGridView(DataGridView dataGridView)
        {
            if (dataGridView.Rows.Count != Rows || dataGridView.Columns.Count != Cols)
                throw new Exception("Несоответствие размеров DataGridView и матрицы");

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (dataGridView.Rows[i].Cells[j].Value != null &&
                        double.TryParse(dataGridView.Rows[i].Cells[j].Value.ToString(), out double value))
                    {
                        matrix[i, j] = value;
                    }
                    else
                    {
                        throw new Exception($"Некорректное значение в ячейке [{i + 1}, {j + 1}]");
                    }
                }
            }
        }

        // Установка вектора из DataGridView
        public void SetVectorFromDataGridView(DataGridView dataGridView)
        {
            if (dataGridView.Columns.Count != 1 || dataGridView.Rows.Count != Cols)
                throw new Exception($"Несоответствие размеров DataGridView вектора и матрицы");

            for (int i = 0; i < Cols; i++)
            {
                if (dataGridView.Rows[i].Cells[0].Value != null &&
                    double.TryParse(dataGridView.Rows[i].Cells[0].Value.ToString(), out double value))
                {
                    vector[i] = value;
                }
                else
                {
                    throw new Exception($"Некорректное значение в векторе на позиции {i + 1}");
                }
            }
        }
    }
}