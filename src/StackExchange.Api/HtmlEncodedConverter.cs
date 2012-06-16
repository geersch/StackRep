using System;
using System.Net;
using Newtonsoft.Json;

namespace StackExchange.Api
{
    public class HtmlEncodedConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception(String.Format("Unexpected token parsing date. Expected string, got {0}.", reader.TokenType));
            }

            var value = HttpUtility.HtmlDecode((string) reader.Value);

            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}