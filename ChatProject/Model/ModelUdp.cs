using System;
using System.ComponentModel;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Configuration;


namespace ChatProject.Model
{
    #region Here goes evil event
    public delegate void ReceivedDataEventHandler(object sender, ReceivedDataEventArgs e);

    public class ReceivedDataEventArgs : EventArgs
    {
        public string ReceivedText;

        public ReceivedDataEventArgs(string text) { ReceivedText = text; }
    }

    #endregion Here goes evil event
    public class ModelUdp : INotifyPropertyChanged,ControllerToLoad
    {
        #region Fields
        Byte[] sendBytes = new byte[0];
        byte[] disconnectingTheReciever = new byte[0];
        UdpClient client = new UdpClient();
        public UdpClient udpClient;
        public IPEndPoint remote;
        public IPEndPoint sendTo;
        public Thread oThread;
        private string connectionText;
        public static bool MessageSent = false;     //flag to know the message sent status
        private string messageEncodedIntoString;
        private string messageToSend;
        #endregion Fields

        #region Events
        public AutoResetEvent _ready = new AutoResetEvent(false);   //Events to work along with threads
        public AutoResetEvent _go = new AutoResetEvent(false);
        public event PropertyChangedEventHandler PropertyChanged;
        public event ReceivedDataEventHandler DataReceived;
        #endregion Events

        #region AllMethods

        string localIp = ConfigurationManager.AppSettings["IpForUdp"];      //acquiring the ip using app.conf
        int sendPort = Convert.ToInt32(ConfigurationManager.AppSettings.Get("sendPort"));
        int receivePort = Convert.ToInt32(ConfigurationManager.AppSettings.Get("receivePort"));
        public ModelUdp()
        {
            Initializing();     //Initializes the udpclient
        }

        public void Initializing()
        {
            try {
                if (udpClient == null)
                    udpClient = new UdpClient(receivePort);  //Must define a port to let the udpclient know at which port to receive
                remote = new IPEndPoint(IPAddress.Any, receivePort);  //receives data from any host on specified port
                if (udpClient != null)
                {
                    connectionText = "Client has started";
                }
            }
            catch(Exception ex) { ex.ToString(); }
        }

        public void ConnectClientPart()
        {
            oThread = new Thread(Start);    //Udp is started on a different thread
            oThread.Start();
            oThread.Name = "WorkerThreadUdp";
        }

        public void Start()
        {
            sendTo = new IPEndPoint(IPAddress.Parse(localIp), sendPort);
            udpClient.Connect(sendTo);
            while (true)
            {
                if (disconnectingTheReciever.Length == 0)       //Recieves messages, for that asyncCallback is used
                {
                    udpClient.BeginReceive(new AsyncCallback(CallBackMethod), null); // need changes
                }
                if (sendBytes.Length > 0)       //Sends messages
                {
                    udpClient.Send(sendBytes, sendBytes.Length);
                    sendBytes = new byte[0];
                }
                Thread.Sleep(50);
            }
        }

        public void CallBackMethod(IAsyncResult ar)
        {
            try
            {
                remote = new IPEndPoint(IPAddress.Any, receivePort);       //receives data from any host on specified port
                byte[] disconnectingTheReciever = udpClient.EndReceive(ar, ref remote);
                if (disconnectingTheReciever.Length > 0)
                {
                    messageEncodedIntoString = Encoding.GetEncoding("Windows-1250").GetString(disconnectingTheReciever); //EncodeIntoString1.GetString(disconnectingTheReciever);
                    ReceivedDataEventArgs ReceivedDataEvent = new ReceivedDataEventArgs(messageEncodedIntoString);
                    if (DataReceived != null)
                    {
                        DataReceived(this, ReceivedDataEvent);      //Everytime a messages arrives this event occurs
                    }
                }
                disconnectingTheReciever = new byte[0];
            }
            catch (Exception e) { e.ToString(); }
        }

        public void SendMessages(string message)        //Preparing the byte to send
        {
            MessageToSend = message;
            try
            {
                sendBytes = Encoding.ASCII.GetBytes(message);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

       public void ClosingThreads()     //Each time the radio button changes, this closing happens
        {
            try {
                if (udpClient != null)
                {
                    udpClient.Close();      
                    oThread.Abort();
                    oThread.Join();
                }
            }catch(Exception ex) { ex.ToString(); }
        }
        #endregion AllMethods

        #region Properties
        public string MessageToSend
        {
            get { return messageToSend; }
            set
            {
                messageToSend = value; OnPropertyChanged("MessageToSend");
            }
        }
        public string ConnectionText
        {
            get { return connectionText; }
            set { connectionText = value; }
        }
        #endregion Properties
    }

}