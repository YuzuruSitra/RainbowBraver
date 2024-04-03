using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace CI.QuickSave.Core.Converters
{
    public class Int2Converter : JsonConverter{
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int2);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var val = JObject.Load(reader);
            return new int2((int) val["x"], (int) val["y"]);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var val = (int2) value;
            serializer.Serialize(writer, new {val.x, val.y});
        }
    }
}
