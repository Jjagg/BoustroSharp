using System;
using System.Collections.Generic;
using System.Linq;

namespace BoustroSharp
{
    public class BoustroDocument
    {
        public List<BoustroParagraph> Paragraphs { get; set; }

        public BoustroDocument(List<BoustroParagraph> paragraphs)
        {
            if (paragraphs is null)
            {
                throw new ArgumentNullException(nameof(paragraphs));
            }

            Paragraphs = paragraphs;
        }

        public override bool Equals(object? obj)
        {
            return obj is BoustroDocument document && Paragraphs.SequenceEqual(document.Paragraphs);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Paragraphs);
        }

        public static bool operator ==(BoustroDocument left, BoustroDocument right)
        {
            if (left is null)
            {
                if (right is null) return true;
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(BoustroDocument left, BoustroDocument right) => !(left == right);
    }
}
