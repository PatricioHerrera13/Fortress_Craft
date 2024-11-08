using System.Collections.Generic;
using UnityEngine;

public class Basurero : MonoBehaviour
{
    public Collider jugadorCollider; // Collider del jugador
    public Collider jugadorCollider1;

    private void OnTriggerStay(Collider other)
    {
        // Verificar si el objeto que está dentro del trigger es el jugador
        if (other == jugadorCollider || other == jugadorCollider1)
        {
            // Comprobar si el jugador presiona la tecla 'E'
            if ((Input.GetKeyDown(KeyCode.X)) || (Input.GetKeyDown(KeyCode.I)))
            {
                Debug.Log("El jugador presionó E.");
                PickUpItem playerPickUp = other.GetComponentInChildren<PickUpItem>();

                // Verificar si el jugador sostiene un ítem
                if (playerPickUp != null && !string.IsNullOrEmpty(playerPickUp.GetPickedItemType()))
                {
                    // Eliminar el ítem del "HandPoint" del jugador
                    Transform hand = other.transform.Find("Hand/HandPoint");
                    if (hand != null && hand.childCount > 0)
                    {
                        // Destruir el objeto que se sostiene
                        GameObject objetoSostenido = hand.GetChild(0).gameObject;
                        Destroy(objetoSostenido);
                        Debug.Log("El objeto ha sido eliminado en el basurero.");

                        // Eliminar el tipo de ítem de las manos del jugador
                        playerPickUp.pickedItemType = null;

                        // Acción de eliminar confirmada (puedes añadir más feedback aquí)
                        Debug.Log("¡Objeto eliminado en el basurero!");
                    }
                }
                else
                {
                    Debug.Log("El jugador no sostiene ningún ítem.");
                }
            }
        }
    }
}
