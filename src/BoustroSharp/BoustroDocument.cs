using System;
using System.Collections.Generic;
using System.Linq;

namespace BoustroSharp
{
    public class BoustroDocument
    {
        public List<BoustroParagraph> Paragraphs { get; set; }

        public BoustroDocument(IEnumerable<BoustroParagraph> paragraphs)
        {
            if (paragraphs is null)
            {
                throw new ArgumentNullException(nameof(paragraphs));
            }

            Paragraphs = new List<BoustroParagraph>(paragraphs);
        }

        public override bool Equals(object? obj)
        {
            return obj is BoustroDocument document && Paragraphs.SequenceEqual(document.Paragraphs);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Paragraphs);
        }
    }
}
