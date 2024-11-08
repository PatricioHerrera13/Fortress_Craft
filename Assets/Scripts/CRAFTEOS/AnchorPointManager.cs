using System.Collections.Generic;
using UnityEngine;

public class AnchorPointManager : MonoBehaviour
{
    [SerializeField] private int anchorPointCount = 3; // Cantidad de puntos de anclaje
    [SerializeField] private float anchorDistance = 1f; // Distancia máxima para anclar ítems
    [SerializeField] private BoxCollider detectionCollider; // Collider para detectar ítems
    [SerializeField] private List<Vector3> anchorPositions; // Posiciones personalizadas para los puntos de anclaje

    private List<Transform> anchorPoints = new List<Transform>();
    private List<bool> anchorOccupied; // Estado de ocupación de cada punto de anclaje

    private void Awake()
    {
        CreateAnchorPoints();
        anchorOccupied = new List<bool>(new bool[anchorPoints.Count]); // Inicializa la lista de ocupación
    }

    private void CreateAnchorPoints()
    {
        foreach (var pos in anchorPositions)
        {
            GameObject anchorPoint = new GameObject($"AnchorPoint_{anchorPoints.Count + 1}");
            anchorPoint.transform.SetParent(transform);
            anchorPoint.transform.position = transform.position + pos; // Usar posiciones definidas

            Rigidbody rb = anchorPoint.AddComponent<Rigidbody>();
            rb.isKinematic = true;

            anchorPoints.Add(anchorPoint.transform);
        }
    }

    private void Update()
    {
        CheckForAnchoringItems();
        UpdateAnchorOccupancy();
    }

    private void CheckForAnchoringItems()
    {
        Collider[] colliders = Physics.OverlapBox(detectionCollider.transform.position, detectionCollider.size / 2, detectionCollider.transform.rotation);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Item"))
            {
                Rigidbody itemRb = collider.GetComponent<Rigidbody>();
                if (itemRb != null && !itemRb.isKinematic)
                {
                    for (int i = 0; i < anchorPoints.Count; i++)
                    {
                        // Verificar si el punto de anclaje está disponible
                        if (!anchorOccupied[i] && Vector3.Distance(collider.transform.position, anchorPoints[i].position) < anchorDistance)
                        {
                            AnchorItem(collider.gameObject, anchorPoints[i]);
                            anchorOccupied[i] = true; // Marcar como ocupado
                            break; // Salir del bucle al anclar el ítem
                        }
                    }
                }
            }
        }
    }

    private void UpdateAnchorOccupancy()
    {
        for (int i = 0; i < anchorPoints.Count; i++)
        {
            // Si el punto de anclaje tiene un hijo (ítem anclado), marcar como ocupado
            anchorOccupied[i] = anchorPoints[i].childCount > 0;
        }
    }

    private void AnchorItem(GameObject item, Transform anchor)
    {
        item.transform.SetParent(anchor);
        Rigidbody itemRb = item.GetComponent<Rigidbody>();
        itemRb.isKinematic = true; // Desactivar gravedad
        itemRb.useGravity = false; // Desactivar gravedad
        item.transform.localPosition = Vector3.zero; // Posicionar el ítem en el anclaje

        // Ajustar posición sobre el anchor
        item.transform.position = anchor.position + new Vector3(0, item.transform.localScale.y / 2, 0); // Ajusta según la altura del ítem
    }

    public void ReleaseItem(GameObject item)
    {
        Transform parentTransform = item.transform.parent;
        if (parentTransform != null && anchorPoints.Contains(parentTransform))
        {
            item.transform.SetParent(null); // Desancla el ítem
            Rigidbody itemRb = item.GetComponent<Rigidbody>();
            itemRb.isKinematic = false; // Reactivar gravedad
            itemRb.useGravity = true; // Activar gravedad

            // Liberar el punto de anclaje correspondiente
            int anchorIndex = anchorPoints.IndexOf(parentTransform);
            if (anchorIndex != -1)
            {
                anchorOccupied[anchorIndex] = false; // Marcar como disponible
            }
        }
    }
}
