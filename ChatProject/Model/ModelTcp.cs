using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Net;
using System.Net.Sockets;


namespace ChatProject.Model
{
    public class ModelTcp : INotifyPropertyChanged, ControllerToLoad
    {
        #region Fields
        private string messageToSend;
        BackgroundWorker worker;
        BackgroundWorker workerForconnect;
        public bool MCompleted;
        public bool MRequested;
        public bool WCompleted1;
        public bool MRequested1;
        public const int ServerPort = 3000;
        public bool Running = true;
        public TcpListener Listener;
        public NetworkStream NetStream;
        public TcpClient ServerSocket = null;
        public byte[] Buffer = new byte[1024];
        byte[] byteSent = new byte[1024];
        private string connectionText;
        public string ReadString = string.Empty;
        #endregion Fields

        //String localIp = ConfigurationManager.AppSettings["Ip"];
        //int serverport = Convert.ToInt32(ConfigurationManager.AppSettings.Get("serverPortTcp"));

        #region Events
        public AutoResetEvent _readyTcp = new AutoResetEvent(false);        //To manage threads these events are helpful
        public AutoResetEvent _goTcp = new AutoResetEvent(false);
        AutoResetEvent stop = new AutoResetEvent(false);
        public event ReceivedDataEventHandler DataReceived;
        #endregion Events

        #region ServerPart
        public ModelTcp()
        {
            Initializing();         //Initialized the listener on a different background thread
        }
        public void Initializing()
        {
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);     //Yes, on a different thread starts the listener
            worker.RunWorkerAsync();
        }

        public void CreateListener()        // A Tcp listener is to start everytime the tcp radio button is clicked cause it accepts incoming connection
        {
            try
            {
                Listener = null;
                IPAddress[] ipadd = Array.FindAll(Dns.GetHostEntry(string.Empty).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
                if (ipadd != null && ipadd.Length > 0)
                {
                    Listener = new TcpListener(ipadd[0], ServerPort);   //The local ip and assigned port for the listener
                    Listener.Start();
                    stop.Set();
                    while (true)
                    {
                        try
                        {
                            if (Listener.Pending())
                            {
                                ServerSocket = new TcpClient();
                                ServerSocket = Listener.AcceptTcpClient();
                            }
                            if (ServerSocket != null)
                            {
                                if (ServerSocket.Available > 0)
                                {
                                    NetStream = ServerSocket.GetStream();       //Tcp needs an ongoing stream for receiving and sending messages
                                    ConnectServerPart(ServerSocket);                                 
                                }
                            }
                        }
                        catch (Exception ex) { ex.ToString(); }
                    }
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }

        public void ConnectServerPart(TcpClient ServerSocket)     //Receive messages only after a connection is created
        {
            TcpClient ClientSocketIn = ServerSocket;
            while (true)
            {
                Thread.Sleep(10);
                try
                {
                    if (ClientSocketIn.Available != 0)
                    {
                        NetStream = ClientSocketIn.GetStream();
                        byte[] inStream = new byte[1024];
                        int buffersize = ServerSocket.ReceiveBufferSize = 1024;
                        if (ServerSocket.Available!=0)
                        {
                            NetStream.BeginRead(inStream, 0, inStream.Length, new System.AsyncCallback(AsyncCallback), null);
                            string dataToRead = Encoding.ASCII.GetString(inStream);
                            ReceivedDataEventArgs dataReceivedEvent = new ReceivedDataEventArgs(dataToRead);
                            if (DataReceived != null)
                            {
                                DataReceived(this, dataReceivedEvent);  
                            }
                        }

                    }

                }
                catch (Exception ex) { ex.ToString(); }
            }
        }

        private void AsyncCallback(IAsyncResult ar)     //Asynchronized callback
        {
            try
            {
                int read = NetStream.EndRead(ar);
                int bufferSize = 256;
                byte[] byteToRead = new byte[bufferSize];
                if (read > 0)
                {
                    ReadString = string.Concat(ReadString, Encoding.ASCII.GetString(byteToRead, 0, read));
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }


        public void serverDisconnect()
        {
            try
            {


            }
            catch (Exception ex) { ex.ToString(); }

        }

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Background Worker
        void worker_DoWork(object sender, DoWorkEventArgs e)    //The background thread
        {
            if (worker.CancellationPending)
            {
                e.Cancel = true;

            }
            else if (worker.CancellationPending == false)
            {
                CreateListener();
                MRequested = true;
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                MCompleted = true;
            }
            else if (!(e.Error == null))
            {
                string errorMessage = e.Error.Message;
            }
        }

        void worker_DoworkerConnect(object sender, DoWorkEventArgs e)
        {
            if (!this.workerForconnect.CancellationPending)
            {
                e.Cancel = true;
            }
            else if (workerForconnect.CancellationPending == false)
            {
                MRequested1 = true;
            }
        }
        void workerForConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                WCompleted1 = true;
            }
            else if (!(e.Error == null))
            {
                string errorMessage1 = e.Error.Message;
            }
        }
        #endregion Background Worker

        #region Properties
        public string MessageToSend     //Preparing the message that is to send
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

        #endregion ServerPart

        #region clientPart

        Thread messageThread;
        public TcpClient TcpClientForClient = null;   //Client side TcpClient
        NetworkStream serverStream = null;
        byte[] byteToServer = new byte[1024];
        string readData = string.Empty;
        public string _Text;
        public string Hello = "hello";
        public string _ConnectText;

        public string RemoteIp = ConfigurationManager.AppSettings["remoteIpTcp"]; //acquiring the ip using app.conf
        int clientPort = Convert.ToInt32(ConfigurationManager.AppSettings.Get("clientPortTcp"));

        public void ConnectClientPart()
        {
            if (Listener == null)
            stop.WaitOne(); //stops here
            try
            {
                TcpClientForClient = new TcpClient();
                try
                {
                    TcpClientForClient.Connect(IPAddress.Parse(RemoteIp), clientPort);
                }catch(Exception ex) { ex.ToString(); }
                
                if (TcpClientForClient.Connected)
                {
                    connectionText = "Connected";
                    serverStream = TcpClientForClient.GetStream();
                    byteToServer = Encoding.ASCII.GetBytes(Hello);
                    serverStream.Write(byteToServer, 0, byteToServer.Length);
                    serverStream.Flush();
                }//need a check later

                if (TcpClientForClient.Connected)
                {
                    messageThread = new Thread(GetMessage);
                    messageThread.IsBackground = true;
                    messageThread.Name = "getMessage";
                    messageThread.Start();
                }
            }
            catch (ArgumentNullException ex) { ex.ToString(); }
            catch (SocketException ex) { ex.ToString(); }
        }

        public void GetMessage()
        {
            while (true)
            {
                Thread.Sleep(20);
                if (TcpClientForClient!=null&&TcpClientForClient.Available > 0) //object dispose exception because closing thread was called before it
                {
                    serverStream = TcpClientForClient.GetStream();
                    byte[] inStream = new byte[1024];
                    int buffersize = TcpClientForClient.ReceiveBufferSize = 1024;
                    serverStream.Read(inStream, 0, buffersize);
                    string dataToRead = Encoding.ASCII.GetString(inStream);
                    ReceivedDataEventArgs dataReceivedEvent = new ReceivedDataEventArgs(dataToRead);
                    if (DataReceived != null)
                    {
                        DataReceived(this, dataReceivedEvent);  // Sometimes the event keeps null because it never gets initialized, unless you initialize in constructor or in setController.
                    }
                }
            }
        }

        public void SendMessages(string message)
        {
            try
            {
                if (ServerSocket != null)
                {
                    byteToServer = Encoding.ASCII.GetBytes(message);
                    NetStream.Write(byteToServer, 0, byteToServer.Length);
                    NetStream.Flush();
                }
                else if (TcpClientForClient.Connected)
                {
                    byteToServer = Encoding.ASCII.GetBytes(message);
                    serverStream.Write(byteToServer, 0, byteToServer.Length);
                    serverStream.Flush();
                }
            }
            catch (Exception ex) { ex.ToString(); }
        }

        public void ClosingThreads()
        {
            try {
                if (TcpClientForClient.Available == 0)
                {
                    TcpClientForClient.Close();
                    serverStream.Close();
                    if (messageThread != null)
                    {
                        messageThread.Abort();
                        messageThread.Join(500);
                    }
                }
                if (Listener != null)
                {
                    Listener.Stop();
                    if (NetStream != null)
                        ServerSocket.GetStream().Close();
                    if (ServerSocket != null)
                    {
                        ServerSocket.Close();
                    }
                } 
        }catch(Exception ex) { ex.ToString(); } //exception because ojbect reference not set to an object!!!
    }

        #endregion clientPart
    }
}
