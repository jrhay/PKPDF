using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    class PDFString : IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.StringLiteral;

        public byte[] RawBytes { get; }

        public PDFString(byte[] stringData) : base()
        {
            this.RawBytes = stringData;
        }

        public PDFString(string stringText) : base()
        {
            this.RawBytes = Encoding.UTF8.GetBytes(stringText);
        }

        /// <summary>
        /// Attempt to parse the given data stream, returning an indicator of parse progress
        /// </summary>
        /// <param name="StartingToken">The token immediately preceeding the starting index in Data stream</param>
        /// <param name="Data">Raw byte stream to parse</param>
        /// <param name="StartingIndex">0-based starting index to start processing data stream (should point to byte immediately after StartingToken)</param>
        /// <param name="EndingIndex">Index into data stream where parsing ended (either successfully or unsuccessfully)</param>
        /// <returns>Object parsed from data stream, or NULL if unable to parse. If NULL and EndingIndex is equal to Data.Length, parsing may be successful with more data</returns>
        public static IPDFObject TryParse(string StartingToken, byte[] Data, int StartingIndex, out int EndingIndex)
        {
            EndingIndex = StartingIndex;

            // NOTE: Doesn't yet process '\'-escaped characters in the string, nested "()", or strings that have whitespace (more than one token)

            if ((StartingToken.Length > 1) && (StartingToken[0] == '(') && (StartingToken[StartingToken.Length - 1] == ')'))
                return new PDFString(StartingToken.Substring(1, StartingToken.Length - 2));

            //if ((StartingToken.Length > 1) && (StartingToken[0] == '('))
            //{
            //    List<byte> StringBytes = new List<byte>();
            //    int ParenNesting = 0;
            //
            //    while (EndingIndex < Data.Length)
            //    {
            //        byte NextByte = Data[EndingIndex];
            //        EndingIndex++;

            //        if (NextByte == ')')
            //        {
            //            if (ParenNesting == 0)
            //                return new PDFString(StringBytes.ToArray());

            //            StringBytes.Add(NextByte);
            //            ParenNesting--;
            //        }
            //        else
            //        {
            //            StringBytes.Add(NextByte);
            //            if (NextByte == '(')
            //                ParenNesting++;
            //        }
            //    }
            //}

            return null;
        }
    }
}
