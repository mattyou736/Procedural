using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGenerator),true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator generator;
    private void Awake()
    {
        generator = (AbstractDungeonGenerator)target; //reference to the script its a generator off
    }
    //create dungeon from unity editor
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }
    }
}
