using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Seleccion : MonoBehaviour
{
    public Button yourButton; // Asigna el botón desde el Inspector

    void Start()
    {
        // Agrega el listener al botón
        yourButton.onClick.AddListener(ChangeScene);
        
        // Inicia la coroutine para cambiar la escena después de 1 minuto
        StartCoroutine(ChangeSceneAfterDelay(60f)); // 60 segundos
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("SeleccionModos");
    }

    IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeScene();
    }
}
