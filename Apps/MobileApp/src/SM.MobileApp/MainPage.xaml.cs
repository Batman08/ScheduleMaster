#if ANDROID
using Android.Webkit;
#endif
using System.Windows.Input;
using WebView = Microsoft.Maui.Controls.WebView;

namespace SM.MobileApp
{
    public partial class MainPage : ContentPage
    {
        private WebView _webView = new();

        private readonly string _url = "https://schedule1master.bsite.net/";

        public MainPage()
        {
            InitWebView();
        }

        private void InitWebView()
        {
            _webView.Source = _url;
            _webView.Navigating += OnWebViewNavigating;
            _webView.Navigated += OnWebViewNavigated;
            Content = InitRefreshView();
        }

        private RefreshView InitRefreshView()
        {
            ScrollView scrollView = new ScrollView();
            scrollView.Content = _webView;

            RefreshView refreshView = new RefreshView();
            ICommand refreshCommand = new Command(() =>
            {
                _webView.Reload();
                refreshView.IsRefreshing = false;
            });
            refreshView.Command = refreshCommand;
            refreshView.RefreshColor = Color.FromRgb(44, 136, 211);
            refreshView.Content = scrollView;

            return refreshView;
        }

        protected override bool OnBackButtonPressed()
        {
            if (_webView.CanGoBack) _webView.GoBack();
            else return false;

            return true;
        }

        private async void OnWebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            var url = e.Url.ToLower();

            var homeUri = new Uri(_url);
            var currentUri = new Uri(e.Url);

            bool isHomePage = homeUri.Scheme == currentUri.Scheme && homeUri.Host == currentUri.Host && homeUri.AbsolutePath.TrimEnd('/') == currentUri.AbsolutePath.TrimEnd('/');
            if (isHomePage)
            {
                // Redirect to login instead
                e.Cancel = true;
                _webView.Source = url + UrlStr_Login();
                return;
            }
        }

        private void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            /*create mobile marker if it doesn't exist*/
            _webView.Eval(Javascript_MobileApp_CreateMarker());

            if (Url_Login(e))
            {
                _webView.Eval(Javascript_MobileApp_HideRememberMe());
                return;
            }
        }

        private string UrlStr_Login() => "Identity/Account/Login";
        private bool Url_Login(WebNavigatedEventArgs e) => e.Url.Contains($"/{UrlStr_Login()}");

        private string Javascript_MobileApp_CreateMarker() => "Utilities.MobileApp_CreateMarker();";
        private string Javascript_MobileApp_HideRememberMe() => "Utilities.MobileApp_HideRememberMe();";
    }
}