using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class TextureAutoTiler : MonoBehaviour {

    private MeshRenderer meshRenderer;

    // The size of the texture relative to the size of the mesh in X
    // Best determined experimentally by making an object with the texture, and adjusting the tiling until
    // it looks good. Then set this variable to (the x scale / the x tiling)
    // and all objects with the same ratio will have the same tiling relative to their scale.
    public float textureToMeshSizeRatio = 1;

    // Track previous script run variables so we don't re-do work every update
    private Vector3 prevScale = -Vector3.one;
    private float prevTextureToMeshSizeRatio = -1;

	// Use this for initialization
	void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        Update();
	}
	
	// Update is called once per frame
    [ContextMenu("Rescale Tiling")]
	void Update () {
        // Redo work only if object resized or desired ratio has changed.
        if (transform.lossyScale != prevScale || 
            !Mathf.Approximately(textureToMeshSizeRatio, prevTextureToMeshSizeRatio)) {
            rescaleTiling();
        }

        prevScale = transform.lossyScale;
        prevTextureToMeshSizeRatio = textureToMeshSizeRatio;
	}

    void rescaleTiling() {
        Vector3 scale = transform.lossyScale;

        Texture texture = meshRenderer.sharedMaterial.mainTexture;
        // New scale.x will be set to scale.x / (texture.width / sizeRatio)
        // this means a smaller texture will be repeated more times on larger objects
        // so that the look is consistent for all sizes.
        float textureToMeshRatio = (texture.width / texture.height) * textureToMeshSizeRatio;

        // since we are dealing with cubes, the default size is 1x1x1
        // could extend this to work with eg. planes (10x10)
        int baseSizeX = 1;

        // Only scale in X for now.
        meshRenderer.sharedMaterial.mainTextureScale = new Vector2(baseSizeX * scale.x / textureToMeshRatio, 
            // leave y alone
            meshRenderer.sharedMaterial.mainTextureScale.y);
    }
}
