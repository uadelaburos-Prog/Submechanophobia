using UnityEngine;

public class WeldingPoint : MonoBehaviour
{
    [SerializeField] private float life = 100f;
    [SerializeField] private float currentLife;

    public float Currentlife => currentLife;

    private void Start()
    {
        currentLife = life;
    }

    public void IsBeingWeld()
    {
        if (currentLife <= life)
        {
            currentLife += 5 * Time.deltaTime;
            currentLife = Mathf.Clamp(currentLife, 0, life);
        }
    }
}
