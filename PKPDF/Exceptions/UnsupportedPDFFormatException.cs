using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF.Exceptions
{
    /// <summary>
    /// Unsupported version of PDF standard
    /// </summary>
    class UnsupportedPDFFormatException : Exception
    {
        /// <summary>
        /// PDF Version number, or 0 if unknown version/extension
        /// </summary>
        public float Version { get; } = 0.0f;

        public UnsupportedPDFFormatException() : base()
        {
        }

        public UnsupportedPDFFormatException(float pdfVersion) : base()
        {
            Version = pdfVersion;
        }

        public UnsupportedPDFFormatException(float pdfVersion, string message) : base(message)
        {
            Version = pdfVersion;
        }

        public UnsupportedPDFFormatException(string message) : base(message)
        {
        }

        public UnsupportedPDFFormatException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
