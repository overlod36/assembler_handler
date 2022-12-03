
namespace _4
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richText_code = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGrid_oper = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGrid_names = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.richText_errors = new System.Windows.Forms.RichTextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.richText_bincode = new System.Windows.Forms.RichTextBox();
            this.by_step_button = new System.Windows.Forms.Button();
            this.full_button = new System.Windows.Forms.Button();
            this.reset_button = new System.Windows.Forms.Button();
            this.default_button = new System.Windows.Forms.Button();
            this.string_num = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_oper)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_names)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richText_code);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 268);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Исходный код";
            // 
            // richText_code
            // 
            this.richText_code.Location = new System.Drawing.Point(19, 32);
            this.richText_code.Name = "richText_code";
            this.richText_code.Size = new System.Drawing.Size(294, 213);
            this.richText_code.TabIndex = 0;
            this.richText_code.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGrid_oper);
            this.groupBox2.Location = new System.Drawing.Point(12, 287);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(382, 246);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Таблица кодов операции";
            // 
            // dataGrid_oper
            // 
            this.dataGrid_oper.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid_oper.ColumnHeadersVisible = false;
            this.dataGrid_oper.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dataGrid_oper.Location = new System.Drawing.Point(19, 33);
            this.dataGrid_oper.Name = "dataGrid_oper";
            this.dataGrid_oper.RowHeadersVisible = false;
            this.dataGrid_oper.RowHeadersWidth = 51;
            this.dataGrid_oper.RowTemplate.Height = 24;
            this.dataGrid_oper.Size = new System.Drawing.Size(338, 189);
            this.dataGrid_oper.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 83;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 83;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.Width = 84;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGrid_names);
            this.groupBox3.Location = new System.Drawing.Point(357, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(562, 268);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Таблица символических имен";
            // 
            // dataGrid_names
            // 
            this.dataGrid_names.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid_names.ColumnHeadersVisible = false;
            this.dataGrid_names.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column5,
            this.Column6});
            this.dataGrid_names.Location = new System.Drawing.Point(30, 32);
            this.dataGrid_names.Name = "dataGrid_names";
            this.dataGrid_names.RowHeadersVisible = false;
            this.dataGrid_names.RowHeadersWidth = 51;
            this.dataGrid_names.RowTemplate.Height = 24;
            this.dataGrid_names.Size = new System.Drawing.Size(504, 213);
            this.dataGrid_names.TabIndex = 0;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Column4";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.Width = 125;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Column5";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.Width = 125;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Column6";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.Width = 125;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.richText_errors);
            this.groupBox4.Location = new System.Drawing.Point(870, 364);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(418, 172);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Ошибки";
            // 
            // richText_errors
            // 
            this.richText_errors.Location = new System.Drawing.Point(18, 30);
            this.richText_errors.Name = "richText_errors";
            this.richText_errors.Size = new System.Drawing.Size(384, 124);
            this.richText_errors.TabIndex = 0;
            this.richText_errors.Text = "";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.richText_bincode);
            this.groupBox5.Location = new System.Drawing.Point(925, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(363, 343);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Двоичный код";
            // 
            // richText_bincode
            // 
            this.richText_bincode.Location = new System.Drawing.Point(18, 32);
            this.richText_bincode.Name = "richText_bincode";
            this.richText_bincode.Size = new System.Drawing.Size(329, 290);
            this.richText_bincode.TabIndex = 0;
            this.richText_bincode.Text = "";
            // 
            // by_step_button
            // 
            this.by_step_button.Location = new System.Drawing.Point(251, 55);
            this.by_step_button.Name = "by_step_button";
            this.by_step_button.Size = new System.Drawing.Size(104, 38);
            this.by_step_button.TabIndex = 5;
            this.by_step_button.Text = "Шаг";
            this.by_step_button.UseVisualStyleBackColor = true;
            this.by_step_button.Click += new System.EventHandler(this.by_step_button_Click);
            // 
            // full_button
            // 
            this.full_button.Location = new System.Drawing.Point(61, 55);
            this.full_button.Name = "full_button";
            this.full_button.Size = new System.Drawing.Size(136, 38);
            this.full_button.TabIndex = 6;
            this.full_button.Text = "Полный проход";
            this.full_button.UseVisualStyleBackColor = true;
            // 
            // reset_button
            // 
            this.reset_button.Location = new System.Drawing.Point(283, 32);
            this.reset_button.Name = "reset_button";
            this.reset_button.Size = new System.Drawing.Size(104, 35);
            this.reset_button.TabIndex = 7;
            this.reset_button.Text = "Сброс";
            this.reset_button.UseVisualStyleBackColor = true;
            this.reset_button.Click += new System.EventHandler(this.reset_button_Click);
            // 
            // default_button
            // 
            this.default_button.Location = new System.Drawing.Point(78, 32);
            this.default_button.Name = "default_button";
            this.default_button.Size = new System.Drawing.Size(100, 35);
            this.default_button.TabIndex = 8;
            this.default_button.Text = "Пример";
            this.default_button.UseVisualStyleBackColor = true;
            this.default_button.Click += new System.EventHandler(this.default_button_Click);
            // 
            // string_num
            // 
            this.string_num.Location = new System.Drawing.Point(371, 63);
            this.string_num.Name = "string_num";
            this.string_num.ReadOnly = true;
            this.string_num.Size = new System.Drawing.Size(57, 22);
            this.string_num.TabIndex = 9;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.full_button);
            this.groupBox6.Controls.Add(this.string_num);
            this.groupBox6.Controls.Add(this.by_step_button);
            this.groupBox6.Location = new System.Drawing.Point(400, 394);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(464, 139);
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Проход";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.reset_button);
            this.groupBox7.Controls.Add(this.default_button);
            this.groupBox7.Location = new System.Drawing.Point(400, 288);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(464, 100);
            this.groupBox7.TabIndex = 11;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Загрузка примера";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 548);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_oper)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_names)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RichTextBox richText_bincode;
        private System.Windows.Forms.RichTextBox richText_code;
        private System.Windows.Forms.DataGridView dataGrid_oper;
        private System.Windows.Forms.RichTextBox richText_errors;
        private System.Windows.Forms.DataGridView dataGrid_names;
        private System.Windows.Forms.Button by_step_button;
        private System.Windows.Forms.Button full_button;
        private System.Windows.Forms.Button reset_button;
        private System.Windows.Forms.Button default_button;
        private System.Windows.Forms.TextBox string_num;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.GroupBox groupBox7;
    }
}

