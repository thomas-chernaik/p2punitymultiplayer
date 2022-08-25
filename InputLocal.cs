using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLocal : MonoBehaviour
{
    public NetworkedMover mover;
    public float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        Vector3 move = moveSpeed * Time.deltaTime * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        mover.UpdateLocation(mover.transform.position + move);
        if(Input.GetKeyDown(KeyCode.R))
        {
            mover.UpdateMaterial(1);
        }
    }
}
