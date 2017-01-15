using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Configuration;

namespace ChatProject
{
    public partial class ConfigurationForm : Form
    {
        //Form that opens for the user to provide changes for the application
        public ConfigurationForm()
        {
            InitializeComponent(); 
        }
        public Configuration Config;
        public string SendPortFromUser;
        public string ReceivePortFromUser;
        public string IpFromUser;
        public string RemoteIp;
        public string RemoteTcpPort;
        public const string SendPort = "sendPort";
        public const string ReceivePort = "receivePort";
        public const string Ip = "IpForUdp";
        public const string RemoteIpTcp = "remoteIpTcp";
        public const string ClientPortTcp = "clientPortTcp";


        private void ConfigurationForm_Load(object sender, EventArgs e)
        {
            Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            PortTextBox.Text = Config.AppSettings.Settings[SendPort].Value;
            Port2TextBox.Text = Config.AppSettings.Settings[ReceivePort].Value;
            IpTextBox.Text = Config.AppSettings.Settings[Ip].Value;
            IpforTcpTextBox.Text = Config.AppSettings.Settings[RemoteIpTcp].Value;
            RemoteTcpPortTextBox.Text = Config.AppSettings.Settings[ClientPortTcp].Value;

            this.FormClosing += new FormClosingEventHandler(ConfigurationForm_FormClosing);
        }

        private void portTextBox_TextChanged(object sender, EventArgs e)
        {
            SendPortFromUser = PortTextBox.Text;
        }

        private void port2TextBox_TextChanged(object sender, EventArgs e)
        {
            ReceivePortFromUser = Port2TextBox.Text;
        }

        private void ipTextBox_TextChanged(object sender, EventArgs e)
        {
            IpFromUser = IpTextBox.Text;
        }

        private void ipforTcpTextBox_TextChanged(object sender, EventArgs e)
        {
            RemoteIp = IpforTcpTextBox.Text;
        }

        private void remoteTcpPortTextBox_TextChanged(object sender, EventArgs e)
        {
            RemoteTcpPort = RemoteTcpPortTextBox.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(SendPortFromUser))
            {
                MessageBox.Show("you haven't specified any port to send data");
            }
            else if (SendPort != Config.AppSettings.Settings[SendPort].Value)
            { dic.Add(SendPort, SendPortFromUser); }
            if (string.IsNullOrWhiteSpace(ReceivePortFromUser))
            {
                MessageBox.Show("you haven't specified any port to receive data");
            }
            else if (SendPort != Config.AppSettings.Settings[ReceivePort].Value)
            { dic.Add(ReceivePort, ReceivePortFromUser); }
            if (string.IsNullOrWhiteSpace(IpFromUser))
            {
                MessageBox.Show("you haven't specified any ip adress to send data");
            }
            else if (Ip != Config.AppSettings.Settings[Ip].Value)
            { dic.Add(Ip, IpFromUser); }

            if (string.IsNullOrWhiteSpace(RemoteIp))
            {
                MessageBox.Show("you haven't specified any Remote Ip to connect");
            }
            else if (RemoteIpTcp != Config.AppSettings.Settings[RemoteIpTcp].Value)
            { dic.Add(RemoteIpTcp, RemoteIp); }

            if (string.IsNullOrWhiteSpace(RemoteTcpPort))
            {
                MessageBox.Show("you haven't specified any Remote Tcp port to send data");
            }
            else if (ClientPortTcp != Config.AppSettings.Settings[ClientPortTcp].Value)
            { dic.Add(ClientPortTcp, RemoteTcpPort); }

            Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            foreach (KeyValuePair<string, string> kvp in dic)
            {
                Config.AppSettings.Settings[kvp.Key].Value = kvp.Value;
            }
            Config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("//appSettings");
            MessageBox.Show("your chanages have been saved");
        }
        public void ConfigurationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
