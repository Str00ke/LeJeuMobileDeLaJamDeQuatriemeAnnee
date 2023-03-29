using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreateLevelGeometry : MonoBehaviour
{
    public Text pName;
    public Text lName;
    public Text tX;
    public Text tY;
    public Text tZ;

    public GameObject ui;
    public void ShowUI()
    {
        ui.SetActive(true);
    }

    public void CreateLevel()
    {
#if UNITY_EDITOR
        GameObject p = GameObject.Find(pName.text);
        if (p != null)
        {
            Level level = ScriptableObject.CreateInstance<Level>();

            string path = "Assets/Levels/" + lName.text + ".asset";
            AssetDatabase.CreateAsset(level, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            level.LevelName = lName.text;
            level.LevelGeometry = CreateMesh(p);
            level.TargetPosition = new Vector3(float.Parse(tX.text), float.Parse(tY.text), float.Parse(tZ.text));
        }
#endif
    }

    GameObject CreateMesh(GameObject go)
    {
        MeshFilter[] meshFilters = go.transform.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        GameObject gameObject = new GameObject();
        gameObject.AddComponent<MeshFilter>();

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        gameObject.GetComponent<MeshFilter>().mesh = new Mesh();
        gameObject.GetComponent<MeshFilter>().mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        gameObject.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        //transform.gameObject.SetActive(true);

        return gameObject;
    }
}
