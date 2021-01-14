using System.Text.Json.Serialization;

namespace BoustroSharp
{
    [JsonConverter(typeof(BoustroParagraphConverter))]
    public abstract class BoustroParagraph
    {
    }
}
