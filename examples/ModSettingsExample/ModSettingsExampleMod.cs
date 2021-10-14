using JetBrains.Annotations;
using ModSettingsExample.UI;
using VoxelTycoon.Modding;
using VTOL.ModSettings;

namespace ModSettingsExample
{
    [UsedImplicitly]
    public class ModSettingsExampleMod: Mod
    {
        protected override void OnGameStarted()
        {
            //this will register SettingsWindowPage as a settings page of this mod
            VTOLModSettingsWindowManager.Current.Register<ModSettingsExampleMod, SettingsWindowPage>("Mod settings example");
        }

    }
}