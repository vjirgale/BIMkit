using DbmsApi.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Material = UnityEngine.Material;

public class ModelObjectScript : MonoBehaviour
{
    public ModelObject ModelObject;
    private Material MainMaterial;
    private MeshRenderer[] MeshRenderers;
    public bool IsHighlighted;

    // Start is called before the first frame update
    void Start()
    {
        IsHighlighted = false;
        MeshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMainMaterial(Material material)
    {
        MainMaterial = material;
    }

    public void Highlight(Material material)
    {
        IsHighlighted = true;
        foreach (var mr in MeshRenderers)
        {
            mr.material = material;
        }
    }

    public void UnHighlight()
    {
        IsHighlighted = false;
        foreach (var mr in MeshRenderers)
        {
            mr.material = MainMaterial;
        }
    }
}
