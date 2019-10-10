using System;
using System.Linq;

namespace PortableKnowledge.PDF
{
    public static class PKPDFExtensions
    {
        /// <summary>
        /// Does this byte represent a whitespace character in a PDF document?
        /// </summary>
        public static bool IsPDFWhitespace(this byte value)
        {
            return PDF.Whitespace.Contains(value);
        }
    }
}
