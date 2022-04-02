using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.WinForms;

namespace CAG
{
    public static class Webview2Extension
    {
        /// <summary>
        /// 添加hostobject
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="_serviceProvider">服务供应商</param>
        /// <param name="attributeType">特性类型</param>
        /// <returns></returns>
        public static WebView2 AddHostObjects(this WebView2 webView, IServiceProvider _serviceProvider, Type attributeType)
        {
            IEnumerable<Type> types = typeof(Program).Assembly.GetTypes().Where(c => c.CustomAttributes.Any(d => d.AttributeType == attributeType));
            foreach (Type item in types)
            {
                webView.CoreWebView2.AddHostObjectToScript(item.Name, _serviceProvider.GetService(item));
            }
            return webView;
        }



        /// <summary>
        /// 添加hostobject
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="_serviceProvider">服务供应商</param>
        /// <param name="filesPath">文件fullpath</param>
        /// <returns></returns>
        public static WebView2 AddJS(this WebView2 webView, IServiceProvider _serviceProvider, string[] filesPath)
        {
            foreach (string item in filesPath)
            {
                webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(File.ReadAllText(item));
            }
            return webView;
        }
    }
}
