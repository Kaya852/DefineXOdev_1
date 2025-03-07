using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrenciYonetim
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TabloAttribute : Attribute
    {
        public string Ad { get; }

        public TabloAttribute(string ad)
        {
            Ad = ad;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class EssizAttribute : Attribute { }
}
