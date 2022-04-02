using CAG.Attributes;
using CAG.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CAG.Dotnet2JS
{
    [Dotnet2JS]
    public class Webview2Common
    {
        /// <summary>
        /// js
        /// </summary>
        public readonly string[] JS = { "JS/Webview2Common.js" };


        public Webview2Common(MainForm mainForm, ILogger<Webview2Common> logger, IServiceProvider serviceProvider)
        {
            _mainForm = mainForm;
            _logger = logger;
            _serviceProvider = serviceProvider;
            InitJS();
        }



        /// <summary>
        /// 获取窗口宽度
        /// </summary>
        /// <returns></returns>
        public int GetFormWidth()
        {
            return _mainForm.WebView.Width;
        }

        /// <summary>
        /// 获取窗口高度
        /// </summary>
        /// <returns></returns>
        public int GetFormHeight()
        {
            return _mainForm.WebView.Height;
        }

        /// <summary>
        /// 最大化或恢复正常
        /// </summary>
        public void MaximizeOrNomalForm()
        {
            switch (_mainForm.WindowState)
            {
                case FormWindowState.Normal:
                    _mainForm.Padding = new System.Windows.Forms.Padding(0);
                    _mainForm.WindowState = FormWindowState.Maximized;
                    break;
                case FormWindowState.Maximized:
                    _mainForm.Padding = new System.Windows.Forms.Padding(3);
                    _mainForm.WindowState = FormWindowState.Normal;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 获取窗口状态
        /// </summary>
        /// <returns></returns>
        public string? GetFormStatus()
        {
            return Enum.GetName(typeof(FormWindowState), _mainForm.WindowState);
        }

        /// <summary>
        /// 最小化窗口
        /// </summary>
        public void MinimizeForm()
        {
            switch (_mainForm.WindowState)
            {
                case FormWindowState.Normal:
                    _mainForm.WindowState = FormWindowState.Minimized;
                    break;
                case FormWindowState.Maximized:
                    _mainForm.WindowState = FormWindowState.Minimized;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void CloseForm()
        {
            _mainForm.Close();
        }

        /// <summary>
        /// 移动窗体
        /// </summary>
        public void MoveForm()
        {
            if (_mainForm.WindowState == FormWindowState.Maximized)
                return;
            //为当前应用程序释放鼠标捕获
            ReleaseCapture();
            //发送消息 让系统误以为在标题栏上按下鼠标
            SendMessage(_mainForm.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }


        private void InitJS()
        {
            _logger.LogInformation("Webview2加载js文件");
            _mainForm.WebView.AddJS(_serviceProvider, JS);
        }

        private MainForm _mainForm;
        private readonly ILogger<Webview2Common> _logger;
        private readonly IServiceProvider _serviceProvider;


        /// <summary>
        /// 定义鼠标左键按下
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;

        /// <summary>
        /// 同级别 Z 序之下
        /// </summary>
        public const int GW_HWNDNEXT = 2;

        /// <summary>
        /// 同级别 Z 序之上
        /// </summary>
        public const int GW_HWNDPREV = 3;


        /// <summary>
        /// 获取下一个窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="wCmd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern IntPtr GetWindow(IntPtr hWnd, uint wCmd);

        /// <summary>
        /// 
        /// </summary>
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, long wParam, long lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
    }
}
