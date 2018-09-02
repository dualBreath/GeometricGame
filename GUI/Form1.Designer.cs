namespace GUI
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
            this.mainField = new System.Windows.Forms.PictureBox();
            this.textBoxStatistics = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainField)).BeginInit();
            this.SuspendLayout();
            // 
            // mainField
            // 
            this.mainField.Location = new System.Drawing.Point(0, 0);
            this.mainField.Name = "mainField";
            this.mainField.Size = new System.Drawing.Size(804, 601);
            this.mainField.TabIndex = 0;
            this.mainField.TabStop = false;
            // 
            // textBoxStatistics
            // 
            this.textBoxStatistics.Location = new System.Drawing.Point(810, 3);
            this.textBoxStatistics.Multiline = true;
            this.textBoxStatistics.Name = "textBoxStatistics";
            this.textBoxStatistics.ReadOnly = true;
            this.textBoxStatistics.Size = new System.Drawing.Size(201, 84);
            this.textBoxStatistics.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 605);
            this.Controls.Add(this.textBoxStatistics);
            this.Controls.Add(this.mainField);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.mainField)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox mainField;
        private System.Windows.Forms.TextBox textBoxStatistics;
    }
}

