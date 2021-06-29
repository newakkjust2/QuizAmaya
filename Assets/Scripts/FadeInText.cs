using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInText : MonoBehaviour
{
    [SerializeField] private float delay = 1f;
    private Text _text;
    private Color32  _endColor;
    private Coroutine _handler;
    
    void Start()
    {
        _text = GetComponent<Text>();
        
        _endColor = _text.color;
        
        delay /= 255;
        if (_handler == null)
        {
            _handler = StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        for (byte i = 0; i < 255; i++)
        {
            _text.color = new Color32(_endColor.r, _endColor.g, _endColor.g, i);
            yield return new WaitForSeconds(delay);
        }
    }
}
