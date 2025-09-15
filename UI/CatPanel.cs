using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CatPanel : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> catInfoPanelList;

    [SerializeField]
    private GameObject catCreateButton;

    [SerializeField]
    private TMP_Text catName;
    [SerializeField]
    private TMP_Text catLevel;
    [SerializeField]
    private TMP_Text catState;
    [SerializeField]
    private TMP_Text catHP;
    [SerializeField]
    private TMP_Text catSP;
    [SerializeField]
    private TMP_Text catEXP;
    [SerializeField]
    private TMP_Text catMoveSpeed; 
    [SerializeField]
    private TMP_Text catAttackDamage;
    [SerializeField]
    private Slider HpSlider;
    [SerializeField]
    private Slider SpSlider;
    [SerializeField]
    private Slider ExpSlider;

    private Cat renameSelectedCat;

    private void Awake()
    {
        Button button = catCreateButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => CatManager.Instance.ActivateCat());
        }
    }

    public void CatListUpdate(Cat cat)
    {
        foreach (GameObject panel in catInfoPanelList)
        {
            if (!panel.activeSelf)
            {
                panel.SetActive(true);

                Button goToInfoButton = panel.GetComponent<Button>();
                TMP_Text catNameT = panel.transform.GetChild(1).GetComponent<TMP_Text>();
                TMP_Text catLevelT = panel.transform.GetChild(3).GetComponent<TMP_Text>();

                goToInfoButton.onClick.AddListener(() => UIManager.Instance.SelectCat(cat));

                cat.CatInfoListUI = panel;

                catNameT.text = cat.CatName;
                catLevelT.text = cat.CatLevel.ToString();

                break;
            }
        }
    }

    public void CatCreatButton(bool see)
    {
        catCreateButton.SetActive(see);
    }

    public void CatInfoUpdate(Cat cat)
    {
        CatInfoTUpdate(cat);
        CatInfoBarUpdate(cat);
    }

    private void CatInfoTUpdate(Cat cat)
    {
        catName.text = cat.CatName;
        catLevel.text = cat.CatLevel.ToString();
        catState.text = cat.CatStateTag.text;
        catHP.text = $"HP {cat.CatHp}/100";
        catSP.text = $"SP {cat.CatSp}/100";
        catEXP.text = $"EXP {cat.CatExp}/100";
        catMoveSpeed.text = $"이동속도 : {cat.CatMoveSpeed}";
        catAttackDamage.text = $"공격력 : {cat.CatAttackDamage}";
    }

    private void CatInfoBarUpdate(Cat cat)
    {
        HpSlider.value = cat.CatHp;
        SpSlider.value = cat.CatSp;
        ExpSlider.value = cat.CatExp;
    }

    public void CatNameChange(TMP_InputField inputField)
    {
        renameSelectedCat.CatName = inputField.text;
    }

    public void RenameCatSelect()
    {
        renameSelectedCat = UIManager.Instance.selectedCat;
    }

    public void InitializeCatPanel()
    {
        foreach (GameObject panel in catInfoPanelList)
        {
            panel.SetActive(false);
        }
    }
}
