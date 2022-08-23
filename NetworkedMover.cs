using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedMover : MonoBehaviour
{
    public void UpdateLocation(Vector3 newLocation)
    {
        transform.position = newLocation;
    }
    public Vector3 GetLocation()
    {
        return transform.position;
    }
}
