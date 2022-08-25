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
        data.mat = mover.GetMaterial();
        byte[] dataToSend = Encoding.ASCII.GetBytes(JsonUtility.ToJson(data));
        connector.SendAMessage(dataToSend);
    }

    // Update is called once per frame
    void Update()
    {
        SendData();
    }
}
