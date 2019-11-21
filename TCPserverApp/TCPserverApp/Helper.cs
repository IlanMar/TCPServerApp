using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPserverApp
{
    class Helper
    {

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
                var LedData = str.Substring(ind + 1);

                
                //KukaPotField.LedDataKuka(s);//отправляем данные с лудара куки
                ReceiveConfData(LedData); //получаем данные     о конфигурации        }
            // float Xrob1, Yrob1, Arob1, Xrob2, Yrob2, Arob2, Xrob3, Yrob3, Arob3;
            if (str.Contains("pt id="))
            {
                Form1.f1.rtb_cam.Invoke(new Action(() => Form1.f1.rtb_cam.Text = str));

                string Firstrobot = str.Substring(55, 17);
                string Secondrobot = str.Substring(112, 17);
                string Thirdrobot = str.Substring(169, 17);

                //  CultureInfo culture = new CultureInfo("ru");
                Xrob3 = float.Parse(Firstrobot.Substring(0, 3), new CultureInfo("en-US"));//раньше это был Хроб1
                Yrob3 = float.Parse(Firstrobot.Substring(6, 4), new CultureInfo("en-US"));
                //    Arob1 = float.Parse(Firstrobot.Substring(10, 4), new CultureInfo("en-US"));

                Xrob2 = float.Parse(Secondrobot.Substring(0, 3), new CultureInfo("en-US"));
                Yrob2 = float.Parse(Secondrobot.Substring(6, 4), new CultureInfo("en-US"));
                ///   Arob2 = float.Parse(Secondrobot.Substring(10, 4), new CultureInfo("en-US"));

                Xrob1 = float.Parse(Thirdrobot.Substring(0, 3), new CultureInfo("en-US"));
                Yrob1 = float.Parse(Thirdrobot.Substring(6, 4), new CultureInfo("en-US"));
                //  Arob3 = float.Parse(Thirdrobot.Substring(10, 4), new CultureInfo("en-US"));

                //или просто по символам, отсчитать символы для чисел в первой второй и третей строчке


            }



            if (str.Contains(".odom#"))
            {
                int ind = str.IndexOf("#");
                var OdometryData = str.Substring(ind + 1);
                //  KukaPotField.RodLocReceivingKuka(s);//отпровляем "s" данные одометрии
                Form1.f1.ShowOdomData(OdometryData);
                float Xdelta = 0, Ydelta = 0;
                if (ip == "192.168.88.23")
                {
                    Xdelta = -1.5f; Ydelta = 0;
                    //   Xdelta = Form1.f1.yatccam.Xrob1; Ydelta = 6 - Form1.f1.yatccam.Yrob1;
                    // Xdelta = Form1.f1.yatccam.Xrob1 - Form1.f1.yatccam.Xrob2; Ydelta = Form1.f1.yatccam.Yrob2 - Form1.f1.yatccam.Yrob1;
                    Xdelta = Xdelta * -1; Ydelta = Ydelta * -1;
                }
                if (ip == "192.168.88.24")
                {
                    Xdelta = 0; Ydelta = 0;
                    // Xdelta =  Form1.f1.yatccam.Xrob2; Ydelta = 6 - Form1.f1.yatccam.Yrob2;
                }
                if (ip == "192.168.88.25")
                {
                    Xdelta = 1.5f; Ydelta = 0;
                    //  Xdelta =  Form1.f1.yatccam.Xrob3- Form1.f1.yatccam.Xrob2; Ydelta =  Form1.f1.yatccam.Yrob2-Form1.f1.yatccam.Yrob3;
                    Xdelta = Xdelta * -1; Ydelta = Ydelta * -1;
                    //Xdelta = -1.2f; Ydelta = 0;
                }
                ReceiveOdomData(OdometryData, Xdelta, Ydelta); //отправляем данные одометрии
            }
            //ra.Receive(LedData, OdometryData); 
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
                control_str = string.Format(CultureInfo.InvariantCulture, "LUA_Base({0}, {1}, {2})", 0, 0, 0);

               
                if (control_str != null) tc.SendConf(control_str);//отправляем команду 
               
            }
            /*youbot_connection.send(ToString(data));*/
        }

        public override void ReceiveConfData(string OdometryData, float Xdelta, float Ydelta)
        {
            ReceiveConfData = new float[3];
            if (OdometryData != "")
            {
                // string someString = RobPos;
                string[] words = OdometryData.Split(new char[] { ';' });//парсим строку в массив words
                for (int i = 0; i < 3; i++)
                {
                    RobotOdomData[i] = float.Parse(words[i], System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

                }
                float alpha = RobotOdomData[0];

                RobotOdomData[0] = RobotOdomData[1];
                RobotOdomData[1] = alpha;
                RobotOdomData[1] = RobotOdomData[1] + Ydelta;
                RobotOdomData[0] = RobotOdomData[0] + Xdelta;
            }
        }
        public override void Deactivate()
        {

            if (tc != null) tc.Disconnect("form closing", false);
        }
    }


}
}
