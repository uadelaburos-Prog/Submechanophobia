using UnityEngine;

public class WeldingTool : MonoBehaviour
{
    // lanzar un raycaste desde una posicion hacia adelante
    [SerializeField] private Camera cam;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask canBeWeld;
    [SerializeField] private Transform weldingPoint;
    [SerializeField] private WeldingPoint weldingTarget;

    private RaycastHit hit;
    private bool isActive;

    //obtener los objectos necesarios para los efectos de soldadura
    [SerializeField] private ParticleSystem weldingParticles;

    private void Awake()
    {
        weldingParticles.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 direction = cam.transform.forward;

            bool gotHit = Physics.Raycast(cam.transform.position, direction, out hit, raycastDistance, canBeWeld);

            if (gotHit)
            {
                isActive = true;
                weldingTarget = hit.collider.gameObject.GetComponent<WeldingPoint>();
                if (weldingTarget == null) return;
                weldingTarget.IsBeingWeld();
                
                Debug.Log($"El raycast golpeó a: {hit.collider.gameObject.name}");
            }
        }
        else
            isActive = false;

        weldingParticles.gameObject.SetActive(isActive);
    }
}
