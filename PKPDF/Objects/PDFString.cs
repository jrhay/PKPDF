using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    class PDFString : IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.StringLiteral;

        public byte[] RawBytes { get; }

        public PDFString(byte[] stringData) : base()
        {
            this.RawBytes = stringData;
        }

        public PDFString(List<byte> stringBytes) : this(stringBytes.ToArray())
        { }

        public PDFString(string stringText) : base()
        {
            this.RawBytes = Encoding.UTF8.GetBytes(stringText);
        }

        public string Description => Encoding.UTF8.GetString(RawBytes);

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
            if (!String.IsNullOrEmpty(StartingToken) && (StartingToken[0] == '('))
            {
                List<byte> StringBytes = new List<byte>(StartingToken.Length);
                int ParenCount = 1;
                EndingIndex = StartingIndex + 1;
                while ((ParenCount > 0) && (EndingIndex < Data.Length))
                {
                    byte NextByte = Data[EndingIndex++];
                    if (NextByte == (byte)'(')
                        ParenCount++;
                    else if (NextByte == (byte)')')
                        ParenCount--;

                    if (ParenCount > 0)
                    {
                        if (NextByte == 0x0D)
                        {
                            StringBytes.Add(0x0A);
                            if ((EndingIndex < Data.Length) && (Data[EndingIndex+1] == 0x0A))
                                EndingIndex++;
                        }
                        else if (NextByte == (byte)'\\')
                        {
                            if (EndingIndex < Data.Length)
                            {
                                byte Unescaped = PDF.UnescapeCharacter(Data[EndingIndex++]);
                                if (Unescaped == 0x0)
                                {
                                    StringBytes.Add(PDF.TranslateOctal(Data, EndingIndex - 1));
                                    EndingIndex++;
                                }
                                else
                                    StringBytes.Add(Unescaped);
                            }
                        }
                        else
                            StringBytes.Add(NextByte);
                    }
                }

                return new PDFString(StringBytes);
            }
            else
            {
                EndingIndex = StartingIndex;
                return null;
            }
        }
    }
}
