using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF.Exceptions
{
    /// <summary>
    /// PDF data stream is missing an object catalog; can not correctly parse
    /// </summary>
    class PDFMissingCatalogException : Exception
    {
        public PDFMissingCatalogException() : base()
        {
        }

        public PDFMissingCatalogException(string message) : base(message)
        {
        }

        public PDFMissingCatalogException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
