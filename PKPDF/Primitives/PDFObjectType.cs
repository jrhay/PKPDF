using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    /// <summary>
    /// Type of PDF object
    /// </summary>
    /// <remarks>
    /// To ensure correct parsing, the objects should be attempted to be parsed in increasing value order
    /// </remarks>
    public enum PDFObjectType
    {
        Comment,

        Dictionary,

        Array,

        StringLiteral,

        ObjectDefinition,

        IndirectReference,

        Number,

        NullObject,

        Name,

        Stream
    }
}
