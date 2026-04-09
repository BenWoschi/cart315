using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnergyPostProcessing : MonoBehaviour
{
    public EnergyManager energyManager;

    [Header("Post Processing Volume")]
    public Volume volume; // Assign your global volume here

    private Vignette vignette;
    private ColorAdjustments colorAdjustments;

    [Header("Settings")]
    public float maxVignetteIntensity = 0.6f; // How strong vignette is at 0% energy
    public float minVignetteIntensity = 0.2f;   // Normal at high energy
    public float maxDesaturation = -90f;     // Fully grey at 0% energy
    public float minDesaturation = 0f;        // Normal color at high energy
    public float energyThreshold = 0.85f;     // Above this, effects are gone

    void Start()
    {
        if (energyManager == null) energyManager = EnergyManager.Instance;

        // Get references to post-processing overrides
        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGet(out vignette);
            volume.profile.TryGet(out colorAdjustments);
        }
    }

    void Update()
    {
        if (energyManager == null || vignette == null || colorAdjustments == null) return;

        float energy = Mathf.Clamp01(energyManager.energy);

        // Remap energy to 0-1 for effect strength
        float t = Mathf.InverseLerp(0f, energyThreshold, energy); // 0 = low energy, 1 = >= threshold

        // Vignette intensity
        vignette.intensity.value = Mathf.Lerp(maxVignetteIntensity, minVignetteIntensity, t);

        // Saturation
        colorAdjustments.saturation.value = Mathf.Lerp(maxDesaturation, minDesaturation, t);
    }
}