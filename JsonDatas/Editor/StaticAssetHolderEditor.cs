using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

namespace GRootPlugins.JsonDatas.Editor
{
    [CustomEditor(typeof(StaticAssetHolder))]
    public class StaticAssetHolderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            StaticAssetHolder holder = (StaticAssetHolder) target;

            // DrawDefaultInspector();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("HolderType"));
            
            if (holder.HolderType == StaticAssetHolder.EType.Unified)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_jsonAsset"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_keyStatics"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_idJsonAssets"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_keyJsonAssets"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
    
}
