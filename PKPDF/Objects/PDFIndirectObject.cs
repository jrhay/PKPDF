using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PortableKnowledge.PDF
{
    public class PDFIndirectObject : IPDFObject
    {
        public int ObjectNumber { get; internal set; }

        public int GenerationNumber { get; internal set; }

        public string Identifier { get { return ObjectNumber.ToString() + " " + GenerationNumber.ToString(); } }

        public IPDFObject Object { get; internal set; }

        public PDFIndirectObject(int Number, int Generation, IPDFObject PDFObject)
        {
            this.ObjectNumber = Number;
            this.GenerationNumber = Generation;
            this.Object = PDFObject;
        }

        /// <summary>
        /// Attempt to parse the given data stream, returning an indicator of parse progress
        /// Returned value will be:
        /// (negative value) - Data can not be parsed as this type of object
        /// 0                - Data stream parsing is successful, but more data is needed before a complete object can be created
        /// (positive value) - Byte count into data (1-based) where parsing produced a valid object
        /// </summary>
        /// <param name="Data">Raw byte stream to parse</param>
        /// <returns>Parsing success indicator. If 0, parsing was successful and object contains parsed data. If negative, parsing failed.</returns>
        public static int TryParse(byte[] Data, int StartingIndex, out IPDFObject indirectObject)
        {
            string Declaration = PDF.GetTokenString(Data, StartingIndex, 3);
            if (!String.IsNullOrEmpty(Declaration))
            {
                Match objMatch = Regex.Match(Declaration, @"(\d+) (\d+) obj");
                if (objMatch.Success)
                {
                    int Number = int.Parse(objMatch.Groups[1].Value);
                    int Generation = int.Parse(objMatch.Groups[2].Value);

                    int EndIndex = PDF.FirstOccurance(Data, Encoding.UTF8.GetBytes("endobj"), StartingIndex);
                    if (EndIndex < 0)
                    {
                        // Need more data to process
                        indirectObject = null;
                        return 0;
                    }

                    // Parse the indirect object content (should be a single object)
                    List<IPDFObject> PDFObjects = PDFObjectParser.Parse(Data, out _, StartingIndex);
                    if (PDFObjects.Count > 0)
                    {
                        indirectObject = new PDFIndirectObject(Number, Generation, PDFObjects[0]);
                        return EndIndex + "endobj".Length + 1;
                    }

                    // Unable to find an object
                    indirectObject = null;
                    return -1;
                }
            }

            // Unable to parse
            indirectObject = null;
            return -1;
        }
    }
}
