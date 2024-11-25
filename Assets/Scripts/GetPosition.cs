using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPosition : MonoBehaviour
{
    private IEnumerator Start()
    {
        while (true)
        {
            Debug.Log(transform.position.ToString("F6"));
            yield return new WaitForSeconds(5);
        }
    }
}
