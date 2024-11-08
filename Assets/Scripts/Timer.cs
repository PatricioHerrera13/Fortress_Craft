using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timer = 500;
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

    public GameObject caja1, caja2, caja3, dragon, bola;
    public Vector3 targetScale = new Vector3(20f, 20f, 20f);
    public float scaleSpeed = 0.1f;

    private bool isPhase2Active = false;
    private bool isPhase3Active = false;
    private bool isPhase4Active = false;

    private void Start()
    {
        worldCollider.enabled = false;
        textoTimer.enabled = false;
        dragon.SetActive(false);
        bola.SetActive(false);
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            textoTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Activar fase 2
            if (portal.cantEntrega >= 20 && !isPhase2Active)
            {
                StartCoroutine(ActivarFase2());
            }

            // Activar fase 3
            if (portal.cantEntrega >= 40 && !isPhase3Active)
            {
                StartCoroutine(ActivarFase3());
            }

            // Activar fase 4
            if (isPhase3Active && timer <= 0 && !isPhase4Active)
            {
                StartCoroutine(ActivarFase4());
            }

            // Crecimiento del dragón en fase 4
            if (isPhase4Active)
            {
                dragon.transform.localScale = Vector3.Lerp(dragon.transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
            }

            // Verificar victoria
            if (vidaJefe.saludActual <= 0)
            {
                PanelVic.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else if (!isPhase4Active)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        timer = 30f;
        tutorialCanvas.ShowPhase3Tutorial();

        orderManager.SetPhase(3); // Activa la fase 3 en OrderManager

        caja2.SetActive(false);
        caja3.SetActive(false);
        yield return null;
    }

    private IEnumerator ActivarFase4()
    {
        isPhase4Active = true;
        timer = 500f;
        dragon.SetActive(true);
        bola.SetActive(true);
        tutorialCanvas.ShowPhase4Tutorial();

        orderManager.SetPhase(4); // Activa la fase 4 en OrderManager

        yield return null;
    }

}
