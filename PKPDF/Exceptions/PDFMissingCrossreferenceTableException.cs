using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF.Exceptions
{
    /// <summary>
    /// PDF data stream is missing a cross reference table; can not correctly parse
    /// </summary>
    class PDFMissingCrossreferenceTableException : Exception
    {
        public PDFMissingCrossreferenceTableException() : base()
        {
        }

        public PDFMissingCrossreferenceTableException(string message) : base(message)
        {
        }

        public PDFMissingCrossreferenceTableException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
