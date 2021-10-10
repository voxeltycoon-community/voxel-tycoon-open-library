using UnityEngine;
using UnityEngine.UI;
using VoxelTycoon.UI;
using VoxelTycoon.UI.Windows;

namespace VTOL.ModSettings
{
    class VTOLModSettingsWindow: RichWindow
    {
        public static VTOLModSettingsWindow ShowFor<T>(string title) where T : VTOLModSettingsWindowPage
        {
            VTOLModSettingsWindow vtolModSettingsWindow = UIManager.Current.CreateFrame<VTOLModSettingsWindow>(FrameAnchoring.Center);
            vtolModSettingsWindow.Initialize<T>(title);
            vtolModSettingsWindow.Show();
            return vtolModSettingsWindow;
        }

        protected override void InitializeFrame() {
            base.InitializeFrame();
            Draggable = false;
            Priority = 1001;
            Width = 500f;
        }

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
