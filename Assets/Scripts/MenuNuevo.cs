using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MenuNuevo : MonoBehaviour
{
    public Button coop;
    public Button vs;
    public Button ctrl;
    public Button credt;
    public Button salir;

    // Start is called before the first frame update
    void Start()
    {
        Button btnVS = vs.GetComponent<Button>();
        Button btnCoop = coop.GetComponent<Button>();
        Button btnC = ctrl.GetComponent<Button>();
        Button btnCred = credt.GetComponent<Button>();
        Button btnSalir = salir.GetComponent<Button>();

        btnVS.onClick.AddListener(Teleport1);
        btnCoop.onClick.AddListener(Teleport2);
        btnC.onClick.AddListener(Control);
        btnCred.onClick.AddListener(Creditos);
        btnSalir.onClick.AddListener(Salir);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Teleport1()
    {

        SceneManager.LoadScene("Versus");

    }

    void Teleport2()
    {

        SceneManager.LoadScene("SampleScene");

    }

    void Control()
    {

        SceneManager.LoadScene("Controles");

    }

    void Creditos()
    {

        SceneManager.LoadScene("Creditos");

    }
    
    public void Salir()
    {

        Application.Quit();
        Debug.Log("salio del juego");

    }
}
