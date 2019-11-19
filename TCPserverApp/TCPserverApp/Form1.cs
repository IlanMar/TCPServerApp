using System;
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

        }
    }
}
