using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// View to display safe web view.
/// </summary>
public class WebViewSafeModeView : MonoBehaviour
{
    /// <summary>
    /// Wrapper for web view.
    /// We use it because <see cref="UniWebViewSafeBrowsing"/> inherits from UnityEngine.Object
    /// and we can't use check for null in the OnDestroy method.
    /// </summary>
    private class WebViewWrapper
    {
        /// <summary>
        /// Web view instance.
        /// </summary>
        public UniWebViewSafeBrowsing WebView { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewWrapper" /> class.
        /// </summary>
        /// <param name="webView">Web view instance.</param>
        public WebViewWrapper(UniWebViewSafeBrowsing webView)
        {
            WebView = webView;
        }
    }

    [SerializeField]
    private Button button;

    [SerializeField]
    private TMP_InputField inputField;

    private WebViewWrapper webViewWrapper;

    private bool isWebViewDone;


    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
        ClearCurrentWebView();
    }


    private void OnButtonClicked()
    {
        var url = inputField.text;

        if (!string.IsNullOrEmpty(url))
        {
            ClearCurrentWebView();
            CreateSafeWebView(url);
        }
    }

    private void OnSafeBrowsingFinishedDelegate(UniWebViewSafeBrowsing browsing)
    {
        isWebViewDone = true;
        ClearCurrentWebView();
    }

    private void CreateSafeWebView(string url)
    {
        webViewWrapper = new WebViewWrapper(UniWebViewSafeBrowsing.Create(url));

        isWebViewDone = false;
        webViewWrapper.WebView.OnSafeBrowsingFinished += OnSafeBrowsingFinishedDelegate;
        webViewWrapper.WebView.Show();
    }

    private void ClearCurrentWebView()
    {
        if (webViewWrapper == null)
            return;

        webViewWrapper.WebView.OnSafeBrowsingFinished -= OnSafeBrowsingFinishedDelegate;
        if (!isWebViewDone)
        {
            // Dismiss webView if it is not already hidden by pressing done button.
            // NOTE: UniWebViewSafeBrowsing creates UniWebViewNativeListener (MonoBehaviour)
            // instance under the hood and does not mark it as don't destroy on load.
            // This might cause issues on scene reloading. UniWebViewNativeListener might be
            // destroyed before WebView.Dismiss() is called. That way Dismiss() will not work
            // and will throw null reference exception.
            webViewWrapper.WebView.Dismiss();
            isWebViewDone = true;
        }
        else
        {
            Destroy(webViewWrapper.WebView);
            webViewWrapper = null;
        }
    }
}

