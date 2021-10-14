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
//    [Harmony]
    public class VTOLModSettingsWindowManager: LazyManager<VTOLModSettingsWindowManager>
    {
        const string HarmonyID = "vtol.modsettings.patch";
        private readonly Dictionary<string, ModData> _registered = new Dictionary<string, ModData>();
        private Harmony _harmony;

        private struct ModData
        {
            public string Title;
            public UnityAction Show;
        }

        /// <summary>
        /// Will register specified window settings page type to the specified mod class
        /// </summary>
        /// <param name="title">Window title</param>
        /// <typeparam name="TMod">type of mod class (must have a unique namespace among other mods)</typeparam>
        /// <typeparam name="TPage">type of window's settings page, that will be shown for settings changes</typeparam>
        [UsedImplicitly]
        public void Register<TMod, TPage>(string title) where TPage : VTOLModSettingsWindowPage where TMod : Mod
        {
            FileLog.Log("Register");
            ModData data = default;
            data.Title = title;
            data.Show = delegate () { ShowWindow<TPage>(title); };

            _registered.Add(GetModClassNamespace<TMod>(), data);
        }

        /// <summary>
        /// Will unregister a mod class 
        /// </summary>
        /// <typeparam name="TMod">Mod class</typeparam>
        [UsedImplicitly]
        public void Unregister<TMod>()
        {
            _registered.Remove(GetModClassNamespace<TMod>());
        }

        protected override void OnInitialize()
        {
            _harmony = new Harmony(HarmonyID);
            MethodInfo addToggleOriginal = typeof(GameSettingsWindowPacksPage).GetMethod("AddToggle", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo addTogglePostfix = GetType().GetMethod("AddToggle_pof", BindingFlags.NonPublic | BindingFlags.Static);
            _harmony.Patch(addToggleOriginal, postfix: new HarmonyMethod(addTogglePostfix));
        }

        protected override void OnDeinitialize()
        {
            _harmony.UnpatchAll(HarmonyID);
            _harmony = null;
        }

        private static string GetModClassNamespace<TMod>()
        {
            string nameSpace = typeof(TMod).Namespace;
            if (nameSpace == null)
                throw new ArgumentException("Cannot determine mod's namespace");
            return nameSpace;
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
