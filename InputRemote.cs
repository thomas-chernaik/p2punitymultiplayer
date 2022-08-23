using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;


public class InputRemote : MonoBehaviour
{
    public NetworkedMover mover;




    public void RecieveData(byte[] data)
    {
        NetworkData decodedData = JsonUtility.FromJson<NetworkData>(Encoding.ASCII.GetString(data));
        mover.UpdateLocation(decodedData.location);
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
