using ChatProject.Controller;
using ChatProject.Model;
using System;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Threading;



namespace ChatProject
{
    static class Program
    {
        #region All Fields
        private static Form1 view;
        private static ControllerToLoad controllerA;    //the interface is to bring in Interface segragation principle
        private static ControllerToLoad controllerB;
        public static Thread ShowD;
        public static bool Release = true;
        #endregion All Fields

        #region Delegate
        public delegate void SetVisible(bool visible);

        public static void DelegateMethod(bool visible)
        {
            view.Visible = visible;
        }
        #endregion Delegate
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        #region The Main Region
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //App.config determines which type of messenger is to start 
            string udp = ConfigurationManager.AppSettings["Udp"];
            bool udpValue = Convert.ToBoolean(udp);
            var tcp = ConfigurationManager.AppSettings["Tcp"];
            bool tcpValue = Convert.ToBoolean(tcp);
            bool radioButtonChecked1 = true, radioButtonChecked2 = false;

            if (udpValue == true) // By default starts Udp
            {
                controllerA = new ModelUdp();
                view = new Form1(controllerA);
                view.Visible = false;
                var controller = new ControllerClass(view, controllerA);
                view.SetController(controller);
                controllerA.ConnectClientPart();  //Here starts the connection of udp
                ShowD = new Thread(showd);
                ShowD.Name = "Dialog";
                ShowD.IsBackground = true;
                ShowD.Start();
                view.Stop.Set();
            }
          
            while (Release)
            {
                Thread.Sleep(500);

                if (view.radioButton1.Checked && view.radioButton1.Checked != radioButtonChecked1 && controllerA==null)     //Taking in consideration all possible happenings
                {
                    radioButtonChecked1 = view.radioButton1.Checked;
                    radioButtonChecked2 = view.radioButton2.Checked;
                    view.ReleaseController.WaitOne();
                    controllerA = new ModelUdp();  // udp starts loading

                    if (view == null)
                        view = new Form1(controllerA);
                    var controller = new ControllerClass(view, controllerA); 

                    view.SetController(controller);     //Sets controller to load 
                    controllerB = null;     //ControllerB, in other words to avoid collision with another controller
                    view.Stop.Set();
                    controllerA.ConnectClientPart();
                }
                else if (view.radioButton2.Checked && view.radioButton2.Checked != radioButtonChecked2 && controllerB==null)
                {
                    radioButtonChecked1 = view.radioButton1.Checked;
                    radioButtonChecked2 = view.radioButton2.Checked;
                    view.ReleaseController.WaitOne();
                    controllerB = new ModelTcp();       // Through interface loads the TcpModel

                    if (view == null)
                        view = new Form1(controllerB);

                    var controllerTcp = new ControllerClass(view, controllerB);
                    view.SetController(controllerTcp);
                    controllerA = null;
                    view.Stop.Set();
                }
            }
            Application.Exit();
        }
        static void showd()
        {
            view.ShowDialog();
            Release = false;
        }
        #endregion The Main Region
    }
}
