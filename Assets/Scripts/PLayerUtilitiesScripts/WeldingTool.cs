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

    //obtener los objectos necesarios para los efectos de soldadura
    [SerializeField] private ParticleSystem weldingParticles;

    private void Update()
    {
        bool isActive = false;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 direction = cam.transform.forward;

            bool gotHit = Physics.Raycast(cam.transform.position, direction, out hit, raycastDistance, canBeWeld);

            if (gotHit)
            {
                weldingTarget = hit.collider.gameObject.GetComponent<WeldingPoint>();
                weldingTarget.IsBeingWeld();
                
                Debug.Log($"El raycast golpeó a: {hit.collider.gameObject.name}");
            }
        }

        if (weldingTarget.Currentlife <= 100)
        {
            isActive = true;
        }
        else
            isActive = false;

        weldingParticles.gameObject.SetActive(isActive);
    }
}
