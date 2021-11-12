using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour, IScene
{
    public static QuizManager qm;
    private GameManager mGameManager;
    private QuestionDatabase mQuestionDatabase;

    public RectTransform answerHolder;
    public Button answerPrefab;
    private Button[] mAnswers;

    public Image questionLogo;
    public Image questionTimer;
    public Image questionPreTimer;
    public Image questionTimerBack;
    public Text questionTimerNumber;
    public Text questionText;
    public Image questionResult;
    public Button questionButton;
    public Color rightColor = Color.green;
    public Color wrongColor = Color.red;
    public Sprite questionResultWrong;
    public Sprite questionResultRight;
    public Sprite questionButtonWrong;
    public Sprite questionButtonRight;

    private int currentPoints;
    private BaseQuestion mCurrentQuestion;
    private BaseWorld mCurrentWorld;
    public bool quizRunning;

    private void Awake()
    {
        if (qm != null)
        {
            Destroy(qm);
        }
        else
        {
            qm = this;
        }
        mGameManager = GameManager.gm;
        if (mGameManager == null) return;
        mQuestionDatabase = mGameManager.questionDatabase;
        questionButton.onClick.AddListener(ResultButton);
    }

    private float mPreCountdown = 0f;
    private float mCountDown;
    private int mDiffScore;
    private float mPreCurrent;
    void FixedUpdate()
    {
        if (quizRunning)
        {
            if (mPreCountdown < mQuestionDatabase.timeBeforeCountdown)
            {
                mPreCountdown += Time.fixedDeltaTime;
                questionPreTimer.fillAmount = 1f -(1f/mQuestionDatabase.timeBeforeCountdown)*mPreCountdown;
            }
            else if(mCountDown > 0f)
            {
                mCountDown -= Time.fixedDeltaTime;
                questionTimer.fillAmount = (1f/mCurrentQuestion.time)*mCountDown;
                if (!mCurrentQuestion.failed && !mCurrentQuestion.completed)
                {
                    currentPoints = Mathf.CeilToInt(mCurrentQuestion.minScore + ((mDiffScore/mCurrentQuestion.time)*mCountDown));
                    questionTimerNumber.text = currentPoints.ToString();
                }
            }
        }
    }

    public void InitializeQuestion(BaseQuestion baseQuestion, BaseWorld baseWorld)
    {
        mPreCountdown = 0f;
        mCurrentQuestion = baseQuestion;
        mPreCurrent = (1f / baseQuestion.time) * mQuestionDatabase.timeBeforeCountdown;
        questionPreTimer.fillAmount = mPreCurrent;
        mCountDown = baseQuestion.time;
        mDiffScore = baseQuestion.maxScore - baseQuestion.minScore;
        mCurrentWorld = baseWorld;
        questionLogo.sprite = baseWorld.worldSprite;
        questionTimerBack.color = baseWorld.worldColor;
        questionTimer.fillAmount = 1f;
        questionResult.gameObject.SetActive(false);
        questionButton.gameObject.SetActive(false);
        if (baseQuestion.failed && !baseQuestion.completed)
        {
            currentPoints = baseQuestion.minScore;
        }
        else if (!baseQuestion.failed && !baseQuestion.completed)
        {
            currentPoints = baseQuestion.maxScore;
        }
        else
        {
            currentPoints = 0;
        }

        questionTimerNumber.text = currentPoints.ToString();
        questionText.color = baseWorld.worldColor;
        questionText.text = baseQuestion.question;

        if (mAnswers != null)
        {
            if (mAnswers.Length != 0)
            {
                foreach (Button answer in mAnswers)
                {
                    Destroy(answer.gameObject);
                }
                mAnswers = new Button[0];
            }
        }

        mAnswers = new Button[baseQuestion.alternative.Length];
        for (int i = baseQuestion.alternative.Length - 1; i >= 0; i--)
        {
            mAnswers[i] = Instantiate(answerPrefab);
            mAnswers[i].GetComponentInChildren<Text>().text = baseQuestion.alternative[i];
            if (baseQuestion.correctAlternative == i)
            {
                var i1 = i;
                mAnswers[i].onClick.AddListener(() => {CorrectAnswer(mAnswers[i1]);});
            }
            else
            {
                var i2 = i;
                mAnswers[i].onClick.AddListener(() => {WrongAnswer(mAnswers[i2]);});
            }
            RectTransform answerRect = mAnswers[i].GetComponent<RectTransform>();
            answerRect.SetParent(answerHolder);
            answerRect.localScale = Vector3.one;
            answerRect.SetAsFirstSibling();
        }
        if (!GameScreen._tips3)
        {
            GameScreen._tips3 = true;
            GameScreen.gs.Tutorial3();
        }
        else
        {
            quizRunning = true;
        }
        
    }

    public void CorrectAnswer(Button button)
    {
        quizRunning = false;
        GameScreen.gs.ChangeMoney(currentPoints);
        mCurrentQuestion.completed = true;
        foreach (Button answer in mAnswers)
        {
            answer.interactable = false;
        }
        button.image.color = rightColor;
        questionButton.image.sprite = questionButtonRight;
        questionButton.gameObject.SetActive(true);
        questionResult.sprite = questionResultRight;
        questionResult.gameObject.SetActive(true);
        LevelsManager.lm.UpdateLevels();
    }

    public void WrongAnswer(Button button)
    {
        quizRunning = false;
        mCurrentQuestion.failed = true;
        foreach (Button answer in mAnswers)
        {
            answer.interactable = false;
        }
        button.image.color = wrongColor;
        questionButton.image.sprite = questionButtonWrong;
        questionButton.gameObject.SetActive(true);
        questionResult.sprite = questionResultWrong;
        questionResult.gameObject.SetActive(true);
    }

    public void ResultButton()
    {
        if (questionButton.image.sprite == questionButtonWrong) // failed
        {
            InitializeQuestion(mCurrentQuestion, mCurrentWorld);
        }
        else
        {
            GameScreen.gs.DoTransition(GameScreen.gs.levelGroup);
        }
    }

    public void ExitScene()
    {
        if (quizRunning && !mCurrentQuestion.completed) mCurrentQuestion.failed = true;
        quizRunning = false;
        GameScreen.gs.videosButton.interactable = true;
        GameScreen.gs.textsButton.interactable = true;
        GameScreen.gs.storeButton.interactable = true;
        GameScreen.gs.statusButton.interactable = true;
    }

    public void EnterScene()
    {
        GameScreen.gs.currentGroup = GameScreen.gs.quizGroup;
        GameScreen.gs.videosButton.interactable = false;
        GameScreen.gs.textsButton.interactable = false;
        GameScreen.gs.storeButton.interactable = false;
        GameScreen.gs.statusButton.interactable = false;
    }
}
