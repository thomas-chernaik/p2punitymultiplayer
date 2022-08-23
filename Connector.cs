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
    public Text myPort;
    public Text recieved;
    public InputField ip;
    public InputField port;
    public InputRemote inputRemote;
    static UdpClient udp;
    Thread thread;
    static readonly object lockObject = new object();
    byte[] returnData;
    bool precessData = false;
    string ipAddress = "";
    int sendPort = 12345;
    // Start is called before the first frame update
    void Start()
    {
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
    }





    // Update is called once per frame
    void Update()
    {
        myPort.text = ((IPEndPoint)udp.Client.LocalEndPoint).Port.ToString();
        ipAddress = ip.text;
        sendPort = int.Parse(port.text);     
        //see if we have data
        if (precessData)
        {
            /*lock object to make sure there data is 
             *not being accessed from multiple threads at thesame time*/
            lock (lockObject)
            {
                precessData = false;

                inputRemote.RecieveData(returnData);
            }
        }
    }


    public void SendMessage(byte[] data)
    {
        try
        {
            using (var client = new UdpClient())
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), sendPort);
                client.Connect(ep);
                client.Send(data, data.Length);
            }
        }
        catch (System.Exception ex)
        {
            print(ex.ToString());
        }
    }

    private void ThreadMethod()
    {
        udp = new UdpClient(0);
        while (true)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes = udp.Receive(ref RemoteIpEndPoint);

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
