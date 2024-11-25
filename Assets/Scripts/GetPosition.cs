using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPosition : MonoBehaviour
{
    private IEnumerator Start()
    {
        while (true)
        {
            Vector3 swappedPosition = new Vector3(transform.position.x, transform.position.z, transform.position.y);
            Debug.LogFormat("Position: X = {0:F6}, Y = {1:F6}, Z = {2:F6}", swappedPosition.x, swappedPosition.y, swappedPosition.z);
            yield return new WaitForSeconds(2);
        }
    }
}