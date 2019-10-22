using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    class PDFHexString : IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.HexString;

        public byte[] RawBytes { get; }

        public PDFHexString(byte[] stringData) : base()
        {
            this.RawBytes = stringData;
        }

        public PDFHexString(List<byte> stringBytes) : this(stringBytes.ToArray())
        { }

        public PDFHexString(string stringText) : base()
        {

            this.RawBytes = HexCharsToBytes(stringText);
        }

        /// <summary>
        /// Convert a string of ASCII Hex characters into an array of bytes
        /// </summary>
        /// <param name="HexString">String of pairs of hexadecimal digits representing bytes, with no spaces or punctuation</param>
        /// <returns>Best attempt at converting the string to an array of bytes</returns>
        public static byte[] HexCharsToBytes(string HexString)
        {
            // Append a "0" if non-even length
            if ((HexString.Length % 2) != 0)
                HexString += "0";

            byte[] Bytes = new byte[HexString.Length / 2];
            for (int ByteCount = 0; ByteCount < HexString.Length; ByteCount += 2)
                Bytes[ByteCount / 2] = Convert.ToByte(HexString.Substring(ByteCount, 2), 16);

            return Bytes;
        }

        /// <summary>
        /// Convert an array of bytes to a string of ASCII Hex characters
        /// </summary>
        /// <param name="Bytes">Byte array to convert to a string of hex characters</param>
        /// <returns>String containing pairs of hexadecimal digits representing bytes, with no spaces or punctuation</returns>
        public static String ToHexChars(byte[] Bytes)
        {
            return BitConverter.ToString(Bytes).Replace("-", "");
        }

        public string Description => "0x" + ToHexChars(RawBytes);

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
            if (!String.IsNullOrEmpty(StartingToken) && (StartingToken[0] == '<') && (!StartingToken.Equals("<<")))
            {
                List<byte> StringBytes = new List<byte>(StartingToken.Length);
                bool HaveString = false;
                EndingIndex = StartingIndex + 1;
                while (!HaveString && (EndingIndex < Data.Length))
                {
                    byte NextByte = Data[EndingIndex++];
                    if (NextByte == (byte)'>')
                        HaveString = true;
                    else if (!PDF.IsWhitespace(NextByte))
                        StringBytes.Add(NextByte);
                }

                return new PDFHexString(Encoding.UTF8.GetString(StringBytes.ToArray()));
            }
            else
            {
                EndingIndex = StartingIndex;
                return null;
            }
        }

        public override string ToString()
        {
            return base.ToString() + "(" + Description + ")";
        }
    }
}
