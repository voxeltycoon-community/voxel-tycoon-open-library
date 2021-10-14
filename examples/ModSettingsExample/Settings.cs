using System;
using Newtonsoft.Json;
using UnityEngine;
using VTOL.ModSettings;

//This namespace must be the same as the namespace of main mod class (used for automatic mod name and mod file path detection)
namespace ModSettingsExample
{
    [JsonObject(MemberSerialization.OptOut)]  //this attribute cause that all public properties will be serialized/deserialized
    internal class Settings : VTOLModSettings<Settings>
    {
        private int _sliderIntValue = 1;
        private bool _booleanValue = true;

        /// <summary>
        /// Name of the mod's directory in the Local folder (from which it will be uploaded to the Steam)
        /// </summary>
        public override string ModPackName => "ModSettingsExample";

        public int SliderIntValue { 
            get => _sliderIntValue;
            set =>  SetProperty(Mathf.RoundToInt(Math.Max(Math.Min(value, 100f), 0f)), ref _sliderIntValue);  //using SetProperty method will properly raise settings changed events and save modified settings to the file
        }

        public bool BooleanValue { 
            get => _booleanValue;
            set =>  SetProperty(value, ref _booleanValue);
        }
    }
}
