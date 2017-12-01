using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DanceMoveSpawner : MonoBehaviour
{
    private bool _Spawning;
    private float _RandomWait;
    private int _RandomMoves;

    public Canvas canvas;
    public List<GameObject> moves;

    //////////////////////////////
    // START
    private void Start()
    {
        _Spawning = false;
    }

    //////////////////////////////
    // START MOVES SPAWNING
    public void StartMovesSpawning()
    {
        //Start state machine
        _Spawning = true;
        StartCoroutine(Spawns());
    }

    //////////////////////////////
    // STOP
    public void StopSpawn()
    {
        _Spawning = false;
    }

    //////////////////////////////
    // SPAWNS
    private IEnumerator Spawns()
    {
        while (_Spawning)
        {
            _RandomWait = Random.Range(1f, 3f);
            yield return new WaitForSeconds(_RandomWait);

            _RandomMoves = Random.Range(0, moves.Count);

            GameObject go = Instantiate(moves[_RandomMoves]) as GameObject;
            go.transform.SetParent(canvas.transform);
            go.transform.position = gameObject.transform.position;
        }

        yield return null;
    }
}
