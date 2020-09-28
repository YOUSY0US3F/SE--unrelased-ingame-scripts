/*
 * R e a d m e
 * -----------
 * 
 * In this file you can include any instructions or other comments you want to have injected onto the 
 * top of your final script. You can safely delete this file if you do not want any such comments.
 */

//This script takes your arguments ans sends it to all programmable blocks using this script as a meesage
//declare variables
//change broadcast tag to whatever you like as long as everyone else is using it
string broadcastTag = "Channel Cool";

MyIGCMessage message = new MyIGCMessage();
List<IMyBroadcastListener> listeners = new List<IMyBroadcastListener>();
// set the name you want displayed next to what you sent
string username = "Unamed";

IMyTextPanel display;
int messagecount = 0;
public Program()
{
    Runtime.UpdateFrequency = UpdateFrequency.Update100;
    IGC.RegisterBroadcastListener(broadcastTag);
    //set LCD panel to the name of what ever text display you intend on using
    display = GridTerminalSystem.GetBlockWithName("LCD Panel") as IMyTextPanel;
    IGC.GetBroadcastListeners(listeners);

}

public void Main(string argument, UpdateType updateSource)
{
    //set the argumant to ClearScreen to clear the screen
    //with the default font a small LCD screen can hold 16 messages
    if (messagecount > 16 || argument.Equals("/ClearScreen"))
    {
        display.WriteText("");
        messagecount = 0;
    }

    if (display != null && argument != "" && !argument.Equals("/ClearScreen"))
    {
        IGC.SendBroadcastMessage(broadcastTag, username + ": " + argument, TransmissionDistance.TransmissionDistanceMax);
        display.WriteText(username + ": " + argument + "\n", true);
        messagecount++;
    }
    //displays recieved messages
    while (listeners[0].HasPendingMessage)
    {
        message = listeners[0].AcceptMessage();
        display.WriteText(message.Data.ToString() + "\n", true);
        messagecount++;
    }
}