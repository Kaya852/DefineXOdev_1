using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrenciYonetim
{
    internal class ConfigureManager
    {
        public static string getSQLiteConnectionString()
        {
            var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

            string result = configuration.GetValue<string>("AppSettings:SQLiteConnectionString");
            return result;

        }

        public static string getXMLPath()
        {
            var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

            string result = configuration.GetValue<string>("AppSettings:XMlpath");
            return result;
        }


    }
}
