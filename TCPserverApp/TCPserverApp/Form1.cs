﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCPserverApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            f1 = this;            
        }
        public static Form1 f1;

        private void Form1_Load(object sender, EventArgs e)
        {
 TcpServer Ts = new TcpServer();

            var currentIP = "127.0.0.1";// Helper.GetLocalIPAddress();//"192.168.0.9"; //GetLocalIPAddress();
            var currentIPstring= currentIP.ToString();
            Ts.Start(currentIPstring, 5555);
            Ts.OnReceive = s => invoke(() =>
            {
                richTextBox1.Text = s;// s это получаемое сообщение
            });//получение строки s
            Ts.ProcessRequest = s => s + "**********\r\n";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

           


        }
        void invoke(Action a) { Invoke(a); }

        
    }
}
