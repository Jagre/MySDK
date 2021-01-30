using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MySDK.Serialization
{
    public static class JsonSerializeExtention
    {
        public static JsonSerializerSettings DefaultSetting = new JsonSerializerSettings
        {
            Converters = new JsonConverter[] { new StringEnumConverter() },
            ContractResolver = new CamelCasePropertyNamesContractResolver(), //DefaultContractResolver(),
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            DateFormatString = "yyyy/MM/dd HH:mm:ss",
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            //DefaultValueHandling = DefaultValueHandling.Include, (Include is default)
            //NullValueHandling = NullValueHandling.Include, (Include is default)
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple (Simple is default)
        };

        public static string ToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value, DefaultSetting);
        }

        public static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value, DefaultSetting);
        }
    }
}
