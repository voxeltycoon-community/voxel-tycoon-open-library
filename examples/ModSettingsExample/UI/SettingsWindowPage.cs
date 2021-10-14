using VoxelTycoon;
using VoxelTycoon.Game.UI;
using VoxelTycoon.Localization;
using VTOL.ModSettings;

namespace ModSettingsExample.UI
{
    public class SettingsWindowPage : VTOLModSettingsWindowPage
    {
        protected override void InitializeInternal(SettingsControl settingsControl)
        {
            Settings settings = Settings.Current;
            Locale locale = LazyManager<LocaleManager>.Current.Locale;
            settingsControl.AddSlider(S.SliderInt, null,() => settings.SliderIntValue, delegate (float value)
            {
                settings.SliderIntValue = value;
            }, 0f, 100f, value => value.ToString("N0"));

            settingsControl.AddToggle(S.BooleanValue, S.BooleanValueDescription, settings.BooleanValue, delegate ()
            {
                settings.BooleanValue = true;
            }, delegate ()
            {
                settings.BooleanValue = false;
            });
        }

    }
}
