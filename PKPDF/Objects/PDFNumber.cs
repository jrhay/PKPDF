using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PortableKnowledge.PDF
{
    public class PDFNumber : IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.Number;

        public Decimal Value;

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
            string numToken = string.Concat(StartingToken.TakeWhile(c => (c >= '0' && c <= '9') || c == '.' || c == '+' || c == '-'));
            EndingIndex = StartingIndex + numToken.Length;

            int IntNumber;
            if (int.TryParse(numToken, out IntNumber))
                return new PDFNumber(IntNumber);

            float RealNumber;
            if (float.TryParse(numToken, out RealNumber))
                return new PDFNumber(RealNumber);

            EndingIndex = StartingIndex;
            return null;
        }

        /// <summary>
        /// Attempt to parse a string as a PDF Number definition (ignoring whitespace characters, etc)
        /// </summary>
        /// <param name="Token">String to parse</param>
        /// <param name="Default">Value to return if unable to parse (default: null)</param>
        /// <returns>PDFNumber represented by the string, or Default if unable to parse</returns>
        public static PDFNumber TryParse(string Token, PDFNumber Default = null)
        {
            PDFNumber Parsed = (PDFNumber)TryParse(Token, null, 0, out _);
            if (Parsed == null)
                return Default;
            return Parsed;
        }

        /// <summary>
        /// Attempt to parse a string as a PDF Number definition (ignoring whitespace characters, etc) and
        /// return the integer version of that number
        /// </summary>
        /// <param name="Token">String to parse</param>
        /// <param name="Default">Value to return if unable to parse (default: null)</param>
        /// <returns>Integer value of parsed string, or Default if unable to parse</returns>
        public static int TryParse(string Token, int Default)
        {
            return (int)TryParse(Token, new PDFNumber(Default)).Value;
        }

        /// <summary>
        /// Attempt to parse a string as a PDF Number definition (ignoring whitespace characters, etc) and
        /// return the float version of that number
        /// </summary>
        /// <param name="Token">String to parse</param>
        /// <param name="Default">Value to return if unable to parse (default: null)</param>
        /// <returns>Float value of parsed string, or Default if unable to parse</returns>
        public static float TryParse(string Token, float Default)
        {
            return (float)TryParse(Token, new PDFNumber(Default)).Value;
        }

        public override string ToString()
        {
            return base.ToString() + "(" + Value.ToString() + ")";
        }
    }
}
