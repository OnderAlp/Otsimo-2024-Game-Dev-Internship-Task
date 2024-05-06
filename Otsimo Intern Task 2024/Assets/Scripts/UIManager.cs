using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("UI Manager is Null!");
            }

            return _instance;
        }
    }

    public GameObject DrawAgent;
    private DrawAgent DrawScript;

    public Image wheelSelectedColorImage;
    public GameObject wheelColorPanel;
    public GameObject stamp;
    public Button[] buttonListForInteractible;
    public Slider sizeSlider;
    public Text sizeSliderText;
    public GameObject penSelectionBG;
    public GameObject stampSelectionBG;
    public GameObject eraserSelectionBG;

    public GameObject[] colorSelectionBG;
    public GameObject colorWheelSelectionBG;
    public GameObject cannonSelectionBG;


    private Color wheelColor;
    private bool cannonSelected = false;

    public GameObject exitPanel;
    private void Awake()
    {
        _instance = this;
        DrawScript = DrawAgent.GetComponent<DrawAgent>();
        wheelColor = Color.white;
        sizeSliderText.text = "Pen Size";
    }

    public void SelectColorIndex(int index)
    {

        switch (index)
        {
            case 1:
                DrawScript.ChangeColor(Color.black);
                ColorBGActivator(0);
                AudioManager.Instance.SelectColor();
                SelectPen();
                break;
            case 2:
                DrawScript.ChangeColor(Color.white);
                ColorBGActivator(1);
                AudioManager.Instance.SelectColor();
                SelectPen();
                break;
            case 3:
                DrawScript.ChangeColor(Color.red);
                ColorBGActivator(2);
                AudioManager.Instance.SelectColor();
                SelectPen();
                break;
            case 4:
                DrawScript.ChangeColor(Color.green);
                ColorBGActivator(3);
                AudioManager.Instance.SelectColor();
                SelectPen();
                break;
            case 5:
                DrawScript.ChangeColor(Color.blue);
                ColorBGActivator(4);
                AudioManager.Instance.SelectColor();
                SelectPen();
                break;
            default: 
                break;
        }
    }

    private void ColorBGActivator(int index)
    {
        for (int i = 0; i < colorSelectionBG.Length; i++)
        {
            if(i == index)
            {
                colorSelectionBG[i].SetActive(true);
            }
            else
            {
                colorSelectionBG[i].SetActive(false);
            }
        }

        colorWheelSelectionBG.SetActive(false);
    }

    private void DeactivateColorsBG()
    {
        foreach (var item in colorSelectionBG)
        {
            item.SetActive(false);
        }
    }

    public void SelectBucket()
    {
        DrawScript.Bucket();
        AudioManager.Instance.SelectBucket();
    }

    public void SelectEraser()
    {
        DrawScript.ChangeColor(Color.white);

        DrawScript.ActivatePen();
        DrawScript.DeactivateStamp();
        DrawScript.DeactivateCannon();

        sizeSlider.interactable = true;

        sizeSliderText.text = "Eraser Size";

        penSelectionBG.SetActive(false);
        stampSelectionBG.SetActive(false);
        eraserSelectionBG.SetActive(true);
        colorWheelSelectionBG.SetActive(false);

        AudioManager.Instance.SelectEraser();

        if (cannonSelected)
        {
            cannonSelectionBG.GetComponent<Animator>().SetTrigger("UnSelected");
            cannonSelected = false;
        }

    }

    public void SelectColorWheelColor(Color color)
    {
        DrawScript.ChangeColor(color);
        wheelSelectedColorImage.color = color;
        wheelColor = color;

        colorWheelSelectionBG.SetActive(true);
        DeactivateColorsBG();

        if (cannonSelected)
        {
            cannonSelectionBG.GetComponent<Animator>().SetTrigger("UnSelected");
            cannonSelected = false;
        }

    }

    public void ActivateColorWheelPanel()
    {
        wheelColorPanel.SetActive(true);
        DrawScript.DeactivateStamp();
        DrawScript.DeactivatePen();
        DrawScript.DeactivateCannon();
        penSelectionBG.SetActive(false);
        stampSelectionBG.SetActive(false);
        eraserSelectionBG.SetActive(false);
        colorWheelSelectionBG.SetActive(false);
        DeactivateButtons();
        DeactivateColorsBG();

        if (cannonSelected)
        {
            cannonSelectionBG.GetComponent<Animator>().SetTrigger("UnSelected");
            cannonSelected = false;
        }

    }

    public void DisableColorWheelPanel()
    {
        wheelColorPanel.SetActive(false);
        ActivateButtons();
    }

    public void SetDrawingActivation(bool val)
    {
        if(val)
        {
            DrawScript.ActivatePen();
            DrawScript.DeactivateStamp();
            DrawScript.DeactivateCannon();

            penSelectionBG.SetActive(true);
        }
        else 
        {
            DrawScript.DeactivatePen(); 
        }
        
    }

    public void SetColorToWheelColor()
    {
        DrawScript.ActivatePen();
        DrawScript.DeactivateStamp();
        DrawScript.DeactivateCannon();

        DrawScript.ChangeColor(wheelColor);
        penSelectionBG.SetActive(true);
        stampSelectionBG.SetActive(false);
        eraserSelectionBG.SetActive(false);
        colorWheelSelectionBG.SetActive(true);

        DeactivateColorsBG();

        AudioManager.Instance.SelectColor();

        if (cannonSelected)
        {
            cannonSelectionBG.GetComponent<Animator>().SetTrigger("UnSelected");
            cannonSelected = false;
        }
    }

    public void DeactivateButtons()
    {
        foreach (var button in buttonListForInteractible)
        {
            button.interactable = false;
        }

        sizeSlider.interactable = false;
    }
    public void ActivateButtons()
    {
        foreach (var button in buttonListForInteractible)
        {
            button.interactable = true;
        }

        sizeSlider.interactable = true;
    }

    public void SelectStamp()
    {
        DrawScript.ActivateStamp();
        DrawScript.DeactivatePen();
        DrawScript.DeactivateCannon();

        sizeSlider.interactable = true;

        sizeSliderText.text = "Stamp Size";

        penSelectionBG.SetActive(false);
        stampSelectionBG.SetActive(true);
        eraserSelectionBG.SetActive(false);

        DeactivateColorsBG();
        colorWheelSelectionBG.SetActive(false);

        AudioManager.Instance.SelectStamp();

        if (cannonSelected)
        {
            cannonSelectionBG.GetComponent<Animator>().SetTrigger("UnSelected");
            cannonSelected = false;
        }
    }

    public void SelectPen()
    {
        DrawScript.ActivatePen();
        DrawScript.DeactivateStamp();
        DrawScript.DeactivateCannon();

        sizeSlider.interactable = true;

        sizeSliderText.text = "Pen Size";

        penSelectionBG.SetActive(true);
        stampSelectionBG.SetActive(false);
        eraserSelectionBG.SetActive(false);

        AudioManager.Instance.SelectPen();
        
        
        if (cannonSelected)
        {
            cannonSelectionBG.GetComponent<Animator>().SetTrigger("UnSelected");
            cannonSelected = false;
        }
        
    }
    public void SelectCannon()
    {
        cannonSelected = true;

        DrawScript.DeactivatePen();
        DrawScript.DeactivateStamp();
        DrawScript.ActivateCannon();

        sizeSlider.interactable = false;

        sizeSliderText.text = "";

        DeactivateColorsBG();

        penSelectionBG.SetActive(false);
        stampSelectionBG.SetActive(false);
        eraserSelectionBG.SetActive(false);
        colorWheelSelectionBG.SetActive(false);

        AudioManager.Instance.SelectCannon();

        cannonSelectionBG.GetComponent<Animator>().SetTrigger("Selected");
    }

    public void PaintBallHit(Color color, Vector3 mousePos)
    {
        DrawScript.CreateBrushOnHit(color, mousePos);
    }

    public void TakeAScreenshot()
    {
        DrawScript.SaveScreenshot();
    }

    public void LoadScreenshot()
    {
        DrawScript.LoadScreenshot();
    }

    public void ActivateExitPanel()
    {
        exitPanel.SetActive(true);

        DrawScript.DeactivateStamp();
        DrawScript.DeactivatePen();
        DrawScript.DeactivateCannon();
        penSelectionBG.SetActive(false);
        stampSelectionBG.SetActive(false);
        eraserSelectionBG.SetActive(false);
        colorWheelSelectionBG.SetActive(false);
        DeactivateButtons();
    }

    public void DeactivateExitPanel()
    {
        exitPanel.SetActive(false);
        ActivateButtons();
        SelectPen();
    }

}
