using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    public class PDFDictionary : Dictionary<string, IPDFObject>, IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.Dictionary;

        public PDFDictionary(Dictionary<string, IPDFObject> keyValuePairs) : base(keyValuePairs)
        {
        }

        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder(this.Keys.Count + 2);
                sb.AppendLine("Dictionary:");
                foreach (string key in Keys)
                {
                    sb.AppendLine("\t" + key + " => " + this[key].Description);
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
            Dictionary<string, IPDFObject> KeyValuePairs = new Dictionary<string, IPDFObject>();

            EndingIndex = StartingIndex;
            if (StartingToken.Equals("<<"))
            {
                EndingIndex += StartingToken.Length;
                while (EndingIndex < Data.Length)
                {
                    int TokEnd;
                    if (">>".Equals(PDFObjectParser.GetTokenString(Data, EndingIndex, out _, out TokEnd)))
                    {
                        EndingIndex = TokEnd;
                        if ("stream".Equals(PDFObjectParser.GetTokenString(Data, EndingIndex, out _, out TokEnd)))
                            return PDFStream.MakeStream(new PDFDictionary(KeyValuePairs), Data, EndingIndex, out EndingIndex);
                        else
                            return new PDFDictionary(KeyValuePairs);
                    }

                    IPDFObject Key = PDFObjectParser.Parse(Data, out EndingIndex, EndingIndex);
                    if (Key == null)
                        return null; // No key found

                    if (Key.Type != PDFObjectType.Name)
                        return null; // Invalid key type found

                    IPDFObject Value = PDFObjectParser.Parse(Data, out EndingIndex, EndingIndex);
                    if (Value == null)
                        return null; // No value found

                    KeyValuePairs.Add(Key.Description, Value);
                }
            }

            // Didn't find a dictionary delimiter
            return null;
        }
    }
}
