using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcessCamera : MonoBehaviour
{
    [SerializeField] Material material;

    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Debug.Log("Hola");
        Graphics.Blit(source, destination, material);
    }
}
