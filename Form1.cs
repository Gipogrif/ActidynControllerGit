using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace ActidinController
{
    public partial class Form1 : Form
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 62000; // порт сервера
        static string address = "192.168.100.11"; // адрес сервера
        IPEndPoint ipPoint;
        Socket socket;
        ActidynCmd actCmd = new ActidynCmd("192.168.100.11", 62000);
        public Form1()
        {
            InitializeComponent();
        }

        

        private void connectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                /*ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
                string message = "RMT 0";
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                Task.Delay(100);

                // получаем ответ
                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                richTextBox1.Text = "ответ сервера: " + builder.ToString();

                // закрываем сокет
               /* socket.Shutdown(SocketShutdown.Both);
                socket.Close();*/

                actCmd.SendMessage("RTM 0");
                actCmd.SendMessage("USR 0, ?");
                actCmd.SendMessage("USR 1,1234");
                actCmd.SendMessage("POS 1,? ");
            }
            catch (Exception ex)
            {
                richTextBox1.Text ="connect error " + ex.Message;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void getPosAx1_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "POS 1,?";
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                Task.Delay(100);

                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                posGetAx1Text.Text = builder.ToString();
            }
            catch (Exception ex)
            {
                richTextBox1.Text = "getPosAx1 error " + ex.Message;
            }
        }

        private void getPosAx2_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "POS 2,?";
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                Task.Delay(100);

                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                posGetAx2Text.Text = builder.ToString();
            }
            catch (Exception ex)
            {
                richTextBox1.Text = "getPosAx2 error " + ex.Message;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // закрываем сокет
             socket.Shutdown(SocketShutdown.Both);
             socket.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string message = "RMT 0 \n";
            message = "POS &,?\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            socket.Send(data,8,0);

            richTextBox1.Text =  message;
        }
    }
}
