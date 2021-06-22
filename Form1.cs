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

        ActidynCmd actCmd = new ActidynCmd("192.168.100.11", 62000);
        public Form1()
        {
            InitializeComponent();
        }

        

        private void connectBtn_Click(object sender, EventArgs e)
        {
            try
            {

                actCmd.SendMessage("RMT 0  \n");
                actCmd.SendMessage("POS &,?\n");
                actCmd.SendMessage("USR 0,?\n");
                actCmd.SendMessage("USR 0,2,4321   \n");
                actCmd.SendMessage("USR 0,?\n");
                actCmd.SendMessage("ALC 0  \n");
                actCmd.SendMessage("ALC 0");
                actCmd.SendMessage("ALC 0          \n");
                actCmd.SendMessage("ALC 0       \n");
                /*actCmd.SendMessage("ALC 0        \n");
                actCmd.SendMessage("ALC 0    \n");
                actCmd.SendMessage("POS &,?\n");

                //actCmd.SendMessage("ALC 0  \n");
                actCmd.SendMessage("MOD 1,?\n");
                actCmd.SendMessage("MOD &,1        \n");
                actCmd.SendMessage("MOD &,1        \n");
                actCmd.SendMessage("MOD 1,?\n");
                actCmd.SendMessage("MOD 1,POS      \n");
                actCmd.SendMessage("POS 1,45,5     \n");
                actCmd.SendMessage("MOD &,OFF      \n");*/


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
                posGetAx1Text.Text = actCmd.SendMessage("POS 1,?\n");
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
                posGetAx2Text.Text = actCmd.SendMessage("POS 2,?\n");
            }
            catch (Exception ex)
            {
                richTextBox1.Text = "getPosAx2 error " + ex.Message;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // закрываем сокет
            //socket.Shutdown(SocketShutdown.Both);
            //socket.Close();
            actCmd.CloseSocket();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //string message = "RMT 0";

            //string message = "POS &,?\nUSR 0,?\n";
            //string message = "ALC 0\n";
            string message = "RMT 0\nUSR 0,2,4321\nALC 0\nMOD 1,POS\n";//MOD 1,1\nMOD 1,?\nRMT 0\nPOS &,?\nUSR 0,?\nMOD 1,?\n//MOD 1,1\nMOD 1,?\nMOD 1,RAT\nMOD 1,?\nMOD 1,?\n";
            byte[] data = Encoding.UTF8.GetBytes(message);
            actCmd.SendMessage(data);

            message = "MOD 1,POS\nPOS 1,10,5\nMOD 1,?\n";
            data = Encoding.UTF8.GetBytes(message);
            actCmd.SendMessage(data);
            // message = "POS 1,45,5\nMOD 1,POS\nMOD 1,?\n";
            message = "POS 1,-45,5\n";
            data = Encoding.UTF8.GetBytes(message);
            actCmd.SendMessage(data);

            message = "MOD 1,STP\n";
            data = Encoding.UTF8.GetBytes(message);
            actCmd.SendMessage(data);

            message = "MOD 1,?\n";
            data = Encoding.UTF8.GetBytes(message);
            actCmd.SendMessage(data);

            message = "MOD 1,POS\n";
            data = Encoding.UTF8.GetBytes(message);
            actCmd.SendMessage(data);

            message = "MOD 1,?\n";
            data = Encoding.UTF8.GetBytes(message);
            actCmd.SendMessage(data);

            message = "POS 1,90\n";
            data = Encoding.UTF8.GetBytes(message);
            actCmd.SendMessage(data);
            /*for (int i = 0; i < 33; i++)
            {
                message += ' ';
                data = Encoding.UTF8.GetBytes(message);
                data[data.Length - 1] = (byte)'\n';
                actCmd.SendMessage(data);
                i++;
            }*/
            //  socket.Send(data);

            richTextBox1.Text =  message;
        }
    }
}
