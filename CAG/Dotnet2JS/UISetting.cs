using CAG.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAG.Dotnet2JS
{
    [Dotnet2JS]
    public class UISetting
    {
        private MainForm _mainForm;

        /// <summary>
        /// 日志管理系统
        /// </summary>
        private readonly ILogger<UISetting> _logger;

        /// <summary>
        /// 配置管理
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// URL
        /// </summary>
        public readonly Uri Url = new Uri("http://cag.studio.com/index.html");

        public UISetting(MainForm mainForm, ILogger<UISetting> logger, IConfiguration configuration)
        {
            _mainForm = mainForm;
            _logger = logger;
            _configuration = configuration;
            _mainForm.MinimumSize = new Size(_mainForm.Width, _mainForm.Height);
            _logger.LogInformation("Webview2虚拟本地路径到域名");
            _mainForm.WebView.CoreWebView2.SetVirtualHostNameToFolderMapping(Url.Host, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "View"), Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
            _mainForm.WebView.Source = new Uri(_configuration.GetValue<string>("StartUrl"));
        }
    }
}
