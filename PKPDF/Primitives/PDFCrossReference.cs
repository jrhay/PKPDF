using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PortableKnowledge.PDF
{
    /// <summary>
    /// A single PDF file cross reference section (part of a cross reference table)
    /// </summary>
    public class PDFCrossReference
    {
        private int[] ObjectOffsets;

        private int[] ObjectGenerations;

        private List<PDFCrossReference> Subsections = new List<PDFCrossReference>();

        /// <summary>
        /// The starting object number in this section
        /// </summary>
        public int StartingObject { get; private set; }

        /// <summary>
        /// The total number of objects contained in this section
        /// </summary>
        public int NumObjects => (ObjectOffsets == null) ? 0 : ObjectOffsets.Length;

        /// <summary>
        /// Does this cross reference section contain information for the given object?
        /// </summary>
        /// <param name="ObjectNumber">Object number of interest</param>
        /// <returns>TRUE if this cross reference section contains information about the given object, FALSE otherwise</returns>
        public bool ContainsObject(int ObjectNumber)
        {
            if (NumObjects == 0)
                return false;

            return ((ObjectNumber >= StartingObject) && (ObjectNumber < StartingObject + NumObjects));
        }

        /// <summary>
        /// Does this cross reference section contain information for the given object?
        /// </summary>
        /// <param name="indirectObject">Indirect object reference to the object of interest</param>
        /// <param name="MatchGeneration">Should the generation number for the object be matched as well? (default: false)</param>
        /// <returns>TRUE if this cross reference section contains information about the given object, FALSE otherwise</returns>
        public bool ContainsObject(PDFIndirectObject indirectObject, bool MatchGeneration = false)
        {
            return ContainsObject(indirectObject.ObjectNumber) &&
                (!MatchGeneration || (GenerationForObject(indirectObject.ObjectNumber) == indirectObject.GenerationNumber));
        }

        /// <summary>
        /// Does this cross reference table indicate the specified object is deleted?
        /// </summary>
        /// <param name="ObjectNumber">Object to inspect</param>
        /// <returns>TRUE if cross reference table contains an entry for the object indicating the object is deleted, FLASE otherwise</returns>
        public bool IsDeleted(int ObjectNumber)
        {
            return ContainsObject(ObjectNumber) && (OffsetForObject(ObjectNumber) <= 0);
        }

        /// <summary>
        /// Return the offset into the PDF file where an object with the given object number is defined
        /// </summary>
        /// <param name="ObjectNumber">Object number to retrieve</param>
        /// <returns>Offset into PDF file of object definition, or 0 if object is not found in this cross reference section or represents a deleted object</returns>
        public int OffsetForObject(int ObjectNumber)
        {
            if (this.ContainsObject(ObjectNumber))
                return ObjectOffsets[ObjectNumber - StartingObject];
            else
            {
                // Search subsections for object
                foreach (PDFCrossReference section in this.Subsections)
                    if (section.ContainsObject(ObjectNumber))
                        return section.OffsetForObject(ObjectNumber);

                return 0;
            }
        }

        /// <summary>
        /// Return the offset into the PDF file where an object with the given object number is defined
        /// </summary>
        /// <param name="indirectObject">Indirect object reference to the object of interest</param>
        /// <param name="MatchGeneration">Should the generation number for the object be matched as well? (default: false)</param>
        /// <returns>Offset into PDF file of object definition, or 0 if object is not found in this cross reference section or represents a deleted object</returns>
        public int OffsetForObject(PDFIndirectObject indirectObject, bool MatchGeneration = false)
        {
            if (this.ContainsObject(indirectObject, MatchGeneration))
                return ObjectOffsets[indirectObject.ObjectNumber - StartingObject];
            else
            {
                // Search subsections for object
                foreach (PDFCrossReference section in this.Subsections)
                    if (section.ContainsObject(indirectObject, MatchGeneration))
                        return section.OffsetForObject(indirectObject);

                return 0;
            }
        }

        /// <summary>
        /// Return the generation number for the specified object number that is defined by this cross reference section
        /// </summary>
        /// <param name="ObjectNumber">Object number to retrieve</param>
        /// <returns>Generation number for the object defined by this cross reference section, or 0 if object is not found in this cross reference section</returns>
        public int GenerationForObject(int ObjectNumber)
        {
            if (this.ContainsObject(ObjectNumber))
                return ObjectGenerations[ObjectNumber - StartingObject];
            else
            {
                // Search subsections for object
                foreach (PDFCrossReference section in this.Subsections)
                    if (section.ContainsObject(ObjectNumber))
                        return section.GenerationForObject(ObjectNumber);

                return 0;
            }
        }

        internal PDFCrossReference() { }
        internal PDFCrossReference(int StartObject, int NumObjects)
        {
            StartingObject = StartObject;
            ObjectOffsets = new int[NumObjects];
            ObjectGenerations = new int[NumObjects];
        }


        /// <summary>
        /// Attempt to read a PDF cross reference subsection from raw data starting at a given offset
        /// </summary>
        /// <param name="Data">PDF File Data</param>
        /// <param name="StartIndex">Index into Data where cross reference subsection should start</param>
        /// <param name="EndIndex">Index into Data where parsing of the cross reference subsection stopped (successfully or not)</param>
        /// <returns>Instance of PDF Cross Reference Subsection on success, NULL on failure to parse</returns>
        private static PDFCrossReference ReadCrossReferenceSubsection(byte[] Data, int StartIndex, out int EndIndex)
        {
            string Declaration = Encoding.UTF8.GetString(PDF.ExtractPDFLine(Data, StartIndex, out EndIndex));
            string[] Definitions = Declaration.Split(' ');
            if (Definitions.Length == 2)
            {
                int StartObject = PDFNumber.TryParse(Definitions[0], -1);
                int NumObjects = PDFNumber.TryParse(Definitions[1], -1);
                if ((StartObject >= 0) && (NumObjects >= 0))
                {
                    PDFCrossReference Subsection = new PDFCrossReference(StartObject, NumObjects);
                    for (int i = 0; i < NumObjects; i++)
                    {
                        // Parse cross reference table entries
                        string Next = Encoding.UTF8.GetString(PDF.ExtractPDFLine(Data, EndIndex, out EndIndex));
                        Match objMatch = Regex.Match(Next, @"(\d\d\d\d\d\d\d\d\d\d) (\d\d\d\d\d) ([n|f])");
                        if (objMatch.Success)
                        {
                            int Generation = int.Parse(objMatch.Groups[2].Value);
                            char type = objMatch.Groups[3].Value[0];
                            if (type == 'n')
                            {
                                // Add active object to table
                                int Offset = int.Parse(objMatch.Groups[1].Value);
                                Subsection.ObjectOffsets[i] = Offset;
                                Subsection.ObjectGenerations[i] = Generation;
                            }
                            else if (type == 'f')
                            {
                                // Add deleted object to table
                                Subsection.ObjectOffsets[i] = 0;
                                Subsection.ObjectGenerations[i] = Generation;
                            }
                            else
                                return null; // Should never get here; invalid cross reference entry
                        }
                        else
                            return null; // Invalid Cross Reference Section
                    }
                    return Subsection;
                }
            }
            return null;
        }

        /// <summary>
        /// Attempt to read a PDF cross reference section from raw data starting at a given offset
        /// </summary>
        /// <param name="Data">PDF File Data</param>
        /// <param name="StartIndex">Index into Data where cross reference section should start</param>
        /// <param name="EndIndex">Index into Data where parsing of the cross reference section stopped (successfully or not)</param>
        /// <returns>Instance of PDF Cross Reference Section on success, NULL on failure to parse</returns>
        public static PDFCrossReference ReadCrossReference(byte[] Data, int StartIndex, out int EndIndex)
        {
            PDFCrossReference section = null;
            string Declaration = Encoding.UTF8.GetString(PDF.ExtractPDFLine(Data, StartIndex + 18, out EndIndex)).Trim();
            if ("xref".Equals(Declaration))
            {
                section = ReadCrossReferenceSubsection(Data, EndIndex, out EndIndex);
                if (section != null)
                {
                    PDFCrossReference subSection;
                    do
                    {
                        StartIndex = EndIndex;
                        subSection = ReadCrossReferenceSubsection(Data, StartIndex, out EndIndex);
                        if (subSection != null)
                            section.Subsections.Add(subSection);
                        else
                            EndIndex = StartIndex;
                    } while (subSection != null);
                }
            }
            return section;
        }

        /// <summary>
        /// Attempt to read a PDF cross reference section from raw data starting at a given offset
        /// </summary>
        /// <param name="Data">PDF File Data</param>
        /// <param name="StartIndex">Index into Data where cross reference section should start</param>
        /// <param name="EndIndex">Index into Data where parsing of the cross reference section stopped (successfully or not)</param>
        /// <returns>Instance of PDF Cross Reference Section on success, NULL on failure to parse</returns>
        public static PDFCrossReference ReadCrossReference(byte[] Data, PDFNumber StartIndex, out int EndIndex)
        {
            return ReadCrossReference(Data, (int)StartIndex.Value, out EndIndex);
        }
    }
}
