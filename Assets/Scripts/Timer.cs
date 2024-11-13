using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
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
    }

    private IEnumerator ActivarFase2()
    {
        isPhase2Active = true;
        worldCollider.enabled = true;
        tutorialCanvas.ShowPhase2Tutorial();

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

        orderManager.SetPhase(4); // Activa la fase 4 en OrderManager

        yield return null;
    }
}
