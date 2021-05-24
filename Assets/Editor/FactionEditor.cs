using UnityEngine;
using UnityEditor;

/* This script makes sure that faction colors are immediately updated
 * when the alignment of a territory is changed in the editor.           */

[CustomEditor(typeof(FactionAlignment))]
[CanEditMultipleObjects]
public class FactionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        FactionAlignment factionAlignment = (FactionAlignment)target;
        factionAlignment.m_Renderer = factionAlignment.GetComponent<Renderer>();
        factionAlignment.SetFactionColor();
    }
}
