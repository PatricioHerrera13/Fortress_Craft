using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MenuPrincipal : MonoBehaviour
{
    public Button play;
    //public Button ctrl;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = play.GetComponent<Button>();
        //Button btnC = ctrl.GetComponent<Button>();
        btn.onClick.AddListener(Teleport);
        //btnC.onClick.AddListener(Control);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Teleport()
    {

        SceneManager.LoadScene("MenuPrincipal");

    }

    
}
