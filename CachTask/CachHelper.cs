using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CachTask
{
    public class CachHelper
    {
        private static ConnectionMultiplexer _redis => _lazyRedis.Value;
        private static Lazy<ConnectionMultiplexer> _lazyRedis;

        static CachHelper()
        {
            _lazyRedis = new Lazy<ConnectionMultiplexer>(() =>
            {
                ConfigurationOptions option = new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    EndPoints = { "localhost" }
                };

                return ConnectionMultiplexer.Connect(option);
            });
        }

        public static void SetInCachWithCustomSerilizable<T>(T value, string key)
        {
            var db = _redis.GetDatabase();
            var json = CustomSerealizable(value);
            var expiry = CheckTimeOut(value);
            db.StringSet(key, json, expiry);
        }

        public static void SetInCach<T>(T value, string key)
        {
            var db = _redis.GetDatabase();
            var json = Serealize(value);
            var expiry = CheckTimeOut(value);
            db.StringSet(key, json, expiry);
        }

        public static T GetFromCach<T>(string key)
        {
            var db = _redis.GetDatabase();
            var json = db.StringGet(key);
            T value = default(T);
            if(!json.IsNullOrEmpty)
            {
                value = Deserialize<T>(json);
            }
            return value;
        }

        public static T GetFromCachWithCustomSerilizable<T>(string key)
        {
            var db = _redis.GetDatabase();
            var json = db.StringGet(key);
            T value = default(T);
            if (!json.IsNullOrEmpty)
            {
                value = CustomDeserialize<T>(json);
            }
            return value;
        }

        private static string Serealize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }

        private static string CustomSerealizable<T>(T value)
        {
            var stringBuilder = new StringBuilder();

            var properties = value.GetType().GetProperties()
                .Where(prop => prop.CanRead).ToList();

            properties.ForEach(item => stringBuilder.Append($"{item.Name} : {GetObjectValue(value, item)},"));

            return stringBuilder.ToString();
        }

        private static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        private static T CustomDeserialize<T>(string json)
        {
            var properties = typeof(T).GetProperties()
                .Where(prop => prop.CanRead).ToList();

            var obj = Activator.CreateInstance<T>();
            var propValue = json.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            properties.ForEach(item => item.SetValue(obj, Convert.ChangeType(FindValue(item.Name, propValue), item.PropertyType)));
            return obj;

        }

        private static TimeSpan? CheckTimeOut<T>(T value)
        {
            Type type = typeof(T);
            TimeSpan? timeOut = null;
            var cachAttribute = (CachAttribute)type.GetCustomAttributes(false).FirstOrDefault(attr => attr.GetType() == typeof(CachAttribute));
            if(!ReferenceEquals(cachAttribute, null))
            {
                timeOut = cachAttribute.Lifetime;
            }

            return timeOut; 
        }

        private static object GetObjectValue<T>(T obj, PropertyInfo item)
        {
            var value = item.GetValue(obj);
            return value ?? "null";
        }

        private static object FindValue(string name, string[] propValue)
        {
            foreach (var item in propValue)
            {
                var tuple = item.Split(new string[] { " : " }, StringSplitOptions.RemoveEmptyEntries);
                if (string.Compare(tuple[0], name) == 0)
                {
                    if (string.Compare(tuple[1], "null") == 0)
                    {
                        return null;
                    }

                    return tuple[1];
                }
            }

            return null;
        }
    }
}
