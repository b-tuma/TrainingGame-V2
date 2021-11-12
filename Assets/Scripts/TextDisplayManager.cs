using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextDisplayManager : MonoBehaviour, IScene
{
    public static TextDisplayManager tdm;
    public Button leftArrow;
    public Button rightArrow;
    public RectTransform contentHolder;
    public Text title;
    private BaseText mCurrentText;
    public RectTransform content;
    private BaseText mPreviousText;
    private BaseText mNextText;

    void Awake()
    {
        if (tdm != null)
        {
            Destroy(tdm);
        }
        else
        {
            tdm = this;
        }
        rightArrow.onClick.AddListener(PressRight);
        leftArrow.onClick.AddListener(PressLeft);
    }

    public void InitializeText(BaseText myText)
    {
        contentHolder.GetComponent<ScrollRect>().content = null;
        if (content != null)
        {
            Destroy(content.gameObject);
        }
        rightArrow.interactable = leftArrow.interactable = false;
        mNextText = mPreviousText = null;
        mCurrentText = myText;
        title.text = myText.title;
        content = Instantiate(myText.textContent);
        content.SetParent(contentHolder);
        content.localScale = Vector3.one;
        content.anchoredPosition = new Vector2(0f, -500f);
        content.sizeDelta = new Vector2(0f, content.sizeDelta.y);
        contentHolder.GetComponent<ScrollRect>().content = content;
        BaseText[] texts = GameManager.gm.textDatabase.texts.ToArray();
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i] == myText)
            {
                //found
                //check next
                if (i + 1 < texts.Length)
                {
                    if (texts[i + 1].isUnlocked)
                    {
                        //can go right
                        rightArrow.interactable = true;
                        mNextText = texts[i + 1];
                    }
                    
                }
                if (i - 1 >= 0)
                {
                    if (texts[i - 1].isUnlocked)
                    {
                        leftArrow.interactable = true;
                        mPreviousText = texts[i - 1];
                    }
                }
                break;
            }
        }
    }


    public void PressRight()
    {
        InitializeText(mNextText);
    }

    public void PressLeft()
    {
        InitializeText(mPreviousText);
    }

    public void ExitScene()
    {

    }

    public void EnterScene()
    {

    }
}
