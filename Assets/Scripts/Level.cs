using UnityEngine;

[ExecuteAlways]
public class Level : MonoBehaviour {

    [SerializeField] private Transform player;
    [SerializeField] private Material worldMaterial;

    void Update() {
        
        worldMaterial.SetVector("_PlaneNormal", Vector3.back);
        worldMaterial.SetVector("_PlanePosition", player.position);

    }

}
