using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject calibratePrompt;

    [SerializeField]
    private GameObject scanImagePrompt;

    [SerializeField]
    private GameObject destinationReachedPrompt;

    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        counter = 0;
        ShowCalibratePrompt();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.touches != null)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                counter++;
                counter %= 3;
            }
        }


        switch(counter)
        {
            case 0:
                ShowCalibratePrompt();
                break;
            case 1:
                ShowScanImagePrompt();
                break;
            case 2:
                ShowDestinationReachedPrompt();
                break;
            default:
                break;
        }
        */
    }

    public void ShowCalibratePrompt()
    {
        if (scanImagePrompt.activeInHierarchy == true)
        {
            scanImagePrompt.SetActive(false);
        }

        if (destinationReachedPrompt.activeInHierarchy == true)
        {
            destinationReachedPrompt.SetActive(false);
        }

        if (calibratePrompt.activeInHierarchy == true)
        {
            return;
        }

        calibratePrompt.SetActive(true);
    }

    
    public void ShowScanImagePrompt()
    {

        if (destinationReachedPrompt.activeInHierarchy == true)
        {
            destinationReachedPrompt.SetActive(false);
        }

        if (calibratePrompt.activeInHierarchy == true)
        {
            calibratePrompt.SetActive(false);
        }

        if (scanImagePrompt.activeInHierarchy == true)
        {
            return;
        }

        scanImagePrompt.SetActive(true);
    }


    public void ShowDestinationReachedPrompt()
    {
        if (scanImagePrompt.activeInHierarchy == true)
        {
            scanImagePrompt.SetActive(false);
        }

        if (calibratePrompt.activeInHierarchy == true)
        {
            calibratePrompt.SetActive(false);
        }

        if (destinationReachedPrompt.activeInHierarchy == true)
        {
            return;
        }

        destinationReachedPrompt.SetActive(true);
    }
}
