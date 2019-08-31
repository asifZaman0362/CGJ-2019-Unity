using UnityEngine;

[ExecuteAlways]
public class Level : MonoBehaviour {

    [SerializeField] private Transform player;
    [SerializeField] private Material worldMaterial;
    public Vector3 normalDir = Vector3.back;

    void Update() {
        
        worldMaterial.SetVector("_PlaneNormal", normalDir);
        worldMaterial.SetVector("_PlanePosition", player.position);

    }

}
