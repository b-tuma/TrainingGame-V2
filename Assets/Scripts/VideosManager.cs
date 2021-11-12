using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VideosManager : MonoBehaviour, IScene
{
    public static VideosManager vm;
    private VideoDatabase mVideoDatabase;

    public RectTransform videosHolder;
    public Button videoPrefab;
    private Button[] mVideos;
    public Text titleShow;
    private BaseVideo mSelectedVideo;
    public Button viewButton;

    void Awake()
    {
        if (vm != null)
        {
            Destroy(vm);
        }
        else
        {
            vm = this;
        }
        if (GameManager.gm == null) return;
        mVideoDatabase = GameManager.gm.videoDatabase;

        mVideos = new Button[mVideoDatabase.videos.Count];
        GridLayoutGroup grid = videosHolder.GetComponent<GridLayoutGroup>();
        RectTransform rect = videosHolder.GetComponent<RectTransform>();
        float rows = Mathf.CeilToInt(mVideoDatabase.videos.Count / grid.constraintCount);
        float width = rect.sizeDelta.x;
        float height = ((rows + 1f) * videoPrefab.GetComponent<RectTransform>().sizeDelta.y) + (rows * grid.spacing.y);
        rect.sizeDelta = new Vector2(width, height);
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -5000f);
        for (int i = mVideoDatabase.videos.Count - 1; i >= 0; i--)
        {
            mVideos[i] = Instantiate(videoPrefab);
            var i1 = i;
            mVideos[i].onClick.AddListener(() => { VideoPressed(mVideoDatabase.videos[i1]); });
            mVideos[i].interactable = mVideoDatabase.videos[i].isUnlocked;
            RectTransform textRect = mVideos[i].GetComponent<RectTransform>();
            textRect.SetParent(videosHolder);
            textRect.localScale = Vector3.one;
            textRect.SetAsFirstSibling();
        }
    }

    public void ViewVideo()
    {
        if (mSelectedVideo != null)
        {
//#if UNITY_ANDROID
            //Handheld.PlayFullScreenMovie("Nova.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);

//#else
            Application.OpenURL(mSelectedVideo.url);
//#endif
            titleShow.text = "";
            viewButton.interactable = false;

        }
    }

    public void VideoPressed(BaseVideo baseVideo)
    {
        if (baseVideo != mSelectedVideo)
        {
            mSelectedVideo = baseVideo;
            titleShow.text = baseVideo.title;
            viewButton.interactable = true;
        }
        else
        {
//#if UNITY_ANDROID
            //Handheld.PlayFullScreenMovie("Nova.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);

//#else
          Application.OpenURL(mSelectedVideo.url);
//#endif
            titleShow.text = "";
            viewButton.interactable = false;
        }
    }

    public void ExitScene()
    {
        titleShow.text = "";
        viewButton.interactable = false;
        mSelectedVideo = null;
    }

    public void EnterScene()
    {

    }
}
