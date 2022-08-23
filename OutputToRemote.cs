using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class OutputToRemote : MonoBehaviour
{
    public Connector connector;
    public NetworkedMover mover;
    
    void SendData()
    {
        NetworkData data = new NetworkData();
        data.location = mover.GetLocation();
        byte[] dataToSend = Encoding.ASCII.GetBytes(JsonUtility.ToJson(data));
        connector.SendMessage(dataToSend);
    }

    // Update is called once per frame
    void Update()
    {
        SendData();
    }
}
