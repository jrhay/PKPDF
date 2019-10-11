using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    /// <summary>
    /// 
    /// </summary>
    public class PDFObjectParser
    {
        /// <summary>
        /// Read a number of tokens (seperated by whitespace) from a PDF data stream as UTF8 characters
        /// </summary>
        /// <param name="Data">Data stream to read</param>
        /// <param name="StartingIndex">Starting offset</param>
        /// <param name="EndingIndex">Offset in the data array where reading stopped</param>
        /// <param name="WordCount">Number of tokens sepreated by whitespace to read. If multiple words are read, all whitespace will be replaced by a single space character (0x20)</param>
        /// <returns>Read tokens as a UTF8 string, or NULL if unable to read the specified number of words before running out of data</returns>
        internal static string GetTokenString(byte[] Data, int StartingIndex, out int EndingIndex, int WordCount = 1)
        {
            List<Byte> TokenString = new List<Byte>(20);
            bool InComment = false;
            bool InWhitespace = false;

            EndingIndex = StartingIndex;
            while ((WordCount > 0) && (EndingIndex < Data.Length))
            {
                byte DataByte = Data[EndingIndex];

                InComment = InComment || (DataByte == PDF.CommentDelimiter);

                if (InComment)
                    InComment = !PDF.IsEOL(DataByte);
                else
                {
                    if (InWhitespace && !PDF.IsWhitespace(DataByte))
                        InWhitespace = false;

                    if (!InWhitespace)
                    {
                        if (PDF.IsWhitespace(DataByte))
                        {
                            InWhitespace = true;
                            if (TokenString.Count > 0)
                            {   
                                // Don't add leading/trailing whitespace
                                WordCount--;
                                if (WordCount > 0)
                                    TokenString.Add(0x20);
                            }
                        }
                        else
                            TokenString.Add(DataByte);
                    }
                }

                EndingIndex++;
            }

            if (WordCount > 0)
                return null;

            return Encoding.UTF8.GetString(TokenString.ToArray());
        }

        /// <summary>
        /// Attempt to parse a sequence of bytes as a PDF object
        /// </summary>
        /// <param name="Data">Bytes to parse</param>
        /// <param name="EndingIndex">0-based index into data indicating the byte after the last parsed byte</param>
        /// <param name="StartingIndex">0-based index into data at which to start parsing (default: 0)</param>
        /// <returns>PDF object successfully parsed from the data, or NULL if no complete object was parsable</returns>
        public static IPDFObject Parse(byte[] Data, out int EndingIndex, int StartingIndex = 0)
        {
            int EndTokenIndex;

            IPDFObject ParsedObject = null;
            string Token = GetTokenString(Data, StartingIndex, out EndTokenIndex);

            if (!string.IsNullOrEmpty(Token))
            {
                if ((ParsedObject = PDFDictionary.TryParse(Token, Data, EndTokenIndex, out EndingIndex)) != null)
                    return ParsedObject;

                if ((ParsedObject = PDFIndirectObject.TryParse(Token, Data, EndTokenIndex, out EndingIndex)) != null)
                    return ParsedObject;

                if ((ParsedObject = PDFName.TryParse(Token, Data, EndTokenIndex, out EndingIndex)) != null)
                    return ParsedObject;

                if ((ParsedObject = PDFString.TryParse(Token, Data, EndTokenIndex, out EndingIndex)) != null)
                    return ParsedObject;
            }

            // Could not parse next object
            EndingIndex = StartingIndex;
            return null;
        }
    }
}
