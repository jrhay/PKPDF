﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    class PDFName : IPDFObject
    {
        public PDFObjectType Type => PDFObjectType.Name;

        public string Text { get; }

        public PDFName(string NameText) : base()
        {
            this.Text = NameText;
        }

        public string Description => (String.IsNullOrEmpty(Text) ? "" : Text);

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
            EndingIndex = StartingIndex;

            if ((StartingToken.Length > 1) && (StartingToken[0] == '/'))
            {
                int DelimIndex = PDF.DelimiterIndex(StartingToken, 1);
                if (DelimIndex >= 0)
                    EndingIndex = StartingIndex + DelimIndex;
                else
                    EndingIndex += StartingToken.Length;
    
                return new PDFName(StartingToken.Substring(1, (EndingIndex - StartingIndex) - 1));
            }

            return null;
        }

        public override string ToString()
        {
            return base.ToString() + "(" + Text + ")";
        }
    }
}
