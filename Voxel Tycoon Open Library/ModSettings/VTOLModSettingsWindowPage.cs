using VoxelTycoon.Game.UI;

namespace VTOL.ModSettings
{
    /// <summary>
    /// Base class for settings page
    /// </summary>
    public abstract class VTOLModSettingsWindowPage : GameSettingsWindowPage
    {
        protected override bool HasFooter => false;

        public void Initialize()
        {
            InitializePage();
        }
    }
}
