using ReportPortal.Shared.Configuration;
using System;
using System.Linq;

namespace ReportPortal.Extensions.Skippable.Extensions
{
    internal static class ConfigurationExtensions
    {
        private static string[] EmptyStringArray { get; } = new string[0]; 

        public static string[] GetSkippableMimeTypes(this IConfiguration configuration)
        {
            return configuration
                .GetValues("Extensions:Skippable:MimeTypes", EmptyStringArray)?.ToArray() ?? EmptyStringArray;
        }
    }
}
