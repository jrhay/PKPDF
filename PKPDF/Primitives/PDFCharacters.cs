using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortableKnowledge.PDF
{
    /// <summary>
    /// Definitions of basic characters for PDF documents, as defined in ISO-32000-1 / PDF 1.7 Section 7.2
    /// https://www.adobe.com/content/dam/acom/en/devnet/pdf/pdfs/PDF32000_2008.pdf
    /// </summary>
    public partial class PDF
    {
        #region Character Set Definitions

        /// <summary>
        /// Valid whitespace characters in PDF documents
        /// </summary>
        public static byte[] Whitespace = { 0x00, 0x09, 0x0A, 0x0C, 0x0D, 0x20 };

        /// <summary>
        /// Valid End Of Line character sequences for PDF documents
        /// (listed in order of shortest sequence to longest sequence)
        /// </summary>
        public static byte[][] EOL = { new byte[] { 0x0D }, new byte[] { 0x0A }, new byte[] { 0x0D, 0x0A } };

        /// <summary>
        /// Valid PDF document delimiter characters
        /// </summary>
        public static byte[] Delimiter = { (byte)'(', (byte)')', (byte)'<', (byte)'>', (byte)'[', (byte)']', (byte)'{', (byte)'}', (byte)'/', (byte)'%' };

        /// <summary>
        /// Delimiter to mark the start of a comment in a PDF document
        /// </summary>
        public static byte CommentDelimiter = (byte)'%';

        /// <summary>
        /// Delimiter to mark the start of a string in a PDF document
        /// </summary>
        public static byte StringStartDelimiter = (byte)'(';

        /// <summary>
        /// Delimiter to mark the end of a string in a PDF document
        /// </summary>
        public static byte StringEndDelimiter = (byte)')';

        #endregion

        #region Character Set Tests

        /// <summary>
        /// Is a particular character PDF whitespace?
        /// </summary>
        /// <param name="testChar">Character to test</param>
        /// <returns>TRUE if character is whitespace for PDF, FALSE otherwise</returns>
        public static bool IsWhitespace(byte testChar)
        {
            return PDF.Whitespace.Contains(testChar);
        }


        /// <summary>
        /// Is a particular character PDF whitespace?
        /// </summary>
        /// <param name="testChar">Character to test</param>
        /// <returns>TRUE if character is whitespace for PDF, FALSE otherwise</returns>
        public static bool IsWhitespace(char testChar)
        {
            return IsWhitespace((byte)testChar);
        }

        /// <summary>
        /// Does a particular character sequence indicate a PDF End of Line?
        /// </summary>
        /// <param name="testString">Character sequence to test</param>
        /// <returns>TRUE if sequence is exactly an EOF, FALSE otherwise</returns>
        public static bool IsEOL(byte[] testString)
        {
            foreach (byte[] sequence in PDF.EOL)
                if (sequence.SequenceEqual(testString))
                    return true;

            return false;
        }

        /// <summary>
        /// Does a particular character sequence indicate a PDF End of Line?
        /// </summary>
        /// <param name="testString">Character sequence to test</param>
        /// <returns>TRUE if sequence is exactly an EOF, FALSE otherwise</returns>
        public static bool IsEOL(byte testChar)
        {
            return IsEOL(new byte[] { testChar });
        }

        /// <summary>
        /// Is a particular character a PDF delimiter?
        /// </summary>
        /// <param name="testChar">Character to test</param>
        /// <returns>TRUE if character is a for PDF delimiter, FALSE otherwise</returns>
        public static bool IsDelimiter(byte testChar)
        {
            return PDF.Delimiter.Contains(testChar);
        }

        /// <summary>
        /// Is a particular character a PDF delimiter?
        /// </summary>
        /// <param name="testChar">Character to test</param>
        /// <returns>TRUE if character is a for PDF delimiter, FALSE otherwise</returns>
        public static bool IsDelimiter(char testChar)
        {
            return IsDelimiter((byte)testChar);
        }

        /// <summary>
        /// Is a particular character a regular/non-special PDF character?
        /// </summary>
        /// <param name="testChar">Character to test</param>
        /// <returns>TRUE if character is a regular character, FALSE otherwise</returns>
        public static bool IsRegularCharacter(byte testChar)
        {
            return !IsWhitespace(testChar) && !IsDelimiter(testChar);
        }

        /// <summary>
        /// Is a particular character a regular/non-special PDF character?
        /// </summary>
        /// <param name="testChar">Character to test</param>
        /// <returns>TRUE if character is a regular character, FALSE otherwise</returns>
        public static bool IsRegularCharacter(char testChar)
        {
            return IsRegularCharacter((byte)testChar);
        }

        #endregion

        #region Character Operations

        /// <summary>
        /// Finds the first occurance of a particular character in a data line from a PDF file
        /// </summary>
        /// <param name="DataLine">Data line from PDF file</param>
        /// <param name="Character">Character to search for</param>
        /// <param name="StartingIndex">Start search at this index number, default (0) = first character of data line</param>
        /// <param name="EndingIndex">End search at this index number, default (-1) = last character of data line</param>
        /// <returns>0-based index of first character in found in data line, -1 if character is not found</returns>
        public static int FirstOccurance(byte[] DataLine, byte Character, int StartingIndex = 0, int EndingIndex = -1)
        {
            if (EndingIndex < 0)
                EndingIndex = DataLine.Length;

            return Array.IndexOf(DataLine, Character, StartingIndex, EndingIndex - StartingIndex);
        }

        /// <summary>
        /// Finds the first occurance of a particular character sequence in a data line from a PDF file
        /// </summary>
        /// <param name="DataLine">Data line from PDF file</param>
        /// <param name="Sequence">Character to search for</param>
        /// <param name="StartingIndex">Start search at this index number, default (0) = first character of data line</param>
        /// <param name="EndingIndex">End search at this index number, default (-1) = last character of data line</param>
        /// <returns>0-based index of first character in found in data line, -1 if character is not found</returns>
        public static int FirstOccurance(byte[] DataLine, byte[] Sequence, int StartingIndex = 0, int EndingIndex = -1)
        {
            if (EndingIndex < 0)
                EndingIndex = DataLine.Length;

            int Index = -1;
            int SeqIndex = 0;
            while ((Index < 0) && (SeqIndex >= 0) && (EndingIndex >= StartingIndex))
            {
                bool Match = false;
                SeqIndex = FirstOccurance(DataLine, Sequence[0], StartingIndex, EndingIndex);
                if (SeqIndex >= 0)
                {
                    Match = true;
                    for (int i = 1; Match && i < Sequence.Length; i++)
                        Match = (DataLine[SeqIndex + i] == Sequence[i]);
                }

                if (Match)
                    Index = SeqIndex;
                else
                    StartingIndex = StartingIndex + 1;
            }
            return Index;
        }

        /// <summary>
        /// Finds the last occurance of a particular character in a data line from a PDF file
        /// </summary>
        /// <param name="DataLine">Data line from PDF file</param>
        /// <param name="Character">Character to search for</param>
        /// <param name="StartingIndex">Start search at this index number, default (-1) = last character of data line</param>
        /// <param name="EndingIndex">End search at this index number, default (0) = first character of data line</param>
        /// <returns>0-based index of last character in found in data line, -1 if character is not found</returns>
        public static int LastOccurance(byte[] DataLine, byte Character, int StartingIndex = -1, int EndingIndex = 0)
        {
            if (StartingIndex < 0)
                StartingIndex = DataLine.Length - 1;

            if (EndingIndex > StartingIndex)
                EndingIndex = StartingIndex;

            return Array.LastIndexOf(DataLine, Character, StartingIndex, StartingIndex - EndingIndex + 1);
        }

        /// <summary>
        /// Find the start of a comment in a PDF data line
        /// </summary>
        /// <param name="DataLine">Data line to scan</param>
        /// <param name="StartingIndex">Start search at this index number, default (0) = first character of data line</param>
        /// <returns>Index of the comment character in the data line, or -1 if line contains no comment</returns>
        public static int CommentStart(byte[] DataLine, int StartingIndex = 0)
        {
            return FirstOccurance(DataLine, PDF.CommentDelimiter, StartingIndex);
        }

        /// <summary>
        /// Find the start of an End of Line sequence in a PDF data line
        /// </summary>
        /// <param name="DataLine">Data line to scan</param>
        /// <param name="EOLLength">Length of the found EOL sequence, 0 if none found</param>
        /// <param name="StartingIndex">Start search at this index number, default (0) = first character of data line</param>
        /// <returns>Index of the first character in an EOL sequence in the data line, or DataLine.Length if none found</returns>
        public static int EOLStart(byte[] DataLine, out int EOLLength, int StartingIndex = 0)
        {
            int Index = -1;
            EOLLength = 0;

            // Search for first occurance of each EOL sequence; take the first that occurs
            for (int sequence = 0; sequence < PDF.EOL.Length; sequence++)
            {
                int SeqIndex = FirstOccurance(DataLine, PDF.EOL[sequence], StartingIndex);
                if ((SeqIndex >= 0) && ((Index < 0) || (SeqIndex < Index)))
                {
                    Index = SeqIndex;
                    EOLLength = PDF.EOL[sequence].Length;
                }
            }

            if (Index < 0)
                Index = DataLine.Length;

            return Index;
        }

        /// <summary>
        /// Find index of the first delimiter in a given string
        /// </summary>
        /// <param name="DataLine">String to search for delimiters</param>
        /// <param name="StartingIndex">Index at which to start searching the string</param>
        /// <returns>Index of first delimiter character in the string, or -1 if string contains no delimiters</returns>
        public static int DelimiterIndex(string DataLine, int StartingIndex = 0)
        {
            int Index = int.MaxValue;
            string SearchString = (StartingIndex <= 0) ? DataLine : DataLine.Substring(StartingIndex);
            foreach (char Delim in PDF.Delimiter)
            {
                int DelimIndex = SearchString.IndexOf(Delim);
                if ((DelimIndex >= 0) && (DelimIndex < Index))
                    Index = DelimIndex;
            }

            if (Index == int.MaxValue)
                return -1;

            return Index + StartingIndex;
        }

        /// <summary>
        /// Find the start of an End of Line sequence in a PDF data line
        /// </summary>
        /// <param name="DataLine">Data line to scan</param>
        /// <param name="StartingIndex">Start search at this index number, default (0) = first character of data line</param>
        /// <returns>Index of the first character in an EOL sequence in the data line, or DataLine.Length if none found</returns>
        public static int EOLStart(byte[] DataLine, int StartingIndex = 0)
        {
            return EOLStart(DataLine, out _, StartingIndex);
        }

        /// <summary>
        /// Extract bytes from a PDF data stream up to an End Of Line byte or sequence of bytes
        /// </summary>
        /// <param name="Data">PDF Data stream to extract bytes from</param>
        /// <param name="StartIndex">Starting index in stream to start extracting bytes</param>
        /// <param name="EndIndex">Index in stream at end of line's EOL characters</param>
        /// <returns>Extracted data line</returns>
        public static byte[] ExtractPDFLine(byte[] Data, int StartIndex, out int EndIndex)
        {
            byte[] Line = null;
            EndIndex = StartIndex;
            if (StartIndex >= 0)
            {
                int EOLLength = 0;
                int EOLStart = PDF.EOLStart(Data, out EOLLength, StartIndex);
                EndIndex = EOLStart + EOLLength;

                int LineLength = EOLStart - StartIndex - 1;
                Line = new byte[LineLength];
                Array.Copy(Data, StartIndex, Line, 0, LineLength);
            }
            return Line;
        }

        /// <summary>
        /// Removes all leading and trailing whitespace in a line from a PDF file, and collapses all other found
        /// whitespace characters to a single space (0x020) character.
        /// </summary>
        /// <param name="DataLine">Data line from PDF file</param>
        /// <returns>Data line with leading and trailing whitespace removed, all other whitespace runs replaced with a single space character (0x20)</returns>
        public static byte[] TrimAndCollapseWhitespace(byte[] DataLine)
        {
            List<byte> NewLine = new List<byte>(DataLine.Length);

            bool inWhitespace = false;
            for (int i = 0; i < DataLine.Length; i++)
            {
                bool isWhitespace = PDF.Whitespace.Contains(DataLine[i]);
                if (!isWhitespace)
                {
                    if (inWhitespace && (NewLine.Count > 0)) // Skip leading whitespace
                        NewLine.Add(0x20);                   // Add a single space character to represent the whitespace we've found
                    NewLine.Add(DataLine[i]);
                    inWhitespace = false;
                }
                else
                    inWhitespace = true;
            }

            return NewLine.ToArray();
        }

        /// <summary>
        /// Translate three ASCII numeric digits into the character they represent in octal
        /// </summary>
        /// <param name="Data">Byte array containing three ASCII digits</param>
        /// <param name="StartingIndex">Starting index of the ASCII digits</param>
        /// <returns>The byte value represented by the digits interpreted as an octal character code</returns>
        public static byte TranslateOctal(byte[] Data, int StartingIndex)
        {
            char[] OctalVal = new char[3];
            OctalVal[0] = (char)Data[StartingIndex];
            OctalVal[1] = (char)Data[StartingIndex + 1];
            OctalVal[2] = (char)Data[StartingIndex + 2];
            return Convert.ToByte(new String(OctalVal, StartingIndex, 3), 8);
        }

        /// <summary>
        /// Attempt to interpred a given byte as an escaped character, returning the character represented
        /// </summary>
        /// <param name="EscapedChar">Escaped Character</param>
        /// <returns>Actual character, or 0 if this is the start of an octal escape sequence</returns>
        public static byte UnescapeCharacter(byte EscapedChar)
        {
            switch (EscapedChar)
            {
                case (byte)'n':
                    return 0x0A;
                case (byte)'r':
                    return 0x0D;
                case (byte)'t':
                    return 0x09;
                case (byte)'b':
                    return 0x08;
                case (byte)'f':
                    return 0x0FF;
            }

            if ((EscapedChar >= '0') && (EscapedChar <= '9'))
                return 0x0;

            return EscapedChar;
        }

        #endregion
    }


}
