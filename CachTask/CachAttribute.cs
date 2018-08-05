using System;

namespace CachTask
{
    public class CachAttribute : Attribute
    {
        public CachAttribute()
        {
        }

        public CachAttribute(int hour, int min, int sec)
        {
            Lifetime = new TimeSpan(hour, min, sec);
        }

        public TimeSpan Lifetime { get; set; }
    }
}
