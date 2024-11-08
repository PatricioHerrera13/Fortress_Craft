using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }

    private const string PROJECT_ID = "cm2kyairz00013qkntrs3g90y";
    private Dictionary<string, string> translations = new Dictionary<string, string>();
    private string currentLanguage = Languages.languageCodes["Spanish"]; // Idioma por defecto es español

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene el objeto al cambiar escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }
    // Método para obtener el idioma actual
    public string GetCurrentLanguage()
    {
        return currentLanguage;
    }

    // Método para obtener las traducciones desde la API
    public IEnumerator GetTranslations(string lang, System.Action callback)
    {
        currentLanguage = lang;
        
        if (currentLanguage == Languages.languageCodes["Spanish"]) // No realiza la solicitud si el idioma es español
        {
            Debug.Log("idiona español");
            callback?.Invoke();
            yield break; // Si el idioma es "es-AR", no se hace la petición
        }

        string url = "https://traducila.vercel.app/api/translations/cm2kyairz00013qkntrs3g90y/en-US";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        Debug.Log(request.result);
    

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error fetching translations: {request.error}");
            Debug.Log(request);
        }
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Eliminar las llaves del JSON
            string jsonString = request.downloadHandler.text;

            // Separar por comas
            string[] pairs = jsonString.Split(',');

            // Recorrer cada par de clave:valor
            foreach (string pair in pairs)
            {
                // Separar por los dos puntos ":"
                string[] keyValue = pair.Split(':');

                // Eliminar comillas y espacios adicionales
                string key = keyValue[0].Trim().Replace("\"", "").Replace("{", "").Replace("}", "");
                string value = keyValue[1].Trim().Replace("\"", "").Replace("{", "").Replace("}", "");
                Debug.Log(value);
                // Añadir al diccionario
                translations[key] = value;
            }

            callback?.Invoke();
        }
            
    }

    // Método para obtener una traducción
    public string GetPhrase(string key)
    {
       // Debug.Log(key);
        if (translations.Count == 0)
        {
            return key;
        }
        if (translations.ContainsKey(key))
        {
            return translations[key];
            
        }
        Debug.Log(translations[key]);
        return key; // Retorna la clave si no hay traducción disponible
    }

    [System.Serializable]
    public class TranslationResponse
    {
        public List<TranslationEntry> translations;
    }

    [System.Serializable]
    public class TranslationEntry
    {
        public string key;
        public string value;
    }
}

public static class Languages
{
    public static readonly Dictionary<string, string> languageCodes = new Dictionary<string, string>()
    {
        { "Spanish", "es-AR" },
        { "English", "en-US" },
        { "Portuguese", "pt-BR" },
        { "German", "de-DE" },
        { "French", "fr-FR" }
    };
}
