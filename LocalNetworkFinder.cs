using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.UI;

public class LocalNetworkFinder : MonoBehaviour
{ 
    [SerializeField] public HashSet<string> ipaddresses = new HashSet<string>();
    [SerializeField] public Text debugText;
    public ChatConnection _chatConnection;
    [SerializeField] private InputField friendCodeInput;
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
    private Thread receiveThread;
    private UdpClient client;
    private UdpClient sendClient;
    public bool isConnected = false;
    public Transform chatFrame;

    void Debug(string s)
    {
        GameObject go = Instantiate(debugText.gameObject, chatFrame);
        go.GetComponent<Text>().text = s;
        UnityEngine.Debug.Log(s);
    }
    private void OnApplicationQuit()
    {
        receiveThread?.Abort();
        client?.Close();
        sendClient?.Close();
    }
    public string temp = "";
    int ByteWordToIndex(string word)
    {
        for (int i=0;i<_byteWords.Length;++i)
            if (_byteWords[i].Word == word)
                return i;
        return -1;
    }
    public void Connect()
    {
        _chatConnection.isServer = false;
        string localIpEndpoint = GetLocalIPAddress() + ":" + 27504;
        var from = new IPEndPoint(IPAddress.Any, 0);
        string code = friendCodeInput.text;
        int finalByte = ByteWordToIndex(code);
        if (finalByte == -1)
        {
            UnityEngine.Debug.Log("You put an invalid friend code.");
            temp = "You put an invalid friend code.";
            friendCodeInput.text = "";
            friendCodeInput.Select();
            return;
        }
        string localPrefix = GetIPPrefix(GetLocalIPAddress());
        string destinationIP = localPrefix + finalByte;
        temp = destinationIP;
        //destinationIP is the ip address of the person who's friend code you just typed into the inputfield
        System.Threading.Tasks.Task.Run(() =>
            {
                while (true)
                {
                    var datap = Encoding.UTF8.GetBytes(localIpEndpoint);
                    try
                    {    //TODO: port is arbitrary and will be turned into a variable
                        client.Send(datap, datap.Length, destinationIP, 27504);
                        temp = "sent a message to " + destinationIP + " from " + localIpEndpoint;
                        Thread.Sleep(1000);
                        _chatConnection.BeginConnection(destinationIP, 27504);
                        return;
                    }
                    catch (Exception e)
                    {
                        continue;
                        throw;
                    }
                    break;
                }
            });
    }
    public void Host()
    {
        _chatConnection.isServer = true;
        string localIpEndpoint = GetLocalIPAddress() + ":" + 27504;
        var from = new IPEndPoint(IPAddress.Any, 0);
        Debug("am hosting now");
            System.Threading.Tasks.Task.Run(() =>
            {
                while (true)
                {
                    UnityEngine.Debug.Log("Started listening");
                    var recvBuffer = client.Receive(ref from);
                    string rec = Encoding.UTF8.GetString(recvBuffer);
                    if (rec == localIpEndpoint)
                        continue;
                    if (EndpointStringToIP(rec) != "0.0.0.0" && EndpointStringToIP(rec) != "::0")
                    {
                        ipaddresses.Add(rec);
                        if (_chatConnection != null)
                            _chatConnection.ipaddresses.Add(rec);
                        try
                        {
                            UnityEngine.Debug.Log(rec); 
                            _chatConnection.BeginConnection(EndpointStringToIP(rec), 27504);
                        }
                        catch (Exception e)
                        {
                            UnityEngine.Debug.Log(e.ToString());
                            continue;
                            throw;
                        }

                    //    break;
                    }
                }
            });
    }
    void Start()
    {
        //shows the output on mobile by displaying a text object
        Debug(GetLocalIPAddress());
        //getipprefix also stores value for int lastByte
        //use lastByte as the index, starts at 0 ends at 255
        friendCodeText.text = _byteWords[lastByte].Word;
        string localIpEndpoint = GetLocalIPAddress() + ":" + 27504;
        client = new UdpClient();
        client.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
        client.Client.Bind(new IPEndPoint(IPAddress.Parse(EndpointStringToIP(localIpEndpoint)), 27504));
    }

    IEnumerator BroadCastMyIP(UdpClient client, byte[] data)
    {
        string prefix = GetIPPrefix(GetLocalIPAddress());
        client.Send(data, data.Length, "255.255.255.255", 27500);
        //client.Send(data, data.Length, "192.168.2.9", 27500);
        Debug("Broadcasting: " + Encoding.ASCII.GetString(data) + " to " + 27500);
        yield return new WaitForSeconds(1f);
        StartCoroutine(BroadCastMyIP(client, data));
    }

    string GetIPPrefix(string ipAddress)
    {
        string[] results = ipAddress.Split('.');
        string result = "";
        if (results.Length >= 3)
        {
            result += results[0];
            result += ".";
            result += results[1];
            result += ".";
            result += results[2];
            result += ".";

            lastByte = int.Parse(results[3]);
            UnityEngine.Debug.Log(lastByte);
        }

        return result;
    }
    private int lastByte = 0;
    private float timer = 0.25f;
    [SerializeField] private Text friendCodeText;

    private void Update()
    {
        debugText.text = temp;
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            timer = 0.05f;
            if (isConnected)
            _chatConnection.ChatNetworkUpdate();
        }
    }

    string EndpointStringToIP(string s)
    {
        string[] contents = s.Split(':');
        return contents[0];
    }

    int EndpointStringToPort(string s)
    {
        //this method isn't used, but may be in the future.
        string[] contents = s.Split(':');
        if (int.TryParse(contents[1], out int result))
            return result;
        //27509 is an arbitrary default port. 
        return 27509;
    }

    private ByteWord[] _byteWords =new ByteWord[]
    {
        new ByteWord(0, "ace"),
        new ByteWord(1, "act"),
        new ByteWord(2, "add"),
        new ByteWord(3, "aft"),
        new ByteWord(4, "age"),
        new ByteWord(5, "ago"),
        new ByteWord(6, "amp"),
        new ByteWord(7, "and"),
        new ByteWord(8, "ant"),
        new ByteWord(9, "any"),
        new ByteWord(10, "ape"),
        new ByteWord(11, "app"),
        new ByteWord(12, "apt"),
        new ByteWord(13, "arc"),
        new ByteWord(14, "are"),
        new ByteWord(15, "arm"),
        new ByteWord(16, "art"),
        new ByteWord(17, "ash"),
        new ByteWord(18, "ask"),
        new ByteWord(19, "ate"),
        new ByteWord(20, "awe"),
        new ByteWord(21, "awn"),
        new ByteWord(22, "axe"),
        new ByteWord(23, "bad"),
        new ByteWord(24, "bag"),
        new ByteWord(25, "ban"),
        new ByteWord(26, "bar"),
        new ByteWord(27, "bat"),
        new ByteWord(28, "bay"),
        new ByteWord(29, "bed"),
        new ByteWord(30, "bee"),
        new ByteWord(31, "beg"),
        new ByteWord(32, "bet"),
        new ByteWord(33, "boa"),
        new ByteWord(34, "bob"),
        new ByteWord(35, "bow"),
        new ByteWord(36, "box"),
        new ByteWord(37, "boy"),
        new ByteWord(38, "bro"),
        new ByteWord(39, "bug"),
        new ByteWord(40, "bun"),
        new ByteWord(41, "bus"),
        new ByteWord(42, "cab"),
        new ByteWord(43, "cad"),
        new ByteWord(44, "can"),
        new ByteWord(45, "cap"),
        new ByteWord(46, "car"),
        new ByteWord(47, "cat"),
        new ByteWord(48, "cod"),
        new ByteWord(49, "cog"),
        new ByteWord(50, "con"),
        new ByteWord(51, "cop"),
        new ByteWord(52, "cot"),
        new ByteWord(53, "cow"),
        new ByteWord(54, "coy"),
        new ByteWord(55, "cry"),
        new ByteWord(56, "cub"),
        new ByteWord(57, "cue"),
        new ByteWord(58, "cup"),
        new ByteWord(59, "cut"),
        new ByteWord(60, "dab"),
        new ByteWord(61, "dad"),
        new ByteWord(62, "day"),
        new ByteWord(63, "den"),
        new ByteWord(64, "dev"),
        new ByteWord(65, "dex"),
        new ByteWord(66, "doe"),
        new ByteWord(67, "dog"),
        new ByteWord(68, "don"),
        new ByteWord(69, "dot"),
        new ByteWord(70, "dug"),
        new ByteWord(71, "ear"),
        new ByteWord(72, "eat"),
        new ByteWord(73, "egg"),
        new ByteWord(74, "ego"),
        new ByteWord(75, "eon"),
        new ByteWord(76, "era"),
        new ByteWord(77, "fab"),
        new ByteWord(78, "fad"),
        new ByteWord(79, "fan"),
        new ByteWord(80, "far"),
        new ByteWord(81, "fax"),
        new ByteWord(82, "fed"),
        new ByteWord(83, "few"),
        new ByteWord(84, "fog"),
        new ByteWord(85, "for"),
        new ByteWord(86, "fox"),
        new ByteWord(87, "fry"),
        new ByteWord(88, "fun"),
        new ByteWord(89, "gap"),
        new ByteWord(90, "gem"),
        new ByteWord(91, "get"),
        new ByteWord(92, "got"),
        new ByteWord(93, "gum"),
        new ByteWord(94, "gut"),
        new ByteWord(95, "gym"),
        new ByteWord(96, "had"),
        new ByteWord(97, "has"),
        new ByteWord(98, "hat"),
        new ByteWord(99, "hem"),
        new ByteWord(100, "hen"),
        new ByteWord(101, "hex"),
        new ByteWord(102, "hmm"),
        new ByteWord(103, "hob"),
        new ByteWord(104, "hog"),
        new ByteWord(105, "hop"),
        new ByteWord(106, "hot"),
        new ByteWord(107, "how"),
        new ByteWord(108, "hue"),
        new ByteWord(109, "hug"),
        new ByteWord(110, "hut"),
        new ByteWord(111, "jab"),
        new ByteWord(112, "jam"),
        new ByteWord(113, "jar"),
        new ByteWord(114, "jay"),
        new ByteWord(115, "jet"),
        new ByteWord(116, "jog"),
        new ByteWord(117, "joy"),
        new ByteWord(118, "jut"),
        new ByteWord(119, "kay"),
        new ByteWord(120, "key"),
        new ByteWord(121, "mac"),
        new ByteWord(122, "mad"),
        new ByteWord(123, "man"),
        new ByteWord(124, "map"),
        new ByteWord(125, "mat"),
        new ByteWord(126, "maw"),
        new ByteWord(127, "max"),
        new ByteWord(128, "may"),
        new ByteWord(129, "med"),
        new ByteWord(130, "meh"),
        new ByteWord(131, "men"),
        new ByteWord(132, "met"),
        new ByteWord(133, "mom"),
        new ByteWord(134, "moo"),
        new ByteWord(135, "mop"),
        new ByteWord(136, "mow"),
        new ByteWord(137, "mud"),
        new ByteWord(138, "nap"),
        new ByteWord(139, "net"),
        new ByteWord(140, "new"),
        new ByteWord(141, "nod"),
        new ByteWord(142, "nor"),
        new ByteWord(143, "not"),
        new ByteWord(144, "now"),
        new ByteWord(145, "oak"),
        new ByteWord(146, "oat"),
        new ByteWord(147, "odd"),
        new ByteWord(148, "ohm"),
        new ByteWord(149, "oof"),
        new ByteWord(150, "orb"),
        new ByteWord(151, "orc"),
        new ByteWord(152, "ore"),
        new ByteWord(153, "out"),
        new ByteWord(154, "own"),
        new ByteWord(155, "pan"),
        new ByteWord(156, "par"),
        new ByteWord(157, "paw"),
        new ByteWord(158, "pay"),
        new ByteWord(159, "pen"),
        new ByteWord(160, "pet"),
        new ByteWord(161, "pho"),
        new ByteWord(162, "pod"),
        new ByteWord(163, "pop"),
        new ByteWord(164, "pro"),
        new ByteWord(165, "pry"),
        new ByteWord(166, "pug"),
        new ByteWord(167, "pun"),
        new ByteWord(168, "pup"),
        new ByteWord(169, "put"),
        new ByteWord(170, "rad"),
        new ByteWord(171, "rag"),
        new ByteWord(172, "ram"),
        new ByteWord(173, "ran"),
        new ByteWord(174, "rap"),
        new ByteWord(175, "rat"),
        new ByteWord(176, "ray"),
        new ByteWord(177, "red"),
        new ByteWord(178, "ref"),
        new ByteWord(179, "rep"),
        new ByteWord(180, "rev"),
        new ByteWord(181, "rob"),
        new ByteWord(182, "rod"),
        new ByteWord(183, "rot"),
        new ByteWord(184, "rug"),
        new ByteWord(185, "run"),
        new ByteWord(186, "rye"),
        new ByteWord(187, "ryu"),
        new ByteWord(188, "sad"),
        new ByteWord(189, "sag"),
        new ByteWord(190, "sap"),
        new ByteWord(191, "sat"),
        new ByteWord(192, "saw"),
        new ByteWord(193, "say"),
        new ByteWord(194, "sea"),
        new ByteWord(195, "see"),
        new ByteWord(196, "set"),
        new ByteWord(197, "sew"),
        new ByteWord(198, "she"),
        new ByteWord(199, "shy"),
        new ByteWord(200, "sky"),
        new ByteWord(201, "sob"),
        new ByteWord(202, "sod"),
        new ByteWord(203, "son"),
        new ByteWord(204, "sow"),
        new ByteWord(205, "sox"),
        new ByteWord(206, "soy"),
        new ByteWord(207, "spa"),
        new ByteWord(208, "spy"),
        new ByteWord(209, "sub"),
        new ByteWord(210, "sum"),
        new ByteWord(211, "sun"),
        new ByteWord(212, "sus"),
        new ByteWord(213, "tab"),
        new ByteWord(214, "tad"),
        new ByteWord(215, "tan"),
        new ByteWord(216, "tap"),
        new ByteWord(217, "tar"),
        new ByteWord(218, "tax"),
        new ByteWord(219, "tea"),
        new ByteWord(220, "ten"),
        new ByteWord(221, "the"),
        new ByteWord(222, "thy"),
        new ByteWord(223, "toe"),
        new ByteWord(224, "ton"),
        new ByteWord(225, "too"),
        new ByteWord(226, "tot"),
        new ByteWord(227, "tow"),
        new ByteWord(228, "toy"),
        new ByteWord(229, "try"),
        new ByteWord(230, "tub"),
        new ByteWord(231, "two"),
        new ByteWord(232, "vac"),
        new ByteWord(233, "van"),
        new ByteWord(234, "vat"),
        new ByteWord(235, "vow"),
        new ByteWord(236, "wad"),
        new ByteWord(237, "wag"),
        new ByteWord(238, "was"),
        new ByteWord(239, "wax"),
        new ByteWord(240, "way"),
        new ByteWord(241, "wed"),
        new ByteWord(242, "who"),
        new ByteWord(243, "why"),
        new ByteWord(244, "wow"),
        new ByteWord(245, "wry"),
        new ByteWord(246, "yak"),
        new ByteWord(247, "yam"),
        new ByteWord(248, "yay"),
        new ByteWord(249, "yen"),
        new ByteWord(250, "yes"),
        new ByteWord(251, "yet"),
        new ByteWord(252, "you"),
        new ByteWord(253, "zap"),
        new ByteWord(254, "zen"),
        new ByteWord(255, "zoo")
    };

}
