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
                var value = await view.EvaluateJavaScriptAsync(Scripts.Selection);
            }
        }
    }
}