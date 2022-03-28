using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CheckMesh : Editor
{  
    //Activate menu item on if selected object has a mesh.
    //Accessible through right click on gameobject in Hierarchy, or MeshFilter options in Inspector.
    [MenuItem("GameObject/Mesh Data", true)]
    [MenuItem("CONTEXT/MeshFilter/Mesh Data", true)]
    static bool ValidateLogSelectedGameObject(){
        return Selection.activeGameObject.GetComponent<MeshFilter>() != null;
    }   

    //Print out name, vertices and triangles in console when using Mesh Data option.
    [MenuItem("GameObject/Mesh Data", false, 0)]
    [MenuItem("CONTEXT/MeshFilter/Mesh Data", false, 0)]
    static void SelectedMeshes(){
        int totalVertices = 0;
        int totalTriangles = 0;
        string objectName;
        int objectVertices = 0;
        int objectTriangles = 0;

        for (int j = 0; j < Selection.gameObjects.Length; j++){
            objectName = Selection.gameObjects[j].name;
            objectVertices = Selection.gameObjects[j].GetComponent<MeshFilter>().sharedMesh.vertexCount;
            objectTriangles = Selection.gameObjects[j].GetComponent<MeshFilter>().sharedMesh.triangles.Length;
            Debug.Log("Name: "+ objectName + " Vertices: " + objectVertices + " Triangles: " + objectTriangles);
            totalVertices += Selection.gameObjects[j].GetComponent<MeshFilter>().sharedMesh.vertexCount;
            totalTriangles += Selection.gameObjects[j].GetComponent<MeshFilter>().sharedMesh.triangles.Length;
        }

        // If more than one object selected, print out total of selected objects
        if (Selection.gameObjects.Length > 1){
            Debug.Log("Total Count:" + " Total Vertices: " + totalVertices + " Total Triangles: " + totalTriangles);
        }
    }

}
