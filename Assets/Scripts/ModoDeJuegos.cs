using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ModoDeJuegos : MonoBehaviour
{
    public Button coop;
    public Button vs;

    // Start is called before the first frame update
    void Start()
    {
        Button btnCoop = coop.GetComponent<Button>();
        Button btnVS = vs.GetComponent<Button>();
        btnCoop.onClick.AddListener(TeleportCoop);
        btnVS.onClick.AddListener(TeleportVS);
    }

    void TeleportCoop()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void TeleportVS()
    {
        SceneManager.LoadScene("Versus");
    }
}
