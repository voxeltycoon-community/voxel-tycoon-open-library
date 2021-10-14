using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VoxelTycoon;
using VoxelTycoon.Game.UI;
using VoxelTycoon.Modding;

namespace VTOL.ModSettings
{
    /// <summary>
    /// Manager class for managing mod settings
    /// </summary>
    [Harmony]
    public class VTOLModSettingsWindowManager: LazyManager<VTOLModSettingsWindowManager>
    {
        private readonly Dictionary<string, ModData> registered = new Dictionary<string, ModData>();

        private struct ModData
        {
            public string Title;
            public UnityAction Show;
        }

        /// <summary>
        /// Will register specified window settings page type to the specified mod class
        /// </summary>
        /// <param name="modPackName">Mod's pack name (Pack.Name), it is a name of directory where mod is placed (for local mods), or where was placed at the moment of uploading to the Steam</param>
        /// <param name="title">Window title</param>
        /// <typeparam name="TPage">type of window's settings page, that will be shown for settings changes</typeparam>
        [UsedImplicitly]
        public void Register<TPage>([NotNull] string modPackName, string title) where TPage : VTOLModSettingsWindowPage
        {
            if (string.IsNullOrEmpty(modPackName)) 
                throw new ArgumentNullException(nameof(modPackName));
            ModData data = default;
            data.Title = title;
            data.Show = delegate () { ShowWindow<TPage>(title); };

            registered.Add(modPackName, data);
        }

        /// <summary>
        /// Will unregister a mod class 
        /// </summary>
        /// <param name="modPackName">Mod's pack name (Pack.Name), it is a name of directory where mod is placed (for local mods), or where was placed at the moment of uploading to the Steam</param>
        [UsedImplicitly]
        public void Unregister([NotNull] string modPackName)
        {
            if (string.IsNullOrEmpty(modPackName)) 
                throw new ArgumentNullException(nameof(modPackName));
            registered.Remove(modPackName);
        }

        private static void ShowWindow<T>(string title) where T : VTOLModSettingsWindowPage
        {
            VTOLModSettingsWindow.ShowFor<T>(title);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameSettingsWindowPacksPage), "AddToggle")]
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void AddToggle_pof(SettingsWindowDropdownItem __result, GameSettingsWindowPacksPage __instance, GameSettingsWindowPacksPageItem item)
        {
            if (!(__instance is GameSettingsWindowInGamePacksPage)) return;
            
            if (Current.registered.TryGetValue(item.Pack.Name, out var data))
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
