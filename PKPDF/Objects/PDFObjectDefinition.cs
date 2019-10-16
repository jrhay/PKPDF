using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PortableKnowledge.PDF
{
    public class PDFObjectDefinition : IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.ObjectDefinition;

        public int ObjectNumber { get; internal set; }

        public int GenerationNumber { get; internal set; }

        public string Identifier { get { return ObjectNumber.ToString() + " " + GenerationNumber.ToString(); } }

        public IPDFObject Object { get; internal set; }

        public PDFObjectDefinition(int Number, int Generation, IPDFObject PDFObject)
        {
            this.ObjectNumber = Number;
            this.GenerationNumber = Generation;
            this.Object = PDFObject;
        }

        public string Description => "Object [" + ObjectNumber + ":" + GenerationNumber + "] => " + (Object != null ? Object.Description : "null");

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
            int Number;

            EndingIndex = StartingIndex;
            if (int.TryParse(StartingToken, out Number))
            {
                string Declaration = PDFObjectParser.GetTokenString(Data, StartingIndex + StartingToken.Length, out _, out EndingIndex, 2);
                if (!String.IsNullOrEmpty(Declaration))
                {
                    Match objMatch = Regex.Match(Declaration, @"([+-]?\d+) obj");
                    if (objMatch.Success)
                    {
                        if (Declaration.Length != objMatch.Length)
                            EndingIndex = EndingIndex - (Declaration.Length - objMatch.Length) - 1;
                        int Generation = int.Parse(objMatch.Groups[1].Value);

                        // Parse the indirect object content (should be a single object)
                        IPDFObject PDFObject = PDFObjectParser.Parse(Data, out EndingIndex, EndingIndex);
                        if (PDFObject != null)
                        {
                            int ObjectEndIndex = PDF.FirstOccurance(Data, Encoding.UTF8.GetBytes("endobj"), EndingIndex);
                            if (ObjectEndIndex > 0)
                            {
                                // Format error - tokens after object definition but before "endobj"
                            }

                            EndingIndex = ObjectEndIndex + "endobj".Length;
                            return new PDFObjectDefinition(Number, Generation, PDFObject);
                        }
                    }
                }
            }

            return null;
        }
    }
}
