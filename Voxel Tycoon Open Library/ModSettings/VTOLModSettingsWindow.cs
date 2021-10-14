using UnityEngine;
using UnityEngine.UI;
using VoxelTycoon.UI;
using VoxelTycoon.UI.Windows;

namespace VTOL.ModSettings
{
    /// <summary>
    /// Class for window with settings, which will be displayed after clicking at settings button
    /// </summary>
    public class VTOLModSettingsWindow: RichWindow
    {
        /// <summary>
        /// Creates and displays settings window with specified page
        /// </summary>
        /// <param name="title">Window title</param>
        /// <typeparam name="T">Type of settings page - instance will be created</typeparam>
        /// <returns></returns>
        public static VTOLModSettingsWindow ShowFor<T>(string title) where T : VTOLModSettingsWindowPage
        {
            VTOLModSettingsWindow vtolModSettingsWindow = UIManager.Current.CreateFrame<VTOLModSettingsWindow>(FrameAnchoring.Center);
            vtolModSettingsWindow.Initialize<T>(title);
            vtolModSettingsWindow.Show();
            return vtolModSettingsWindow;
        }

        /// <summary>
        /// Will be called on windows frame initialization
        /// </summary>
        protected override void InitializeFrame() {
            base.InitializeFrame();
            Draggable = false;
            Priority = 1001;
            Width = 500f;
        }

        /// <summary>
        /// Will be called before first window show
        /// </summary>
        protected override void Prepare()
        {
            base.Prepare();
            Overlay.ShowFor(this, false);
        }

        private T CreateContent<T>() where T : VTOLModSettingsWindowPage
        {
            T t = new GameObject("ModSettingsTab").AddComponent<T>();
            t.gameObject.AddComponent<VerticalLayoutGroup>();
            t.transform.SetParent(base.ContentContainer);
            return t;
        }

        private void Initialize<T>(string title) where T: VTOLModSettingsWindowPage
        {
            Title = title;
            CreateContent<T>().Initialize();
        }
    }
}
