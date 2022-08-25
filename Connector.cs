using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


/*
 * https://stackoverflow.com/questions/37131742/how-to-use-udp-with-unity-methods
 */


public class Connector : MonoBehaviour
{
    public InputRemote inputRemote;
    static UdpClient udp;
    Thread thread;
    static readonly object lockObject = new object();
    byte[] returnData;
    bool precessData = false;
    bool gotSender = false;
    string ipAddresstoSend = "";
    int sendPort = 12345;
    bool gotListener = false;
    string ipAddressListen = "";
    int listenPort;
    // Start is called before the first frame update
    void Start()
    {
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
    }





    // Update is called once per frame
    void Update()
    {
        //myPort.text = ((IPEndPoint)udp.Client.LocalEndPoint).Port.ToString();
        //see if we have data
        if (precessData)
        {
            /*lock object to make sure there data is 
             *not being accessed from multiple threads at thesame time*/
            lock (lockObject)
            {
                precessData = false;
                if(gotSender)
                {
                    inputRemote.RecieveData(returnData);
                }
                else
                {
                    ParseConnectionData(returnData);
                }
            }
        }
    }

    void ParseConnectionData(byte[] bytes)
    {

        string data = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        print(data);
        string[] splitData = data.Split(';');
        SetSender(splitData[0], int.Parse(splitData[1]));
        Debug.Log ("wow");
    }

    public void SetSender(string ipAddress, int port)
    {
        ipAddresstoSend = ipAddress;
        sendPort = port;
        gotSender = true;
    }

    public void SetListener(string ipAddress, int port)
    {
        ipAddressListen = ipAddress;
        listenPort = port;
        gotListener = true;
    }

    public void SendAMessage(byte[] data)
    {
        if(gotSender)
        {
            print(ipAddresstoSend);
            print(sendPort);
            print(Encoding.UTF8.GetString(data, 0, data.Length));
            try
            {
                using (var client = new UdpClient())
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddresstoSend), sendPort);
                    client.Connect(ep);
                    client.Send(data, data.Length);
                    print("sent");
                }
            }
            catch (System.Exception ex)
            {
                print(ex.ToString());
            }
        }
        
    }

    private void ThreadMethod()
    {
        while(!gotListener)
        {
            
        }
        udp = new UdpClient(listenPort);
        while (true)
        {
            if(gotListener && !precessData)
            {
                print(".");
                print(ipAddressListen);
                print(listenPort);
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                print("listening");
                byte[] receiveBytes = udp.Receive(ref RemoteIpEndPoint);
                print("recieved");
                /*lock object to make sure there data is 
                *not being accessed from multiple threads at the same time*/
                lock (lockObject)
                {
                    returnData = receiveBytes;

                    //Done, notify the Update function
                    precessData = true;
                }
            }
           
        }
    }
}
