using System;
using System.IO;
using Newtonsoft.Json;
using VoxelTycoon;
using VoxelTycoon.AssetManagement;

namespace VTOL.ModSettings
{
    /// <summary>
    /// Base class for mod settings
    /// Inherit this class in the mod project under mods namespace and add all needed properties
    /// </summary>
    /// <example>
    /// <b> Class declaration </b>
    /// <code>
    /// Settings: &lt;VTOLModSettings&gt;
    /// </code>
    /// 
    /// <b> Declaration of one property: </b> 
    /// <code>
    ///    private bool _someProperty;
    ///
    ///    public bool SomeProperty { 
    ///        get => _someProperty;
    ///        set =>  SetProperty(value, ref _someProperty);
    ///    }
    /// </code>
    ///
    /// More information about usage showed in example project under <b>examples/ModSettingsExample</b> folder
    /// </example>
    /// 
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class VTOLModSettings<T> where T: VTOLModSettings<T>, new()
    {
        private string _modSettingsFilePath;
        protected bool Initialized { get; private set; }
        
        /// <summary>
        /// Event raised when some settings value is changed
        /// </summary>
        public event Action SettingsChanged;
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

        /// <summary>
        /// Returns initialized settings class
        /// </summary>
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

        /// <summary>
        /// Called when first Current property is used on first time 
        /// </summary>
        protected virtual void OnInitialize()
        {
            LoadSettings();
        }

        /// <summary>
        /// Called on de-initializing the game 
        /// </summary>
        protected virtual void OnDeinitialize()
        {
        }

        /// <summary>
        /// Returns mod's settings file path
        /// </summary>
        /// <exception cref="Exception">Raised when cannot determine mod's installation path</exception>
        protected string ModSettingsFilePath
        {
            get
            {
                if (_modSettingsFilePath == null)
                {
                    string modNamespace = GetType().Namespace;
                    foreach (Pack pack in EnabledPacksPerSaveHelper.GetEnabledPacks())
                    {
                        if (pack.Name == modNamespace)
                        {
                            return _modSettingsFilePath = pack.Directory.FullName + "/settings.json";
                        }
                    }
                    throw new Exception("Mod '" + modNamespace + "' not found. Namespace of ModSettings class must be same as mod class name");
                }
                return _modSettingsFilePath;
            }
        }

        /// <summary>
        /// Saves settings into mod directory (using JsonConvert.SerializeObject(product))
        /// </summary>
        public void SaveSettings()
        {   
            using (StreamWriter writer = new StreamWriter(ModSettingsFilePath, append: false))
            {
                writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
                writer.Flush();                
            }
        }

        /// <summary>
        /// Loads settings from mod directory, if there is no settings file, creates one with current settings values
        /// </summary>
        public void LoadSettings()
        {
            if (File.Exists(ModSettingsFilePath)) {
                try
                {
                    using (StreamReader reader = new StreamReader(ModSettingsFilePath))
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

        /// <summary>
        /// Sets provided value to the specified property backing field and if there is a change, it calls OnChange() method  
        /// </summary>
        /// <param name="value">Value to be set</param>
        /// <param name="propertyField">Reference to the property backing field</param>
        /// <typeparam name="TU">Type of property field</typeparam>
        protected void SetProperty<TU>(TU value, ref TU propertyField)
        {
            if (!propertyField.Equals(value))
            {
                propertyField = value;
                OnChange();
            }
        }

        /// <summary>
        /// Called when some property value is changed
        /// Saves settings to the file and raise SettingsChanged event
        /// </summary>
        protected virtual void OnChange()
        {
            if (Initialized)
            {
                SaveSettings();
                SettingsChanged?.Invoke();
            }
        }

        private static T _current;

    }
}
