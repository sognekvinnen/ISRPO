namespace Matrix
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonUploadMatrix = new System.Windows.Forms.Button();
            this.buttonUploadVector = new System.Windows.Forms.Button();
            this.buttonCount = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownM = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownN = new System.Windows.Forms.NumericUpDown();
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.dataGridViewMatrix = new System.Windows.Forms.DataGridView();
            this.dataGridViewVector = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatrix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVector)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonUploadMatrix
            // 
            this.buttonUploadMatrix.Location = new System.Drawing.Point(89, 372);
            this.buttonUploadMatrix.Margin = new System.Windows.Forms.Padding(4);
            this.buttonUploadMatrix.Name = "buttonUploadMatrix";
            this.buttonUploadMatrix.Size = new System.Drawing.Size(231, 28);
            this.buttonUploadMatrix.TabIndex = 0;
            this.buttonUploadMatrix.Text = "Загрузить матрицу из файла";
            this.buttonUploadMatrix.UseVisualStyleBackColor = true;
            this.buttonUploadMatrix.Click += new System.EventHandler(this.buttonUploadMatrix_Click);
            // 
            // buttonUploadVector
            // 
            this.buttonUploadVector.Location = new System.Drawing.Point(566, 372);
            this.buttonUploadVector.Margin = new System.Windows.Forms.Padding(4);
            this.buttonUploadVector.Name = "buttonUploadVector";
            this.buttonUploadVector.Size = new System.Drawing.Size(231, 28);
            this.buttonUploadVector.TabIndex = 1;
            this.buttonUploadVector.Text = "Загрузить вектор из файла";
            this.buttonUploadVector.UseVisualStyleBackColor = true;
            this.buttonUploadVector.Click += new System.EventHandler(this.buttonUploadVector_Click);
            // 
            // buttonCount
            // 
            this.buttonCount.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.buttonCount.Location = new System.Drawing.Point(329, 372);
            this.buttonCount.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCount.Name = "buttonCount";
            this.buttonCount.Size = new System.Drawing.Size(231, 57);
            this.buttonCount.TabIndex = 2;
            this.buttonCount.Text = "Умножить матрицу на вектор";
            this.buttonCount.UseVisualStyleBackColor = true;
            this.buttonCount.Click += new System.EventHandler(this.buttonCount_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(329, 597);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(231, 28);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Сохранить в файл";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Введите размеры матрицы:";
            // 
            // numericUpDownM
            // 
            this.numericUpDownM.Location = new System.Drawing.Point(206, 44);
            this.numericUpDownM.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownM.Name = "numericUpDownM";
            this.numericUpDownM.Size = new System.Drawing.Size(45, 23);
            this.numericUpDownM.TabIndex = 5;
            this.numericUpDownM.ValueChanged += new System.EventHandler(this.numericUpDownM_ValueChanged);
            // 
            // numericUpDownN
            // 
            this.numericUpDownN.Location = new System.Drawing.Point(44, 44);
            this.numericUpDownN.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownN.Name = "numericUpDownN";
            this.numericUpDownN.Size = new System.Drawing.Size(45, 23);
            this.numericUpDownN.TabIndex = 6;
            this.numericUpDownN.ValueChanged += new System.EventHandler(this.numericUpDownN_ValueChanged);
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResult.Location = new System.Drawing.Point(380, 436);
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.RowHeadersWidth = 51;
            this.dataGridViewResult.Size = new System.Drawing.Size(146, 145);
            this.dataGridViewResult.TabIndex = 7;
            // 
            // dataGridViewMatrix
            // 
            this.dataGridViewMatrix.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridViewMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMatrix.Location = new System.Drawing.Point(20, 136);
            this.dataGridViewMatrix.Name = "dataGridViewMatrix";
            this.dataGridViewMatrix.RowHeadersWidth = 51;
            this.dataGridViewMatrix.Size = new System.Drawing.Size(372, 229);
            this.dataGridViewMatrix.TabIndex = 8;
            // 
            // dataGridViewVector
            // 
            this.dataGridViewVector.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridViewVector.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVector.Location = new System.Drawing.Point(605, 136);
            this.dataGridViewVector.Name = "dataGridViewVector";
            this.dataGridViewVector.RowHeadersWidth = 51;
            this.dataGridViewVector.Size = new System.Drawing.Size(157, 229);
            this.dataGridViewVector.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(441, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(417, 51);
            this.label2.TabIndex = 10;
            this.label2.Text = "Для умножения вектора Mx1 на матрицу MxN, длина вектора \r\nдолжна соответствовать " +
    "количеству столбцов матрицы (N),\r\nпоэтому вектор создается автоматически.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(257, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "строк";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(95, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "столбцов";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(178, 436);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "Результат умножения:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(118, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(623, 40);
            this.label6.TabIndex = 14;
            this.label6.Text = "Если вы хотите загрузить матрицу и вектор из файлов, начинайте с матрицы! \r\nДлина" +
    " вектора обязательно должна быть равна количеству столбцов матрицы.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(921, 638);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridViewVector);
            this.Controls.Add(this.dataGridViewMatrix);
            this.Controls.Add(this.dataGridViewResult);
            this.Controls.Add(this.numericUpDownN);
            this.Controls.Add(this.numericUpDownM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCount);
            this.Controls.Add(this.buttonUploadVector);
            this.Controls.Add(this.buttonUploadMatrix);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Умножение матрицы на вектор";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatrix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonUploadMatrix;
        private System.Windows.Forms.Button buttonUploadVector;
        private System.Windows.Forms.Button buttonCount;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownM;
        private System.Windows.Forms.NumericUpDown numericUpDownN;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.DataGridView dataGridViewMatrix;
        private System.Windows.Forms.DataGridView dataGridViewVector;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

