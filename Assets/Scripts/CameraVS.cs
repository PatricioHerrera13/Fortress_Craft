using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CameraVS : MonoBehaviour
{
    public Camera Cam1;
    public Camera Cam2;
    public Button back;

    public float borderSize = 0.02f; // Tamaño del borde (ajustable)

    // Start is called before the first frame update
    public void Start()
    {
        Button btn = back.GetComponent<Button>();
        btn.onClick.AddListener(Teleport);
        // Configuración de pantalla dividida con bordes negros
        Cam1.rect = new Rect(borderSize, borderSize, 0.5f - borderSize * 2, 1 - borderSize * 2); // Mitad izquierda con borde
        Cam2.rect = new Rect(0.5f + borderSize, borderSize, 0.5f - borderSize * 2, 1 - borderSize * 2); // Mitad derecha con borde

        // Ajustar la rotación inicial de las cámaras (45 grados en el eje X)
        Cam1.transform.rotation = Quaternion.Euler(45, 0, 0);
        Cam2.transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Apuntar las cámaras hacia los respectivos jugadores
        //

        // Mantener la inclinación de 45 grados en el eje X mientras miran a los jugadores
        Cam1.transform.rotation = Quaternion.Euler(45, Cam1.transform.rotation.eulerAngles.y, 0);
        Cam2.transform.rotation = Quaternion.Euler(45, Cam2.transform.rotation.eulerAngles.y, 0);
    }

    void Teleport()
    {
        SceneManager.LoadScene("MENU");
    }
}
