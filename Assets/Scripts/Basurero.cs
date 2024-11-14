using System.Collections.Generic;
using UnityEngine;

public class Basurero : MonoBehaviour
{
    public Collider jugadorCollider; // Collider del jugador 1
    public Collider jugadorCollider1; // Collider del jugador 2

    // Variable para manejar el efecto de sonido al eliminar un ítem
    public AudioSource audioSource; // El AudioSource que reproducirá el sonido
    public AudioClip sfxEliminarItem; // Sonido para cuando se elimina un ítem

    private void OnTriggerStay(Collider other)
    {
        // Verificar si el objeto que está dentro del trigger es el jugador 1 o el jugador 2
        if (other == jugadorCollider || other == jugadorCollider1)
        {
            // Comprobar si el jugador presiona la tecla 'X' o 'I'
            if ((Input.GetKeyDown(KeyCode.X) && other == jugadorCollider) ||
                (Input.GetKeyDown(KeyCode.I) && other == jugadorCollider1))
            {
                Debug.Log("El jugador presionó la tecla de acción en el basurero.");
                
                // Determinar el script PickUpItem correspondiente al jugador que activó el basurero
                PickUpItem playerPickUp = other.GetComponentInChildren<PickUpItem>();
                PickUpItem2 playerPickUp2 = other.GetComponentInChildren<PickUpItem2>();

                // Verificar si el jugador 1 sostiene un ítem
                if (playerPickUp != null && !string.IsNullOrEmpty(playerPickUp.GetPickedItemType()))
                {
                    EliminarItem(playerPickUp, other);
                }
                // Verificar si el jugador 2 sostiene un ítem
                else if (playerPickUp2 != null && !string.IsNullOrEmpty(playerPickUp2.GetPickedItemType()))
                {
                    EliminarItem(playerPickUp2, other);
                }
                else
                {
                    Debug.Log("El jugador no sostiene ningún ítem.");
                }
            }
        }
    }

    private void EliminarItem(PickUpItem playerPickUp, Collider jugador)
    {
        Transform hand = jugador.transform.Find("Hand/HandPoint");
        if (hand != null && hand.childCount > 0)
        {
            // Destruir el objeto que se sostiene
            Destroy(hand.GetChild(0).gameObject);
            playerPickUp.ReleaseItem(); // Llamar a ReleaseItem para liberar el ítem
            Debug.Log("¡Objeto eliminado en el basurero para el jugador 1!");
            // Reproducir el efecto de sonido para eliminar un ítem
            if (sfxEliminarItem != null && audioSource != null)
            {
                audioSource.PlayOneShot(sfxEliminarItem);
            }
        }
    }

    private void EliminarItem(PickUpItem2 playerPickUp2, Collider jugador)
    {
        Transform hand = jugador.transform.Find("Hand/HandPoint");
        if (hand != null && hand.childCount > 0)
        {
            // Destruir el objeto que se sostiene
            Destroy(hand.GetChild(0).gameObject);
            playerPickUp2.ReleaseItem(); // Llamar a ReleaseItem para liberar el ítem
            Debug.Log("¡Objeto eliminado en el basurero para el jugador 2!");
            // Reproducir el efecto de sonido para eliminar un ítem
            if (sfxEliminarItem != null && audioSource != null)
            {
                audioSource.PlayOneShot(sfxEliminarItem);
            }
        }
    }
}
