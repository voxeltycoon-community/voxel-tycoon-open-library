using JetBrains.Annotations;
using ModSettingsExample.UI;
using VoxelTycoon.Modding;
using VTOL.ModSettings;

namespace ModSettingsExample
{
    [UsedImplicitly]
    public class ModSettingsExampleMod: Mod
    {
        protected override void Initialize()
        {
        }

        protected override void OnGameStarted()
        {
            VTOLModSettingsWindowManager.Current.Register<ModSettingsExampleMod, SettingsWindowPage>("Mod settings example");
        }

        protected override void Deinitialize()
        {
        }
        
    }
}