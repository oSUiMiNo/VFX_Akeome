using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System.Linq;


//[ExecuteInEditMode]
//[RequireComponent(typeof(VisualEffect))]
//public class GenerateDynamicAttributeMaps : MonoBehaviourMyExtention
//{

//    [SerializeField]
//    int width = 32;

//    [SerializeField]
//    int height = 32;

//    VisualEffect vfx;
//    Texture2D positionMap;
//    Texture2D colorMap;

//    void Start()
//    {
//        vfx = GetComponent<VisualEffect>();
//        CreateAndSetAttributeMaps();
//    }

//    void Update()
//    {
//        if (width != positionMap.width || height != positionMap.height)
//        {
//            CreateAndSetAttributeMaps();
//        }
//        UpdateAttributeMaps();
//    }

//    void CreateAndSetAttributeMaps()
//    {
//        positionMap = new Texture2D(width, height, TextureFormat.RGBAFloat, false);
//        positionMap.filterMode = FilterMode.Point;
//        positionMap.wrapMode = TextureWrapMode.Clamp;
//        //colorMap = new Texture2D(width, height, TextureFormat.RGBAFloat, false);
//        //colorMap.filterMode = FilterMode.Point;
//        //colorMap.wrapMode = TextureWrapMode.Clamp;
//        vfx.SetTexture("PositionMap", positionMap);
//        //vfx.SetTexture("ColorMap", colorMap);
//    }

//    void UpdateAttributeMaps()
//    {
//        Vector3 center = 3.0f * new Vector3(Mathf.Cos(Time.time), Mathf.Sin(Time.time), 0.0f);
//        int count = width * height;
//        Color[] positions = new Color[count];
//        Color[] colors = new Color[count];
//        for (int i = 0; i < count; i++)
//        {
//            Vector3 position = center + Random.onUnitSphere;
//            positions[i] = new Color(position.x, position.y, position.z, 0.0f);
//            colors[i] = Color.HSVToRGB((Mathf.Atan2(position.y, position.x) + Mathf.PI) / (2.0f * Mathf.PI), 1.0f, 1.0f);
//        }
//        positionMap.SetPixels(positions);
//        positionMap.Apply();
//        //colorMap.SetPixels(colors);
//        //colorMap.Apply();
//    }
//}