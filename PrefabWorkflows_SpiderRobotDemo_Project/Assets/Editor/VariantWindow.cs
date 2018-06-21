using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VariantWindow : EditorWindow {
	public GameObject parent;

	[MenuItem ("Window/Variant Window")]
	public static void ShowWindow()
    {
        GetWindow<VariantWindow>(false, "Variant Window", true);
    }

	void OnGUI()
	{
		EditorGUILayout.LabelField ("View all prefab variants", EditorStyles.boldLabel);
		EditorGUILayout.Space();


		parent = (GameObject)EditorGUILayout.ObjectField("Parent Prefab", parent, typeof(GameObject));

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		List<Object> variants = GetVariants();

		EditorGUI.BeginDisabledGroup(true);
		foreach (Object variant in variants)
		{
			EditorGUILayout.ObjectField("Variant:", variant, typeof(Object), false);
			Texture2D image = AssetPreview.GetAssetPreview(variant);
			//image.Resize(100, 100);
			
			EditorGUILayout.LabelField(new GUIContent("Preview:"),new GUIContent(image), GUILayout.Height(100.0f), GUILayout.Width(500.0f));
			EditorGUILayout.Space();
		}
		EditorGUI.EndDisabledGroup();
		
	}

	List<Object> GetVariants()
	{
		string[] guids = AssetDatabase.FindAssets("t:Prefab");

		List<Object> variants = new List<Object>();

		foreach (string guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
        	Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);
			if (PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Variant)
			{
				if (PrefabUtility.GetCorrespondingObjectFromSource(obj) == parent)
				{
					variants.Add(obj);
				}				
			}
		}
		return variants;
	}
}
