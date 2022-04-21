using UnityEngine;
using Chess_Classes.Interfaces;

namespace Chess_Classes {
    [RequireComponent(typeof(MeshRenderer))]
    public class MaterialSetter :MonoBehaviour{
        MeshRenderer meshRenderer;
        public void SetMaterial(Material material)
        {
            if(meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

            meshRenderer.material = material;
        }
    }
}