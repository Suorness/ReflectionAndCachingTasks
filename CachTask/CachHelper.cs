using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace CachTask
{
    public class CachHelper
    {
        private ConnectionMultiplexer _redis => _lazyRedis.Value;
        private Lazy<ConnectionMultiplexer> _lazyRedis;

        public CachHelper()
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

        public void SetInCach(string value, string key)
        {
            var db = _redis.GetDatabase();
            db.StringSet(key, value);
        }

        public string GetFromCach(string key)
        {
            var db = _redis.GetDatabase();
            return db.StringGet(key);
        }

    }

    public class CachAttribute : Attribute
    {
        public DateTime Lifetime { get; set; }
    }
}
