using System;
using Newtonsoft.Json;
using UnityEngine;
using VTOL.ModSettings;

namespace ModSettingsExample
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class Settings : VTOLModSettings<Settings>
    {
        private float _sliderIntValue = 1f;
        private bool _booleanValue = true;

        public float SliderIntValue { 
            get => _sliderIntValue;
            set =>  SetProperty(Mathf.Round(Math.Max(Math.Min(value, 10f), 1f)), ref _sliderIntValue);
        }

        public bool BooleanValue { 
            get => _booleanValue;
            set =>  SetProperty(value, ref _booleanValue);
        }
    }
}
