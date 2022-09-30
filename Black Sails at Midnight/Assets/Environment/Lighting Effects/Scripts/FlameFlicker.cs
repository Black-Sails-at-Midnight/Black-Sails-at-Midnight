using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlameFlicker : MonoBehaviour
{
    [Header("Flicker Settings")]
    [SerializeField]
    [Min(0)]
    public float brightnessVariation = 0.1f;

    [SerializeField]
    [Min(0)]
    public float transformVariation = 0.005f;

    [SerializeField]
    [Min(0)]
    public float flickerSpeed = 10f;

    private Light sourceLight;
    private Vector3 baseTransform;
    private float baseBrightness;

    // Monobehaviour Methods
    public void Start() {
        sourceLight = GetComponent<Light>();

        baseBrightness = sourceLight.intensity;
        baseTransform = transform.position;

        StartCoroutine(Flicker());
    }

    // Private Methods
    private IEnumerator Flicker()
    {
        while(true)
        {
            sourceLight.intensity = Random.Range(baseBrightness - brightnessVariation, baseBrightness + brightnessVariation);
            transform.position = new Vector3(Random.Range(baseTransform.x - transformVariation, baseTransform.x + transformVariation), transform.position.y, Random.Range(baseTransform.z - transformVariation, baseTransform.z + transformVariation));

            yield return new WaitForSeconds(1 / flickerSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
