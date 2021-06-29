using UnityEngine.UI;
using UnityEngine;

public class ListedCell
{


    public RectTransform rttBack, rttFace; 
    public Image face, back;
    public string name;
 

    public ListedCell(RectTransform r) 
    {
        rttBack = r;
        back = rttBack.GetComponent<Image>();
        rttFace = rttBack.transform.GetChild(0).GetComponent<RectTransform>();
        face = rttBack.transform.GetChild(0).GetComponent<Image>();
    }
}
