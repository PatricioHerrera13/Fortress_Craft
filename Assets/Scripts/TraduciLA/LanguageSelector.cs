using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LanguageSelector : MonoBehaviour
{
    public Button buttonSpanish;
    public Button buttonEnglish;
    public TMP_Text jugarButton;
    //public TMP_Text opcionesButton;

    void Start()
    {
        // Asignar eventos a los botones para seleccionar el idioma
        buttonSpanish.onClick.AddListener(() => StartCoroutine(SelectLanguage(Languages.languageCodes["Spanish"])));
        buttonEnglish.onClick.AddListener(() => StartCoroutine(SelectLanguage(Languages.languageCodes["English"])));

        // Iniciar con el idioma por defecto
        StartCoroutine(SelectLanguage(LanguageManager.Instance.GetCurrentLanguage()));
    }

    IEnumerator SelectLanguage(string language)
    {
        // Desactivar botones mientras se cargan las traducciones
        buttonSpanish.interactable = false;
        buttonEnglish.interactable = false;

        // Obtener las traducciones desde la API
        yield return StartCoroutine(LanguageManager.Instance.GetTranslations(language, UpdateMenuText));
        
        // Reactivar los botones una vez obtenidas las traducciones
        buttonSpanish.interactable = true;
        buttonEnglish.interactable = true;
    }

    void UpdateMenuText()
    {
        // Aquí actualizas los textos de la UI con las traducciones obtenidas
        jugarButton.text = LanguageManager.Instance.GetPhrase("JUGAR");
        //opcionesButton.text = LanguageManager.Instance.GetPhrase("Controles");
        Debug.Log("Deberia actualizar el texto");
        
        
    }
}
