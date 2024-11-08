using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialImages;
    [SerializeField] private List<GameObject> tutorialMesa;
    [SerializeField] private List<GameObject> tutorialHorno;
    [SerializeField] private List<GameObject> tutorialFase2;
    [SerializeField] private List<GameObject> tutorialFase3;
    [SerializeField] private List<GameObject> tutorialTienda;
    [SerializeField] private List<GameObject> tutorialFase4;
    [SerializeField] private Canvas tutorialCanvas;

    private List<GameObject> currentTutorialImages;
    private int currentIndex = 0;
    private bool tutorialMesaShown = false;
    private bool tutorialHornoShown = false;
    private bool tutorialTiendaShown = false;
    private bool initialTutorialShown = false;
    private bool isTutorialActive = false;  // Nueva variable para controlar el estado del tutorial

    private void Start()
    {
        if (!initialTutorialShown)
        {
            currentTutorialImages = tutorialImages;
            ShowTutorial(currentTutorialImages);
            initialTutorialShown = true;
        }
    }

    private void Update()
    {
        if (!isTutorialActive) return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            NextImage();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            PreviousImage();
        }
    }

    public void ShowMesaCrafteoTutorial()
    {
        if (!tutorialMesaShown)
        {
            tutorialMesaShown = true;
            ShowTutorial(tutorialMesa);
        }
    }

    public void ShowHornoTutorial()
    {
        if (!tutorialHornoShown)
        {
            tutorialHornoShown = true;
            ShowTutorial(tutorialHorno);
        }
    }

    public void ShowTiendaTutorial()
    {
        if (!tutorialTiendaShown)
        {
            tutorialTiendaShown = true;
            ShowTutorial(tutorialTienda);
        }
    }

    public void ShowPhase2Tutorial()
    {
        ShowTutorial(tutorialFase2);
    }

    public void ShowPhase3Tutorial()
    {
        ShowTutorial(tutorialFase3);
    }

    public void ShowPhase4Tutorial()
    {
        ShowTutorial(tutorialFase4);
    }

    private void ShowTutorial(List<GameObject> tutorialList)
    {
        if (isTutorialActive)
        {
            CloseTutorial();  // Cerrar tutorial activo antes de abrir otro
        }

        currentTutorialImages = tutorialList;
        currentIndex = 0;

        tutorialCanvas.gameObject.SetActive(true);
        isTutorialActive = true;
        Time.timeScale = 0f;

        ShowCurrentImage();
    }

    private void NextImage()
    {
        if (currentIndex < currentTutorialImages.Count - 1)
        {
            currentIndex++;
            ShowCurrentImage();
        }
        else
        {
            CloseTutorial();
        }
    }

    private void PreviousImage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowCurrentImage();
        }
    }

    private void ShowCurrentImage()
    {
        for (int i = 0; i < currentTutorialImages.Count; i++)
        {
            currentTutorialImages[i].SetActive(i == currentIndex);
        }
    }

    private void CloseTutorial()
    {
        if (currentTutorialImages != null)
        {
            foreach (var image in currentTutorialImages)
            {
                image.SetActive(false);
            }
        }
        tutorialCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
        isTutorialActive = false;
    }
}
