using ChatProject.Model;

namespace ChatProject
{
    public interface ControllerToLoad //Interface decouples dependencies
    {
        string ConnectionText { get; set; } 
        string MessageToSend { get; set; }
        void Initializing();
        void ConnectClientPart();
        void SendMessages(string text);
        void ClosingThreads();
        event ReceivedDataEventHandler DataReceived;
        
    }
}
