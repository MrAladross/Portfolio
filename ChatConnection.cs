using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.UI;
public class StateObject
{
    // Size of receive buffer.  
    public const int BufferSize = 1024;

    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];

    // Received data string.
    public StringBuilder sb = new StringBuilder();

    // Client socket.
    public Socket workSocket = null;
}  
public class ChatConnection : MonoBehaviour
{
    //TODO: still need to wire up a couple things including moving messages from these lists to chatlog via displaymessage
    //TODO: still need to collect chat inputs without blocking send/receive
    //TODO: still need to beginconnection once connection is found.


    private ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

    public void QueueMessage()
    {
        _messagesToSend.Add(username+": "+_messageInputField.text);
        _messageInputField.text = "";
        _messageInputField.Select();
    }
/// <summary>
/// used for messages on a separate thread
/// </summary>
    void QueueSocketMessage(string s)
{
    _messagesToDisplay.Add(s);
}

    public void SetUsername()
    {
        username = _usernameInputField.text;
    }
    
    private List<string> _messagesToSend = new List<string>();
    private List<string> _messagesToDisplay = new List<string>();
    [SerializeField] private InputField _messageInputField;
    [SerializeField] private InputField _usernameInputField;
    [SerializeField]
    private GameEvent _chatMessage;

    [SerializeField] private LocalNetworkFinder _localNetworkFinder;

    [SerializeField]
    public HashSet<string> ipaddresses = new HashSet<string>();
    private void Awake()
    {
        _localNetworkFinder._chatConnection = this;
        ipAddress =GetLocalIPAddress();
        int localport = 27504;
        ipEnd = new IPEndPoint(IPAddress.Parse( ipAddress), localport);
        connection = new Socket(IPAddress.Parse(ipAddress).AddressFamily, SocketType.Stream,
            ProtocolType.Tcp);
        connection.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        connection.Bind(ipEnd);
        connection.Blocking = true;
        serverButtonText = serverButton.GetComponentInChildren<Text>();
    }
    string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                Console.WriteLine(ip.ToString());
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
    private string username = "UnityUser";
    public void DisplayMessage(string message)
    {
        _chatMessage.InvokeWithMessage(message);
    }

    public bool isServer = false;
    [SerializeField]
    private Button serverButton;
    private Text serverButtonText;
    public void ToggleIsServer()
    {
        isServer = !isServer;
        if (isServer)
            serverButtonText.text = "Host";
        else serverButtonText.text = "Join";
    }
    public void BeginConnection(string ipaddress,int port)
    {
        ipAddress =GetLocalIPAddress();
        Debug.Log(ipAddress);
        Debug.Log("Attempting to begin connection with "+ ipaddress+ ":"+port);
        int localport = 27504;
        ipEnd = new IPEndPoint(IPAddress.Parse( ipAddress), localport);
        //moved connection information to awake, but may revisit it here 
     //   connection = new Socket(IPAddress.Parse(ipAddress).AddressFamily, SocketType.Stream,
     //       ProtocolType.Tcp);
     //   connection.Bind(ipEnd);
     //   connection.Blocking = false;
        //handle connection info here
        System.Threading.Tasks.Task.Run(() =>
        {
                if (isServer)
                {
                    try
                    {
                        connection.Listen(1);
                        Debug.Log("listening");
                        
                        //TODO: implement async socket once familiarized with the data flow
                     //   _manualResetEvent.Reset();
                     //   connection.BeginAccept(new AsyncCallback(AcceptCallback), connection);
                     //   _manualResetEvent.WaitOne();
                        connection = connection.Accept();
                        _localNetworkFinder.isConnected = true;
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
                else
                {
                    try
                    {
                        connection.Connect(ipaddress, port);
                        _localNetworkFinder.isConnected = true;
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            
        });
        

    }
    //needs a connection
    //needs a method to send to endpoint
    //needs a method to receive from endpoint
    //needs a username

    #region  importedAsyncMethods
    //these are not being used in code. They are only examples for future if async methods are used.
    public void AcceptCallback(IAsyncResult ar)
    {
        // Signal the main thread to continue.  
        _manualResetEvent.Set();
        // Get the socket that handles the client request.  
        Socket listener = (Socket) ar.AsyncState;  
        Socket handler = listener.EndAccept(ar);  
  
        // Create the state object.  
        StateObject state = new StateObject();  
        state.workSocket = handler;  
        handler.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,  
            new AsyncCallback(ReadCallback), state);  
    }
    public static void ReadCallback(IAsyncResult ar)
    {
        //TODO: uses a tag to indicate end of data received
        //possible to receive incomplete package without end of stream notifier
        String content = String.Empty;  
  
        // Retrieve the state object and the handler socket  
        // from the asynchronous state object.  
        StateObject state = (StateObject) ar.AsyncState;  
        Socket handler = state.workSocket;  
  
        // Read data from the client socket.
        int bytesRead = handler.EndReceive(ar);  
  
        if (bytesRead > 0) {  
            // There  might be more data, so store the data received so far.  
            state.sb.Append(Encoding.ASCII.GetString(  
                state.buffer, 0, bytesRead));  
  
            // Check for end-of-file tag. If it is not there, read
            // more data.  
            content = state.sb.ToString();  
            if (content.IndexOf("<EOF>") > -1) {  
                // All the data has been read from the
                // client. Display it on the console.  
                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",  
                    content.Length, content );  
                // Echo the data back to the client.  
                
                //TODO: this is where the code continues after receive request is complete
                Send(handler, content);  
            } else {  
                // Not all data received. Get more.  
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,  
                    new AsyncCallback(ReadCallback), state);  
            }  
        }  
    }
    private static void Send(Socket handler, String data)
    {
        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);  
  
        // Begin sending the data to the remote device.  
        handler.BeginSend(byteData, 0, byteData.Length, 0,  
            new AsyncCallback(SendCallback), handler);  
        
        //TODO: continues to sendcallback method after data is sent
    }
    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket handler = (Socket) ar.AsyncState;  
  
            // Complete sending the data to the remote device.  
            int bytesSent = handler.EndSend(ar);  
            Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            //TODO: the socket is shut down in this example but will probably be left open and 
            //      assigned next operation
            handler.Shutdown(SocketShutdown.Both);  
            handler.Close();  
  
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());  
        }  
    }
    #endregion importedAsyncMethods
    
    private IPGlobalProperties ipProperties;
    private TcpConnectionInformation[] tcpConnections;
    private IPEndPoint ipEnd;
    [SerializeField] private string ipAddress;
    private Socket connection;
    [SerializeField] private int receivedByte=0;
    private bool isConnected()
    {
        //this works on windows devices and can be used at the server level to check when clients disconnect
        //tcpconnectioninformation isn't available on Android
        for (int i = 0; i < tcpConnections.Length; ++i)
        {
            if (tcpConnections != null && tcpConnections.Length > 0)
            {
                //must use tostring when checking equality of ipendpoints otherwise it returns not equal
                //connection refers to the socket used to track position of this particular object
                //tcpconnections is an array of all the active sockets on the device
                //network permissions may be required for this app to run
                if (tcpConnections[i].LocalEndPoint.ToString() == connection.LocalEndPoint.ToString())
                {
                    TcpState stateOfConnection = tcpConnections[i].State;
                    if (stateOfConnection == TcpState.Established)
                    {
                        // Connection is OK
                       Debug.Log("There is a connection at " + tcpConnections[i].LocalEndPoint +" with "+ tcpConnections[i].RemoteEndPoint);
                       return true;
                    }
                    else
                    {
                       Debug.Log("There is NOT a connection at " + tcpConnections[i].LocalEndPoint);
                        // No active tcp Connection to hostName:port
                        return false;
                    }
                }

            }
        }
        Debug.Log("no active connections found with the same ip address" + ipEnd);
        //there are no active connections to check
        return false;
    }
    private bool isreceiving = false;
    public void ChatNetworkUpdate()
    {
        while (_messagesToDisplay.Count > 0)
        {
            DisplayMessage(_messagesToDisplay[0]);
            _messagesToDisplay.Remove(_messagesToDisplay[0]);
        }
        /*
        #region isConnectedTCPSocket

        _localNetworkFinder.temp = "has checked neither network;";
        _localNetworkFinder.debugText.text = _localNetworkFinder.temp;

    ipProperties = IPGlobalProperties.GetIPGlobalProperties();
        _localNetworkFinder.temp = "has checked network part one";
        _localNetworkFinder.debugText.text = _localNetworkFinder.temp;

//android gets stuck here

            tcpConnections = ipProperties.GetActiveTcpConnections();
        _localNetworkFinder.temp = "has checked network part two";
        _localNetworkFinder.debugText.text = _localNetworkFinder.temp;

        #endregion isConnectedTCPSocket

      //  _localNetworkFinder.temp = isConnected().ToString();
    //    _localNetworkFinder.debugText.text = _localNetworkFinder.temp;

        if (!isConnected())
            return;
*/
        //TODO: need to fix this part asap
        //TODO: possible fixes include
        
        
        //TODO: --- This was the selected method for now ---
        //TODO: passing data from another thread which updates a list on the main thread
        //TODO: --- end selected method ---
        //TODO: using UDP instead of TCP
        //TODO: possible asyncsocket approach


        //Need to establish a host button and a connect button
        //since tcp requires sequence, a host will have to listen, client sends first message, then host responds
        //periodic send receive can have a byte signaling message waiting or not
        //example: send byte 44, no message to send. Receive byte 44, no message to receive
        //       : send byte 45, message to send. Receive byte 45, message to receive
        //       : bigger buffer to accomodate message. Message passed through tcp and received through tcp
        //order may be mixed up when messages are sent simultaneously
        if (!isreceiving)
        {
            isreceiving = true;

            System.Threading.Tasks.Task.Run(() =>
            {

                if (!isServer)
                {
                    try
                    {
                        #region transactionOne

                        //receive from other person to see if message waiting
                        byte[] result = new byte[1];
                        connection.Receive(result);
                        receivedByte = result[0];
                        //send to other person to indicate if message waiting
                        byte[] b = new byte[1];
                        if (_messagesToSend.Count > 0)
                            b[0] = (byte) 45; //message waiting
                        else
                        {
                            b[0] = (byte) 44; //no messages waiting
                        }

                        connection.Send(b);

                        #endregion

                        #region transactionTwo

                        //ready to send and receive messages
                        if (b[0] == 44 && receivedByte == 44)
                        {
                            //no messages waiting
                            isreceiving = false;
                            return;
                        }

                        if (b[0] == 45 && receivedByte == 44)
                        {

                            connection.Receive(result);
                            //send message, but receive none
                            byte[] message = Encoding.ASCII.GetBytes(_messagesToSend[0]);
                            connection.Send(message);
_messagesToDisplay.Add(_messagesToSend[0]);
                            _messagesToSend.Remove(_messagesToSend[0]);
                            isreceiving = false;

                            return;
                        }

                        if (b[0] == 45 && receivedByte == 45)
                        {
                            byte[] receivedMessage = new byte[128];
                            connection.Receive(receivedMessage);
                            _messagesToDisplay.Add(Encoding.ASCII.GetString(receivedMessage));

                            //send message and receive message
                            byte[] message = Encoding.ASCII.GetBytes(_messagesToSend[0]);
                            connection.Send(message);
                            _messagesToDisplay.Add(_messagesToSend[0]);
                            _messagesToSend.Remove(_messagesToSend[0]);
                            isreceiving = false;

                            return;
                        }

                        if (b[0] == 44 && receivedByte == 45)
                        {
                            byte[] receivedMessage = new byte[128];
                            connection.Receive(receivedMessage);
                            _messagesToDisplay.Add(Encoding.ASCII.GetString(receivedMessage));
                            //receive message, but send none
                            connection.Send(b);
                        }

                        isreceiving = false;

                        #endregion

                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                        isreceiving = false;
                       _localNetworkFinder.isConnected = false;
                       connection.Close();
                       
                       connection = new Socket(IPAddress.Parse(ipAddress).AddressFamily, SocketType.Stream,
                           ProtocolType.Tcp);
        
                       connection.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                       connection.Bind(ipEnd);
                    }
                }
                else //isServer ==true
                {
                    try
                    {
                        #region firstTransaction

//send to other person to indicate if message waiting
                        byte[] b = new byte[1];
                        if (_messagesToSend.Count > 0)
                            b[0] = (byte) 45; //message waiting
                        else
                        {
                            b[0] = (byte) 44; //no messages waiting
                        }

                        connection.Send(b);

                        //receive from other person to see if message waiting
                        byte[] result = new byte[1];
                        connection.Receive(result);
                        receivedByte = result[0];

                        #endregion

                        #region secondTransaction

                        //ready to send and receive messages
                        if (b[0] == 44 && receivedByte == 44)
                        {
                            //no messages waiting
                            isreceiving = false;

                            return;
                        }

                        if (b[0] == 45 && receivedByte == 44)
                        {

                            //send message, but receive none
                            byte[] message = Encoding.ASCII.GetBytes(_messagesToSend[0]);
                            connection.Send(message);                            
                            _messagesToDisplay.Add(_messagesToSend[0]);

                            _messagesToSend.Remove(_messagesToSend[0]);
                            connection.Receive(result);
                            isreceiving = false;

                            return;
                        }

                        if (b[0] == 45 && receivedByte == 45)
                        {
                            //send message and receive message
                            byte[] message = Encoding.ASCII.GetBytes(_messagesToSend[0]);
                            connection.Send(message);
                            _messagesToDisplay.Add(_messagesToSend[0]);

                            _messagesToSend.Remove(_messagesToSend[0]);

                            byte[] receivedMessage = new byte[128];
                            connection.Receive(receivedMessage);
                            _messagesToDisplay.Add(Encoding.ASCII.GetString(receivedMessage));
                            isreceiving = false;

                            return;
                        }

                        if (b[0] == 44 && receivedByte == 45)
                        {
                            Debug.Log("they are sending us some goods");
                            //receive message, but send none
                            connection.Send(b);
                            Debug.Log("they are sending us more goods");

                            byte[] receivedMessage = new byte[128];
                            connection.Receive(receivedMessage);
                            Debug.Log("they are sending us the most goods");
                            _messagesToDisplay.Add(Encoding.ASCII.GetString(receivedMessage));

                        }

                        isreceiving = false;

                        #endregion

                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                        isreceiving = false;
                        _localNetworkFinder.isConnected = false;
                        connection.Close();
                        connection = new Socket(IPAddress.Parse(ipAddress).AddressFamily, SocketType.Stream,
                            ProtocolType.Tcp);
        
                        connection.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        connection.Bind(ipEnd);
                    }
                }
            });
        }
    }
    private void OnApplicationQuit()
    {
        connection.Close();
    }
}
