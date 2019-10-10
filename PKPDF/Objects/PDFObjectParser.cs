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
        /// Attempt to parse an sequence of bytes as a collection of PDF objects.
        /// </summary>
        /// <param name="Data">Bytes to parse</param>
        /// <param name="EndingIndex">0-based index into data indicating the byte after the last parsed byte</param>
        /// <param name="StartingIndex">0-based index into data at which to start parsing (default: 0)</param>
        /// <returns>Collection of PDF objects successfully parsed from the data</returns>
        public static List<IPDFObject> Parse(byte[] Data, out int EndingIndex, int StartingIndex = 0)
        {
            int index = StartingIndex - 1;
            List<IPDFObject> PDFObjects = new List<IPDFObject>();

            while (index++ < Data.Length)
            {
                byte DataByte = Data[index];
                if (!DataByte.IsPDFWhitespace())
                {
                    // Figure out what our next line is
                    int EOLLen = 0;
                    int EOL = PDF.EOLStart(Data, out EOLLen, index);

                    if (DataByte == PDF.CommentDelimiter)
                    {
                        // Comment - skip to EOL
                        index = EOL + EOLLen;
                    }
                    else
                    {
                        IPDFObject ParsedObject = null;
                        int ParseResult;

                        ParseResult = PDFIndirectObject.TryParse(Data, index, out ParsedObject);
                        if (ParseResult > 0)
                        {
                            PDFObjects.Add(ParsedObject);
                            index = ParseResult;
                            continue;
                        }

                        ParseResult = PDFDictionaryObject.TryParse(Data, index, out ParsedObject);
                        if (ParseResult > 0)
                        {
                            PDFObjects.Add(ParsedObject);
                            index = ParseResult;
                            continue;
                        }
                    }
                }
            }

            EndingIndex = index;
            return PDFObjects;
        }
    }
}
