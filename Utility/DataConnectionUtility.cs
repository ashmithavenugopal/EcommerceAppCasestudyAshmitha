using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplicationCasestudy.Utility
{
        internal static class DataConnectionUtility
        {
            private static IConfiguration iconfiguration;
            static DataConnectionUtility()
            {
                GetAppSettingsFile();
            }

            private static void GetAppSettingsFile()
            {
                var builder = new ConfigurationBuilder().SetBasePath
                    (Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json");
                iconfiguration = builder.Build();
            }

            public static string GetConnectionString()
            {
                return iconfiguration.GetConnectionString("LocalConnectionString");
            }
        }  
}
