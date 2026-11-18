using UnityEngine;

[ExecuteAlways] // Hace que el cambio se vea también en el Editor sin darle a Play
public class TintByPrefix : MonoBehaviour
{
    [Header("Configuración")]
    public string prefix = "C_";
    public Color tintColor = Color.white;

    private MaterialPropertyBlock _propBlock;
    // URP usa "_BaseColor" (si usaras Built-in sería "_Color")
    private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");

    private void OnEnable()
    {
        ApplyTint();
    }

    private void OnValidate()
    {
        ApplyTint();
    }

    public void ApplyTint()
    {
        if (_propBlock == null)
            _propBlock = new MaterialPropertyBlock();

        // Busca todos los Renderers (SkinnedMeshRenderer y MeshRenderer) dentro del prefab
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        foreach (Renderer rend in renderers)
        {
            if (rend.gameObject.name.StartsWith(prefix))
            {
                // Se aplica a cada material que tenga el objeto (por si tiene más de 1)
                for (int i = 0; i < rend.sharedMaterials.Length; i++)
                {
                    rend.GetPropertyBlock(_propBlock, i);
                    _propBlock.SetColor(BaseColorID, tintColor);
                    rend.SetPropertyBlock(_propBlock, i);
                }
            }
        }
    }
}