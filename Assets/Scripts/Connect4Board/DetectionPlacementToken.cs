using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token behaviour to detect collision with a column
public class DetectionPlacementToken : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Column")
        {
            if(GameManager.GetInstance())
            {
                GameManager.GetInstance().Hit(other.gameObject.name);
            }
            else if(GameManagerNetwork.Instance)
            {
                GameManagerNetwork.Instance.Hit(other.gameObject.name, gameObject);
            }
            
        }
       
    }
}
