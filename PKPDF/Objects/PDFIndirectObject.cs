using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PortableKnowledge.PDF
{
    public class PDFIndirectObject : IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.IndirectReference;

        public int ObjectNumber { get; internal set; }

        public int GenerationNumber { get; internal set; }

        public string Identifier { get { return ObjectNumber.ToString() + " " + GenerationNumber.ToString(); } }

        public PDFIndirectObject(int Number, int Generation)
        {
            this.ObjectNumber = Number;
            this.GenerationNumber = Generation;
        }

        public string Description => "Object [" + ObjectNumber + ":" + GenerationNumber + "]";

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
            int StartIndex;
            int Number;

            EndingIndex = StartingIndex;
            if (int.TryParse(StartingToken, out Number))
            {
                string Declaration = PDFObjectParser.GetTokenString(Data, StartingIndex + StartingToken.Length, out StartIndex, out EndingIndex, 2);
                if (!String.IsNullOrEmpty(Declaration))
                {
                    Match objMatch = Regex.Match(Declaration, @"([+-]?\d+) R");
                    if (objMatch.Success)
                    {
                        int Generation = int.Parse(objMatch.Groups[1].Value);
                        if (Declaration.Length != objMatch.Length)
                            EndingIndex = EndingIndex - (Declaration.Length - objMatch.Length) - 1;
                        return new PDFIndirectObject(Number, Generation);
                    }
                }
            }

            return null;
        }

        public override string ToString()
        {
            return base.ToString() + "(" + Identifier + ")";
        }
    }
}
