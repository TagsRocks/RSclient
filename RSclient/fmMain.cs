﻿using System;
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
        List<ThreadParams> tps = new List<ThreadParams>();
        public fmMain()
        {
            InitializeComponent();
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            //clientThreads.Add(new Thread(new ParameterizedThreadStart(new Worker().doWork)));
            //tps.Add(new ThreadParams("test1", "test", mainData));
            clientThreads.Add(new Thread(new ParameterizedThreadStart(new Worker().doWork)));
            tps.Add(new ThreadParams("traider2", "SNmLwpLn", mainData));
            
            for (int i = 0; i < clientThreads.Count; i++)
            {
                clientThreads[i].Name = @"Worker: %username%";
                clientThreads[i].Start(tps[i]);
                clientThreads[i].IsBackground = true;
            }
        }

        
    }
}
