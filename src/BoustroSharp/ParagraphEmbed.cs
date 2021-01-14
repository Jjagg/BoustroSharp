using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BoustroSharp
{
    public class ParagraphEmbed : BoustroParagraph
    {
        public string Type { get; set; }
        public JsonElement? Value { get; set; }

        public ParagraphEmbed(string type, JsonElement? value = null)
        {
            Type = type;
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is ParagraphEmbed embed &&
                   Type == embed.Type &&
                   EqualityComparer<JsonElement?>.Default.Equals(Value, embed.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value);
        }
    }
}
