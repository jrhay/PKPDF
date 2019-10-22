using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    /// <summary>
    /// Defines methods for working with PDF file trailer sections
    /// </summary>
    public class PDFTrailer : PDFDictionary
    {
        public PDFCrossReference CrossReference = null;

        public new PDFObjectType Type => PDFObjectType.Trailer;

        public new string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder(this.Keys.Count + 2);
                sb.AppendLine("Trailer Dictionary:");
                foreach (string key in Keys)
                {
                    sb.AppendLine("\t" + key + " => " + this[key].Description);
                }
                return sb.ToString();
            }
        }

        public PDFTrailer(PDFDictionary TrailerDictionary, PDFCrossReference CrossReference) : base(TrailerDictionary)
        {
            if (CrossReference == null)
                this.CrossReference = new PDFCrossReference();
            else
                this.CrossReference = CrossReference;
        }

        /// <summary>
        /// The total number of entries in the cross reference table
        /// </summary>
        public int MaxObjects
        {
            get
            {
                IPDFObject SizeObject = this["Size"];
                if ((SizeObject != null) && (SizeObject.Type == PDFObjectType.Number))
                    return (int)((PDFNumber)SizeObject).Value;

                return 0;
            }
        }

        /// <summary>
        /// Read the trailer from a PDF data file
        /// </summary>
        /// <param name="Data">Data to read</param>
        /// <param name="StartIndex">Starting index of where to look for trailer, or -1 to look from end of file (default: -1)</param>
        /// <returns>TRUE if a trailer was successfully read, FALSE otherwise</returns>
        public static PDFTrailer ReadTrailer(byte[] Data, int StartIndex = -1)
        {
            int EndIndex = StartIndex;
            if (EndIndex < 0)
                EndIndex = PDF.FindEOF(Data, Data.Length - 1);

            if (EndIndex < 0)
                return null;

            int EndOfLineIndex;
            byte[] LineData = PDF.ExtractPreviousPDFLine(Data, EndIndex, out EndIndex, out EndOfLineIndex);
            while (LineData != null)
            {
                if ("trailer".Equals(Encoding.UTF8.GetString(LineData).Trim()))
                {
                    int TokenStartIndex = 0;
                    string Token = PDFObjectParser.GetTokenString(Data, EndOfLineIndex, out TokenStartIndex, out EndIndex);
                    PDFDictionary TrailerDictionary = (PDFDictionary)PDFDictionary.TryParse(Token, Data, TokenStartIndex, out EndIndex);
                    if (TrailerDictionary != null)
                    {
                        LineData = PDF.ExtractPDFLine(Data, EndIndex, out EndIndex);
                        if ("startxref".Equals(Encoding.UTF8.GetString(LineData).Trim()))
                        {
                            Token = PDFObjectParser.GetTokenString(Data, EndIndex, out TokenStartIndex, out _);
                            PDFNumber Offset = (PDFNumber)PDFNumber.TryParse(Token, Data, TokenStartIndex, out EndIndex);
                            PDFCrossReference CrossRef = PDFCrossReference.ReadCrossReference(Data, Offset, out _);
                            return new PDFTrailer(TrailerDictionary, CrossRef);
                        }
                        else
                            return null;
                    }
                    else
                        return null;
                }
                else
                    LineData = PDF.ExtractPreviousPDFLine(Data, EndIndex, out EndIndex, out EndOfLineIndex);
            }

            return null;
        }
    }
}
