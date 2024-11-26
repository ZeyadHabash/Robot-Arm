using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
       if (!other.gameObject.CompareTag("block")) return;
       
       Debug.Log("touch");
       
   }
}
