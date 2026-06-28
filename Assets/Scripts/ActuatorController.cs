using UnityEngine;

public class ActuatorController : MonoBehaviour
{
    [Header("Fans")]
    public Transform fan1;
    public Transform fan2;
    public float spinSpeed = 500f;
    private bool isSpinning = false;

    [Header("Lighting")]
    public Light bulbLight;
    public Renderer bulbRenderer;
    public int glowMaterialIndex = 3; // Index of the 'glow' material in the Bulb renderer

    [ColorUsage(true, true)]
    public Color activeEmissionColor = new Color(2f, 1.6f, 0.5f); // Soft warm light
    private Material glowMaterial;

    private void Start()
    {
        // Cache the instantiated material to avoid editing shared asset material
        if (bulbRenderer != null && glowMaterialIndex >= 0 && glowMaterialIndex < bulbRenderer.materials.Length)
        {
            glowMaterial = bulbRenderer.materials[glowMaterialIndex];
        }

        // Set initial states
        SetLightActive(false);
    }

    private void Update()
    {
        if (isSpinning)
        {
            // Rotate fans around their local Z-axis (standard for fan meshes)
            if (fan1 != null) fan1.Rotate(Vector3.forward * spinSpeed * Time.deltaTime, Space.Self);
            if (fan2 != null) fan2.Rotate(Vector3.forward * spinSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void SetFansSpinning(bool active)
    {
        isSpinning = active;
    }

    public void SetLightActive(bool active)
    {
        if (bulbLight != null)
        {
            bulbLight.enabled = active;
        }

        if (glowMaterial != null)
        {
            if (active)
            {
                glowMaterial.EnableKeyword("_EMISSION");
                glowMaterial.SetColor("_EmissionColor", activeEmissionColor);
            }
            else
            {
                glowMaterial.SetColor("_EmissionColor", Color.clear);
                glowMaterial.DisableKeyword("_EMISSION");
            }
        }
    }
}