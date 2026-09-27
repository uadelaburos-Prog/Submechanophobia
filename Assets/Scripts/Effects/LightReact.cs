using System.Collections.Generic;
using UnityEngine;

public class LightReact : MonoBehaviour
{
    [Header("Sistema de partículas")]
    public ParticleSystem sistemaParticulas;

    [Header("Fuentes de luz a considerar")]
    public List<Light> fuentesLuz = new List<Light>();

    [Header("Falloff por distancia")]
    public float distanciaMax = 3f;      // a esta distancia, apagado total
    public float distanciaMin = 0.3f;    // a esta distancia, brillo máximo

    [Header("Cono (solo aplica a Spot Lights)")]
    [Range(0f, 1f)] public float conoEdgeSuave = 0.8f;  // empieza a atenuarse acá
    [Range(0f, 1f)] public float conoEdgeDuro = 0.95f;  // brillo completo acá

    [Header("Colores")]
    public Color colorBase = new Color(0.6f, 0.8f, 0.9f, 0.3f);
    public Color colorIluminado = new Color(1f, 1f, 1f, 0.9f);

    private ParticleSystem.Particle[] particulas;

    void LateUpdate()
    {
        if (sistemaParticulas == null || fuentesLuz.Count == 0) return;

        int cantidad = sistemaParticulas.particleCount;
        if (cantidad == 0) return;

        if (particulas == null || particulas.Length < sistemaParticulas.main.maxParticles)
            particulas = new ParticleSystem.Particle[sistemaParticulas.main.maxParticles];

        int vivas = sistemaParticulas.GetParticles(particulas);

        for (int i = 0; i < vivas; i++)
        {
            float influenciaMax = 0f;

            // Recorremos todas las luces y nos quedamos con la más fuerte
            for (int l = 0; l < fuentesLuz.Count; l++)
            {
                Light luz = fuentesLuz[l];
                if (luz == null || !luz.isActiveAndEnabled) continue;

                float influencia = CalcularInfluencia(particulas[i].position, luz);
                if (influencia > influenciaMax)
                    influenciaMax = influencia;
            }

            particulas[i].startColor = Color.Lerp(colorBase, colorIluminado, influenciaMax);
        }

        sistemaParticulas.SetParticles(particulas, vivas);
    }

    float CalcularInfluencia(Vector3 posParticula, Light luz)
    {
        Vector3 posLuz = luz.transform.position;
        float dist = Vector3.Distance(posParticula, posLuz);

        // Fuera del rango de la luz, ni vale la pena seguir calculando
        if (dist > Mathf.Max(distanciaMax, luz.range)) return 0f;

        float factorDistancia = Mathf.InverseLerp(distanciaMax, distanciaMin, dist);
        factorDistancia = Mathf.SmoothStep(0f, 1f, factorDistancia);

        // Si es Spot Light, sumamos el chequeo de cono
        if (luz.type == LightType.Spot)
        {
            Vector3 dirHaciaParticula = (posParticula - posLuz).normalized;
            float dot = Vector3.Dot(luz.transform.forward, dirHaciaParticula);

            // Convertimos el spotAngle real de la luz a un rango de dot product
            float mitadAngulo = luz.spotAngle * 0.5f;
            float cosMitadAngulo = Mathf.Cos(mitadAngulo * Mathf.Deg2Rad);

            float factorCono = Mathf.InverseLerp(cosMitadAngulo, cosMitadAngulo + (1f - cosMitadAngulo) * (conoEdgeDuro - conoEdgeSuave), dot);
            factorCono = Mathf.Clamp01(factorCono);

            return factorDistancia * factorCono;
        }

        // Point Light: solo importa la distancia, sin cono
        return factorDistancia;
    }
}
