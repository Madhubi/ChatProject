using System;
using System.Windows.Forms;
using ChatProject.Model;
using ChatProject.Controller;
using System.Threading;
using System.Configuration; // 
using System.Drawing;

namespace ChatProject //
{
    public partial class Form1 : Form
    {
        #region Fields&Events
        public ModelUdp ModelUdp;
        ControllerClass controller;
        ControllerToLoad controllerToLoad;
        public Configuration Config;
        public bool Flag;
        string ReceivedString;
        public string TextFromTextBox;

        public AutoResetEvent ReleaseController = new AutoResetEvent(false);
        public AutoResetEvent Stop = new AutoResetEvent(false);
        public AutoResetEvent Go = new AutoResetEvent(false);
        #endregion Fields&Events

        #region FormLoad,Controller
        public Form1(ControllerToLoad cont)// : this() Proper controller is given to
        {
            InitializeComponent();
            controllerToLoad = cont;
        }

        public void SetController(ControllerClass controller)   //Sets the controller
        {
            controllerToLoad = controller.controllerToLoad;
            this.controller = controller;
            this.controllerToLoad.DataReceived += Model_DataReceived; //otherwise event would be null
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            listBox1.MeasureItem += lst_MeasureItem;
            listBox1.DrawItem += lst_DrawItem;
        }

        private void lst_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString(listBox1.Items[e.Index].ToString(), listBox1.Font, listBox1.Width).Height;
        }

        private void lst_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }

        #endregion FormLoad,Controller

        #region All CheckedChanged
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
         {
              
            if (radioButton1.Checked)
            {
                Flag = true;                
                Stop.WaitOne();
                textBox1.DataBindings.Clear();
                textBox1.DataBindings.Add("Text", controllerToLoad, "MessageToSend", true, DataSourceUpdateMode.OnPropertyChanged);
               
                if (controllerToLoad!=null)
                {
                    /*controller.AddTexts(controllerToLoad.ConnectionText);
                    listBox1.DataSource = null;
                    listBox1.DataSource = controller.TextBoxText;
                    this.listBox1.SelectedIndex = (listBox1.Items.Count - 1);*/
                }
                else { MessageBox.Show("Client isn't available"); }  //when tcp is active udp need to be totally off; 
            }
            else
            {               
                controllerToLoad.ClosingThreads();
                ReleaseController.Set();
            }
        
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                Flag = true;
            }
            else
            {
                controllerToLoad.ClosingThreads();
                ReleaseController.Set();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
                button1.Enabled = false;
            else
                button1.Enabled = true;
        }
        #endregion All CheckedChanged

        #region All ClickedEvents
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) {
                controller.SendMessageViaUdp(textBox1.Text);
                AddsToTheListBox();
            }
            else if(radioButton2.Checked){
                controller.SendMessageViaTcp(textBox1.Text);
                AddsToTheListBox();
            }
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                controllerToLoad.ConnectClientPart();

                if (controllerToLoad.ConnectionText != null) //
                {
                    controller.AddTexts(controllerToLoad.ConnectionText);
                    listBox1.DataSource = null;
                    listBox1.DataSource = controller.TextBoxText;
                    this.listBox1.SelectedIndex = (listBox1.Items.Count - 1);
                    this.listBox1.SelectedItem = null;
                }
                else { MessageBox.Show("Connection attempt was failed because the target machine actively refused it"); }

            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void portNoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurationForm CF = new ConfigurationForm();
            ConfigurationManager.RefreshSection("//appsettings");
            CF.Refresh();

            CF.Invalidate();
            Application.DoEvents();
            CF.Show();
        }
        #endregion All ClickedEvents

        #region ReceiveEvents
        public string _ReceivedString
        {
            get { return ReceivedString; }
        }

        public void Model_DataReceived(object sender, ReceivedDataEventArgs e)
        {
            ReceivedString = e.ReceivedText;
            if (listBox1.InvokeRequired)
            {
                listBox1.BeginInvoke(new InvokeDelegate(InvokeMethod));
                updateListBox();
            }
            else { controller.AddTexts(e.ReceivedText); }
        }

        public delegate void InvokeDelegate();
        public void InvokeMethod()
        {
            controller.AddTexts("Friend:" + _ReceivedString);
            listBox1.DataSource = null;
            listBox1.DataSource = controller.TextBoxText;
            this.listBox1.SelectedIndex = (listBox1.Items.Count - 1);
            this.listBox1.SelectedItem = null;
        }
        #endregion ReceiveEvents

        #region Related to ListBox
        void updateListBox()   //method to prevent 
        {
            int count = listBox1.Items.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (listBox1.Items[i].ToString() == "")
                {
                    listBox1.Items.RemoveAt(i);
                }
            }

        }
        public void AddsToTheListBox()
        {
            listBox1.ClearSelected();
            controller.AddTexts("You:" + textBox1.Text);
            listBox1.DataSource = null;
            listBox1.DataSource = controller.TextBoxText;
            this.listBox1.SelectedIndex = (listBox1.Items.Count - 1);     //focus on last entry
            listBox1.SelectedIndex = -1;
            this.textBox1.Clear();
        }

        
        public void AddLine(string text)        //invoking this from another thread to update listbox
        {
            if (listBox1.InvokeRequired)
            {
                Action<string> action = SetText;
                Invoke(action, new object[] { text });
            }
        }
        private void SetText(string text)
        {
            listBox1.Items.Add(text);
        }
        #endregion Related to ListBox

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = false;
            }
            if (MessageBox.Show("This will close down the whole application. Confirm?", "Close Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (radioButton1.Checked)
                {
                    controllerToLoad.ClosingThreads();
                }
                else if (radioButton2.Checked)
                {
                    controllerToLoad.ClosingThreads();
                }
                Application.ExitThread();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();      //Restarts the whole application
            
        }

       
    }
}
