using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour, IScene
{
    private GameManager mGameManager;
    public RectTransform statusPrefab;
    public RectTransform statusHolder;
    private RectTransform[] mStatus;
    public Text geralText;
    public Image geralImage;


    private void Awake()
    {
        mGameManager = GameManager.gm;
        if (mGameManager == null) return;
        mStatus = new RectTransform[Account.ac.contentName.Length];
        float percentSum = 0f;
        foreach (int percent in Account.ac.contentPercent)
        {
            percentSum += percent;
        }
        int totalPercent = Mathf.CeilToInt(percentSum/Account.ac.contentPercent.Length);
        geralText.text = totalPercent + "%";
        geralImage.fillAmount = (1f/100f)*totalPercent;
        for (int i = Account.ac.contentName.Length - 1; i >= 0; i--)
        {
            mStatus[i] = Instantiate(statusPrefab);
            mStatus[i].Find("Name").GetComponent<Text>().text = Account.ac.contentName[i];
            mStatus[i].Find("Clock").GetComponent<Image>().fillAmount = 0.01f*Account.ac.contentPercent[i];
            mStatus[i].Find("Percent").GetComponent<Text>().text = Account.ac.contentPercent[i] + "%";
            mStatus[i].SetParent(statusHolder);
            mStatus[i].localScale = Vector3.one;
            mStatus[i].SetAsFirstSibling();
        }
    }

    public void ExitScene()
    {
        
    }

    public void EnterScene()
    {

    }
}
