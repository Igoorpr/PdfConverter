namespace ConversorPDF
{
    partial class Principal_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fechar_bt = new System.Windows.Forms.Button();
            this.Converter_bt = new System.Windows.Forms.Button();
            this.NomePDF_TB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // fechar_bt
            // 
            this.fechar_bt.Location = new System.Drawing.Point(154, 85);
            this.fechar_bt.Name = "fechar_bt";
            this.fechar_bt.Size = new System.Drawing.Size(53, 23);
            this.fechar_bt.TabIndex = 1;
            this.fechar_bt.Text = "Fechar";
            this.fechar_bt.UseVisualStyleBackColor = true;
            this.fechar_bt.Click += new System.EventHandler(this.fechar_bt_Click);
            // 
            // Converter_bt
            // 
            this.Converter_bt.Location = new System.Drawing.Point(12, 85);
            this.Converter_bt.Name = "Converter_bt";
            this.Converter_bt.Size = new System.Drawing.Size(75, 23);
            this.Converter_bt.TabIndex = 9;
            this.Converter_bt.Text = "Converter";
            this.Converter_bt.UseVisualStyleBackColor = true;
            this.Converter_bt.Click += new System.EventHandler(this.Converter_bt_Click);
            // 
            // NomePDF_TB
            // 
            this.NomePDF_TB.Location = new System.Drawing.Point(12, 48);
            this.NomePDF_TB.Name = "NomePDF_TB";
            this.NomePDF_TB.Size = new System.Drawing.Size(195, 20);
            this.NomePDF_TB.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Nome do Arquivo em PDF";
            // 
            // Principal_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 123);
            this.Controls.Add(this.Converter_bt);
            this.Controls.Add(this.NomePDF_TB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fechar_bt);
            this.Name = "Principal_Form";
            this.Text = "Principal";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button fechar_bt;
        private System.Windows.Forms.Button Converter_bt;
        private System.Windows.Forms.TextBox NomePDF_TB;
        private System.Windows.Forms.Label label1;
    }
}