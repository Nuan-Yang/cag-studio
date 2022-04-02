using CAG.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System.Runtime.InteropServices;

namespace CAG
{
    public partial class MainForm : Form
    {


        /// <summary>
        /// 日志管理工具
        /// </summary>
        private ILogger<MainForm> _logger;

        /// <summary>
        /// 服务供应商
        /// </summary>
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// 配置管理
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// webview
        /// </summary>
        public WebView2 WebView { get; private set; }

        public MainForm(ILogger<MainForm> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            InitializeComponent();
            InitWebview();
        }

        /// <summary>
        /// 初始化webview
        /// </summary>
        private async void InitWebview()
        {
            _logger.LogInformation("开始初始化Webview2");
            WebView = new WebView2();
            WebView.Dock = DockStyle.Fill;
            WebView.BackColor = Color.RoyalBlue;
            WebView.DefaultBackgroundColor = Color.Transparent;
            WebView.CreationProperties = new CoreWebView2CreationProperties() { Language = _configuration.GetSection("Webview:Language").Value, UserDataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetSection("Webview:UserDataFolder").Value) };
            WebView.Source = new Uri("about:blank");
            WebView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
            Controls.Add(WebView);

        }

        /// <summary>
        /// 初始化完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebView_CoreWebView2InitializationCompleted(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            var setting = WebView.CoreWebView2.Settings;
            setting.IsScriptEnabled = true;
            setting.IsZoomControlEnabled = false;
            setting.IsPinchZoomEnabled = false;
            setting.IsStatusBarEnabled = false;
            setting.AreDefaultContextMenusEnabled = false;
            setting.IsPasswordAutosaveEnabled = false;
            setting.IsGeneralAutofillEnabled = false;
            setting.AreDevToolsEnabled = _configuration.GetValue<bool>("Dev:Enable", false);
            WebView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
            _logger.LogInformation("Webview2加载HostObjects");
            WebView.AddHostObjects(_serviceProvider, typeof(Dotnet2JSAttribute));
            _logger.LogInformation("Webview2初始化已完成");
        }

        /// <summary>
        /// 修改背景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoreWebView2_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            WebView.DefaultBackgroundColor = Color.White;
            WebView.CoreWebView2.DOMContentLoaded -= CoreWebView2_DOMContentLoaded;
        }

        public Func<Message, bool> WndProcHandler { get; set; }


        #region resize
        const int Border = 5;

        const int HTLEFT = 10;
        const int HTRIGHT = 11;
        const int HTTOP = 12;
        const int HTTOPLEFT = 13;
        const int HTTOPRIGHT = 14;
        const int HTBOTTOM = 15;
        const int HTBOTTOMLEFT = 0x10;
        const int HTBOTTOMRIGHT = 17;
        protected override void WndProc(ref Message m)
        {
            if (WndProcHandler != null && WndProcHandler(m))
                return;
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= Border)
                        if (vPoint.Y <= Border)
                            m.Result = (IntPtr)HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - Border)
                            m.Result = (IntPtr)HTBOTTOMLEFT;
                        else m.Result = (IntPtr)HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - Border)
                        if (vPoint.Y <= Border)
                            m.Result = (IntPtr)HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - Border)
                            m.Result = (IntPtr)HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)HTRIGHT;
                    else if (vPoint.Y <= Border)
                        m.Result = (IntPtr)HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)HTBOTTOM;
                    break;
                case 0x0201://鼠标左键按下的消息
                    m.Msg = 0x00A1;//更改消息为非客户区按下鼠标
                    m.LParam = IntPtr.Zero;//默认值
                    m.WParam = new IntPtr(2);//鼠标放在标题栏内
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion

    }
}