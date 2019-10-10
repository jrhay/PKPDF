using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    public enum PDFFormFieldType
    {
        Generic,
        Text
    }

    public class PDFFormField
    {
        public string Name;
        public string Value;
        public PDFFormFieldType FieldType;

        public PDFFormField(string Name, string Value, PDFFormFieldType FieldType = PDFFormFieldType.Generic)
        {
            this.Name = Name;
            this.Value = Value;
            this.FieldType = FieldType;
        }
    }
}
