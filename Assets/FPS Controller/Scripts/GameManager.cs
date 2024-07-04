using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Transform> RespawnPoints;
    public static List<Transform> S_RespawnPoints;

    void Awake()
    {
        S_RespawnPoints = RespawnPoints;
    }
}