using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

/// <summary>
///SmsSocketClient 的摘要说明
/// </summary>
public class SocketClient
{

    private System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient();
    private System.Net.Sockets.NetworkStream streamToClient;
    private BinaryReader bReader;
    private BinaryWriter bWriter;

    private readonly int BufferSize = 1024 * 4;
    private byte[] bufferReceive;

    /// <summary>
    /// 服务器IP
    /// </summary>
    public string IP { get; set; }
    /// <summary>
    /// 服务器端口
    /// </summary>
    public int Port { get; set; }
    /// <summary>
    /// 字符编码
    /// </summary>
    public Encoding encoding { get; set; }
    /// <summary>
    /// 回复超时时间，单位：毫秒
    /// </summary>
    public int ResponseTimeout { get; set; }
    /// <summary>
    /// //回复数据
    /// </summary>
    public string ResponseData { get; set; }

    #region  构造函数
    public SocketClient()
	{
        this.IP = "127.0.0.1";
        this.Port = 0;
        this.ResponseTimeout = 1800;
        this.encoding = System.Text.Encoding.GetEncoding("gb2312");
	}

    public SocketClient(string ip, int port)
    {
        try
        {
            this.IP = ip;
            this.Port = port;
            this.ResponseTimeout = 1800;
            this.encoding = System.Text.Encoding.GetEncoding("gb2312");

            if (!ip.Equals(string.Empty) && port > 0)
            {
                this.ConnectServer(ip, port);
            }
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  连接服务器
    public bool ConnectServer()
    {
        try
        {
            return this.ConnectServer(this.IP, this.Port, false);
        }
        catch (Exception ex) { throw (ex); }
    }

    public bool ConnectServer(bool isConnect)
    {
        try
        {
            return this.ConnectServer(this.IP, this.Port, isConnect);
        }
        catch (Exception ex) { throw (ex); }
    }

    public bool ConnectServer(string ip, int port)
    {
        try
        {
            return this.ConnectServer(ip, port, false);
        }
        catch (Exception ex) { throw (ex); }
    }

    public bool ConnectServer(string ip, int port, bool isConnect)
    {
        try
        {
            if (!tcpClient.Connected || isConnect)
            {
                tcpClient = new System.Net.Sockets.TcpClient();
                tcpClient.Connect(IPAddress.Parse(ip), port);
                streamToClient = tcpClient.GetStream();
            }
            return tcpClient.Connected;
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion
    
    #region  读取回复
    public string ReadResponse(int bufferSize)
    {
        try
        {
            bufferReceive = new byte[bufferSize];
            //设置读取超时的时间，单位：毫秒
            streamToClient.ReadTimeout = ResponseTimeout;
            while (true)
            {
                //返回接收的数据的字节
                int read = streamToClient.Read(bufferReceive, 0, bufferSize);
                if (read > 0)
                {
                    //有返回数据表示连接成功
                    break;
                }
            }
            string strResponse = this.encoding.GetString(bufferReceive).TrimEnd('\0');

            Array.Clear(bufferReceive, 0, bufferReceive.Length); //清空缓存

            return strResponse;
        }
        catch (Exception ex) { throw (ex); }
    }
    #endregion

    #region  检测连接是否存在
    public bool CheckServerConnected()
    {
        return tcpClient.Connected;
    }
    #endregion
    
    #region  发送数据内容
    public bool SendData(string strData)
    {
        try
        {
            byte[] bytData = this.encoding.GetBytes(strData);
            return SendData(bytData);
        }
        catch (Exception ex) { throw (ex); }
    }

    public bool SendData(string strData, System.Text.Encoding encoding)
    {
        try
        {
            this.encoding = encoding;

            byte[] bytData = encoding.GetBytes(strData);
            return SendData(bytData);
        }
        catch (Exception ex) { throw (ex); }
    }

    public bool SendData(byte[] bytData)
    {
        bool isSend = false;
        int num = 0;
        while (!isSend && num < 3)
        {
            try
            {
                ConnectServer(num++ > 0);

                streamToClient.Write(bytData, 0, bytData.Length);

                string strRead = ReadResponse(BufferSize);
                if (strRead.Length > 0)
                {
                    this.ResponseData = strRead;
                    isSend = true;
                }
                ServerLog.WriteDebugLog(HttpContext.Current.Request, "SocketResponse", strRead);
            }
            catch (Exception ex)
            {
                ServerLog.WriteDebugLog(HttpContext.Current.Request, "SocketSend", "第" + num + "次指令发送失败");
                ServerLog.WriteErrorLog(ex, HttpContext.Current);
            }
        }
        return isSend;
    }
    #endregion

}