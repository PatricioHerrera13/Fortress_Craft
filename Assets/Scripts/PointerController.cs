using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone;
    public float moveSpeed = 100f;
    public TanqueComb tanqueComb; // Referencia al TanqueComb para finalizar el QTE
     // Tecla de activación del QTE en el inspector
    private float direction = 1f;
    private RectTransform pointerTransform;
    private Vector3 targetPosition;

    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = pointB.position;
    }

    void Update()
    {
        pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
            direction = 1f;
        }
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
        {
            targetPosition = pointA.position;
            direction = -1f;
        }

        if ((Input.GetKeyDown(KeyCode.X)) || (Input.GetKeyDown(KeyCode.I)))
        {
            CheckSuccess();
        }
    }

    void CheckSuccess()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            Debug.Log("¡Éxito!");
            tanqueComb.CompleteQTE(true);
            // Opcionalmente, agrega retroalimentación visual/auditiva aquí
        }
        else
        {
            Debug.Log("¡Fallido!");
            tanqueComb.CompleteQTE(false);
            // Opcionalmente, agrega retroalimentación visual/auditiva aquí
        }
    }
}
