using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 20f;
    public float dashSpeed = 50f; // Velocidad del dash
    public float dashDuration = 0.2f; // Duración del dash
    public float dashCooldown = 1f; // Tiempo de espera entre dashes
    private Rigidbody rb;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;
    private Vector3 lastMovementDirection; // Guardar la última dirección de movimiento

    public float idJugador = 1;
    public Wallet wallet;
    public Text moneyText;

    public Transform hand; // Referencia al objeto Hand
    public Transform handpoint;
    public float handOffsetDistance = 1f; // Distancia de la mano desde el jugador

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        wallet = new Wallet();
        wallet.SetTextComponent(moneyText);
    }

    public void AddMoneyToWallet(float amount)
    {
        wallet.AddMoney(amount);
    }

    private void Update()
    {
        // Actualiza el temporizador de cooldown
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Movimiento normal
        if (!isDashing)
        {
            // Movimiento utilizando las flechas del teclado
            float moveHorizontal = 0f;
            float moveVertical = 0f;

            if (Input.GetKey(KeyCode.A))
            {
                moveHorizontal = -1f; // Mover hacia la izquierda
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveHorizontal = 1f; // Mover hacia la derecha
            }

            if (Input.GetKey(KeyCode.W))
            {
                moveVertical = 1f; // Mover hacia adelante
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveVertical = -1f; // Mover hacia atrás
            }

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

            // Guarda la última dirección de movimiento válida
            if (movement.magnitude > 0)
            {
                lastMovementDirection = movement;
            }

            // Limita la velocidad en diagonal para evitar que el jugador se mueva más rápido
            rb.velocity = Vector3.ClampMagnitude(movement * speed, speed);

            // Mueve la mano usando la posición del jugador y la dirección de movimiento o la última dirección válida
            Vector3 handTargetPosition = transform.position + (lastMovementDirection.normalized * handOffsetDistance);

            // Si hay movimiento, actualiza la posición y rotación de la mano según la dirección actual
            if (movement != Vector3.zero)
            {
                hand.position = handTargetPosition; // Mover la mano hacia la nueva posición
                hand.rotation = Quaternion.LookRotation(-movement); // Invertir la dirección de movimiento
            }
            else if (lastMovementDirection != Vector3.zero)
            {
                // Si no hay movimiento, mantén la mano en la última dirección válida
                hand.position = handTargetPosition; // Mover la mano hacia la última dirección
                hand.rotation = Quaternion.LookRotation(-lastMovementDirection);
            }

            // Dash
            if (Input.GetKeyDown(KeyCode.V) && dashCooldownTimer <= 0)
            {
                StartCoroutine(Dash(lastMovementDirection));
            }
        }
    }

    private IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;

        // Aumenta la velocidad durante el dash
        rb.velocity = direction.normalized * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        // Vuelve a la velocidad normal
        isDashing = false;

        // Reiniciar el cooldown
        dashCooldownTimer = dashCooldown;
    }

    public bool TienePrefabConSprite(Sprite spriteEsperado)
    {
        if (handpoint.childCount > 0)
        {
            SpriteRenderer objetoEnMano = handpoint.GetChild(0).GetComponent<SpriteRenderer>();
            if (objetoEnMano != null && objetoEnMano.sprite == spriteEsperado)
            {
                Debug.Log("El sprite del objeto en handpoint coincide con el sprite esperado.");
                return true;
            }
            Debug.Log("El sprite no coincide.");
        }
        Debug.Log("No hay objeto en handpoint para comparar el sprite.");
        return false;
    }

    public bool TienePrefab(GameObject prefabRequerido)
    {
        if (handpoint.childCount > 0)
        {
            GameObject objetoEnMano = handpoint.GetChild(0).gameObject;
            if (objetoEnMano == prefabRequerido)
            {
                Debug.Log("El prefab en handpoint coincide con el prefab requerido.");
                return true;
            }
        }
        Debug.Log("No hay objeto en handpoint o no coincide con el prefab requerido.");
        return false;
    }

    public void RemoverPrefabDelHandPoint()
    {
        if (handpoint.childCount > 0)
        {
            Debug.Log("Prefab removido del handpoint.");
            Destroy(handpoint.GetChild(0).gameObject);
        }
        else
        {
            Debug.Log("No hay ningún prefab en el handpoint para remover.");
        }
    }
}
