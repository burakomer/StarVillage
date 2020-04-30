using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeTest : MonoBehaviour
{
    public GameObject ropePrefab;
    public int amount;

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(ropePrefab, Vector3.zero, Random.rotation);
        }
    }
}
