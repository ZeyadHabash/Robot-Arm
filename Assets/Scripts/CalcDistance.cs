using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcDistance : MonoBehaviour
{
    [SerializeField] private Transform[] points;

    private void Start()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 vectorDistance = points[i + 1].position - points[i].position;
            float magnitudeDistance = vectorDistance.magnitude;
            Debug.Log("Distance between " + points[i].name + " and " + points[i + 1].name + " is " + vectorDistance);
            Debug.Log("Magnitude of distance between " + points[i].name + " and " + points[i + 1].name + " is " + magnitudeDistance);
        }
    }
}
