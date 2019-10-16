using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    class PDFStream : IPDFObject
    {
        PDFDictionary StreamDictionary = null;

        byte[] Data;

        public PDFObjectType Type => PDFObjectType.Stream;

        public string Description => "Stream of " + ((Data != null) ? Data.Length.ToString() : "0") + " bytes.";

        public PDFStream(PDFDictionary StreamDictionary, byte[] StreamData)
        {
            StreamDictionary["Length"] = new PDFNumber(StreamData.Length);
            this.StreamDictionary = StreamDictionary;
            this.Data = StreamData;
        }

        public static PDFStream MakeStream(PDFDictionary StreamDictionary, byte[] Data, int StartingIndex, out int EndingIndex)
        {
            int TokEnd;

            int StreamLength = 0;
            if (StreamDictionary.ContainsKey("Length"))
                StreamLength = (int)(Decimal.Parse(StreamDictionary["Length"].Description));
            else
                StreamLength = PDF.FirstOccurance(Data, Encoding.UTF8.GetBytes("endstream"), StartingIndex);

            EndingIndex = StartingIndex;
            if ("stream".Equals(PDFObjectParser.GetTokenString(Data, EndingIndex, out _, out TokEnd)))
            {
                if ((StreamLength > 0) && (Data.Length >= TokEnd + 1 + StreamLength))
                {
                    byte[] StreamBytes = new byte[StreamLength];
                    Array.Copy(Data, TokEnd + 1, StreamBytes, 0, StreamLength);
                    EndingIndex = TokEnd + 1 + StreamLength;
                    if (!"endstream".Equals(PDFObjectParser.GetTokenString(Data, EndingIndex, out _, out EndingIndex)))
                    { } // Error - missing "endstream"
                    return new PDFStream(StreamDictionary, StreamBytes);
                }
            }

            // Unable to make a stream from this data
            return null;
        }
    }
}
