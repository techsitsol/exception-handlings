namespace ExceptionHandlings.Shared
{
    public class AppSettingsHelper
    {
        private static IConfiguration? _config;


        public static void AppSettingConfigure(IConfiguration config)
        {
            _config = config;
        }

        public static string? Setting(string Key)
        {
            return _config?.GetSection(Key).Value ?? null;
        }

    }
}
