using UnityEngine;
using System.Collections;

public class Area : MonoBehaviour
{
    public string Type;
    public string Player;
    public Sprite Image;
    public Transform Ring;
    public Transform RingOuter;
    public Transform Light;

    void Start()
    {
        RingOuter.gameObject.SetActive(false);
        Ring.gameObject.SetActive(false);
    }
}
