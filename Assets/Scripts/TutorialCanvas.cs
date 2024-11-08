using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialImages;
    [SerializeField] private List<GameObject> tutorialMesa;
    [SerializeField] private List<GameObject> tutorialHorno;
    [SerializeField] private List<GameObject> tutorialFase2;
    [SerializeField] private List<GameObject> tutorialFase3;
    [SerializeField] private List<GameObject> tutorialFase4;
    [SerializeField] private Canvas tutorialCanvas;

    private List<GameObject> currentTutorialImages;
    private int currentIndex = 0;
    private bool tutorialMesaShown = false;
    private bool tutorialHornoShown = false;
    private bool initialTutorialShown = false;

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
            currentTutorialImages = tutorialMesa;
            ShowTutorial(currentTutorialImages);
            tutorialMesaShown = true;
        }
    }

    public void ShowHornoTutorial()
    {
        if (!tutorialHornoShown)
        {
            currentTutorialImages = tutorialHorno;
            ShowTutorial(currentTutorialImages);
            tutorialHornoShown = true;
        }
    }

    public void ShowPhase2Tutorial()
    {
        currentTutorialImages = tutorialFase2;
        ShowTutorial(currentTutorialImages);
    }

    public void ShowPhase3Tutorial()
    {
        currentTutorialImages = tutorialFase3;
        ShowTutorial(currentTutorialImages);
    }

    public void ShowPhase4Tutorial()
    {
        currentTutorialImages = tutorialFase4;
        ShowTutorial(currentTutorialImages);
    }

    private void ShowTutorial(List<GameObject> tutorialList)
    {
        if (currentTutorialImages != null)
        {
            foreach (var image in currentTutorialImages)
            {
                image.SetActive(false);
            }
        }

        currentTutorialImages = tutorialList;
        currentIndex = 0;

        tutorialCanvas.gameObject.SetActive(true);
        Time.timeScale = 0f;

        for (int i = 0; i < currentTutorialImages.Count; i++)
        {
            currentTutorialImages[i].SetActive(i == currentIndex);
        }
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
        tutorialCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
