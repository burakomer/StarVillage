using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeTest : MonoBehaviour
{
    public GameObject ropePrefab;

    private void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Instantiate(ropePrefab, Vector3.zero, Random.rotation);
        }
    }
}
