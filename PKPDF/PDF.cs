﻿using PortableKnowledge.PDF.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PortableKnowledge.PDF
{
    /// <summary>
    /// A single PDF file
    /// </summary>
    public partial class PDF
    {
        /// <summary>
        /// Maximum PDF specification revision that this PDF conforms to
        /// </summary>
        public float Version { get; }

        /// <summary>
        /// PDF specification revision that the header of the PDF lists
        /// </summary>
        public float HeaderVersion { get; }

        /// <summary>
        /// Original file pathname of the PDF file
        /// </summary>
        public string Filepath { get; private set; }

        /// <summary>
        /// Did the original PDF file contain 8-bit data?
        /// </summary>
        public bool isBinary { get; }

        /// <summary>
        /// Final trailer in PDF file
        /// </summary>
        public PDFTrailer Trailer { get; }

        /// <summary>
        /// Maximum number of objects defined in this PDF
        /// 0 if no trailer is found, or if the trailer does not indicate an object count (which is invalid)
        /// </summary>
        public int MaxObjects => (Trailer == null ? 0 : Trailer.MaxObjects);

        /// <summary>
        /// Raw bytes of the PDF file
        /// </summary>
        private byte[] RawData;

        /// <summary>
        /// Create a new PDF instance by reading a PDF file from disk.  If file is not successfully read or is an
        /// invalid or unsupported PDF file, an will be thrown.
        /// </summary>
        /// <param name="Pathname">Full pathname of PDF file to read from disk</param>
        /// <param name="AllowUnsupported">Allow unsupported PDF versions?  (default: false)</param>
        /// <remarks>Throws UnsupportedPDFFormatException if unable to determine PDF file version or if version exceeds PDF-1.7</remarks>
        public PDF(string Pathname, bool AllowUnsupported = false)
        {
            bool is8bit = false;

            Open(Pathname);
            HeaderVersion = ReadPDFHeader(out is8bit);
            Version = HeaderVersion;
            this.isBinary = is8bit;

            Trailer = PDFTrailer.ReadTrailer(RawData);

            if ((Version < 1) || (!AllowUnsupported && (Math.Truncate(Version * 10) > 17d)))
                throw new UnsupportedPDFFormatException(Version);
        }

        /// <summary>
        /// Determine the maximum PDF version of a PDF file
        /// </summary>
        /// <param name="Pathname">Full pathname to the PDF file</param>
        /// <param name="isBinary">Does the PDF header indicate binary content?</param>
        /// <returns>PDF version number (1.0 - 1.7) as indicated by the PDF header, or 0.0 if file does not appear to be a PDF file</returns>
        private float ReadPDFHeader(out bool isBinary)
        {
            int CommentEnd;
            float HeaderVersion = 0.0f;
 
            // Check for PDF version
            byte[] HeaderBytes;
            PDFComment.ExtractPDFComment(RawData, out HeaderBytes, out _, out CommentEnd);
            if (HeaderBytes.Length > 5)
            {
                string HeaderComment = Encoding.UTF8.GetString(HeaderBytes);
                if ("%PDF-".Equals(HeaderComment.Substring(0, 5)))
                    if (!float.TryParse(HeaderComment.Substring(5), out HeaderVersion))
                        HeaderVersion = 0.0f;
            }

            // Check for 8-bit-data flag
            isBinary = false;
            byte[] NextBytes = PDF.ExtractPDFLine(RawData, CommentEnd, out CommentEnd);      // Read to next EOL
            PDFComment.ExtractPDFComment(NextBytes, out HeaderBytes, out _, out CommentEnd); // Attempt to find a comment line
            if (HeaderBytes != null)
            {
                int binCount = 0;
                foreach (byte Byte in HeaderBytes)
                    binCount = binCount + (Byte > 128 ? 1 : 0);
                isBinary = (binCount > 3);
            }

            return HeaderVersion;
        }

        /// <summary>
        /// Open and read all bytes from a PDF file on disk.
        /// Throws exceptions if unable to read file.
        /// </summary>
        /// <param name="Pathname">Full pathname to the PDF file</param>
        /// <returns>TRUE if file read off disk successfully, FALSE otherwise</returns>
        public bool Open(string Pathname)
        {
            Filepath = Pathname;
            RawData = File.ReadAllBytes(Filepath);
            return true;
        }

        public List<byte[]> GetAllLines()
        {
            List<byte[]> Lines = new List<byte[]>();

            int LineIndex = 0;
            while (LineIndex <RawData.Length)
            {
                int EOLLength = 0;
                int EOL = EOLStart(RawData, out EOLLength, LineIndex);
                int LineLength = EOL - LineIndex + EOLLength;
                byte[] Line = new byte[LineLength];
                Array.Copy(RawData, LineIndex, Line, 0, LineLength);
                Lines.Add(Line);
                LineIndex += LineLength;
            }

            return Lines;
        }

        public List<IPDFObject> GetObjects(int MaxObjects)
        {
            byte[] AllData = File.ReadAllBytes(Filepath);
            int EndingIndex = 0;
            List<IPDFObject> objects = new List<IPDFObject>();
            IPDFObject nextObject;

            while ((objects.Count < MaxObjects) && ((nextObject = PDFObjectParser.Parse(AllData, out EndingIndex, EndingIndex)) != null))
                objects.Add(nextObject);

            return objects;
        }

        public List<IPDFObject> GetAllObjects()
        {
            return GetObjects(int.MaxValue);
        }

        /// <summary>
        /// Return all form fillable fields in the currently open PDF document
        /// </summary>
        /// <returns>All form fillable fields, may be an empty list if no form fillable fields are present</returns>
        public List<PDFFormField> GetFields()
        {
            return null;
        }
    }
}
