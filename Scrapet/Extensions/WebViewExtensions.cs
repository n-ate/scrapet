using System.Reflection;

namespace Scrapet
{
    public static class WebViewExtensions
    {
        /// <summary>
        /// Opens the DevTools if available.
        /// </summary>
        public static bool TryOpenDevToolsWindow(this WebView webView)
        { //Since this is a non-critical operation, use reflection instead of adding a permanent reference to
          //Microsoft.UI.Xaml.Controls namespace for CoreWebView2..
            if (webView != null)
            {
                var platfromView = webView.Handler.PlatformView;
                if (platfromView != null)
                {
                    var property = platfromView.GetType().GetProperty("CoreWebView2", BindingFlags.Instance | BindingFlags.Public);
                    if (property != null)
                    {
                        var coreWebView2 = property.GetValue(platfromView);
                        if (coreWebView2 != null)
                        {
                            var openMethod = coreWebView2.GetType().GetMethod("OpenDevToolsWindow");
                            if (openMethod != null)
                            {
                                openMethod.Invoke(coreWebView2, null);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Injects a multiline JavaScript snippet.
        /// </summary>
        /// <param name="name">The name of the JavaScript snippet.</param>
        /// <param name="script">The JavaScript to evaluate and execute.</param>
        /// <returns>The output value.</returns>
        public static async Task<string> InjectMultilineScript(this WebView webView, string name, string script)
        {
            var sourceUrl = string.IsNullOrWhiteSpace(name) ? string.Empty : $"//# sourceURL=//{Assembly.GetExecutingAssembly().GetName().Name}/{name}";
            var escapedScript = script.Replace("\\", @"\\").Replace("\r\n", @"\n").Replace("\n", @"\n").Replace("\t", @"    ");
            var result = await webView.EvaluateJavaScriptAsync($"{sourceUrl}\\n//# logErrors\\n{escapedScript}");
            return result;
        }

        /// <summary>
        /// Injects a multiline JavaScript snippet and registers it to share with other registered scripts.
        /// </summary>
        /// <param name="name">The name of the JavaScript snippet.</param>
        /// <param name="script">The JavaScript to evaluate and execute.</param>
        /// <returns>The output value.</returns>
        public static async Task<string> InjectAndRegisterMultilineScript(this WebView webView, string name, string script)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(script)) throw new ArgumentNullException(nameof(script));
            name = $"{char.ToLower(name[0])}{name.Substring(1)}";

            var js =
@$"//# sourceURL=//{Assembly.GetExecutingAssembly().GetName().Name}/{name}
//# logErrors
(function() {{
    if (window.__SCRAPET) {{
        let namespace = window.__SCRAPET.Register(""{name}"");
        (
/* begin {name} script injection */
{script}
/*  end  {name} script injection */
        )(namespace, namespace.{name});
    }}
}})();".Replace("\\", @"\\").Replace("\r\n", @"\n").Replace("\n", @"\n").Replace("\t", @"    ");

            var result = await webView.EvaluateJavaScriptAsync(js);
            return result;
        }
    }
}