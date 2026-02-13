using Playnite.SDK;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ClearCookies
{
	public class ClearCookies : GenericPlugin
	{
		private static readonly ILogger logger = LogManager.GetLogger();

		private ClearCookiesSettingsViewModel settings { get; set; }

		public override Guid Id { get; } = Guid.Parse("6fa4e7c4-8429-4d93-80f4-790c91fdd7e5");

		public ClearCookies(IPlayniteAPI api) : base(api)
		{
			//settings = new ClearCookiesSettingsViewModel(this);
			Properties = new GenericPluginProperties
			{
				HasSettings = false,
			};
		}

		public override IEnumerable<MainMenuItem> GetMainMenuItems(GetMainMenuItemsArgs args)
		{
			yield return new MainMenuItem
			{
				MenuSection = $"@{ResourceProvider.GetString("LOCClearCookies")}",
				Description = ResourceProvider.GetString("LOCClearCookiesMenu"),
				Action = (arg) =>
				{
					var domains = ClearCookiesPopup.Show(PlayniteApi);

					if (domains is null)
					{
						return;
					}

					using (var webView = PlayniteApi.WebViews.CreateOffscreenView())
					{
						foreach (string domain in domains)
						{
							webView.DeleteDomainCookies(domain);
						}
					}

					PlayniteApi.Dialogs.ShowMessage($"{ResourceProvider.GetString("LOCClearCookiesSuccess")}\n\n{string.Join("\n", domains)}", ResourceProvider.GetString("LOCClearCookies"));
				}
			};

			yield return new MainMenuItem
			{
				MenuSection = $"@{ResourceProvider.GetString("LOCClearCookies")}",
				Description = ResourceProvider.GetString("LOCClearCookiesAllMenu"),
				Action = (arg) =>
				{
					var res = PlayniteApi.Dialogs.ShowMessage(ResourceProvider.GetString("LOCClearCookiesAllWarning"), ResourceProvider.GetString("LOCClearCookies"), MessageBoxButton.YesNo, MessageBoxImage.Warning);

					if (res != MessageBoxResult.Yes)
					{
						return;
					}

					using (var webView = PlayniteApi.WebViews.CreateOffscreenView())
					{
						webView.DeleteDomainCookiesRegex(".*");
					}

					PlayniteApi.Dialogs.ShowMessage(ResourceProvider.GetString("LOCClearCookiesAllSuccess"), ResourceProvider.GetString("LOCClearCookies"));
				}
			};
		}

		//public override ISettings GetSettings(bool firstRunSettings)
		//{
		//    return settings;
		//}

		//public override UserControl GetSettingsView(bool firstRunSettings)
		//{
		//    return new ClearCookiesSettingsView();
		//}
	}
}