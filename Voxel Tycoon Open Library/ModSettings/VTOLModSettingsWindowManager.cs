using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VoxelTycoon;
using VoxelTycoon.Game.UI;

namespace VTOL.ModSettings
{
    [Harmony]
    [UsedImplicitly]
    class VTOLModSettingsWindowManager: LazyManager<VTOLModSettingsWindowManager>
    {
        private readonly Dictionary<string, ModData> _registered = new Dictionary<string, ModData>();

        private struct ModData
        {
            public string Title;
            public UnityAction Show;
        }

        private static void ShowWindow<T>(string title) where T : VTOLModSettingsWindowPage
        {
            VTOLModSettingsWindow.ShowFor<T>(title);
        }

        public void Register<T>(string modClassName, string title) where T : VTOLModSettingsWindowPage
        {
            ModData data = default;
            data.Title = title;
            data.Show = delegate () { ShowWindow<T>(title); };

            _registered.Add(modClassName, data);
        }

        public void Unregister(string modClassName)
        {
            _registered.Remove(modClassName);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameSettingsWindowPacksPage), "AddToggle")]
        // ReSharper disable once InconsistentNaming
        private static void AddToggle_pof(SettingsWindowDropdownItem __result, GameSettingsWindowPacksPage __instance, GameSettingsWindowPacksPageItem item)
        {
            if (!(__instance is GameSettingsWindowInGamePacksPage)) return;
            
            if (Current._registered.TryGetValue(item.Pack.Name, out var data))
            {
                Transform hint = __result.transform.Find("Row1/NameContainer/Hint");
                Transform settings = UnityEngine.Object.Instantiate<Transform>(hint, hint.parent);
                Text text = settings.GetComponent<Text>();
                text.text = "";
                text.font = R.Fonts.FontawesomeWebfont;
                text.color = Color.black;

                Button button = settings.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(data.Show);
                settings.gameObject.SetActive(true);
            }
        }

    }
}
