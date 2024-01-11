using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionPlacementToken : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.GetInstance().Hit(gameObject.name);
        }
       
    }
}
