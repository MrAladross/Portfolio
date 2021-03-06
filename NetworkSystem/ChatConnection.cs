using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Net;
using System.Text;
using System.Net.NetworkInformation;
using UnityEngine.UI;
public class ChatConnection : MonoBehaviour
{
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
    //needs a method to send to endpoint -- currently uses byte 45 as message waiting, 44 means no message
    //needs a method to receive from endpoint
    //needs a username
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
