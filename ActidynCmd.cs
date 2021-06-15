using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ActidinController
{
    class ActidynCmd
    {
        IPEndPoint ipPoint;
        Socket socket;
        public ActidynCmd(string address, int port)
        {

            ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
        }

        public string SendMessage(string message)
        {
            try
            {
                /*if (message.Length < 9)
                {
                    byte[] data = new byte[8];
                    data = Encoding.UTF8.GetBytes(message);

                    socket.Send(data, 8, 0);
                }
                else
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    socket.Send(data, 16, 0);
                }*/

                byte[] data = Encoding.UTF8.GetBytes(message);
                socket.Send(data);

                // получаем ответ
                byte[] buf = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                while (socket.Available > 0)
                {
                    bytes = socket.Receive(buf, buf.Length, 0);
                    builder.Append(Encoding.UTF8.GetString(buf, 0, bytes));
                }

                return builder.ToString();
            }
            catch (Exception ex)
            {
                return "connect error " + ex.Message;
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
                    bytes = socket.Receive(buf, buf.Length, 0);
                    builder.Append(Encoding.UTF8.GetString(buf, 0, bytes));
                }

                return builder.ToString();
            }
            catch (Exception ex)
            {
                return "connect error " + ex.Message;
            }
        }

        public void CloseSocket()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
