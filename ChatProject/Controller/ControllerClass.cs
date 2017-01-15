using System.Collections.Generic;

namespace ChatProject.Controller
{
    public class ControllerClass
    {
        internal ControllerToLoad controllerToLoad;
        public List<string> TextBoxText;
        private Form1 view;

        #region All You Methods
        public ControllerClass(Form1 view, ControllerToLoad controllerToLoad)
        {
            this.view = view;
            this.controllerToLoad = controllerToLoad;
            TextBoxText = new List<string>();
        }

        public void AddTexts(string text)
        {
            TextBoxText.Add(text);
        }

        public void SendMessageViaUdp(string message)
        {
            controllerToLoad.SendMessages(message);
        }

        public void SendMessageViaTcp(string message)
        {
           controllerToLoad.SendMessages(message);
        }
        #endregion All You Methods
    }
}