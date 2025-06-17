using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager instance { get; private set; }

    public List<bool> waypoints = new List<bool>();

    public int selecePointNum = 0;

    public bool isReturn = false;
    public bool inDungeon = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}