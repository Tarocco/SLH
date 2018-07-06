﻿using Newtonsoft.Json;
using OpenMetaverse;
using System;

namespace LibSLH
{
    public class SLHConverter : JsonConverter
    {
        public override bool CanWrite { get { return true; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Simulator)
                writer.WriteValue(((Simulator)value).Handle);
            else
                writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Integer)
                return Convert.ToInt32(reader.Value); // convert to Int32 instead of Int64
            if (reader.TokenType == JsonToken.String)
            {
                {
                    if (UUID.TryParse((string)reader.Value, out UUID result))
                        return result;
                }
                {
                    if (Vector3.TryParse((string)reader.Value, out Vector3 result))
                        return result;
                }
            }
            return serializer.Deserialize(reader);
        }

        public override bool CanConvert(Type objectType)
        {
            var non_converting =
                // Custom types
                objectType == typeof(Avatar) ||
                objectType.IsSubclassOf(typeof(GridClient)) ||
                objectType == typeof(AgentManager);

            var converting =
                objectType == typeof(Int32) ||
                objectType == typeof(Int64) ||
                objectType == typeof(Single) ||
                objectType == typeof(Double) ||
                objectType == typeof(int) ||
                // Custom types
                objectType == typeof(UUID) ||
                objectType == typeof(Vector3) ||
                objectType.IsSubclassOf(typeof(Primitive)) ||
                objectType == typeof(Simulator) ||
                // Default
                objectType == typeof(object);

            return !non_converting && converting;
        }
    }
}