using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mhyprot2Wrapper;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Library library = new Library();
            string mspd = textBox1.Text;
            float mspd1 = float.Parse(mspd, CultureInfo.InvariantCulture.NumberFormat);
            string aspd = textBox2.Text;
            int aspd1 = Int32.Parse(aspd, NumberStyles.Integer);
            if (mspd1 != 0 && aspd1 != 0)
            {
                library.OpenProcess((uint)Process.GetProcessesByName("ProjectN-Win64-Shipping")[0].Id);
                library.Write<float>(mspd1, "ProjectN-Win64-Shipping.exe+0636F5C8,0,A0,288,18C");
                library.Write<int>(aspd1, "ProjectN-Win64-Shipping.exe+0636F5C8,0,20,804");
                library.CloseDriver();
                Thread.Sleep(100);
            }
        }
    }
}
