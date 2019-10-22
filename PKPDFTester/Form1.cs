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
            textBox3.Clear();

            if (pdfFile != null)
            {
                foreach (IPDFObject pdfObject in pdfFile.GetAllObjects())
                {
                    textBox3.AppendText(pdfObject.Description);
                    textBox3.AppendText(Environment.NewLine);
                }
            }
        }

        private void OpenPDF(string PDFPathname)
        {
            _pdf = new PDF(PDFPathname);
            txtPDFName.Text = PDFPathname;
            txtPDFVersion.Text = _pdf.Version.ToString();
            txtBinary.Text = _pdf.isBinary.ToString();
            txtMaxObjects.Text = _pdf.MaxObjects.ToString();

            if (_pdf.Version == 0.0)
                MessageBox.Show("File does not contain a PDF version number; can not parse", "Invalid PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                //ShowFields(_pdf);
            }
        }

        private void btnOpenPDF_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                OpenPDF(openFileDialog1.FileName);
        }
    }
}
