using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlameFlicker))]
public class FlameFlickerEditor : Editor
{
    private void OnSceneGUI() {
        FlameFlicker flameFlicker = (FlameFlicker)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(flameFlicker.transform.position, Vector3.up, Vector3.forward, 360, flameFlicker.transformVariation);
    }
}
