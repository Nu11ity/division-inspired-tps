using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nu11ity
{
    [ExecuteAlways]
    public class LightingManager : MonoBehaviour
    {
        //references
        [SerializeField] private Light directionalLight;
        [SerializeField] private LightingPreset preset;
        //variables
        [SerializeField, Range(0, 24)] private float timeOfDay;

        private void Update()
        {
            if (preset == null)
                return;

            if (Application.isPlaying)
            {
                timeOfDay += Time.deltaTime;
                timeOfDay %= 24; //clamp between 0-24
                UpdateLighting(timeOfDay / 24f);
            }
            else
            {
                UpdateLighting(timeOfDay / 24f);
            }
        }
        private void UpdateLighting(float timePercent)
        {
            RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
            RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);

            if (directionalLight != null)
            {
                directionalLight.color = preset.directionalColor.Evaluate(timePercent);
                directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
            }
        }

        private void OnValidate()
        {
            if (directionalLight != null)
                return;

            if (RenderSettings.sun != null)
            {
                directionalLight = RenderSettings.sun;
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
}

