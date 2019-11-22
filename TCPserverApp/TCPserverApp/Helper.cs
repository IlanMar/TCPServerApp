using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPserverApp
{
    class Helper
    {
        float[] RobotOdomData;
        public TcpConnection tc;

        public void TCPconnect(string ip, string process, bool is_robot)
        {
            tc = new TcpConnection(0, Form1.f1,
                   str => MessageBox.Show("Connected!"),
                   str =>
                   {
                       try
                       {
                           ProcessTCP(str, ip);
                       }
                       catch { }
                   },
                   str => MessageBox.Show("Disconnected!"));

            tc.Connect(ip, process, is_robot);
        }
        void ProcessTCP(string str, string ip)
        {
            if (str.StartsWith(".conf#"))//начало строки
            {
                int ind = str.IndexOf("#");
                var confData = str.Substring(ind + 1);


               
                ReceiveConfData(confData); //получаем данные     о конфигурации  
            }   
            if (tc == null)
            {

                return;
            }

        }


        public  void SendConf()//сюда рпописать тип соощения
        {
       
            if (tc != null)// здесь происходит отправка задающих команд на куку
            {
                string control_str;
              
                //var k_slow = 0.1f;
              //  var arg1 = Vrob; arg1 = (float)Math.Max(-speed, Math.Min(arg1, speed));//надо переделать эти выводы для адекватного вывода
               // var arg2 = LRrob; arg2 = (float)Math.Max(-speed, Math.Min(arg2, speed));
                //var arg3 = Wrob; arg3 = Math.Max(-speed, Math.Min(arg3, speed));//возможно(left-right)
                control_str = string.Format(CultureInfo.InvariantCulture, "", 0, 0, 0);

               
                if (control_str != null) tc.Send(control_str);//отправляем команду 
               
            }
            /*youbot_connection.send(ToString(data));*/
        }

        public  void ReceiveConfData(string OdometryData)
        {
           float[] ReceiveConfData = new float[3];
            if (OdometryData != "")
            {
                // string someString = RobPos;
                string[] words = OdometryData.Split(new char[] { ';' });//парсим строку в массив words
                for (int i = 0; i < 3; i++)
                {
                    RobotOdomData[i] = float.Parse(words[i], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

                }
                float alpha = RobotOdomData[0];

         
            }
        }
        public  void Deactivate()
        {

            if (tc != null) tc.Disconnect("form closing", false);
        }
    }


}

