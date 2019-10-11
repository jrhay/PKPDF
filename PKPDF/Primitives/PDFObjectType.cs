using System;
using System.Collections.Generic;
using System.Text;

namespace PortableKnowledge.PDF
{
    /// <summary>
    /// Type of PDF object
    /// </summary>
    public enum PDFObjectType
    {
        NullObject,
        
        Name,
        
        StringLiteral,

        Dictionary,
        
        IndirectDefinition,

        IndirectReference
    }
}
