using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BoustroSharp
{
    public class LineParagraph : BoustroParagraph
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string Type => "text";
        public string Text { get; set; }
        public List<AttributeSpan>? Spans { get; set; }
        public List<LineModifier>? Modifiers { get; set; }

        public LineParagraph(string text, List<AttributeSpan>? spans = null, List<LineModifier>? modifiers = null)
        {
            Text = text;
            Spans = spans is not null && spans.Any() ? spans : null;
            Modifiers = modifiers is not null && modifiers.Any() ? modifiers : null;
        }

        public override bool Equals(object? obj)
        {
            return obj is LineParagraph paragraph &&
                   Type == paragraph.Type &&
                   Text == paragraph.Text &&
                   EqualityComparer<List<AttributeSpan>?>.Default.Equals(Spans, paragraph.Spans) &&
                   EqualityComparer<List<LineModifier>?>.Default.Equals(Modifiers, paragraph.Modifiers);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Text, Spans, Modifiers);
        }

        public static bool operator ==(LineParagraph left, LineParagraph right)
        {
            if (left is null)
            {
                if (right is null) return true;
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(LineParagraph left, LineParagraph right) => !(left == right);
    }
}
