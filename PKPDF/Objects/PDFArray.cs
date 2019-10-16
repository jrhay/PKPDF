using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    class PDFArray : List<IPDFObject>, IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.Array;

        public byte[] RawBytes { get; }

        public PDFArray(IEnumerable<IPDFObject> objects) : base(objects)
        {
        }

        public PDFArray() : base()
        {
        }

        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder(this.Count + 2);
                sb.AppendLine("Array:");
                foreach (IPDFObject key in this)
                {
                    sb.AppendLine("\t" + key.Description);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Attempt to parse the given data stream, returning an indicator of parse progress
        /// </summary>
        /// <param name="StartingToken">The token immediately preceeding the starting index in Data stream</param>
        /// <param name="Data">Raw byte stream to parse</param>
        /// <param name="StartingIndex">0-based starting index into Data where StartingToken appears</param>
        /// <param name="EndingIndex">Index into data stream where parsing ended (either successfully or unsuccessfully)</param>
        /// <returns>Object parsed from data stream, or NULL if unable to parse. If NULL and EndingIndex is equal to Data.Length, parsing may be successful with more data</returns>
        public static IPDFObject TryParse(string StartingToken, byte[] Data, int StartingIndex, out int EndingIndex)
        {
            EndingIndex = StartingIndex;

            if (!String.IsNullOrEmpty(StartingToken) && (StartingToken[0] == '['))
            {
                PDFArray ObjectArray = new PDFArray();

                EndingIndex = StartingIndex + 1;
                while (EndingIndex < Data.Length)
                {
                    IPDFObject nextObject = PDFObjectParser.Parse(Data, out EndingIndex, EndingIndex);
                    if (nextObject != null)
                        ObjectArray.Add(nextObject);
                    char nextChar = (char)Data[EndingIndex];
                    if (nextChar == ']')
                    {
                        EndingIndex++;
                        return ObjectArray;
                    }
                }
            }

            return null;
        }
    }
}
