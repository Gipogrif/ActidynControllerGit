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

        ActidynCmd actCmd;// = new ActidynCmd("192.168.100.11", 62000);
        bool actidynConnect = false;

        // ************************************** обработчики атрибутов движения *******************************************
        string pAx1;
        string pAx2;
        string rAx1;
        string rAx2;
        string aAx1;
        string aAx2;
        string fAx1;
        string fAx2;

        public Form1()
        {
            InitializeComponent();
        }

        

        private void connectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                actCmd = new ActidynCmd();
                actidynConnect = actCmd.ActidynCmdConnect("192.168.100.11", 62000);
                if (actidynConnect)
                {
                    richTextBox1.AppendText(actCmd.SendMessage("RMT 0\n")+"\n");
                    richTextBox1.AppendText(actCmd.SendMessage("USR 0,?\n") + "\n");
                    richTextBox1.AppendText(actCmd.SendMessage("USR 0,1,1234\n") + "\n");
                    richTextBox1.AppendText(actCmd.SendMessage("USR 0,?\n") + "\n");
                    richTextBox1.AppendText(actCmd.SendMessage("ALC 0\n") + "\n");

                    timer.Enabled = true;
                    timerAlarm.Enabled = true;
                }
                else MessageBox.Show("Ошибка подключения к Actidyn");
            }
            catch (Exception ex)
            {
                richTextBox1.Text ="connect error " + ex.Message;
            }
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
            if (actidynConnect)
            {
                // закрываем сокет
                //socket.Shutdown(SocketShutdown.Both);
                //socket.Close();
                actCmd.SendMessage("MOD &,OFF\n");
                //actCmd.SendMessage("LOC 0\n");
                actCmd.CloseSocket();
            }
            else { }
        }

        private void button10_Click(object sender, EventArgs e) // Включение\Выключение таймера (неактивна)
        {
            if (timer.Enabled)
            {
                timer.Enabled = false;
                timerAlarm.Enabled = false;
            }
            else
            {
                timer.Enabled = true;
                timerAlarm.Enabled = true;
            }
        }

        private void servoAx1Btn_Click(object sender, EventArgs e) //Включение Привода 1
        {
            //if (!modOn1) // условие включает Сервопривод
            if (statusGetAx1Text.Text == " OFF")
            {
                actCmd.SendMessage("MOD 1,1\n");
            }
            else // выключает
            {
                actCmd.SendMessage("MOD 1,OFF\n");
            }
        }

        private void servoAx2Btn_Click(object sender, EventArgs e) //Включение Привода 2
        {
            if (statusGetAx2Text.Text == " OFF")
            {
                actCmd.SendMessage("MOD 2,1\n");
            }
            else // выключает
            {
                actCmd.SendMessage("MOD 2,OFF\n");
            }
        }

        private void startAx1Btn_Click(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 0) // движение позиционное
            {
                if(statusGetAx1Text.Text == " POS") { actCmd.SendMessage($"POS 1,{pAx1},{rAx1}\n"); }
                else
                {
                    actCmd.SendMessage("MOD 1,POS\n");
                    actCmd.SendMessage($"POS 1,{pAx1},{rAx1}\n");
                }
            }
            else if(tabControl1.SelectedIndex == 1) // движение синусоидальное
            {
                if (statusGetAx1Text.Text == " SIN")
                {
                    actCmd.SendMessage($"RMS 1,{rampASetAx1Text.Text},{rampFSetAx1Text.Text}\n");
                    actCmd.SendMessage($"SIN 1,{aAx1},{fAx1},{phaseSetAx1Text.Text}\n");
                }
                else
                {
                    actCmd.SendMessage("MOD 1,SIN\n");
                    actCmd.SendMessage($"RMS 1,{rampASetAx1Text.Text},{rampFSetAx1Text.Text}\n");
                    actCmd.SendMessage($"SIN 1,{aAx1},{fAx1},{phaseSetAx1Text.Text}\n");
                }
            }
            else
            {
                actCmd.SendMessage("MOD 1,POS\n");
                actCmd.SendMessage($"POS 1,{posGetAx1Text.Text},10\n");
                actCmd.SendMessage("MOD 1,STP\n"); // стоп
            }
        }

        private void startAx2Btn_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0) // движение позиционное
            {
                if (statusGetAx2Text.Text == " POS") { actCmd.SendMessage($"POS 2,{pAx2},{rAx2}\n"); }
                else
                {
                    actCmd.SendMessage("MOD 2,POS\n");
                    actCmd.SendMessage($"POS 2,{pAx2},{rAx2}\n");
                }
            }
            else if (tabControl1.SelectedIndex == 1) // движение синусоидальное
            {
                if (statusGetAx2Text.Text == " SIN")
                {
                    actCmd.SendMessage($"RMS 2,{rampASetAx2Text.Text},{rampFSetAx2Text.Text}\n");
                    actCmd.SendMessage($"SIN 2,{aAx2},{fAx2},{phaseSetAx2Text.Text}\n");
                }
                else
                {
                    actCmd.SendMessage("MOD 2,SIN\n");
                    actCmd.SendMessage($"RMS 2,{rampASetAx2Text.Text},{rampFSetAx2Text.Text}\n");
                    actCmd.SendMessage($"SIN 2,{aAx2},{fAx2},{phaseSetAx2Text.Text}\n");
                }
            }
            else
            {
                actCmd.SendMessage("MOD 2,POS\n");
                actCmd.SendMessage($"POS 2,{posGetAx2Text.Text},5\n");
                actCmd.SendMessage("MOD 2,STP\n"); // стоп
            }

            /* if (tabPagePos.Focused)
             {
                 actCmd.SendMessage("MOD 2,POS\n");
                 actCmd.SendMessage($"POS 2,{posSetAx1Text},{rateSetAx1Text}\n");
             }
             else if (tabPageSine.Focused)
             {
                 actCmd.SendMessage("MOD 2,SIN\n");
                 actCmd.SendMessage($"RMS 2,{rampASetAx1Text},{rampFSetAx1Text}\n");
                 actCmd.SendMessage($"SIN 2,{amplSetAx1Text},{freqSetAx1Text},{phaseSetAx1Text}\n");
             }
             else
             {
                 actCmd.SendMessage("MOD 2,STP\n");
             }*/
        }

        private void stopAx1Btn_Click(object sender, EventArgs e)
        {
            actCmd.SendMessage("MOD 1,POS\n"); // основной вариант
            actCmd.SendMessage($"POS 1,{posGetAx1Text.Text},10\n");
            actCmd.SendMessage("MOD 1,STP\n");

            /*actCmd.SendMessage($"MOD 1,POS :POS 1,{posGetAx1Text.Text},10\n"); // второй вариант (проверить)
            actCmd.SendMessage("MOD 1,STP\n");

            actCmd.SendMessage("MOD 1,RAT\n"); // третий вариант (проверить)
            actCmd.SendMessage("RAT 1,0,5\n");
            actCmd.SendMessage("MOD 1,STP\n");*/

        }

        private void stopAx2Btn_Click(object sender, EventArgs e)
        {
            actCmd.SendMessage("MOD 2,POS\n");
            actCmd.SendMessage($"POS 2,{posGetAx2Text.Text},5\n");
            actCmd.SendMessage("MOD 2,STP\n");

            //actCmd.SendMessage("MOD 2,STP\n"); 
        }

        private void AlarmBtn_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText(actCmd.SendMessage("ALC 0\n") + "\n");
        }


        private void QuietSelect (string quiet) // обработчик ответов от команд
        {
            string[] split = quiet.Split(',');
            try
            {
                switch (split[0] + "," + split[1])
                {
                    /*case "USR 0,?\n":
                        if ((int)char.GetNumericValue(split[2][split[2].Length - 1]) == 1 &&
                            (int)char.GetNumericValue(split[3][1]) == 1)
                        {
                            richTextBox1.AppendText("Соединение установленно");
                        }
                        else { richTextBox1.AppendText($"Ошибка ответа команды {split[0] + "," + split[1]} \n"); }
                        break;*/
                    case "PRV 1,?\n":
                        if ((int)char.GetNumericValue(split[2][split[2].Length - 1]) == 3)
                        {
                            posGetAx1Text.Text = split[3];
                            rateGetAx1Text.Text = split[4];
                            accGetAx1Text.Text = split[5];
                        }
                        else { richTextBox1.AppendText($"Ошибка ответа команды {split[0] + "," + split[1]} \n"); }
                        break;
                    case "PRV 2,?\n":
                        if ((int)char.GetNumericValue(split[2][split[2].Length - 1]) == 3)
                        {
                            posGetAx2Text.Text = split[3];
                            rateGetAx2Text.Text = split[4];
                            accGetAx2Text.Text = split[5];
                        }
                        else { richTextBox1.AppendText($"Ошибка ответа команды {split[0] + "," + split[1]} \n"); }
                        break;
                    case "MOD &,?\n":
                        if ((int)char.GetNumericValue(split[2][split[2].Length - 1]) == 2)
                        {
                            statusGetAx1Text.Text = split[3];
                            statusGetAx2Text.Text = split[4];
                        }
                        else { richTextBox1.AppendText($"Ошибка ответа команды {split[0] + "," + split[1]} \n"); }
                        break;
                    case "PRV &,?\n":
                        if ((int)char.GetNumericValue(split[2][split[2].Length - 1]) == 6)
                        {
                            posGetAx1Text.Text = split[3];
                            rateGetAx1Text.Text = split[4];
                            accGetAx1Text.Text = split[5];
                            posGetAx2Text.Text = split[6];
                            rateGetAx2Text.Text = split[7];
                            accGetAx2Text.Text = split[8];
                        }
                        else { richTextBox1.AppendText($"Ошибка ответа команды {split[0] + "," + split[1]} \n"); }
                        break;
                    case "MOD &,?:PRV &,?\n":
                        if ((int)char.GetNumericValue(split[1][split[1].Length - 1]) == 8)
                        {
                            statusGetAx1Text.Text = split[2];
                            statusGetAx2Text.Text = split[3];
                            posGetAx1Text.Text = split[4];
                            rateGetAx1Text.Text = split[5];
                            accGetAx1Text.Text = split[6];
                            posGetAx2Text.Text = split[7];
                            rateGetAx2Text.Text = split[8];
                            accGetAx2Text.Text = split[9];
                        }
                        else { richTextBox1.AppendText($"Ошибка ответа команды {split[0]} &\n"); }
                        break;
                    case "FRZ &,?\n":
                        if ((int)char.GetNumericValue(split[2][split[2].Length - 1]) == 10)
                        {
                            posGetAx1Text.Text = split[5];
                            rateGetAx1Text.Text = split[6];
                            accGetAx1Text.Text = split[7];
                            posGetAx2Text.Text = split[10];
                            rateGetAx2Text.Text = split[11];
                            accGetAx2Text.Text = split[12];
                        }
                        else { richTextBox1.AppendText($"Ошибка ответа команды {split[0]} &\n"); }
                        break;
                    case "MOD &,?:FRZ &,?\n":
                        if ((int)char.GetNumericValue(split[1][split[1].Length - 1]) == 12)
                        {
                            statusGetAx1Text.Text = split[2];
                            statusGetAx2Text.Text = split[3];
                            posGetAx1Text.Text = split[6];
                            rateGetAx1Text.Text = split[7];
                            accGetAx1Text.Text = split[8];
                            posGetAx2Text.Text = split[11];
                            rateGetAx2Text.Text = split[12];
                            accGetAx2Text.Text = split[13];
                        }
                        else { richTextBox1.AppendText($"Ошибка ответа команды {split[0]} &\n"); }
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка в обработчике ответов от команд.\nПерезапустите программу.");
            }
        }

        private void StatusGetAx() // обработчик кнопок Servo On/Off
        {
            if (statusGetAx1Text.Text == " OFF")
            {
                servoAx1Btn.Text = "Servo OFF";
                servoAx1Btn.BackColor = Color.Red;
            }
            else
            {
                servoAx1Btn.Text = "Servo ON";
                servoAx1Btn.BackColor = Color.Green;
            }
            if (statusGetAx2Text.Text == " OFF")
            {
                servoAx2Btn.Text = "Servo ON";
                servoAx2Btn.BackColor = Color.Red;
            }
            else
            {
                servoAx2Btn.Text = "Servo OFF";
                servoAx2Btn.BackColor = Color.Green;
            }
        }

        private void timer_Tick(object sender, EventArgs e)  // ТАЙМЕР 
        {
            QuietSelect(actCmd.SendMessage("MOD &,?\n")); // запрос режима работы привода
            QuietSelect(actCmd.SendMessage("PRV &,?\n")); // запрос положения, скорости и ускорения
            // QuietSelect(actCmd.SendMessage("RPV &,?\n"));
            // QuietSelect(actCmd.SendMessage("MOD &,?:FRZ &,?\n"));
            // QuietSelect(actCmd.SendMessage("MOD &,?:PRV &,?\n"));
            StatusGetAx();
            ControlInputText();
        }

        private void timerAlarm_Tick(object sender, EventArgs e) // раз в 20 сек сбрасывает накопленные ошибки 
                                                                 //(если в течении 30сек не сбрасывать ошибки, то привод отключаеться)
        {
            actCmd.SendMessage("ALC 0\n");
        }

        private void ControlInputText()
        {
            double countPos1 = Convert.ToDouble(posSetAx1Text.Text);
            pAx1 = (countPos1 >= -380 && countPos1 <= 380) ? posSetAx1Text.Text : "0";
            double countPos2 = Convert.ToDouble(posSetAx2Text.Text);
            pAx2 = (countPos2 >= -50 && countPos2 <= 50) ? posSetAx2Text.Text : "0";

            double countRate1 = Convert.ToDouble(rateSetAx1Text.Text);
            rAx1 = (countRate1 >= 0 && countRate1 <= 50) ? rateSetAx1Text.Text : "0";
            double countRate2 = Convert.ToDouble(rateSetAx2Text.Text);
            rAx2 = (countRate2 >= 0 && countRate2 <= 50) ? rateSetAx2Text.Text : "0";

            double countAmpl1 = Convert.ToDouble(amplSetAx1Text.Text);
            aAx1 = (countAmpl1 >= 0 && countAmpl1 <= 180) ? amplSetAx1Text.Text : "0";
            double countAmpl2 = Convert.ToDouble(amplSetAx2Text.Text);
            aAx2 = (countAmpl2 >= 0 && countAmpl2 <= 180) ? amplSetAx2Text.Text : "0";

            double countFreq1 = Convert.ToDouble(freqSetAx1Text.Text);
            fAx1 = (countFreq1 >= 0 && countFreq1 <= 2) ? freqSetAx1Text.Text : "0";
            double countFreq2 = Convert.ToDouble(freqSetAx2Text.Text);
            fAx2 = (countFreq2 >= 0 && countFreq2 <= 2) ? freqSetAx2Text.Text : "0";
        }
    }

}
