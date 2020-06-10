using SocketWebServer.模拟asp.net管道;
using SocketWebServer.网络交互请求封装类;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketWebServer
{
    public partial class Main : Form
    {
        /// <summary>
        /// 创建监听
        /// </summary>
        private Socket socketWatch = null;
        //后台线程执行监听
        private Thread threadWatch = null;
        // 标志是否已经关闭监听服务
        private bool isEndService = true;
        public Main()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            // 创建Socket->绑定IP与端口->设置监听队列的长度->开启监听连接
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketWatch.Bind(new IPEndPoint(IPAddress.Parse(txtIPAddress.Text), int.Parse(txtPort.Text)));
            socketWatch.Listen(10);
            // 创建Thread->后台执行
            threadWatch = new Thread(ListenClientConnect);
            //启动后台线程
            threadWatch.IsBackground = true;
            //开始执行
            threadWatch.Start(socketWatch);
            isEndService = false;
            txtIPAddress.ReadOnly = true;
            txtPort.ReadOnly = true;
            btnStart.Enabled = false;
            ShowMessage("消息:【成功启动Web服务！】");

        }

        private void ListenClientConnect(object obj)
        {
            Socket socketListen = obj as Socket;

            while (!isEndService)
            {
                //监听客户端信息
                Socket proxSocket = socketListen.Accept();
                byte[] data = new byte[1024 * 1024 * 2];
                int length = proxSocket.Receive(data, 0, data.Length, SocketFlags.None);
                // Step1:接收HTTP请求
                string requestText = Encoding.Default.GetString(data, 0, length);
                if (!string.IsNullOrEmpty(requestText))
                {
                    HttpContext context = new HttpContext(requestText);
                    // Step2:处理HTTP请求
                    HttpApplication application = new HttpApplication();
                    application.ProcessRequest(context);
                    ShowMessage(string.Format("{0} {1} from {2}", context.Request.HttpMethod, context.Request.Url, proxSocket.RemoteEndPoint.ToString()));
                    // Step3:响应HTTP请求
                    proxSocket.Send(context.Response.GetResponseHeader());
                    if (context.Response.Body != null)
                    {
                        proxSocket.Send(context.Response.Body);
                    }
                }

                // Step4:即时关闭Socket连接
                proxSocket.Shutdown(SocketShutdown.Both);
                proxSocket.Close();
            }
        }


        /// <summary>
        /// 停止监听
        /// </summary>
        /// <param name="proxSocket"></param>
        private void StopConnection(Socket proxSocket)
        {
            try
            {
                if (proxSocket.Connected)
                {
                    // 正常退出连接
                    proxSocket.Shutdown(SocketShutdown.Both);
                    // 释放相关资源
                    proxSocket.Close();
                }
            }
            catch (SocketException ex)
            {
                ShowMessage("·_·异常:【" + ex.Message + "】");
            }
            catch (Exception ex)
            {
                ShowMessage("·_·异常:【" + ex.Message + "】");
            }
        }

        /// <summary>
        /// 关闭监听
        /// </summary>
        private void CloseService()
        {
            isEndService = true;

            if (threadWatch != null)
            {
                threadWatch.Abort();
            }
            StopConnection(socketWatch);
        }

        /// <summary>
        /// 窗体关闭后关闭服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseService();
        }

        /// <summary>
        /// 显示运行信息
        /// </summary>
        /// <param name="msg"></param>
        private void ShowMessage(string msg)
        {
            if (txtStatus.InvokeRequired)
            {
                txtStatus.BeginInvoke(new Action<string>((str) =>
                {
                    txtStatus.AppendText(str + Environment.NewLine);
                }), msg);
            }
            else
            {
                txtStatus.AppendText(msg + Environment.NewLine);
            }
        }

        /// <summary>
        /// 默认加载本地及8888端口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {
            this.txtIPAddress.Text = "127.0.0.1";
            this.txtPort.Text = "8888";
        }
    }
}
