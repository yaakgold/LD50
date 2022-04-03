using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    public bool doTheWobble = false;
    public float wobbleSpeed;
    public float maxWobbleAmt;

    // Update is called once per frame
    void Update()
    {
        if(doTheWobble)
        {
            transform.Rotate(Vector3.forward, wobbleSpeed * Time.deltaTime);
            if (Mathf.Abs(transform.rotation.z * Mathf.Rad2Deg) >= maxWobbleAmt) wobbleSpeed *= -1;
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
