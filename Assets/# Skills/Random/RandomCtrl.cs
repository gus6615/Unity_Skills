using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RandomCtrl : MonoBehaviour
{
    // 공용
    [SerializeField] private GameObject input_prefab;
    [SerializeField] private Image[] menuButtons;
    [SerializeField] private GameObject[] menus;
    [SerializeField] private Transform content_panel;
    [SerializeField] private Text resultText;
    private UIBox[] slots;
    private int menuIndex;

    // Single-Rand 전용
    [SerializeField] private InputField single_input;

    // Multi-Rand 전용
    [SerializeField] private Text multi_infoText;
    [SerializeField] private GameObject[] multi_pages;
    [SerializeField] private InputField multi_input_num;
    [SerializeField] private Transform multi_content;
    [SerializeField] private GameObject multi_result;
    [SerializeField] private Text multi_resultText;
    private bool isOnOff_multi;

    // 임시 데이터
    Separator separator;
    Separator[] separators;
    InputField inputField;
    Color[] colors;

    // Start is called before the first frame update
    void Start()
    {
        slots = content_panel.GetComponentsInChildren<UIBox>();
        SetMenu();
    }

    /// <summary>
    /// 모든 UI를 초기 상태로 돌리는 함수
    /// </summary>
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
        resultText.text = "";
    }

    /// <summary>
    /// UI를 초기 상태로 초기화하고 메뉴를 설정하는 함수
    /// </summary>
    private void SetMenu()
    {
        SetDefault();
        menuButtons[menuIndex].color = new Color(0.8f, 0.8f, 0.8f);
        menus[menuIndex].SetActive(true);

        switch (menuIndex)
        {
            case 0: // Single
                break;
            case 1: // Multi
                multi_infoText.text = "확률의 개수를 입력하세요 (ex. 4 = 4개의 확률 생성, 최대 20개)";
                SetMultiPage(0);
                break;
        }
    }

    /// <summary>
    /// 메뉴 버튼을 눌러 Menu를 설정하는 함수
    /// </summary>
    public void ClickMenuButton()
    {
        separator = EventSystem.current.currentSelectedGameObject.GetComponent<Separator>();
        if (separator == null)
            return;
        menuIndex = separator.data;

        SetMenu();
    }

    /// <summary>
    /// Multi_Rand에서 확률의 개수를 설정하는 함수
    /// </summary>
    public void ClickCreateRandNum()
    {
        // 데이터 설정
        int num = 0;

        try
        {
            num = int.Parse(multi_input_num.text);
            if (num > 20 || num < 0)
                return; // Error

            colors = new Color[num];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = new Color(Random.value, Random.value, Random.value) * 1.5f;
            multi_infoText.text = "각 확률을 입력하세요 (ex. 1 / 1 / 2 / 4 = 12.5% / 12.5% / 25% / 50%)";
            SetMultiPage(1);
        }
        catch (System.Exception)
        {
            resultText.text = "올바른 값을 입력해주세요.";
            throw;
        }

        // 확률 입력란 초기화
        separators = multi_content.GetComponentsInChildren<Separator>();
        for (int i = 0; i < separators.Length; i++)
            Destroy(separators[i].gameObject);

        // 확률 입력란 생성
        for (int i = 0; i < num; i++)
        {
            Separator separator = Instantiate(input_prefab, multi_content).GetComponent<Separator>();
            separator.data = i;
        }
    }

    /// <summary>
    /// Single_Rand 시뮬레이션 시작
    /// </summary>
    public void ClickRandButton_Single()
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
            resultText.text = "올바른 값을 입력해주세요.";
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

        resultText.text = "예상 결과 = ( " + Mathf.RoundToInt(slots.Length * percent) + " / " + slots.Length + " ) | 실제 결과 = ( " + true_count + " / " + slots.Length + " )";
    }

    /// <summary>
    /// Multi_Rand 시뮬레이션 시작
    /// </summary>
    public void ClickRandButton_Multi()
    {
        // 데이터 설정
        separators = multi_content.GetComponentsInChildren<Separator>();
        float[] percents = new float[separators.Length];
        int[] results = new int[separators.Length];
        SetMultiPage(2);
        multi_infoText.text = "결과를 확인하세요.";
        resultText.text = "";

        for (int i = 0; i < separators.Length; i++)
        {
            inputField = separators[i].GetComponent<InputField>();
            try
            {
                percents[i] = float.Parse(inputField.text);
            }
            catch (System.Exception)
            {
                resultText.text = "올바른 값을 입력해주세요.";
                throw;
            }
        }

        // 결과 출력
        slots = content_panel.GetComponentsInChildren<UIBox>();
        for (int i = 0; i < slots.Length; i++)
        {
            int index = GetRandFlag(percents);
            results[index]++;
            slots[i].texts[0].text = (index + 1).ToString();
            slots[i].images[0].color = colors[index];
        }

        multi_resultText.text = "";
        for (int i = 0; i < results.Length; i++)
            multi_resultText.text += "[" + (i + 1) + "] 번 확률 - " + results[i] + " 개 / 확률 : " + Mathf.RoundToInt((float)results[i] / slots.Length * 100f) + " %\n";
    }

    /// <summary>
    /// Multi_Rand 결과창을 열고 닫는 함수
    /// </summary>
    public void OnOffResult_Multi()
    {
        isOnOff_multi = !isOnOff_multi;
        multi_result.SetActive(isOnOff_multi);
    }

    /// <summary>
    /// Multi_Rand 초기 설정으로 돌아가는 함수
    /// </summary>
    public void ReSet_Multi()
    {
        SetMultiPage(0);
    }

    /// <summary>
    /// 'Page'에 해당하는 Page를 제외하고 모두 False로 설정하는 함수
    /// </summary>
    /// <param name="page">Open할 Multi_Page의 Index</param>
    private void SetMultiPage(int page)
    {
        for (int i = 0; i < multi_pages.Length; i++)
            multi_pages[i].SetActive(false);
        multi_pages[page].SetActive(true);
    }

    /// <summary>
    /// 'percent' 확률에 따라 bool을 반환하는 함수 = 시간복잡도 O(1)
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

    /// <summary>
    /// 'percent' 확률 배열에 따라 배열 Index를 반환하는 함수 = 시간복잡도 O(3n - 1)
    /// </summary>
    /// <param name="percents">확률 배열</param>
    /// <returns></returns>
    static public int GetRandFlag(params float[] _percents)
    {
        float[] percents = (float[])_percents.Clone();
        float value = Random.value;

        // 데이터 설정
        for (int i = 1; i < percents.Length; i++)
            percents[i] += percents[i - 1];

        // 데이터를 (0f ~ 1f)로 정규화 작업
        for (int i = 0; i < percents.Length; i++)
            percents[i] /= percents[percents.Length - 1];

        // 확률 Index 정하기
        for (int i = 1; i < percents.Length; i++)
        {
            if (percents[i - 1] <= value && value < percents[i])
                return i;
        }

        return 0;
    }
}
