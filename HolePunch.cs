using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class HolePunch : MonoBehaviour
{
    public Connector connector;
    public InputField gameName;


    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            StartClient();
        }
    }


    public void StartClient()
    {
        byte[] bytes = new byte[1024];

        try
        {
            // Connect to a Remote server
            // Get Host IP Address that is used to establish a connection
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses
            IPHostEntry host = Dns.GetHostEntry("127.0.0.1");
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            print(host.AddressList[1]);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 65433);

            // Create a TCP/IP  socket.
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.
            try
            {
                // Connect to Remote EndPoint
                sender.Connect(remoteEP);

                print("Socket connected to {0}");
                print(sender.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.
                byte[] msg = Encoding.ASCII.GetBytes(gameName.text);

                // Send the data through the socket.
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device.
                int bytesRec = sender.Receive(bytes);
                print(Encoding.ASCII.GetString(bytes, 0, bytesRec));

                ParseData(bytes);
                print("Echoed test = {0}");

                // Release the socket.
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch
            {
                
            }

        }
        catch (System.Exception e)
        {
            print(e.ToString());
        }
    }
    void ParseData(byte[] bytes)
    {
        string data = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        string[] splitData = data.Split(';');
        connector.SetListener(splitData[0], int.Parse(splitData[1]));
        if(splitData.Length > 2)
        {
            connector.SetSender(splitData[2], int.Parse(splitData[3]));
            print("connecting");
            connector.SendAMessage(bytes);
        }
    }


}
