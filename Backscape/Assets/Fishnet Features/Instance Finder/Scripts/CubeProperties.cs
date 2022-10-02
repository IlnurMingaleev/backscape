using UnityEngine;
using FishNet;
using FishNet.Object.Synchronizing;
using FishNet.Object;
using FishNet.Transporting;

/*public static class MaterailSerializer 
{
    //Write each axis of a Vector2.
    public static void WriteVector2(this Writer writer, Material value)
    {
        writer.WriteSingle(value);
        
    }

    //Read and return a Vector2.
    public static Vector2 ReadVector2(this Reader reader)
    {
        return new Vector2()
        {
            x = reader.ReadSingle(),
            y = reader.ReadSingle()
        };
    }


}*/

public class CubeProperties : NetworkBehaviour
{
    [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(OnMaterialChange))]
    public Color g_cubeColor;
   /* public Color g_CubeColor 
    {
        get 
        {
            return g_cubeColor;
        }

        set 
        {
            g_cubeColor = value;
        }
    }*/

    private void OnMaterialChange(Color prev, Color next, bool asServer) 
    {
        if (!asServer)
            GetComponent<MeshRenderer>().material.color = next;
    }



}
