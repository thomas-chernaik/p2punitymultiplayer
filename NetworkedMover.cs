using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedMover : MonoBehaviour
{
    public Material[] mats;
    int currentMat = 0;
    public void UpdateLocation(Vector3 newLocation)
    {
        transform.position = newLocation;
    }

    public void UpdateMaterial(int mat)
    {
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = mats[mat];
        currentMat = mat;
    }
    public int GetMaterial()
    {
        return currentMat;
    }

    public Vector3 GetLocation()
    {
        return transform.position;
    }
}
