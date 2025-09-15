using TMPro;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField]
    private string catName;
    [SerializeField]
    private int catLevel;
    [SerializeField]
    private float catAttackDamage;
    [SerializeField]
    private float catmoveSpeed;
    [SerializeField]
    private int catSp;
    [SerializeField]
    private int catHp;
    [SerializeField]
    private int catExp;
    [SerializeField]
    private TMP_Text catNameTag;
    [SerializeField]
    private TMP_Text catStateTag;
    private GameObject catInfoListUI;

    private string[] catNames = {
        "나비", "애옹", "코코",
        "보리", "미미", "치즈", "다람", "냥이", "모카", "두부", "토리", "하루", "밀키" };

    public string CatName
    {
        get { return catName; }
        set 
        { 
            catName = value;
            catNameTag.text = value;
            UpdateCatInfoListUI();
            UIManager.Instance.SelectedCatInfoUpdate(this);
        }
    }

    public int CatLevel
    {
        get { return catLevel; }
        set 
        { 
            catLevel = value; 
            UIManager.Instance.SelectedCatInfoUpdate(this); 
        }
    }

    public float CatAttackDamage
    {
        get { return catAttackDamage; }
        set { catAttackDamage = value; }
    }

    public float CatMoveSpeed
    {
        get { return catmoveSpeed; }
        set { catmoveSpeed = value; }
    }

    public int CatSp
    {
        get { return catSp; }
        set
        {
            catSp = value;
            UIManager.Instance.SelectedCatInfoUpdate(this);
        }
    }

    public int CatHp
    {
        get { return catHp; }
        set { catHp = value; }
    }

    public int CatExp
    {
        get { return catExp; }
        set
        {
            catExp = value;
            if (catExp >= 100 && catLevel < 10)
            {
                CatLevelUp();
                catExp = 0;
            }
            UIManager.Instance.SelectedCatInfoUpdate(this);
        }
    }

    public TMP_Text CatNameTag
    {
        get { return catNameTag; }
        set { catNameTag = value; }
    }

    public TMP_Text CatStateTag
    {
        get { return catStateTag; }
        set { catStateTag = value; }
    }

    public GameObject CatInfoListUI
    {
        get { return catInfoListUI; }
        set { catInfoListUI = value; }
    }

    private void UpdateCatInfoListUI()
    {
        if (GameManager.Instance.GameLoad) return;

        TMP_Text catNameT = catInfoListUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TMP_Text catLevelT = catInfoListUI.transform.GetChild(3).GetComponent<TextMeshProUGUI>();

        catNameT.text = catName;
        catLevelT.text = catLevel.ToString();
    }

    private void OnEnable()
    {
        if (GameManager.Instance.GameLoad)
        {
            LinkCatInfoListUI();
            return;
        }

        StateSetting();
        AssignRandomName();
        LinkCatInfoListUI();
    }

    private void AssignRandomName()
    {
        int randomIndex = Random.Range(0, catNames.Length);
        catName = catNames[randomIndex];
        catNameTag.text = catName;
    }

    private void StateSetting()
    {
        catLevel = 1;
        CatAttackDamage = 3;
        CatMoveSpeed = 1;
        CatSp = 100;
        CatHp = 100;
        CatExp = 0;
    }

    private void LinkCatInfoListUI()
    {
        UIManager.Instance.CreateCat(this);
    }

    private void CatLevelUp()
    {
        SoundManager.Instance.PlaySFX("up1");
        catLevel++;
        CatAttackDamage += 0.5f;
        CatMoveSpeed += 0.1f;

        if(catLevel == 10) CatManager.Instance.MaxLevelCat = true;
        UpdateCatInfoListUI();
    }
}
