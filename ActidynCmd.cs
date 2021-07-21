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
        /*public ActidynCmd(string address, int port)
        {
            try
            {
                ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
            }
            finally
            {

            }

        }*/

        public bool ActidynCmdConnect(string address, int port)
        {
            try
            {
                ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
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
               //string quiet;
               //string[] split;
               // int count = -1;
                //char x;
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
               // quiet = builder.ToString();
               // split = quiet.Split(',');
                return message + "," + builder.ToString();

               /* quiet = builder.ToString();
                if (quiet.Length > 0)
                {
                    split = quiet.Split(',');
                    count = (int)char.GetNumericValue(split[0][split[0].Length - 1]);
                    // x = split[0][split[0].Length - 1];

                    switch (count)
                    {
                        case 0:
                            return split[0];
                        case 1:
                            return split[1];
                        case 2:
                            return
                      
                            
                    }
                    if (count == 0)
                    {
                        return split[0];
                    }
                    else if (count == 1)
                    {
                        return split[1];
                    }
                    else
                    {
                        return "Ошибка if2";
                    }
                }
                else return "ошибка if1";
               /* string[] words = new string[2];
                words = textBox1.Text.Split(new char[] { '°', });
                try
                {
                    words[1] = words[1].Substring(0, words[1].Length - 1); // удаляем последний знак в строке
                    int a = Convert.ToInt32(words[0]) + Convert.ToInt32(words[1]);
                    if (words[0].StartsWith("-"))
                    {
                        words[0] = words[0].Substring(1);
                        //trackBar1.Value = Convert.ToInt32(words[0]) * 65536 / 360 + Convert.ToInt32(words[1]) * 65536 / 21600; ошибка видимо по тому что выходит за диапазон возможных значений
                        //trackBar1.Value = -trackBar1.Value;
                        a = Convert.ToInt32(words[0]) * 65536 / 360 + Convert.ToInt32(words[1]) * 65536 / 21600;
                        trackBar1.Value = -a;
                    }
                    else
                    {
                        trackBar1.Value = Convert.ToInt32(words[0]) * 65536 / 360 + Convert.ToInt32(words[1]) * 65536 / 21600;
                    }*/

                   // return message + " : " + builder.ToString()+" after split: " + split[1] + split[2];
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

        public void CloseSocket()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
