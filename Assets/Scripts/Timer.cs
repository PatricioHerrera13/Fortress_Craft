using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{   
    public AudioSource audioSource;           // AudioSource para la música o efectos
    public AudioSource audioSourceBola;       // AudioSource para el sonido de la bola
    public AudioSource audioSourceDragon;     // AudioSource para el sonido del dragón
    public AudioSource backgroundAudioSource;     // AudioSource para efectos de fondo o música ambiental
    public AudioClip fase1Audio;              // Clip de audio para la fase 1
    public AudioClip fase2Audio;              // Clip de audio para la fase 2
    public AudioClip fase3Audio;              // Clip de audio para la fase 3
    public AudioClip fase4Audio;              // Clip de audio para la fase 4
    public AudioClip fase1BackgroundSound;        // Efecto de fondo para la fase 1
    public AudioClip fase2BackgroundSound;        // Efecto de fondo para la fase 2
    public AudioClip fase3BackgroundSound;        // Efecto de fondo para la fase 3
    public AudioClip fase4BackgroundSound;        // Efecto de fondo para la fase 4

    public AudioClip sonidoBola;         // Sonido para la bola
    public AudioClip sonidoDragon;       // Sonido para el dragón
    public AudioClip sonidoMaxBola;      // Sonido cuando la bola alcanza su tamaño máximo

    public float timer;
    public Text textoTimer;
    public PortalDeEntregas portal;
    public GameObject PanelFase;
    public GameObject PanelVic;
    public GameObject PanelDerrota;
    public float segundos = 2;
    public GameObject planos;
    public GameObject bloqueo;
    public VidaJefe vidaJefe;
    public Collider worldCollider;
    public TutorialCanvas tutorialCanvas;
    public OrderManager orderManager; // Asegúrate de asignar esto en el Inspector

    public GameObject caja1, caja2, caja3, dragon, bola, TORMETA;
    public Vector3 targetScale = new Vector3(20f, 20f, 20f);
    public float scaleSpeed = 0.00001f;

    private bool isPhase2Active = false;
    private bool isPhase3Active = false;
    private bool isPhase4Active = false;

    private void Start()
    {
        worldCollider.enabled = false;
        textoTimer.enabled = false;
        dragon.SetActive(false);
        bola.SetActive(false);
        TORMETA.SetActive(false);

        // Iniciar con música de fase 1
        audioSource.clip = fase1Audio;
        audioSource.Play();

         // Iniciar el audio de fondo para fase 1
        backgroundAudioSource.clip = fase1BackgroundSound;
        backgroundAudioSource.loop = true;  // Asegúrate de que se repita
        backgroundAudioSource.Play();
    }

    void Update()
    {
        if (isPhase3Active && timer > 0 && !isPhase4Active)
        {
            // Conteo regresivo en la fase 3 (10 segundos)
            timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            textoTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Cuando el temporizador de fase 3 termina, activar fase 4
            if (timer <= 0)
            {
                StartCoroutine(ActivarFase4());
            }
        }
        else if (isPhase4Active && timer > 0)
        {
            // Conteo regresivo en la fase 4 (100 segundos)
            timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            textoTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Crecimiento del dragón en fase 4
            dragon.transform.localScale = Vector3.Lerp(dragon.transform.localScale, targetScale, scaleSpeed * Time.deltaTime / 65);

            // Verificar victoria
            if (vidaJefe.saludActual <= 0)
            {
                PanelVic.SetActive(true);
                Time.timeScale = 0;
            }
        }

        // Activar fase 2
        if (portal.cantEntrega >= 5 && !isPhase2Active)
        {
            StartCoroutine(ActivarFase2());
        }

        // Activar fase 3
        if (portal.cantEntrega >= 10 && !isPhase3Active)
        {
            StartCoroutine(ActivarFase3());
        }

        // Reproducir el sonido de la bola en su crecimiento (solo cuando se está escalando)
        if (bola.transform.localScale != targetScale && !audioSourceBola.isPlaying)
        {
            audioSourceBola.PlayOneShot(sonidoBola);
        }
    }

    private IEnumerator ActivarFase2()
    {
        isPhase2Active = true;
        worldCollider.enabled = true;
        tutorialCanvas.ShowPhase2Tutorial();

        // Cambiar el audio a fase 2
        audioSource.clip = fase2Audio;
        audioSource.Play();

        // Cambiar el sonido de fondo a fase 2
        backgroundAudioSource.clip = fase2BackgroundSound;
        backgroundAudioSource.loop = true;  // Repetir el sonido de fondo
        backgroundAudioSource.Play();

        orderManager.SetPhase(2); // Activa la fase 2 en OrderManager

        caja1.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator ActivarFase3()
    {
        isPhase3Active = true;
        textoTimer.enabled = true;

        timer = 10f; // Temporizador de 10 segundos para la fase 3
        tutorialCanvas.ShowPhase3Tutorial();

        // Cambiar el audio a fase 3
        audioSource.clip = fase3Audio;
        audioSource.Play();

        // Cambiar el sonido de fondo a fase 3
        backgroundAudioSource.clip = fase3BackgroundSound;
        backgroundAudioSource.loop = true;  // Repetir el sonido de fondo
        backgroundAudioSource.Play();

        orderManager.SetPhase(3); // Activa la fase 3 en OrderManager

        caja2.SetActive(false);
        caja3.SetActive(false);

        yield return null;
    }

    private IEnumerator ActivarFase4()
    {
        isPhase4Active = true;
        timer = 120f; // Temporizador de 100 segundos para la fase 4
        dragon.SetActive(true);
        bola.SetActive(true);
        TORMETA.SetActive(true);
        tutorialCanvas.ShowPhase4Tutorial();

        // Cambiar el audio a fase 4
        audioSource.clip = fase4Audio;
        audioSource.Play();

        // Cambiar el sonido de fondo a fase 4
        backgroundAudioSource.clip = fase4BackgroundSound;
        backgroundAudioSource.loop = true;  // Repetir el sonido de fondo
        backgroundAudioSource.Play();

        // Reproducir sonido del dragón
        if (sonidoDragon != null && !audioSourceDragon.isPlaying)
        {
            audioSourceDragon.PlayOneShot(sonidoDragon);
        }

        orderManager.SetPhase(4); // Activa la fase 4 en OrderManager

        yield return null;
    }
}
