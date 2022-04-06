using UnityEngine;
using VoxelTycoon;
using VoxelTycoon.Modding;

class SettingsModHelper
{
	/// <summary>
	/// Set and save a mod setting
	/// </summary>
	/// <typeparam name="T">The variable type to set</typeparam>
	/// <typeparam name="TSettingsMod">The SettingsMod for this mod</typeparam>
	/// <param name="name">Key name of the variable to set</param>
	/// <param name="value">Value of the variable to set</param>
	/// <param name="worldSettings">A reference to the worldSettings used in the SettingsMod</param>
	public static void SetSetting<T, TSettingsMod>(string name, T value, WorldSettings worldSettings) where TSettingsMod : SettingsMod, new()
	{
		// For now, using PlayerPrefs.  I want to switch to a local file within the mod's folder but there's a LOT of potential issues with that.
		// First, VT doesn't seem to like Mods using System.IO
		// Second, saving a file into the mod's folder (Given it's in the workshop folder for Steam) means that the hash value would change.
		// This means that Steam 'might' see it as corrupt and re-download the mod or remove the saved file.
		// I will test this once the first issue is resolved.
		PlayerPrefs.SetString(typeof(TSettingsMod).Name + "|" + name, value.ToString());
		PlayerPrefs.Save();

		if (typeof(T) == typeof(float))
		{
			worldSettings.SetFloat<TSettingsMod>(name, (float)(object)value);
			return;
		}
		if (value.GetType() == typeof(bool))
		{
			worldSettings.SetBool<TSettingsMod>(name, (bool)(object)value);
			return;
		}
		if (value.GetType() == typeof(string))
		{
			worldSettings.SetString<TSettingsMod>(name, (string)(object)value);
			return;
		}
		if (value.GetType() == typeof(int))
		{
			worldSettings.SetInt<TSettingsMod>(name, (int)(object)value);
			return;
		}
		if (value.GetType() == typeof(double))
		{
			worldSettings.SetDouble<TSettingsMod>(name, (double)(object)value);
			return;
		}
	}

	/// <summary>
	/// Get a mod setting
	/// </summary>
	/// <typeparam name="T">The variable type to get</typeparam>
	/// <typeparam name="TSettingsMod">The SettingsMod for this mod</typeparam>
	/// <param name="name">Key name of the variable to get</param>
	/// <param name="defaultValue">The default value if a key does not already exist</param>
	/// <returns>Returns the variable of type T</returns>
	public static T GetSetting<T, TSettingsMod>(string name, T defaultValue)
	{
		string returnData = PlayerPrefs.GetString(typeof(TSettingsMod).Name + "|" + name, defaultValue.ToString());
		// Wanted to use a switch but a switch can't use typeof(float) etc because it's not technically a static value since typeof, on the VERY low level, returns a hex value that could be different per OS
		// This is in a try-catch because I'm blindly attempting to parse.  It doesn't really matter if it fails.  It'll just return defaultValue
		try
		{
			if (typeof(T) == typeof(float))
			{
				return (T)(object)float.Parse(returnData);
			}
			if (typeof(T) == typeof(bool))
			{
				return (T)(object)bool.Parse(returnData);
			}
			if (typeof(T) == typeof(string))
			{
				return (T)(object)returnData;
			}
			if (typeof(T) == typeof(int))
			{
				return (T)(object)int.Parse(returnData);
			}
			if (typeof(T) == typeof(double))
			{
				return (T)(object)double.Parse(returnData);
			}
			return defaultValue;
		}
		catch
		{
			return defaultValue;
		}
	}
}