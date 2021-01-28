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
                   this == span;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value, Start, End);
        }

        public static bool operator ==(AttributeSpan left, AttributeSpan right)
        {
            return left.Type == right.Type &&
                EqualityComparer<JsonElement?>.Default.Equals(left.Value, right.Value) &&
                left.Start == right.Start &&
                left.End == right.End;
        }

        public static bool operator !=(AttributeSpan left, AttributeSpan right) => !(left == right);

    }
}
