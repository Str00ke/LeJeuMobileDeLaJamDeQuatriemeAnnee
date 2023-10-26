using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Rendering;

public static class ConvertToSpriteExtensiton
{
    public static Sprite ConvertToSprite(this Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}

public class GenBallsMatSprites : MonoBehaviour
{
#if UNITY_EDITOR
    public void Generate()
    {
        GlobalManager gm = FindObjectOfType<GlobalManager>();
        gm.BallsSpr.Clear();
        for (int i = 0; i < gm.Balls.Count; i++)
        {
            Object mat = gm.Balls[i];
            //Object mat = AssetDatabase.LoadAssetAtPath<Object>("Assets/Art/Materials/M_Bille_0" + (i+1) + ".mat");
            Texture2D obj = AssetPreview.GetAssetPreview(mat);
            
            Texture2D tex = readableClone(obj);
            AssetDatabase.CreateAsset(tex, "Assets/Art/Imgs/tex_" + i + ".asset");
            Sprite spr = tex.ConvertToSprite();
            AssetDatabase.CreateAsset(spr, "Assets/Art/Imgs/img_" + i + ".asset");
            //gm.BallsSpr.Add(spr);
        }
    }

    public void Clear()
    {
        GlobalManager gm = FindObjectOfType<GlobalManager>();

        for (int i = 0; i < gm.Balls.Count; i++)
            AssetDatabase.DeleteAsset("Assets/Art/Imgs/tex_" + i + ".asset");

    }

    public static Texture2D readableClone(Texture2D texture2D)
    {
        RenderTexture rt = new RenderTexture(texture2D.width, texture2D.height, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);

        Texture2D result = new Texture2D(texture2D.width, texture2D.height);
        result.ReadPixels(new Rect(0, 0, texture2D.width, texture2D.height), 0, 0);
        result.Apply();

        return result;
    }

#endif
}
