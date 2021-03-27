using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Dfe.Wizard.Prototype.Application
{
    public static class SessionExtension
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None
        };

        public static void Set<T>(this ISession session, string key, T value)
        {
            var objString = JsonConvert.SerializeObject(value, Formatting.Indented, settings);
            session.SetString(key, objString);
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value, settings);
        }
    }
}
