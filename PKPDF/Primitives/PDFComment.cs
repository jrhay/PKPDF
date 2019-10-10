using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    /// <summary>
    /// Class to represent a comment on a line in a PDF file
    /// </summary>
    public class PDFComment
    {
        public byte[] Data { get; }
        public string Text => Encoding.UTF8.GetString(Data);

        public int Location { get; }

        /// <summary>
        /// Trim a string of any PDF comment, returning the trimmed string and the extracted comment text
        /// </summary>
        /// <param name="OriginalLine">Data line as read from PDF file</param>
        /// <param name="Comment">Complete text of the comment in the line, null if line contains no comment</param>
        /// <param name="TrimmedLine">Line with comment removed (still containes any EOL characters)</param>
        /// <param name="CommentStart">Index into OriginalLine of comment character, -1 if line contains no comment</param>
        public static void ExtractPDFComment(byte[] OriginalLine, out byte[] Comment, out byte[] TrimmedLine, out int CommentStart)
        {
            Comment = null;
            TrimmedLine = OriginalLine;
            CommentStart = PDF.CommentStart(OriginalLine);
            if (CommentStart >= 0)
            {
                int EOLLength = 0;
                int EOLStart = PDF.EOLStart(OriginalLine, out EOLLength, CommentStart);
                int CommentLength = EOLStart - CommentStart - 1;
                Comment = new byte[CommentLength];
                TrimmedLine = new byte[OriginalLine.Length - CommentLength - 1];
                Array.Copy(OriginalLine, CommentStart + 1, Comment, 0, CommentLength);

                Array.Copy(OriginalLine, 0, TrimmedLine, 0, CommentStart);
                Array.Copy(OriginalLine, EOLStart, TrimmedLine, CommentStart, EOLLength);
            }
        }

        public PDFComment(byte[] CommentData, int Location)
        {
            if (CommentData == null)
                this.Data = new byte[0];
            else
                this.Data = CommentData;

            this.Location = Location;
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
