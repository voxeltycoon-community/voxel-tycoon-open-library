using UnityEngine;
using VoxelTycoon;
using VoxelTycoon.Game.UI;
using VoxelTycoon.Localization;
using VTOL.ModSettings;

namespace ModSettingsExample.UI
{
    internal class SettingsWindowPage : VTOLModSettingsWindowPage
    {
        /// <summary>
        /// Method will be called when settings page is initialized
        /// Used for adding settings controls elements (using prefab methods Add.... of SettingsControl)
        /// </summary>
        /// <param name="settingsControl"></param>
        protected override void InitializeInternal(SettingsControl settingsControl)
        {
            Settings settings = Settings.Current;
            Locale locale = LazyManager<LocaleManager>.Current.Locale;
            
            //this will add a slider
            settingsControl.AddSlider(S.SliderInt, null,() => settings.SliderIntValue, delegate (float value)
            {
                settings.SliderIntValue = Mathf.RoundToInt(value);
            }, 0f, 100f, value => value.ToString("N0"));

            //this will add a dropdown with On/Off options
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
