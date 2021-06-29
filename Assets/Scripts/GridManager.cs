using System;
using System.Collections; 
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using static UnityEngine.Screen;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent gamePassed;

    [SerializeField] private GameObject blockScreen;
    [SerializeField] private ParticleSystem particles;

    [SerializeField] private Text questionText;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Cell[] letters, numbers;

    private List<ListedCell> _cells;
    private int[] cellsShuffledArray;
 
    private RectTransform _frame;
    private Vector2 _center;

    private Coroutine delayNextLevelHandler;
    private int _lvl = 1;
    private float _minScreenSize, _spacing, _cellSize;
    private string _answer;
    
    
    void Start()
    {
        _minScreenSize = height > width ?  width : height;
        _minScreenSize *= 0.85f;
        _spacing = _minScreenSize * 0.02f;
        _cellSize = (_minScreenSize - 5 * _spacing) / 3;
        _frame = GetComponent<RectTransform>();
        _frame.sizeDelta = Vector2.one * _minScreenSize;
        _center = _frame.anchoredPosition;
        cellPrefab.GetComponent<RectTransform>().sizeDelta = Vector2.one * _cellSize;

        _cells = new List<ListedCell>();
        for (int i = 0; i < 9; i++)
        {
            RectTransform temp = Instantiate(cellPrefab, transform).GetComponent<RectTransform>();
            _cells.Add(new ListedCell(temp));
        }
        cellPrefab.SetActive(false);

        cellsShuffledArray = new int[letters.Length > numbers.Length ? letters.Length : numbers.Length];
        
        NextLvl();
    }

    public void _CompareAnswer(Button b)
    {
        if (string.Equals(b.name, _answer) ) 
        {
            Win(b.transform.GetChild(0));
        }
        else
        {
            if (true)
            {
                b.transform.GetChild(0).DOShakePosition(0.5f, 6f, 20,0f, true).SetEase(Ease.InBounce);
            }
            
        }
    }
    private void Win(Transform winner)
    {
         blockScreen.SetActive(true);
        if (delayNextLevelHandler == null)
        {
            winner.DOShakePosition(1.5f,20).SetEase(Ease.InBounce);
            delayNextLevelHandler = StartCoroutine(DelayNextLevel());
            particles.Play(); 
            _lvl++;
        }
        if (_lvl > 3)
        {
            _lvl = 0;
            gamePassed.Invoke();
        }
    }
    
    
    private int[] Shuffle(int max)
    {
        int[] shuffledArray = new int[max];
        for (int i = 0; i < max; i++)
        {
            shuffledArray[i] = i;
        }
        for (int i = max; i > 0; i--)
        {
            int k = Random.Range(0, i-1);
            int temp = shuffledArray[k];
            shuffledArray[k] = shuffledArray[i-1];
            shuffledArray[i-1] = temp;
        }

        return shuffledArray;
    }
    
    private void NextLvl()
    {
        if (Random.value > 0.5f)
        {
           cellsShuffledArray = Shuffle(letters.Length);
            for (var i = 0; i < _lvl * 3; i++)
            {
                int r = cellsShuffledArray[i];
                _cells[i].face.sprite = letters[r].sprite;
                _cells[i].rttFace.sizeDelta = new Vector2(
                    _cells[i].rttBack.sizeDelta.x * 0.9f * letters[r].xToYRatio, 
                    _cells[i].rttBack.sizeDelta.y * 0.9f);
                _cells[i].back.color = letters[r].backColor; 
                _cells[i].rttBack.name = _cells[i].name = letters[r].name;
            }
        }
        else
        {
            cellsShuffledArray = Shuffle(numbers.Length);
            for (var i = 0; i < _lvl * 3; i++)
            {
                int r = cellsShuffledArray[i];
                _cells[i].face.sprite = numbers[r].sprite;
                _cells[i].rttFace.sizeDelta = new Vector2(
                    _cells[i].rttBack.sizeDelta.x * 0.9f * numbers[r].xToYRatio, 
                    _cells[i].rttBack.sizeDelta.y * 0.9f);
                _cells[i].back.color = numbers[r].backColor; 
                _cells[i].rttBack.name = _cells[i].name = numbers[r].name; 
            }
        } 
        int rq = Random.Range(0, 3 * _lvl); 
        _answer = _cells[rq].name;
        questionText.text = "Find " + _cells[rq].name;
        
        for (int i = 0; i < _lvl * 3; i++)
        {
            _cells[i].rttBack.gameObject.SetActive(true);
            _cells[i].rttFace.localPosition = Vector3.zero;
        }
        
        switch (_lvl)
        {
           case 1:
               _frame.sizeDelta = new Vector2(_minScreenSize, _minScreenSize / 3);
               
               for (int i = 0; i < 3; i++)
               {
                   _cells[i].rttBack.anchoredPosition =
                       new Vector2(_center.x + ((_cellSize + _spacing) * (i-1))
                           , _center.y);  
               } 
               for (int i = 3; i < 9; i++)           //Inactivate
               {
                   _cells[i].rttBack.gameObject.SetActive(false);
               }
                                                     //BounceIn
               Vector3[] pos = new Vector3[3];
               for (int i = 0; i < 3; i++)
               {
                   pos[i] = _cells[i].rttBack.position;
                  // _cells[i].rttBack.position = new Vector2(-1, pos[i].y);
                  _cells[i].rttBack.anchoredPosition = new Vector2(width * -0.6f, 0);//DOMoveX(-30, 0.1f);
                   _cells[i].rttBack.DOJump(pos[i], 1,  1, 2).SetEase(Ease.InBounce);
                   _cells[i].rttBack.DOMoveX(pos[i].x, 2);
               } 
               
               
               break;
           case 2:
               _frame.sizeDelta = new Vector2(_minScreenSize, (_minScreenSize / 3) * 2);
               for (int i = 0; i < 3; i++)
               {
                   _cells[i].rttBack.anchoredPosition =
                       new Vector2(_center.x + ((_cellSize + _spacing) * (i-1))
                           , _center.y + ((_cellSize + _spacing) / 2));  
               }
               for (int i = 3; i < 6; i++)
               {
                   _cells[i].rttBack.anchoredPosition =
                       new Vector2(_center.x + ((_cellSize + _spacing) * (i-1 -3))
                           , _center.y - ((_cellSize + _spacing) / 2));  
               }
               for (int i = 6; i < 9; i++)           //Inactivate
               {
                   _cells[i].rttBack.gameObject.SetActive(false);
               }
               break;
           case 3:
               _frame.sizeDelta = new Vector2(_minScreenSize, _minScreenSize);
               for (int i = 0; i < 3; i++)
               {
                   _cells[i].rttBack.anchoredPosition =
                       new Vector2(_center.x + ((_cellSize + _spacing) * (i-1))
                           , _center.y + (_cellSize + _spacing));  
               }
               for (int i = 3; i < 6; i++)
               {
                   _cells[i].rttBack.anchoredPosition =
                       new Vector2(_center.x + ((_cellSize + _spacing) * (i-1 -3))
                           , _center.y);  
               }
               for (int i = 6; i < 9; i++)
               {
                   _cells[i].rttBack.anchoredPosition =
                       new Vector2(_center.x + ((_cellSize + _spacing) * (i-1 -6))
                           , _center.y - (_cellSize + _spacing));  
               }
               break;
        }
        blockScreen.SetActive(false);
    }


    IEnumerator DelayNextLevel()
    {
        yield return new WaitForSeconds(3);
        NextLvl();
        delayNextLevelHandler = null;
    }
}
