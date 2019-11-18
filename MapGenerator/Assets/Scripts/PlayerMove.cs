using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;
    void Update()
    {
        transform.position +=  (Input.GetAxis("Vertical") * transform.forward
            + Input.GetAxis("Horizontal") * transform.right)*Time.deltaTime*speed;
    }
}
