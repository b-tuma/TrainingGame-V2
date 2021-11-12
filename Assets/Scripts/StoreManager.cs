using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour, IScene
{
    public static StoreManager sm;
    private GameManager mGameManager;
    private StoreDatabase mStoreDatabase;
    public Button storePrefab;
    public RectTransform storeHolder;
    public Button buyButton;
    public Button returnButton;
    private Button[] mItens;
    private BaseProduct itemSelected;
    public Text buyText;
    public CanvasGroup confirmBox;

    void Awake()
    {
        if (sm != null)
        {
            Destroy(sm);
        }
        else
        {
            sm = this;
        }
        mGameManager = GameManager.gm;
        if (mGameManager == null) return;
        mStoreDatabase = mGameManager.storeDatabase;
        buyButton.onClick.AddListener(BuyItem);
        mItens = new Button[mStoreDatabase.products.Count];
        GridLayoutGroup grid = storeHolder.GetComponent<GridLayoutGroup>();
        RectTransform rect = storeHolder.GetComponent<RectTransform>();
        for (int i = mStoreDatabase.products.Count - 1; i >= 0; i--)
        {
            mItens[i] = Instantiate(storePrefab);
            var i1 = i;
            mItens[i].onClick.AddListener(() => {ItemSelected(mStoreDatabase.products[i1]);});
            mItens[i].transform.Find("Price").GetComponent<Text>().text = "$ " + mStoreDatabase.products[i].price;
            mItens[i].transform.Find("Item").GetComponent<Image>().sprite = mStoreDatabase.products[i].image;
            mItens[i].transform.Find("Name").GetComponent<Text>().text = mStoreDatabase.products[i].name;
            RectTransform itemRect = mItens[i].GetComponent<RectTransform>();
            itemRect.SetParent(storeHolder);
            itemRect.localScale = Vector3.one;
            itemRect.SetAsFirstSibling();
        }
        UpdateCatalog();
        storeHolder.Find("Store").SetAsFirstSibling();
    }

    public void UpdateCatalog()
    {
        for (int i = mStoreDatabase.products.Count - 1; i >= 0; i--)
        {
            mItens[i].interactable = mStoreDatabase.products[i].price <= Account.ac.currentMoney;
        }
    }

    public void ItemSelected(BaseProduct product)
    {
        itemSelected = product;
        buyText.text = "Deseja mesmo comprar o item \"" + itemSelected.name + "\"? Saldo resultante: " +
                       (Account.ac.currentMoney - itemSelected.price);
        confirmBox.interactable = true;
        confirmBox.blocksRaycasts = true;
        confirmBox.DOFade(1f, 0.5f);
    }

    public void BuyItem()
    {
        buyText.text = "";
        if (itemSelected == null) return;
        GameScreen.gs.ChangeMoney(-itemSelected.price);
        itemSelected = null;
        confirmBox.interactable = false;
        confirmBox.blocksRaycasts = false;
        confirmBox.DOFade(0f, 0.5f);
        UpdateCatalog();
    }

    public void ReturnButton()
    {
        itemSelected = null;
        confirmBox.interactable = false;
        confirmBox.blocksRaycasts = false;
        confirmBox.DOFade(0f, 0.5f);
    }

    public void ExitScene()
    {
    }

    public void EnterScene()
    {
    }
}
