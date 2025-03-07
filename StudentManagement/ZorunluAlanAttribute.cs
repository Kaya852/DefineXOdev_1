using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OgrenciYonetim
{
    
    

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ZorunluAlanAttribute : Attribute
    {
        public static bool Dogrula(object dogrulanacakEntity)
        {
            if (dogrulanacakEntity == null)
                return false;

            Type dogrulamacakTur = dogrulanacakEntity.GetType();
            PropertyInfo[] dogrulanacakTurAlanlari = dogrulamacakTur.GetProperties(
                                                    BindingFlags.Public |
                                                    BindingFlags.Instance);
            foreach (PropertyInfo dogrulanacakTurAlani in dogrulanacakTurAlanlari)
            {
                object[] zorunluAlanOznitelikleri = dogrulanacakTurAlani.GetCustomAttributes(typeof(ZorunluAlanAttribute), true);
                if (zorunluAlanOznitelikleri.Length != 0)
                {
                    object alanDegeri = dogrulanacakTurAlani.GetValue(dogrulanacakEntity);
                    if (alanDegeri == null || string.IsNullOrEmpty(alanDegeri.ToString()))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
