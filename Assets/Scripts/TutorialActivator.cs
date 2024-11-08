using UnityEngine;

public class TutorialActivator : MonoBehaviour
{
    [SerializeField] private TutorialCanvas tutorialCanvas; // Referencia al TutorialCanvas UI

    public enum TutorialType { MesaCrafteo, Horno }
    [SerializeField] private TutorialType tutorialType; // Tipo de tutorial asociado a este objeto

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorialType == TutorialType.MesaCrafteo)
            {
                tutorialCanvas.ShowMesaCrafteoTutorial();
            }
            else if (tutorialType == TutorialType.Horno)
            {
                tutorialCanvas.ShowHornoTutorial();
            }
        }
    }
}
