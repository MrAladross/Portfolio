using UnityEngine;
using UnityEngine.UI;
public class TextResizer : MonoBehaviour
{

    private Text t;
    Text b1Text;
    Text b2Text;
    public float physicalSizeInches;
    private RectTransform rt;
    public RectTransform b1;
    public RectTransform b2;
    RectTransform bpTransform;
    public Image backgroundPanel;
    public void ClearYDistance()
    {
        rt.localPosition = Vector3.zero;
    }
    private void Awake()
    {
        t = GetComponent<Text>();
        rt = GetComponent<RectTransform>();
        bpTransform = backgroundPanel.GetComponent<RectTransform>();
        t.fontSize = Mathf.RoundToInt( Screen.dpi *physicalSizeInches);
        b1.GetComponentInChildren<Text>().resizeTextMaxSize = Mathf.RoundToInt(Screen.dpi * physicalSizeInches);
        b2.GetComponentInChildren<Text>().resizeTextMaxSize = Mathf.RoundToInt(Screen.dpi * physicalSizeInches);
    }
    private void Start()
    {
        bpTransform.sizeDelta = new Vector2(Screen.width * 0.8f, Screen.height * 0.8f);
        Resize();
    }
    public void SetText(string s)
    {
        t.text = s;
        t.text += "\n";
        t.text += "\n";
        t.text += "\n";
        t.text += "\n";
        t.text += "\n";
        Resize();
    }

    private void Resize()
    {
        Canvas.ForceUpdateCanvases();
        rt.sizeDelta= new Vector2(bpTransform.sizeDelta.x*0.85f, t.cachedTextGenerator.lineCount * Screen.dpi* physicalSizeInches*1.12f);
        b1.sizeDelta = new Vector2(rt.sizeDelta.x / 2.2f, rt.sizeDelta.x / 2.5f);
        b2.sizeDelta = new Vector2(rt.sizeDelta.x / 2.2f, rt.sizeDelta.x / 2.5f);
    }

}
