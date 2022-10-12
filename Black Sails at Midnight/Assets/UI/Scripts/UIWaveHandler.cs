using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIWaveHandler : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI waveCountLabel;

    [SerializeField]
    int waveCount = 0;

    // Update is called once per frame
    void Update()
    {
        waveCountLabel.text = waveCount.ToString();
    }
}
