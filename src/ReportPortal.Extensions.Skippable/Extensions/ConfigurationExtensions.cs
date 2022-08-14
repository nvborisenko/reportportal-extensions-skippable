using ReportPortal.Shared.Configuration;

namespace ReportPortal.Extensions.Skippable.Extensions
{
    internal static class ConfigurationExtensions
    {
        private const char ArrayItemsDelimiter = ';';

        public static string[] GetSkippableMimeTypes(this IConfiguration configuration)
        {
            return configuration
                .GetValue("Extensions:Skippable:MimeTypes", string.Empty)
                .Split(ArrayItemsDelimiter);
        }
    }
}
