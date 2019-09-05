using Newtonsoft.Json;
using System;

namespace WebsiteTemplate.Core.Tools
{
    public class GoogleChartDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(DateTime))
                return true;

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                return;

            DateTime d = (DateTime)value;

            string val = string.Format("\"Date({0},{1},{2},{3},{4},{5})\"", d.Year, d.Month - 1, d.Day, d.Hour, d.Minute, d.Second);

            writer.WriteRawValue(val);
        }
    }
}
