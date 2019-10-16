using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    /// <summary>
    /// Class to represent a comment on a line in a PDF file
    /// </summary>
    public class PDFComment : IPDFObject
    {
        public byte[] Data { get; }

        public int Location { get; }

        public PDFObjectType Type => PDFObjectType.Comment;

        public string Description => "% " + Encoding.UTF8.GetString(Data);

        public PDFComment(byte[] CommentData, int Location)
        {
            if (CommentData == null)
                this.Data = new byte[0];
            else
                this.Data = CommentData;

            this.Location = Location;
        }

        //<summary>
        //Attempt to parse the given data stream, returning an indicator of parse progress
        //</summary>
        //<param name="StartingToken">The token immediately preceeding the starting index in Data stream</param>
        //<param name="Data">Raw byte stream to parse</param>
        /// <param name="StartingIndex">0-based starting index into Data where StartingToken appears</param>
        //<param name="EndingIndex">Index into data stream where parsing ended (either successfully or unsuccessfully)</param>
        //<returns>Object parsed from data stream, or NULL if unable to parse. If NULL and EndingIndex is equal to Data.Length, parsing may be successful with more data</returns>
        public static IPDFObject TryParse(string StartingToken, byte[] Data, int StartingIndex, out int EndingIndex)
        {
            if (!String.IsNullOrEmpty(StartingToken) && (StartingToken[0] == PDF.CommentDelimiter))
                return new PDFComment(PDF.ExtractPDFLine(Data, StartingIndex + 1, out EndingIndex), StartingIndex);

            EndingIndex = StartingIndex;
            return null;
        }

        /// <summary>
        /// Trim a string of any PDF comment, returning the trimmed string and the extracted comment text
        /// </summary>
        /// <param name="OriginalLine">Data line as read from PDF file</param>
        /// <param name="Comment">Complete text of the comment in the line, null if line contains no comment</param>
        /// <param name="CommentStart">Index into OriginalLine of comment character, -1 if line contains no comment</param>
        /// <param name="CommentEnd">Index into OriginalLine of last character in comment (after EOL). Will be -1 if line contains no comment</param>
        public static void ExtractPDFComment(byte[] OriginalLine, out byte[] Comment, out int CommentStart, out int CommentEnd)
        {
            CommentStart = PDF.CommentStart(OriginalLine);
            Comment = PDF.ExtractPDFLine(OriginalLine, CommentStart, out CommentEnd);
        }

        /// <summary>
        /// Create a new PDF comment instance from a data line read from a PDF file
        /// </summary>
        /// <param name="OriginalLine">PDF data line</param>
        /// <returns>Instance populated with comment data and location, data may be null if no comment is found in the line</returns>
        public static PDFComment FromString(byte[] OriginalLine)
        {
            byte[] Comment = null;
            int Index = -1;
            ExtractPDFComment(OriginalLine, out Comment, out _, out Index);
            return new PDFComment(Comment, Index);
        }
    }
}
