using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(GameEditor))]
public class _Editor : Editor {

	public override void OnInspectorGUI()
	{

		base.OnInspectorGUI ();

		GameEditor edit = target as GameEditor;

		edit.GenerateMap ();
	}
}
