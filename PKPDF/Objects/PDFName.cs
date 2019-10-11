using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    class PDFName : IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.Name;

        public string Text { get; }

        public PDFName(string NameText) : base()
        {
            this.Text = NameText;
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

            if ((StartingToken.Length > 1) && (StartingToken[0] == '/'))
                return new PDFName(StartingToken.Substring(1));

            return null;
        }
    }
}
