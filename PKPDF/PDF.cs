using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PortableKnowledge.PDF
{
    /// <summary>
    /// A single PDF file
    /// </summary>
    public partial class PDF
    {
        public string Filepath { get; private set; }

        public PDF(string Pathname)
        {
            Filepath = Pathname;
        }

        public List<byte[]> GetAllLines()
        {
            List<byte[]> Lines = new List<byte[]>();

            byte[] AllData = File.ReadAllBytes(Filepath);
            int LineIndex = 0;
            while (LineIndex < AllData.Length)
            {
                int EOLLength = 0;
                int EOL = EOLStart(AllData, out EOLLength, LineIndex);
                int LineLength = EOL - LineIndex + EOLLength;
                byte[] Line = new byte[LineLength];
                Array.Copy(AllData, LineIndex, Line, 0, LineLength);
                Lines.Add(Line);
                LineIndex += LineLength;
            }

            return Lines;
        }

        public List<IPDFObject> GetObjects(int MaxObjects)
        {
            byte[] AllData = File.ReadAllBytes(Filepath);
            int EndingIndex = 0;
            List<IPDFObject> objects = new List<IPDFObject>();
            IPDFObject nextObject;

            while ((objects.Count < MaxObjects) && ((nextObject = PDFObjectParser.Parse(AllData, out EndingIndex, EndingIndex)) != null))
                objects.Add(nextObject);

            return objects;
        }

        public List<IPDFObject> GetAllObjects()
        {
            return GetObjects(int.MaxValue);
        }

        /// <summary>
        /// Return all form fillable fields in the currently open PDF document
        /// </summary>
        /// <returns>All form fillable fields, may be an empty list if no form fillable fields are present</returns>
        public List<PDFFormField> GetFields()
        {
            return null;
        }
    }
}
