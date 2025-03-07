using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OgrenciYonetim
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OgrenciNumarasiAttribute : Attribute
    {
        public static bool Dogrula(object entity)
        {
            Type dogrulamacakTur = entity.GetType();
            PropertyInfo[] dogrulanacakTurAlanlari = dogrulamacakTur.GetProperties(
                                                  BindingFlags.Public |
                                                  BindingFlags.Instance);
            foreach (PropertyInfo dogrulanacakTurAlani in dogrulanacakTurAlanlari)
            {
                object[] zorunluAlanOznitelikleri = dogrulanacakTurAlani.GetCustomAttributes(typeof(OgrenciNumarasiAttribute), true);
                if (zorunluAlanOznitelikleri.Length != 0)
                {
                    string alanDegeri = dogrulanacakTurAlani.GetValue(entity) as string;
                    if (!(!string.IsNullOrEmpty(alanDegeri) && Regex.IsMatch(alanDegeri, @"^\d{8}$")))
                    {
                        return false;
                    }
                }

            }

            return true;

        }
    }
}
