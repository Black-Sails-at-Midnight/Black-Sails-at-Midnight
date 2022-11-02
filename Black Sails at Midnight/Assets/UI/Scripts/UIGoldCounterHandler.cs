using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIGoldCounterHandler : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI goldCounter;

    private EconomySystem economySystem;
    // Start is called before the first frame update
    void Start()
    {
        economySystem = FindObjectOfType<EconomySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        goldCounter.text = economySystem.Gold.ToString();
    }
}
