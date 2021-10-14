using VoxelTycoon;
using VoxelTycoon.Localization;

namespace ModSettingsExample
{
    /// <summary>
    /// Static class for text translations
    /// </summary>
    public static class S
    {
        public static string SliderInt => LazyManager<LocaleManager>.Current.Locale.GetString("mod_settings_example/slider_int");
        public static string BooleanValue => LazyManager<LocaleManager>.Current.Locale.GetString("mod_settings_example/boolean_value");
        public static string BooleanValueDescription => LazyManager<LocaleManager>.Current.Locale.GetString("mod_settings_example/boolean_value_desc");
    }
}