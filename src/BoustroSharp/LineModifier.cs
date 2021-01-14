using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BoustroSharp
{
    public class LineModifier
    {
        public string Type { get; set; }
        public JsonElement? Value { get; set; }

        public LineModifier(string type, JsonElement? value = null)
        {
            Type = type;
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is LineModifier modifier &&
                   Type == modifier.Type &&
                   EqualityComparer<JsonElement?>.Default.Equals(Value, modifier.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value);
        }
    }
}
