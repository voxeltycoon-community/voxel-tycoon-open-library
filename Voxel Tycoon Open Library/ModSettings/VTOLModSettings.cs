using System;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using VoxelTycoon;
using VoxelTycoon.AssetManagement;

namespace VTOL.ModSettings
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class VTOLModSettings<T> where T: VTOLModSettings<T>, new()
    {
        private Action _settingsChanged;
        private string _modSettingsPath;
        protected bool Initialized { get; private set; }
        protected VTOLModSettings()
        {
            Behaviour = UpdateBehaviour.Create(typeof(T).Name);
            Behaviour.OnDestroyAction = delegate ()
            {
                OnDeinitialize();
                _current = default(T);
            };
            // ReSharper disable once VirtualMemberCallInConstructor
            OnInitialize();
            Initialized = true;
        }

        public static T Current
        {
            get
            {
                T result;
                if ((result = _current) == null)
                {
                    result = (_current = Activator.CreateInstance<T>());
                }
                return result;
            }
        }

        private protected UpdateBehaviour Behaviour { get; private set; }

        protected virtual void OnInitialize()
        {
            LoadSettings();
        }

        protected virtual void OnDeinitialize()
        {
        }

        protected string ModSettingsPath
        {
            get
            {
                if (_modSettingsPath == null)
                {
                    string modNamespace = GetType().Namespace;
                    foreach (Pack pack in EnabledPacksPerSaveHelper.GetEnabledPacks())
                    {
                        if (pack.Name == modNamespace)
                        {
                            return _modSettingsPath = pack.Directory.FullName + "/settings.json";
                        }
                    }
                    throw new Exception("Mod '" + modNamespace + "' not found. Namespace of ModSettings class must be same as mod class name");
                }
                return _modSettingsPath;
            }
        }

        /** saves settings into mod directory (using JsonConvert.SerializeObject(product)) */
        public void SaveSettings()
        {   
            using (StreamWriter writer = new StreamWriter(ModSettingsPath, append: false))
            {
                writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
                writer.Flush();                
            }
        }

        public void LoadSettings()
        {
            if (File.Exists(ModSettingsPath)) {
                try
                {
                    using (StreamReader reader = new StreamReader(ModSettingsPath))
                    {
                        string data = reader.ReadToEnd();
                        JsonConvert.PopulateObject(data, this);
                    }
                } catch (Exception)
                {
                    SaveSettings();
                }

            } else
            {
                SaveSettings();
            }
        }

        protected void SetProperty<TU>(TU value, ref TU propertyField)
        {
            if (!propertyField.Equals(value))
            {
                propertyField = value;
                OnChange();
            }
        }

        public void Subscribe(Action settingsChanged)
        {
            _settingsChanged += settingsChanged;
        }

        public void Unsubscribe(Action settingsChanged)
        {
            _settingsChanged -= settingsChanged;
        }

        protected virtual void OnChange()
        {
            if (Initialized)
            {
                SaveSettings();
                _settingsChanged?.Invoke();
            }
        }

        private static T _current;

    }
}
