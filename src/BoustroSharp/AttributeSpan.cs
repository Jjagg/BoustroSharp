using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BoustroSharp
{
    public struct AttributeSpan
    {
        public string Type { get; set; }
        public JsonElement? Value { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public AttributeSpan(string type, JsonElement? value, int start, int end)
        {
            Type = type;
            Value = value;
            Start = start;
            End = end;
        }

        public override bool Equals(object? obj)
        {
            return obj is AttributeSpan span &&
                   Type == span.Type &&
                   EqualityComparer<JsonElement?>.Default.Equals(Value, span.Value) &&
                   Start == span.Start &&
                   End == span.End;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value, Start, End);
        }
    }
}
