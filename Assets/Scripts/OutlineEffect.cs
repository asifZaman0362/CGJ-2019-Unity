using UnityEngine;

[ExecuteAlways]
public class OutlineEffect : MonoBehaviour {

    public Material material;

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, material);
    }

}