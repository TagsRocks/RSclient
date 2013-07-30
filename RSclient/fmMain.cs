using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RSclient
{
    public partial class fmMain : Form
    {
        private Config config = new Config();
        public MainData mainData = new MainData();
        List<Thread> clientThreads = new List<Thread>();
        public fmMain()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            clientThreads.Add(new Thread(new ParameterizedThreadStart(new Worker().doWork)));
            for (int i = 0; i < clientThreads.Count; i++)
            {
                ThreadParams tp = new ThreadParams("tRaider", "SNmLwpNm", mainData);
                clientThreads[i].Name = @"Worker: %username%";
                clientThreads[i].Start(tp);
                clientThreads[i].IsBackground = true;
            }
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            
        }

        
    }
}
