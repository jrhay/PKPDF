using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF.Objects
{
    class PDFDictionary : Dictionary<IPDFObject, IPDFObject>, IPDFObject
    {
        public PDFDictionary() : base()
        {
        }

        /// <summary>
        /// Attempt to parse the given data stream, returning an indicator of parse progress
        /// Returned value will be:
        /// (negative value) - Data can not be parsed as this type of object
        /// 0                - Data stream parsing is successful, but more data is needed before a complete object can be created
        /// (positive value) - Byte count into data (1-based) where parsing produced a valid object
        /// </summary>
        /// <param name="Data">Raw byte stream to parse</param>
        /// <returns>Parsing success indicator. If 0, parsing was successful and object contains parsed data. If negative, parsing failed.</returns>
        public static int TryParse(byte[] Data, int StartingIndex, out IPDFObject indirectObject)
        {
            string Declaration = PDF.GetTokenString(Data, StartingIndex);
            if (!String.IsNullOrEmpty(Declaration) && (Declaration == "<<"))
            {

            }

            // Unable to parse
            indirectObject = null;
            return -1;
        }
    }
}
