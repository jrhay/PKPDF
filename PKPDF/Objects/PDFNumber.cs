using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PortableKnowledge.PDF
{
    public class PDFNumber : IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.Number;

        Decimal Value;

        public PDFNumber(int Number)
        {
            this.Value = Number;
        }

        public PDFNumber(float Number)
        {
            this.Value = (Decimal)Number;
        }

        public string Description => Value.ToString();

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
            EndingIndex = StartingIndex + StartingToken.Length;

            int IntNumber;
            if (int.TryParse(StartingToken, out IntNumber))
                return new PDFNumber(IntNumber);

            float RealNumber;
            if (float.TryParse(StartingToken, out RealNumber))
                return new PDFNumber(RealNumber);

            EndingIndex = StartingIndex;
            return null;
        }
    }
}
