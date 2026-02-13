using Playnite.SDK;
using Playnite.SDK.Data;
using System.Collections.Generic;

namespace ClearCookies
{
	public class ClearCookiesSettings : ObservableObject
	{

	}

	public class ClearCookiesSettingsViewModel : ObservableObject, ISettings
	{
		private readonly ClearCookies plugin;
		private ClearCookiesSettings editingClone { get; set; }

		private ClearCookiesSettings settings;
		public ClearCookiesSettings Settings
		{
			get => settings;
			set
			{
				settings = value;
				OnPropertyChanged();
			}
		}

		public ClearCookiesSettingsViewModel(ClearCookies plugin)
		{
			this.plugin = plugin;

			var savedSettings = plugin.LoadPluginSettings<ClearCookiesSettings>();

			if (savedSettings != null)
			{
				Settings = savedSettings;
			}
			else
			{
				Settings = new ClearCookiesSettings();
			}
		}

		public void BeginEdit()
		{
			editingClone = Serialization.GetClone(Settings);
		}

		public void CancelEdit()
		{
			Settings = editingClone;
		}

		public void EndEdit()
		{
			plugin.SavePluginSettings(Settings);
		}

		public bool VerifySettings(out List<string> errors)
		{
			errors = new List<string>();
			return true;
		}
	}
}