using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPar : MonoBehaviour
{
    public GameObject child;
    private void FixedUpdate()
    {
        if (transform.position.z-PlayerController.instance.CheckPosition().z>=7)
        {
            child.SetActive(false);
        }
        else if (transform.position.z - PlayerController.instance.CheckPosition().z <= -7)
        {
            Destroy(gameObject);
        }
        else
        {
            child.SetActive(true);
        }
    }
}
