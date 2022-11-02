using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineLayer : MonoBehaviour
{
    [SerializeField]
    int SecondsBetweenMines = 10;
    [SerializeField]
    GameObject MineObject;
    [SerializeField]
    Transform LocationToSpawnMines;

    private bool isLayingMines;

    // Update is called once per frame
    void Update()
    {
        if (!isLayingMines)
        {
            StartCoroutine(LayMine());
        }
    }

    IEnumerator LayMine()
    {
        isLayingMines = true;
        yield return new WaitForSeconds(SecondsBetweenMines);
        isLayingMines = false;
    }
}
