using JetBrains.Annotations;
using ModSettingsExample.UI;
using VoxelTycoon;
using VoxelTycoon.Modding;
using VoxelTycoon.Notifications;
using VTOL.ModSettings;

namespace ModSettingsExample
{
    [UsedImplicitly]
    public class ModSettingsExampleMod: Mod
    {
        protected override void OnGameStarted()
        {
            //this will register SettingsWindowPage as a settings page of this mod
            VTOLModSettingsWindowManager.Current.Register<SettingsWindowPage>( VTOLModSettings<Settings>.Current.ModPackName,"Mod settings example");
            VTOLModSettings<Settings>.Current.SettingsChanged += SettingsChanged;
        }

        private void SettingsChanged()
        {
            Manager<NotificationManager>.Current?.Push( S.SettingsChangedTitle, S.SettingsChangedDesc.Format(VTOLModSettings<Settings>.Current.SliderIntValue), null);
        }

    }
}