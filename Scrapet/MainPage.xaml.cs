using Scrapet.Resources;

namespace Scrapet
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (sender is WebView view)
            {
                _ = await view.InjectMultilineScript(nameof(Scripts.Shim), Scripts.Shim);
                _ = await view.InjectMultilineScript(nameof(Scripts.Namespace), Scripts.Namespace);
                _ = await view.InjectAndRegisterMultilineScript(nameof(Scripts.Selections), Scripts.Selections);
                _ = await view.InjectAndRegisterMultilineScript(nameof(Scripts.Events), Scripts.Events);
            }
        }

        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (sender is WebView view) view.TryOpenDevToolsWindow();
        }
    }
}