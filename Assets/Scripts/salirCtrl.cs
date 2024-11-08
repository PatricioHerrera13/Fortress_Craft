using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class salirCtrl : MonoBehaviour
{
    public Button salir;

    // Start is called before the first frame update
    void Start()
    {
        Button btnSalir = salir.GetComponent<Button>();
        btnSalir.onClick.AddListener(Salir);
    }

    public void Salir()
    {

        SceneManager.LoadScene("MENU");

    }
}
