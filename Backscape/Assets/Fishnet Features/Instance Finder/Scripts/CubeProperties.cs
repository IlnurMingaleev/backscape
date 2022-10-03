using UnityEngine;
using FishNet;
using FishNet.Object.Synchronizing;
using FishNet.Object;
using FishNet.Transporting;
using FishNet.Serializing;
using UnityEngine.Rendering;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public static class MaterialSerilizer
{

   //Write each axis of a Material.
   // public static void WriteMaterial(this Writer writer, Material value)
   // {
    //    writer.WriteColor(value.color);
    //    writer.WriteBoolean(value.doubleSidedGI);
    //    writer.WriteArray<LocalKeyword>(value.enabledKeywords);
    //    writer.WriteBoolean(value.enableInstancing);
    //    writer.Write<MaterialGlobalIlluminationFlags>(value.globalIlluminationFlags);
    //    //writer.Write<Texture>(value.mainTexture);
    //    writer.Write<Vector2>(value.mainTextureOffset);
    //    writer.Write<Vector2>(value.mainTextureScale);
    //    writer.Write<int>(value.passCount);
    //    writer.Write<int>(value.renderQueue);
    //   writer.Write<Shader>(value.shader);
    //    writer.WriteArray<string>(value.shaderKeywords);

   // }

    ////Read and return a Material.
    //public static Material ReadMaterial(this Reader reader)
    //{
     //   return new Material(reader.Read<Shader>()) { };
    //    {
    //        color = reader.ReadColor(),
    //        doubleSidedGI = reader.ReadBoolean(),
    //        enabledKeywords = reader.ReadArrayAllocated<LocalKeyword>(),
    //        enableInstancing = reader.ReadBoolean(),
    //        globalIlluminationFlags = reader.Read<MaterialGlobalIlluminationFlags>(),
    //        //mainTexture = reader.Read<Texture>(),
    //        mainTextureOffset = reader.Read<Vector2>(),
    //        mainTextureScale = reader.Read<Vector2>(),
    //        renderQueue = reader.Read<int>(),
    //        shaderKeywords = reader.ReadArrayAllocated<string>()
    //    }; 
    //}

    //public static void WriteMaterial(this Writer writer, Material value) 
    //{
    //    GameObject writeGameobject = SingletonColors.Instance.materialPrefab;
    //    //MeshRenderer meshRenderer = new MeshRenderer()
    //    //writeGameobject.AddComponent<MeshRenderer>();
    //    writeGameobject.GetComponent<MeshRenderer>().material = value;
    //    writer.WriteGameObject(writeGameobject);


    //}

    //public static Material ReadMaterial(this Reader reader) 
    //{
    //    return reader.ReadGameObject().GetComponent<MeshRenderer>().material;
    
    //}
}

public class CubeProperties : NetworkBehaviour
{
    [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(OnMaterialChange))]
    private Color g_cubeColor;

    //[SyncVar(Channel = Channel.Unreliable, OnChange = nameof(OnMaterialChange))]
    //public Material g_cubeMaterial;
     public Color g_CubeColor 
     {
         get 
         {
             return g_cubeColor;
         }

         set 
         {
             g_cubeColor = value;
         }
     }

     private void OnMaterialChange(Color prev, Color next, bool asServer) 
     {
         if (!asServer)
             GetComponent<MeshRenderer>().material.color = next;
     }

    /*private void OnMaterialChange(Material prev, Material next, bool asServer) 
    {
        if (!asServer)
            GetComponent<MeshRenderer>().material = next;
    }*/



}
