using UnityEngine.SceneManagement; 
using UnityEngine;
using UnityEngine.Events;

using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    public UnityEvent nextlvl;
     
    private bool pause;
    private Coroutine _handler;
        
    void Start()
    {
        pause = false;
        FindObjectOfType<GridManager>().gamePassed.AddListener((FadePanel));
    }

    public void Pause()
    {
        pause = !pause;
        pausePanel.SetActive(pause);
    }

    public void Home()
    {
        Pause(); 
        SceneManager.LoadScene(0);
    }

    public void NextLvl()
    {
        nextlvl.Invoke();
    }
    
    public void Restart()
    {
        Pause(); 
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.buildIndex);
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    private void FadePanel()
    {
        if (_handler == null)
        {
            _handler = StartCoroutine(Fade());
        }
    }
    IEnumerator Fade()
    {
        Image _image = pausePanel.GetComponent<Image>();
        Color32 _endColor = _image.color;
        _image.color = new Color32(_endColor.r, _endColor.g, _endColor.g, 0);
        float delay = 0.01f; 
        
        yield return new WaitForSeconds(0.35f);
        
        Pause();
        
        for (byte i = 0; i < 150; i++)
        {
            _image.color = new Color32(_endColor.r, _endColor.g, _endColor.g, i);
            yield return new WaitForSeconds(delay);
        }
    }
}
