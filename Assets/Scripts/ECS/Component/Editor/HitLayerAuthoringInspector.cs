using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HitLayerAuthoring))]
public class HitLayerAuthoringInspector : Editor
{
    public string[] hitLayerMaskTypeNames = null;
    public System.Array hitLayerMaskTypeValues = null;

    public override void OnInspectorGUI()
    {
        if (targets.Length > 1)
        {
            base.OnInspectorGUI();
            return;
        }

        var script = target as HitLayerAuthoring;

        if (hitLayerMaskTypeNames == null) hitLayerMaskTypeNames = System.Enum.GetNames(typeof(HitLayerType));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackLayerMask"));
        var attackLayerMask = EditorGUILayout.MaskField("Attack Layer Mask", (int)script.attackLayerMask, hitLayerMaskTypeNames);
        if (script.attackLayerMask != attackLayerMask)
        {
            script.attackLayerMask = attackLayerMask;
            EditorUtility.SetDirty(target);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("hitLayer"));

        serializedObject.ApplyModifiedProperties();
    }
}
