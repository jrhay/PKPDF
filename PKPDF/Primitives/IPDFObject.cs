using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    /// <summary>
    /// Interface for a generic object in a PDF document
    /// </summary>
    public interface IPDFObject
    {
        /// <summary>
        /// Type of object
        /// </summary>
        PDFObjectType Type { get; }

        /// <summary>
        /// Returns a human-readable description of the PDF object.  May consist of multiple lines of text.
        /// </summary>
        string Description { get; }

        // In addition to these methods and properties, all IPDFObject implementations should implement the following static method:
        //
        // <summary>
        // Attempt to parse the given data stream, returning an indicator of parse progress
        // </summary>
        // <param name="StartingToken">The token immediately preceeding the starting index in Data stream</param>
        // <param name="Data">Raw byte stream to parse</param>
        // <param name="StartingIndex">0-based starting index into Data where StartingToken appears</param>
        // <param name="EndingIndex">Index into data stream where parsing ended (either successfully or unsuccessfully)</param>
        // <returns>Object parsed from data stream, or NULL if unable to parse. If NULL and EndingIndex is equal to Data.Length, parsing may be successful with more data</returns>
        //
        //public static IPDFObject TryParse(string StartingToken, byte[] Data, int StartingIndex, out int EndingIndex)
    }
}
