using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    public static GameScreen gs;


    public CanvasGroup worldGroup;
    public CanvasGroup levelGroup;
    public CanvasGroup textsGroup;
    public CanvasGroup storeGroup;
    public CanvasGroup quizGroup;
    public CanvasGroup textDisplayGroup;
    public CanvasGroup videoGroup;
    public CanvasGroup statusGroup;

    public CanvasGroup currentGroup;

    public float transitionSpeed = 0.5f;
    public Ease transitionEnterEase = Ease.InCubic;
    public Ease transitionExitEase = Ease.OutCubic;

    private GameManager mGameManager;
    private WorldDatabase mWorldDatabase;
    public Button returnButton;
    public Button videosButton;
    public Button textsButton;
    public Button storeButton;
    public Button statusButton;
    public Text moneyText;

    private bool mLevelGroupOpen;

    private int mMoneyIndicator;

    //Tips
    public float tipDelay = 2f;
    private static bool _tips;
    public CanvasGroup tip1;
    public CanvasGroup tip2;
    public CanvasGroup tip3;
    public CanvasGroup tip4;
    public CanvasGroup tip5;

    public static bool _tips2;
    public CanvasGroup tip6;
    public CanvasGroup tip7;

    public static bool _tips3;
    public CanvasGroup tip8;
    public CanvasGroup tip9;

    void Awake()
    {
        if (gs != null)
        {
            Destroy(gs);
        }
        else
        {
            gs = this;
        }

        mGameManager = GameManager.gm;
        if (mGameManager == null) return;
    }

    void Start()
    {
        if (!_tips)
        {
            _tips = true;
            StartCoroutine(TutorialPart1());
        }
        mMoneyIndicator = Account.ac.currentMoney;
    }

    IEnumerator TutorialPart1()
    {
        tip1.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = true;
        tip1.blocksRaycasts = true;
        tip1.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f + tipDelay);
        tip1.DOFade(0f, 0.5f);
        tip2.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f + tipDelay);
        tip2.DOFade(0f, 0.5f);
        tip3.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f + tipDelay);
        tip3.DOFade(0f, 0.5f);
        tip4.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f + tipDelay);
        tip4.DOFade(0f, 0.5f);
        tip5.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f + tipDelay);
        tip5.DOFade(0f, 0.5f);
        yield return  new WaitForSeconds(0.5f);
        tip1.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
        tip1.blocksRaycasts = false;
        yield return null;
    }

    void Update()
    {
        moneyText.text = mMoneyIndicator.ToString();
    }

    public void ReturnButton()
    {
        if (mLevelGroupOpen && currentGroup != levelGroup)
        {
            DoTransition(levelGroup);
        }
        else if (currentGroup == levelGroup)
        {
            DoTransition(worldGroup);
            mLevelGroupOpen = false;
        }
        else if (!mLevelGroupOpen && currentGroup != worldGroup)
        {
            DoTransition(worldGroup);
        }
        else
        {
            mGameManager.LoadLevel("MainMenu");
        }
    }

    public void Tutorial2()
    {
        StartCoroutine(TutorialPart2());
    }

    IEnumerator TutorialPart2()
    {
        tip6.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = true;
        tip6.blocksRaycasts = true;
        tip6.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f + tipDelay);
        tip6.DOFade(0f, 0.5f);
        tip7.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f + tipDelay);
        tip7.DOFade(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        tip6.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
        tip6.blocksRaycasts = false;
        yield return null;
    }

    public void Tutorial3()
    {
        StartCoroutine(TutorialPart3());
    }

    IEnumerator TutorialPart3()
    {
        tip8.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = true;
        tip8.blocksRaycasts = true;
        tip8.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f + tipDelay);
        tip8.DOFade(0f, 0.5f);
        tip9.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f + tipDelay);
        tip9.DOFade(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        tip8.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = false;
        tip8.blocksRaycasts = false;
        QuizManager.qm.quizRunning = true;
        yield return null;
    }

    public void VideosButton()
    {
        DoTransition(videoGroup);
    }

    public void TextsButton()
    {
        DoTransition(textsGroup);
    }

    public void StoreButton()
    {
        DoTransition(storeGroup);
    }

    public void StatusButton()
    {
        DoTransition(statusGroup);
    }

    public void ChangeMoney(int amount)
    {
        Account.ac.currentMoney += amount;
        DOTween.To(() => mMoneyIndicator, x => mMoneyIndicator = x, Account.ac.currentMoney, 1f).SetEase(Ease.OutSine);
    }

    public void DoTransition(CanvasGroup newGroup)
    {
        currentGroup.blocksRaycasts = false;
        currentGroup.interactable = false;
        currentGroup.DOFade(0f, transitionSpeed / 2f).SetEase(transitionEnterEase).OnComplete(() =>
        {
            currentGroup.GetComponent<IScene>().ExitScene();
            //currentGroup.gameObject.SetActive(false);
            //newGroup.gameObject.SetActive(true);
            newGroup.GetComponent<IScene>().EnterScene();

            newGroup.DOFade(1f, transitionSpeed / 2f).SetEase(transitionExitEase).OnComplete(() =>
            {
                newGroup.blocksRaycasts = true;
                newGroup.interactable = true;
                currentGroup = newGroup;
                if (newGroup == levelGroup) mLevelGroupOpen = true;
            });
        });
    }
}
