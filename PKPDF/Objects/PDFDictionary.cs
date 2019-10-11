using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    class PDFDictionary : Dictionary<IPDFObject, IPDFObject>, IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.Dictionary;

        public PDFDictionary(Dictionary<IPDFObject, IPDFObject> keyValuePairs) : base(keyValuePairs)
        {
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
            Dictionary<IPDFObject, IPDFObject> KeyValuePairs = new Dictionary<IPDFObject, IPDFObject>();

            EndingIndex = StartingIndex;
            if (StartingToken.Equals("<<"))
            {
                while (EndingIndex < Data.Length)
                {
                    if (">>".Equals(PDFObjectParser.GetTokenString(Data, EndingIndex, out _)))
                        return new PDFDictionary(KeyValuePairs);

                    IPDFObject Key = PDFObjectParser.Parse(Data, out EndingIndex, EndingIndex);
                    if (Key == null)
                        return null; // No key found

                    if (Key.Type != PDFObjectType.Name)
                        return null; // Invalid key type found

                    IPDFObject Value = PDFObjectParser.Parse(Data, out EndingIndex, EndingIndex);
                    if (Value == null)
                        return null; // No value found

                    KeyValuePairs.Add(Key, Value);
                }
            }

            // Didn't find a dictionary delimiter
            return null;
        }
    }
}
