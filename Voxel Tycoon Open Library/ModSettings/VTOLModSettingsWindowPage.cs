using VoxelTycoon.Game.UI;

namespace VTOL.ModSettings
{
    public abstract class VTOLModSettingsWindowPage : GameSettingsWindowPage
    {
        protected override bool HasFooter => false;

        public void Initialize()
        {
            InitializePage();
        }
    }
}
