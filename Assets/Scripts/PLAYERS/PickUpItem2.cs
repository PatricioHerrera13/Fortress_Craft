using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem2 : MonoBehaviour
{
    public GameObject Hand; // Referencia al objeto Hand
    private GameObject pickedItem = null;

    public string pickedItemType = ""; // Tipo de ítem que se está sosteniendo
    public float throwForce = 15f;
    public float throwAngle = 45f;

    private Vector3 lastMoveDirection = Vector3.zero; // Dirección del movimiento anterior
    private Vector3 lastPosition = Vector3.zero; // Última posición conocida

    public float HoldTime = 2;
    private bool StartTimer;

    private BoxCollider handCollider; // Referencia al BoxCollider de la mano
    public Transform HandPoint; // Referencia al punto donde el ítem aparecerá

    void Start()
    {
        handCollider = Hand.GetComponent<BoxCollider>(); // Obtiene el BoxCollider de Hand

        // Verifica si el BoxCollider está presente
        if (handCollider == null)
        {
            Debug.LogError("No BoxCollider found on Hand. Please add one.");
        }
    }

    void Update()
    {
        DetectMovement();

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (pickedItem == null)
            {
                TryPickUpItem(); // Intentar recoger un ítem
            }
            else
            {
                ReleaseItem(); // Soltar el ítem actual
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            StartTimer = true;
            StartCoroutine(HoldTimer());
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            StartTimer = false;
        }

        // Interacción con dispensadores
        TryInteractWithDispenser();
    }

    IEnumerator HoldTimer()
    {
        Debug.Log("Empezó Timer!");
        yield return new WaitForSeconds(HoldTime);
        if (!StartTimer)
        {
            Debug.Log("No se pudo");
        }
        else
        {
            Debug.Log("Lanzamiento!");
            //ThrowItem();
        }
    }

    private void DetectMovement()
    {
        if (transform.position != lastPosition)
        {
            lastMoveDirection = (transform.position - lastPosition).normalized;
            lastPosition = transform.position;
        }
    }

    private void TryPickUpItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider other in colliders)
        {
            if (other.CompareTag("Item"))
            {
                AnchorPointManager anchorManager = FindObjectOfType<AnchorPointManager>();
                if (anchorManager != null)
                {
                    // Verifica si el ítem está anclado
                    if (other.transform.parent != null)
                    {
                        anchorManager.ReleaseItem(other.gameObject); // Liberar el ítem del anclaje
                    }
                }

                Item itemComponent = other.GetComponent<Item>();
                if (itemComponent != null)
                {
                    other.GetComponent<Rigidbody>().useGravity = false;
                    other.GetComponent<Rigidbody>().isKinematic = true;
                    other.transform.position = HandPoint.position; // Mueve el ítem a HandPoint
                    other.gameObject.transform.SetParent(HandPoint); // Establece HandPoint como padre

                    pickedItem = other.gameObject;
                    pickedItemType = itemComponent.itemType;
                    break;
                }
            }
        }
    }

    public void ReleaseItem()
    {
        if (pickedItem != null)
        {
            pickedItem.GetComponent<Rigidbody>().useGravity = true;
            pickedItem.GetComponent<Rigidbody>().isKinematic = false;
            pickedItem.transform.SetParent(null);
            pickedItemType = "";
            pickedItem = null;
        }
    }

    private void ThrowItem()
    {
        if (pickedItem != null)
        {
            pickedItem.transform.SetParent(null);
            Rigidbody itemRb = pickedItem.GetComponent<Rigidbody>();
            itemRb.useGravity = true;
            itemRb.isKinematic = false;

            Vector3 throwDirection = lastMoveDirection;
            if (throwDirection == Vector3.zero)
            {
                throwDirection = transform.forward;
            }

            throwDirection = (throwDirection + Vector3.up * Mathf.Tan(throwAngle * Mathf.Deg2Rad)).normalized;
            itemRb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);

            pickedItem = null;
            pickedItemType = "";
        }
    }

    private void TryInteractWithDispenser()
    {
        if (Input.GetKeyDown(KeyCode.O) && pickedItem == null && handCollider != null)
        {
            Collider[] colliders = Physics.OverlapBox(Hand.transform.position, handCollider.size / 2, Hand.transform.rotation);
            foreach (Collider other in colliders)
            {
                Dispenser dispenser = other.GetComponent<Dispenser>();
                if (dispenser != null)
                {
                    dispenser.Interact(this); // Llama al método Interact del dispensador
                    break; // Sale del bucle después de interactuar
                }
            }
        }
    }

    public void GrabItemFromDispenser(Item item)
    {
        if (pickedItem == null)
        {
            pickedItem = item.gameObject;
            pickedItemType = item.itemType;
            item.transform.position = HandPoint.position; // Mueve el ítem a HandPoint
            item.transform.SetParent(HandPoint); // Establece HandPoint como padre
            item.GetComponent<Rigidbody>().isKinematic = true; // Hace el ítem kinemático
            item.GetComponent<Rigidbody>().useGravity = false; // Desactiva la gravedad
            Debug.Log($"Grabbed from dispenser: {pickedItemType}");
        }
    }

    public string GetPickedItemType()
    {
        return pickedItemType;
    }
    
    // NUEVO MÉTODO para obtener el prefab que está siendo sostenido
    public GameObject GetPickedPrefab()
    {
        return pickedItem; // Retorna el prefab del ítem recogido
    }
}
