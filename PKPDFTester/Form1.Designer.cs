namespace PKPDFTester
{
    partial class Form1
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
            this.btnOpenPDF = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPDFName = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPDFVersion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBinary = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMaxObjects = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOpenPDF
            // 
            this.btnOpenPDF.Location = new System.Drawing.Point(15, 15);
            this.btnOpenPDF.Name = "btnOpenPDF";
            this.btnOpenPDF.Size = new System.Drawing.Size(87, 38);
            this.btnOpenPDF.TabIndex = 0;
            this.btnOpenPDF.Text = "Open PDF...";
            this.btnOpenPDF.UseVisualStyleBackColor = true;
            this.btnOpenPDF.Click += new System.EventHandler(this.btnOpenPDF_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(127, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1201, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Opened PDF";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPDFName
            // 
            this.txtPDFName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPDFName.Location = new System.Drawing.Point(127, 33);
            this.txtPDFName.Name = "txtPDFName";
            this.txtPDFName.ReadOnly = true;
            this.txtPDFName.Size = new System.Drawing.Size(1201, 20);
            this.txtPDFName.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "PDF Files|*.pdf|All Files|*.*";
            this.openFileDialog1.Title = "Open PDF Document...";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(127, 145);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox3.Size = new System.Drawing.Size(1198, 305);
            this.textBox3.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label4.Location = new System.Drawing.Point(124, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1204, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Objects in PDF";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPDFVersion
            // 
            this.txtPDFVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPDFVersion.Location = new System.Drawing.Point(127, 85);
            this.txtPDFVersion.Name = "txtPDFVersion";
            this.txtPDFVersion.ReadOnly = true;
            this.txtPDFVersion.Size = new System.Drawing.Size(89, 20);
            this.txtPDFVersion.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(127, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "PDF Version";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBinary
            // 
            this.txtBinary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBinary.Location = new System.Drawing.Point(232, 85);
            this.txtBinary.Name = "txtBinary";
            this.txtBinary.ReadOnly = true;
            this.txtBinary.Size = new System.Drawing.Size(89, 20);
            this.txtBinary.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label3.Location = new System.Drawing.Point(232, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Contains 8-bit";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMaxObjects
            // 
            this.txtMaxObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaxObjects.Location = new System.Drawing.Point(345, 85);
            this.txtMaxObjects.Name = "txtMaxObjects";
            this.txtMaxObjects.ReadOnly = true;
            this.txtMaxObjects.Size = new System.Drawing.Size(89, 20);
            this.txtMaxObjects.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label5.Location = new System.Drawing.Point(345, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Num Objects";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1340, 450);
            this.Controls.Add(this.txtMaxObjects);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBinary);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPDFVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPDFName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOpenPDF);
            this.Name = "Form1";
            this.Text = "PKPDF Tester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenPDF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPDFName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPDFVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBinary;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMaxObjects;
        private System.Windows.Forms.Label label5;
    }
}

