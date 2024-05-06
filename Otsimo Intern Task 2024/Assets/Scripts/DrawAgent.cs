using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;
using UnityEngine.Pool;

public class DrawAgent : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brush;
    public GameObject BrushContainer;
    public SpriteRenderer canvasSprite;
    public Sprite blankCanvas;
    public GameObject stamp;
    public GameObject cannon;
    public GameObject paintBall;


    LineRenderer currentLineRenderer;
    private Color currentColor = Color.black;

    Vector2 lastPos;

    private bool outOfBoundsCheck = false;
    private bool penActivation = true;
    private bool stampActivation = false;
    private bool cannonActivation = false;
    private float _brushSize = 0.1f;


    [SerializeField]
    private float minDistance = 0.1f;
    [SerializeField]
    public Color bucketColor = Color.white;

    [SerializeField]
    private Slider _slider;

    private Vector3 canvasLocalTopRight;
    private Vector3 canvasLocalTopLeft;
    private Vector3 canvasLocalBottomRight;
    private Vector3 canvasLocalBottomLeft;

    private Vector3 screenLocationLocalTopRight;
    private Vector3 screenLocationLocalTopLeft;
    private Vector3 screenLocationLocalBottomRight;
    private Vector3 screenLocationLocalBottomLeft;

    private float widthRatio = 0.79f;
    private float heightRatio = 0.71f;

    private bool widthCorrection = false;
    private bool heightCorrection = false;


    private string galleryPath;

    private void Start()
    {
        float[] temp = CalculateCanvasSize();
        CanvasResizer();
        galleryPath = GetGalleryPath();
        StartCoroutine(CoroutineLoadScreenshot());
        

        _slider.onValueChanged.AddListener((val) => {
            _brushSize = val;
        });

    }

    private void Update()
    {
        if (penActivation)
        {
            Draw();
        }
        if(stampActivation)
        {
            StampCreator();
        }
        if(cannonActivation)
        {
            CannonController();
        }

    }

    private void CanvasResizer()
    {
        while(true)
        {
            float[] currentSizes = CalculateCanvasSize();

            if (currentSizes[0] / Screen.width >= widthRatio)
            {
                canvasSprite.transform.localScale -= new Vector3(1f, 0, 0);
                continue;
            }
            else
            {
                widthCorrection = true;
            }

            if (currentSizes[1] / Screen.height >= heightRatio)
            {
                canvasSprite.transform.localScale -= new Vector3(0, 1f, 0);
                continue;
            }
            else
            {
                heightCorrection = true;
            }

            if (widthCorrection && heightCorrection) 
            {
                break;
            }
        }
    }

    private float[] CalculateCanvasSize()
    {
        var bounds = canvasSprite.bounds;

        Vector3 canvasPosition = new Vector3(Mathf.Round(canvasSprite.transform.position.x), Mathf.Round(canvasSprite.transform.position.y), canvasSprite.transform.position.z);

        canvasLocalTopRight = new Vector3(Mathf.Round(bounds.extents.x), Mathf.Round(bounds.extents.y), 0) + canvasPosition;
        canvasLocalTopLeft = new Vector3(Mathf.Round(bounds.extents.x) - Mathf.Round(bounds.size.x), Mathf.Round(bounds.extents.y), 0) + canvasPosition;
        canvasLocalBottomRight = new Vector3(Mathf.Round(bounds.extents.x), Mathf.Round(bounds.extents.y) - Mathf.Round(bounds.size.y), 0) + canvasPosition;
        canvasLocalBottomLeft = new Vector3(Mathf.Round(bounds.extents.x) - Mathf.Round(bounds.size.x), Mathf.Round(bounds.extents.y) - Mathf.Round(bounds.size.y), 0) + canvasPosition;


        screenLocationLocalTopRight = Camera.main.WorldToScreenPoint(canvasLocalTopRight);
        screenLocationLocalTopLeft = Camera.main.WorldToScreenPoint(canvasLocalTopLeft);
        screenLocationLocalBottomRight = Camera.main.WorldToScreenPoint(canvasLocalBottomRight);
        screenLocationLocalBottomLeft = Camera.main.WorldToScreenPoint(canvasLocalBottomLeft);


        float currentWidth = screenLocationLocalTopRight.x - screenLocationLocalTopLeft.x;
        float currentHeight = screenLocationLocalTopRight.y - screenLocationLocalBottomRight.y;
        Debug.Log(currentHeight);

        float[] val = { currentWidth, currentHeight };

        return val;

    }


    public void Draw()
    {

        if(Input.GetMouseButtonDown(0) || outOfBoundsCheck)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    CreateBrush();
                    outOfBoundsCheck = false;
                }
            }
        }

        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Debug.Log("BBBBB");

                    Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                    if (Vector2.Distance(mousePos, lastPos) > minDistance)
                    {
                        AddPoint(mousePos);
                        lastPos = mousePos;
                        AudioManager.Instance.SelectPen();
                    }
                }
            }
            else
            {
                Debug.Log("ÝMDAT");
                if(currentLineRenderer != null)
                {
                    outOfBoundsCheck = true;
                }
                currentLineRenderer = null;
                
            }

        }
        else
        {
            currentLineRenderer = null;
        }

    }

    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush, BrushContainer.transform);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);

        Gradient tempGradient = new Gradient();

        GradientColorKey[] tempColorKeys = new GradientColorKey[2];
        tempColorKeys[0] = new GradientColorKey(currentColor, 0);
        tempColorKeys[1] = new GradientColorKey(currentColor, 1);

        tempGradient.colorKeys = tempColorKeys;

        currentLineRenderer.colorGradient = tempGradient;

        currentLineRenderer.startWidth = _brushSize;
        currentLineRenderer.endWidth = _brushSize;
    }

    public void CreateBrushOnHit(Color color, Vector3 mousePos)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject brushInstance = Instantiate(brush, BrushContainer.transform);
            currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

            mousePos += new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0);
            mousePos.z = 0;
            currentLineRenderer.SetPosition(0, mousePos);
            currentLineRenderer.SetPosition(1, mousePos);

            Gradient tempGradient = new Gradient();

            GradientColorKey[] tempColorKeys = new GradientColorKey[2];
            tempColorKeys[0] = new GradientColorKey(color, 0);
            tempColorKeys[1] = new GradientColorKey(color, 1);

            tempGradient.colorKeys = tempColorKeys;

            currentLineRenderer.colorGradient = tempGradient;

            float brushSize = UnityEngine.Random.Range(0.5f, 1f);
            currentLineRenderer.startWidth = brushSize;
            currentLineRenderer.endWidth = brushSize;

            AddPoint(mousePos);
        }
    }

    void AddPoint(Vector2 pointPos)
    {

        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    public void ChangeColor(Color color)
    {
        currentColor = color;
    }

    public void Bucket()
    {
        bucketColor = currentColor;
        canvasSprite.sprite = blankCanvas;
        canvasSprite.color = currentColor;

        BrushContainer.transform.Cast<Transform>().ToList().ForEach(c => UnityEngine.Object.DestroyImmediate(c.gameObject));
    }

    public void ActivatePen()
    {
        penActivation = true;
        stampActivation = false;
    }

    public void DeactivatePen()
    {
        penActivation = false;
    }

    

    private IEnumerator CoroutineScreenshot()
    {
        UIManager.Instance.DeactivateExitPanel();
        yield return new WaitForEndOfFrame();

        int width = Mathf.CeilToInt(screenLocationLocalTopRight.x - screenLocationLocalTopLeft.x);
        int height = Mathf.CeilToInt(screenLocationLocalTopRight.y - screenLocationLocalBottomRight.y);

        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(Mathf.Round(screenLocationLocalBottomLeft.x), Mathf.Round(screenLocationLocalBottomLeft.y), width, height);

        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();

        //PC
        //string screenshotName = "/Painting" + System.DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".png";
        //byte[] byteArray = screenshotTexture.EncodeToPNG();
        //System.IO.File.WriteAllBytes(Application.dataPath + screenshotName, byteArray);

        string screenshotName = "Painting" + System.DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".png";

        NativeGallery.SaveImageToGallery(screenshotTexture, "Myapp Pictures", screenshotName);

        StartCoroutine(AppQuitCoroutine());

    } 

    public void SaveScreenshot()
    {
        StartCoroutine(CoroutineScreenshot());
    }

    private IEnumerator CoroutineLoadScreenshot()
    {
        yield return new WaitForSeconds(1.0f);
        LoadScreenshot();
    }

    public void LoadScreenshot()
    {
        string[] files = Directory.GetFiles(galleryPath, "*.png"); // .jpg uzantýlý dosyalarý al
        if (files.Length > 0)
        {
            Texture2D photoTexture = GetImageFromGallery(files[files.Length - 1]);

            Sprite sprite = Sprite.Create(photoTexture, new Rect(0, 0, photoTexture.width, photoTexture.height), new Vector2(0.5f, 0.5f));

            canvasSprite.sprite = sprite;
            BrushContainer.transform.Cast<Transform>().ToList().ForEach(c => UnityEngine.Object.DestroyImmediate(c.gameObject));

            float[] temp = CalculateCanvasSize();
            CanvasResizer();
        }
        else
        {
            Debug.LogError("Galeride fotoðraf bulunamadý.");
        }
    }


    string GetGalleryPath()
    {
        string path = "";

#if UNITY_ANDROID && !UNITY_EDITOR
            // Android'de galeri yolu
            path = "/storage/emulated/0/DCIM/Myapp Pictures"; // Örnek bir yol, gerçek yolu cihazda bulmanýz gerekecek
#elif UNITY_IOS && !UNITY_EDITOR
            // iOS'ta galeri yolu
            path = Application.persistentDataPath + "/../../Gallery";
#else
        path = Application.persistentDataPath;
#endif

        return path;
    }

    Texture2D GetImageFromGallery(string imagePath)
    {
        Texture2D tex = null;

        if (File.Exists(imagePath))
        {
            byte[] fileData = File.ReadAllBytes(imagePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }
        else
        {
            Debug.LogError("Dosya bulunamadý: " + imagePath);
        }

        return tex;
    }


    void StampCreator()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Vector3 mousePos = Input.mousePosition;
                    Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
                    GameObject stampInstance = Instantiate(stamp, new Vector3(objectPos.x, objectPos.y, -1f) , Quaternion.identity, BrushContainer.transform);

                    AudioManager.Instance.SelectStamp();

                    stampInstance.transform.localScale *= _brushSize * 2;
                }
            }
        }
    }

    void CannonController()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Vector3 mousePos = Input.mousePosition;

                    cannon.GetComponent<Canon>().RotateCanon(mousePos);

                    GameObject paintInstance = Instantiate(paintBall, new Vector3(7,4,0), Quaternion.identity, BrushContainer.transform);

                    AudioManager.Instance.FireCannon();
                }
            }
        }
    }
    public void ActivateStamp()
    {
        stampActivation = true;
    }

    public void DeactivateStamp()
    {
        stampActivation = false;
    }

    public void ActivateCannon()
    {
        cannonActivation = true;
    }

    public void DeactivateCannon()
    {
        cannonActivation = false;
    }

    private IEnumerator AppQuitCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        Application.Quit();
    }

}
