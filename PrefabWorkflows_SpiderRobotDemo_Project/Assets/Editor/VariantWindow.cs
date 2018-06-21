using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VariantWindow : EditorWindow {
	public GameObject parent;
	Vector2 scrollPos;

	[MenuItem ("Window/Variant Window")]
	public static void ShowWindow()
    {
        GetWindow<VariantWindow>(false, "Variant Window", true);
    }

	void OnGUI()
	{
		EditorGUILayout.LabelField ("View all prefab variants", EditorStyles.boldLabel);
		EditorGUILayout.Space();

		// This is the field where the prefab can be dragged into
		parent = (GameObject)EditorGUILayout.ObjectField("Parent Prefab", parent, typeof(GameObject));

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		List<Object> variants = GetVariants();

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);
		// The group is disabled because we want these ObjectFields to be read-only
		EditorGUI.BeginDisabledGroup(true);
		foreach (Object variant in variants)
		{
			EditorGUILayout.ObjectField("Variant:", variant, typeof(Object), false);

			// Show a thumbnail of the variant prefabs
			Texture2D image = AssetPreview.GetAssetPreview(variant);
			
			// The thumbnail image is inside a LabelField because DrawPreviewTexture isn't available in EditorGUILayout
			EditorGUILayout.LabelField(new GUIContent("Preview:"),new GUIContent(image), GUILayout.Height(100.0f), GUILayout.Width(500.0f));
			EditorGUILayout.Space();
		}
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndScrollView();
	}

	List<Object> GetVariants()
	{
		// Find all prefabs
		string[] guids = AssetDatabase.FindAssets("t:Prefab");

		List<Object> variants = new List<Object>();

		foreach (string guid in guids)
		{
			// Convert the prefab guid string into Object
			string path = AssetDatabase.GUIDToAssetPath(guid);
        	Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);

			// Check if the prefab is a variant
			if (PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Variant)
			{
				// Check that the base prefab is the same as the one we've dragged into the ObjectField (this used to be PrefabUtility.GetPrefabParent)
				if (PrefabUtility.GetCorrespondingObjectFromSource(obj) == parent)
				{
					variants.Add(obj);
				}				
			}
		}
		return variants;
	}

	void OnInspectorUpdate()
	{
		// OnInspectorUpdate is called 10 times per second
		Repaint();
	}
}
