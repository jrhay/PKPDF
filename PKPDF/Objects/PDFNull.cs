using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    class PDFNull : IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.NullObject;

        public PDFNull() : base()
        {
        }

        public string Description => "null";

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

            if ("null".Equals(StartingToken))
            {
                EndingIndex = StartingIndex + StartingToken.Length;
                return new PDFNull();
            }

            return null;
        }
    }
}
