using UnityEngine;
using System.Collections;

public class TrafficLight : MonoBehaviour
{
    public GameObject light = null;

    void OnTriggerEnter ( Collider other )
    {
        if ( other.tag == "train" )
        {
            light.SetActive ( true );
        }
    }

    void OnTriggerExit ( Collider other )
    {
        if ( other.tag == "train" )
        {
            light.SetActive ( false );
        }
    }
}
