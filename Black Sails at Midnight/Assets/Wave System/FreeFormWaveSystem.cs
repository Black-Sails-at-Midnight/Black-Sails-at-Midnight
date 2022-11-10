using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FreeFormWaveSystem : MonoBehaviour
{
    public Canvas playerCanvas;
    public Canvas deathCanvas;
    public TextMeshProUGUI winText;


    // Update is called once per frame
    void Update()
    {
        if(FindObjectsOfType<BasicShipEquivelant>().Length == 0)
        {            
            playerCanvas.gameObject.SetActive(false);
            deathCanvas.gameObject.SetActive(true);
            winText.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
        }
    }
}
