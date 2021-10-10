using VoxelTycoon.Game.UI;

namespace VTOL.ModSettings
{
    public abstract class VTOLModSettingsWindowPage : GameSettingsWindowPage
    {
        protected override bool HasFooter { get => false; }

        public void Initialize()
        {
            base.InitializePage();
        }
    }
}
