namespace SM.MobileApp
{
    public partial class MainPage : ContentPage
    {
        private WebView _webView = new WebView();

        private readonly string _url = "https://schedulemaster.bsite.net/";

        public MainPage()
        {
            InitWebView();
        }

        private void InitWebView()
        {
            _webView.Source = _url;
            _webView.Navigated += OnWebViewNavigated;
            Content = _webView;
        }

        private void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            /*create mobile marker if it doesn't exist*/
            //_webView.Eval(Javascript_MobileApp_CreateMarker()); -- todo: create Utilities function
        }

        private string Javascript_MobileApp_CreateMarker() => "Utilities.MobileApp_CreateMarker();";
    }
}