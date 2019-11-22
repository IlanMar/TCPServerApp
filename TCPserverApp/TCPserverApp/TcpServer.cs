using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPserverApp
{
    public class TcpServer
    {
        TcpListener server;
        Encoding enc;

        public void Start(string ip, int port)
        {
            server = new TcpListener(IPAddress.Parse(ip), port);
            server.Start();

            ServerThread = new Thread(Run);
            ServerThread.Start();

            
        }

        public Thread ServerThread;
        bool stop = false;

        public Action<string> OnReceive;
        public Func<string, string> ProcessRequest;
        public string MessageBoundary = "^^^";
        public string DisconnectString = "~~~";
        public bool KeepAlive = true;

        bool EndsWith(StringBuilder sb, string s)
        {
            if (sb.Length < s.Length) return false;
            return sb.ToString(sb.Length - s.Length, s.Length) == s;
        }

        void Run()
        {
            // запуск слушателя
            server.Start();
            

            while (!stop)
            {
                // получаем входящее подключение
                var client = server.AcceptTcpClient();

                // получаем сетевой поток для чтения и записи
                var stream = client.GetStream();

                var sr = new StreamReader(stream);
                var sb = new StringBuilder();
                char ch;

                while(true)
                {
                    var res=sr.Read();
                    if (res < 0) continue;

                    ch = (char)res;
                    sb.Append(ch);

                    int ind = sb.ToString().IndexOf(MessageBoundary);
                    if (ind>=0)
                    {
                        var s = sb.ToString(0, ind);
                        if (OnReceive!=null)
                            OnReceive(s);

                        // сообщение для отправки клиенту
                        string response = "ProcessRequest function not set\r\n";
                        if (ProcessRequest != null)
                            response = ProcessRequest(s);

                        var i0 = ind + MessageBoundary.Length;
                        var s2 = i0<sb.Length?sb.ToString(i0, sb.Length):"";

                        sb.Clear();
                        sb.Append(s2);

                        // преобразуем сообщение в массив байтов
                        byte[] data = Encoding.ASCII.GetBytes(response);

                        // отправка сообщения
                        stream.Write(data, 0, data.Length);

                        if (!KeepAlive) break;
                    }
                    else if (EndsWith(sb, DisconnectString))
                    {
                        break;
                    }
                }

                //sr.Close();
                //stream.Flush();
                //stream.Close();

                // закрываем подключение
                client.Close();
            }
        }

        public void Close()
        {
            if(ServerThread!=null && ServerThread.IsAlive)
            {
                stop = true;
                Thread.Sleep(10);
                ServerThread.Abort();
            }
        }
    }
}
