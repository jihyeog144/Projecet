using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai : MonoBehaviour
{
    public float fearLevel = 0f;

    public void IncreaseFear()
    {
        fearLevel += Random.Range(10f, 30f);
    }
}
