using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RandomCtrl : MonoBehaviour
{
    [SerializeField] private Image[] menuButtons;
    [SerializeField] private GameObject[] menus;
    [SerializeField] private InputField single_input;
    [SerializeField] private Text single_resultText;
    [SerializeField] private Transform content_panel;
    private UIBox[] slots;

    private int menuIndex;

    // 임시 데이터
    Separator separator;

    // Start is called before the first frame update
    void Start()
    {
        slots = content_panel.GetComponentsInChildren<UIBox>();
        SetDefault();
        SetMenu();
    }

    private void SetDefault()
    {
        for (int i = 0; i < menuButtons.Length; i++)
            menuButtons[i].color = Color.white;
        for (int i = 0; i < menus.Length; i++)
            menus[i].SetActive(false);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].images[0].color = new Color(0.8f, 0.8f, 0.8f);
            slots[i].texts[0].text = "";
        }
        single_resultText.text = "";
    }

    private void SetMenu()
    {
        SetDefault();
        menuButtons[menuIndex].color = new Color(0.8f, 0.8f, 0.8f);
        menus[menuIndex].SetActive(true);
    }

    public void ClickMenuButton()
    {
        separator = EventSystem.current.currentSelectedGameObject.GetComponent<Separator>();
        if (separator == null)
            return;
        menuIndex = separator.data;

        SetMenu();
    }

    public void ClickRandButton()
    {
        // 데이터 설정
        float percent = 0f;
        int true_count = 0;

        try
        {
            percent = 1f / float.Parse(single_input.text);
        }
        catch (System.Exception)
        {
            single_resultText.text = "올바른 값을 입력해주세요.";
            throw;
        }

        // 결과 출력
        slots = content_panel.GetComponentsInChildren<UIBox>();
        for (int i = 0; i < slots.Length; i++)
        {
            bool flag = GetRandFlag(percent);
            if (flag)
            {
                // True
                true_count++;
                slots[i].images[0].color = new Color(0.6f, 1f, 0.6f);
            }
            else
            {
                // False
                slots[i].images[0].color = new Color(1f, 0.6f, 0.6f);
            }
        }

        single_resultText.text = "예상 결과 = ( " + (int)(slots.Length * percent) + " / " + slots.Length + " ) | 실제 결과 = ( " + true_count + " / " + slots.Length + " )";
    }

    /// <summary>
    /// 'percent' 확률에 따라 bool을 반환하는 함수
    /// </summary>
    /// <param name="percent">확률</param>
    /// <returns></returns>
    static public bool GetRandFlag(float percent)
    {
        if (Random.value < percent)
            return true;
        else
            return false;
    }
}
