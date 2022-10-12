using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIWaveHandler : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI waveCountLabel;


    private PrimaryWaveSystem waveSystem;

    void Start() 
    {
        waveSystem = FindObjectOfType<PrimaryWaveSystem>();
    }

    void Update()
    {
        waveCountLabel.text = waveSystem.currentWave.ToString();
    }
}
