using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    Transform[] children;

    int index = 0;

    public Vector3 NextTarget => children[index].position;

    private void Awake()
    {
        children = new Transform[transform.childCount];
        for(int i = 0; i < children.Length; i++)
        {
            children[i] = transform.GetChild(i);
        }
    }

    public void StepNextWaypoint()
    {
        index++;
        index %= children.Length;
    }
}