using Playnite.SDK;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ClearCookies
{
	public partial class ClearCookiesPopup : UserControl
	{
		public string Domain { get; set; }
		public Window Window { get; }

		public ClearCookiesPopup(IPlayniteAPI api)
		{
			InitializeComponent();

			Window = api.Dialogs.CreateWindow(new WindowCreationOptions
			{
				ShowMinimizeButton = false,
				ShowMaximizeButton = false,
			});

			Window.Height = 350;
			Window.Width = 400;
			Window.Title = ResourceProvider.GetString("LOCClearCookies");
			Window.Content = this;
			Window.Owner = api.Dialogs.GetCurrentAppWindow();
			Window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
		}

		public static string[] Show(IPlayniteAPI api)
		{
			var popup = new ClearCookiesPopup(api);

			if (!(popup.Window.ShowDialog() ?? false))
			{
				return null;
			}

			return popup.Domain.Trim().Split('\n')
				.Select(s => s.Trim())
				.ToArray();
		}

		private void Send(object sender, RoutedEventArgs e)
		{
			Domain = Input.Text;
			Window.DialogResult = true;
			Window.Close();
		}

		private void InputUpdate(object sender, TextChangedEventArgs e)
		{
			ConfirmButton.IsEnabled = !string.IsNullOrWhiteSpace(Input.Text);
		}
	}
}