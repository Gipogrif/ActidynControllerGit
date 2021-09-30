using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ActidinController
{
    class ActidynCmd
    {
        IPEndPoint ipPoint;
        Socket socket;

        public bool ActidynCmdConnect(string address, int port)
        {
            try
            {
                ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту

                IAsyncResult result = socket.BeginConnect(ipPoint, null, null);

                bool success = result.AsyncWaitHandle.WaitOne(1000, true); // если не можем подключиться больше секунды выдаем ошибку

                if (socket.Connected)
                {
                    socket.EndConnect(result);
                }
                else
                {
                    socket.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public string SendMessage(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                socket.Send(data);

                Thread.Sleep(100);

                // получаем ответ
                byte[] buf = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                while (socket.Available > 0)
                {
                    bytes = socket.Receive(buf, buf.Length, 0);
                    builder.Append(Encoding.UTF8.GetString(buf, 0, bytes));
                }
                return message + "," + builder.ToString();
            }

            catch (Exception ex)
            {
                return "SendMessage error" + ex.Message;
            }
        }

        public string SendMessage(byte[] buf)
        {
            try
            {
                socket.Send(buf);

                // получаем ответ
                byte[] data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                while (socket.Available > 0)
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }

                return builder.ToString();
            }
            catch (Exception ex)
            {
                return "SendMessage error " + ex.Message;
            }
        }

        public string CommandPosAxis1(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes("MOD 1,POS\n");
                socket.Send(data);

                data = Encoding.UTF8.GetBytes(message);
                socket.Send(data);

                Thread.Sleep(100);

                // получаем ответ
                byte[] buf = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                while (socket.Available > 0)
                {
                    bytes = socket.Receive(buf, buf.Length, 0);
                    builder.Append(Encoding.UTF8.GetString(buf, 0, bytes));
                }
                return message + "," + builder.ToString();
                 
            }
            catch (Exception ex)
            {
                return "SendMessage error" + ex.Message;
            }
        }

        public string CommandPosAxis2(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes("MOD 2,POS\n");
                socket.Send(data);

                data = Encoding.UTF8.GetBytes(message);
                socket.Send(data);

                Thread.Sleep(100);

                // получаем ответ
                byte[] buf = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                while (socket.Available > 0)
                {
                    bytes = socket.Receive(buf, buf.Length, 0);
                    builder.Append(Encoding.UTF8.GetString(buf, 0, bytes));
                }
                return message + "," + builder.ToString();

            }
            catch (Exception ex)
            {
                return "SendMessage error" + ex.Message;
            }
        }

        public void CloseSocket()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
