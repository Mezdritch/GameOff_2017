using UnityEngine;
using System.Collections;

public class PlayersStats : MonoBehaviour
{
    // ---------- ---------- ---------- ---------- 
    // AWAKE
    // Keep this object from scene to scene
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
