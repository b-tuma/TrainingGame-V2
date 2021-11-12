using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MainMenuScreen : MonoBehaviour
{
    
    public CanvasGroup mainGroup;
    public CanvasGroup aboutGroup;
    public CanvasGroup loginGroup;
    public CanvasGroup registerGroup;

    public CanvasGroup currentGroup;

    public float transitionSpeed = 0.5f;
    public Ease transitionEnterEase = Ease.InCubic;
    public Ease transitionExitEase = Ease.OutCubic;

    //Tips
    public float tipDelay = 4f;
    private static bool _cadastreTip;
    public CanvasGroup cadastreTip;
    private static bool _enterTip;
    public CanvasGroup enterTip;

    void Awake()
    {
        currentGroup = mainGroup;
        if (!_cadastreTip)
        {
            _cadastreTip = true;
            cadastreTip.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = true;
            cadastreTip.blocksRaycasts = true;
            cadastreTip.DOFade(1f, 0.5f).OnComplete(() =>
            {
                cadastreTip.DOFade(0f, 0.5f).SetDelay(tipDelay).OnComplete(() =>
                {
                    cadastreTip.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    cadastreTip.blocksRaycasts = false;

                });
            });
        }
    }

    void DoTransition(CanvasGroup newGroup)
    {
        currentGroup.blocksRaycasts = false;
        currentGroup.DOFade(0f, transitionSpeed / 2f).SetEase(transitionEnterEase).OnComplete(() =>
        {
            currentGroup.gameObject.SetActive(false);
            newGroup.gameObject.SetActive(true);
            
            newGroup.DOFade(1f, transitionSpeed / 2f).SetEase(transitionExitEase).OnComplete(() =>
            {
                newGroup.blocksRaycasts = true;
                currentGroup = newGroup;
            });
        });
    }

    public void AboutButton()
    {
        DoTransition(aboutGroup);
    }

    public void BeginButton()
    {
        GameManager.gm.LoadLevel("GameScene");
    }

    public void LoginButton()
    {
        DoTransition(loginGroup);
    }

    public void RegisterButton()
    {
        if (currentGroup == registerGroup)
        {
            if (!_enterTip)
            {
                _enterTip = true;
                enterTip.blocksRaycasts = true;
                enterTip.DOFade(1f, 0.5f).OnComplete(() =>
                {
                    enterTip.DOFade(0f, 0.5f).SetDelay(tipDelay).OnComplete(() =>
                    {
                        enterTip.blocksRaycasts = false;

                    });
                });
            }
            return;
        }
        DoTransition(registerGroup);
    }

    public void AllButton()
    {
        Application.OpenURL("http://www.allcom.com.br/");
    }

    public void KirinButton()
    {
        
    }

    public void ReturnButton()
    {
        DoTransition(mainGroup);
    }
}
