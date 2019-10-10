using PortableKnowledge.PDF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PKPDFTester
{
    public partial class Form1 : Form
    {
        PDF _pdf = null;

        public Form1()
        {
            InitializeComponent();
            txtPDFName.ReadOnly = true;
        }

        private void ShowFields(PDF pdfFile)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            if (pdfFile != null)
            {
                //foreach (PDFFormField field in pdfFile.GetFields())
                //    listFields.Items.Add(field.Name + " : " + field.Value);
                foreach (byte[] Line in pdfFile.GetAllLines())
                {
                    string LineText = Encoding.UTF8.GetString(Line);
                    textBox1.AppendText(LineText); 
                    textBox1.AppendText(Environment.NewLine);

                    byte[] Comment = null;
                    int Index = -1;
                    byte[] Trimmed = null;
                    PDFComment.ExtractPDFComment(Line, out Comment, out Trimmed, out Index);
                    if ((Comment != null) && (Comment.Length > 0))
                    {
                        textBox2.AppendText(Encoding.UTF8.GetString(Comment));
                        textBox2.AppendText(Environment.NewLine);
                    }
                    textBox3.AppendText(Encoding.UTF8.GetString(PDF.TrimAndCollapseWhitespace(Trimmed)));
                    textBox3.AppendText(Environment.NewLine);
                }
            }
        }

        private void OpenPDF(string PDFPathname)
        {
            _pdf = new PDF(PDFPathname);
            txtPDFName.Text = PDFPathname;
            ShowFields(_pdf);
        }

        private void btnOpenPDF_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                OpenPDF(openFileDialog1.FileName);
        }
    }
}
