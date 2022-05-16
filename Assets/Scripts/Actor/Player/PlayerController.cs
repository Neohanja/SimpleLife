using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Movement
{
    protected override void GetMovement()
    {
        float forward = Input.GetAxis("Vertical");
        float sideways = Input.GetAxis("Horizontal");

        momentum = Vector3.up * forward + Vector3.right * sideways;
    }
}
