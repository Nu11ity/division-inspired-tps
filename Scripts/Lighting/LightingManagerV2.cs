using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManagerV2 : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    [SerializeField, Range(-10, 10)] private float speedMultiplier;  // used to adjust the cycle time. Note that values < 0 will reverse it!
    [SerializeField, Range(1, 10)] private float nightSpeed; // how much to speed up late-night hours
    [SerializeField] private float maxIntensity = 1.5f;
    private float baseIntensity = 0f;
    [SerializeField] private float maxShadowStrength = 1f;
    [SerializeField] private float minShadowStrength = 0.2f;
    private float nightSpeedUpStart = 20f;
    private float nightSpeedUpEnd = 4f;
    private float dawn = 6f;
    private float dusk = 18f;
    private float noon = 12f;

    private void Start()
    {
        // default values
        speedMultiplier = 0.1f;
        nightSpeed = 10.0f;
        baseIntensity = maxIntensity / 2f;
    }
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.fogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.directionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        }
    }
    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            // speed up the time in dead of night
            if (TimeOfDay > nightSpeedUpStart || TimeOfDay < nightSpeedUpEnd) // speed up the passage of night from 9pm to 3am
            {
                // TimeOfDay += Time.deltaTime * speedMultiplier * nightSpeed;
                TimeOfDay += Time.deltaTime * nightSpeed;
            }
            else
            {
                TimeOfDay += Time.deltaTime * speedMultiplier;

                // adjust light intensity and shadow softness for time of day
                if (TimeOfDay >= dawn && TimeOfDay <= noon)
                {
                    DirectionalLight.intensity = baseIntensity + (baseIntensity / (noon - dawn)) * (TimeOfDay - dawn);
                    DirectionalLight.shadowStrength = minShadowStrength + ((maxShadowStrength - minShadowStrength) / (noon - dawn)) * (TimeOfDay - dawn);
                }
                else if (TimeOfDay > noon && TimeOfDay <= dusk)
                {
                    DirectionalLight.intensity = baseIntensity + (baseIntensity / (dusk - noon)) * (dusk - TimeOfDay);
                    DirectionalLight.shadowStrength = minShadowStrength + ((maxShadowStrength - minShadowStrength) / (dusk - noon)) * (dusk - TimeOfDay);
                }
                else
                {
                    DirectionalLight.intensity = baseIntensity;
                    DirectionalLight.shadowStrength = minShadowStrength;

                }
            }
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {

                }
            }
        }
    }
}
