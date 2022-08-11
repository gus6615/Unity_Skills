using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RandomCtrl : MonoBehaviour
{
    // ����
    [SerializeField] private GameObject input_prefab;
    [SerializeField] private Image[] menuButtons;
    [SerializeField] private GameObject[] menus;
    [SerializeField] private Transform content_panel;
    [SerializeField] private Text resultText;
    private UIBox[] slots;
    private int menuIndex;

    // Single-Rand ����
    [SerializeField] private InputField single_input;

    // Multi-Rand ����
    [SerializeField] private Text multi_infoText;
    [SerializeField] private GameObject[] multi_pages;
    [SerializeField] private InputField multi_input_num;
    [SerializeField] private Transform multi_content;
    [SerializeField] private GameObject multi_result;
    [SerializeField] private Text multi_resultText;
    private bool isOnOff_multi;

    // �ӽ� ������
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
    /// ��� UI�� �ʱ� ���·� ������ �Լ�
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
    /// UI�� �ʱ� ���·� �ʱ�ȭ�ϰ� �޴��� �����ϴ� �Լ�
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
                multi_infoText.text = "Ȯ���� ������ �Է��ϼ��� (ex. 4 = 4���� Ȯ�� ����, �ִ� 20��)";
                SetMultiPage(0);
                break;
        }
    }

    /// <summary>
    /// �޴� ��ư�� ���� Menu�� �����ϴ� �Լ�
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
    /// Multi_Rand���� Ȯ���� ������ �����ϴ� �Լ�
    /// </summary>
    public void ClickCreateRandNum()
    {
        // ������ ����
        int num = 0;

        try
        {
            num = int.Parse(multi_input_num.text);
            if (num > 20 || num < 0)
                return; // Error

            colors = new Color[num];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = new Color(Random.value, Random.value, Random.value) * 1.5f;
            multi_infoText.text = "�� Ȯ���� �Է��ϼ��� (ex. 1 / 1 / 2 / 4 = 12.5% / 12.5% / 25% / 50%)";
            SetMultiPage(1);
        }
        catch (System.Exception)
        {
            resultText.text = "�ùٸ� ���� �Է����ּ���.";
            throw;
        }

        // Ȯ�� �Է¶� �ʱ�ȭ
        separators = multi_content.GetComponentsInChildren<Separator>();
        for (int i = 0; i < separators.Length; i++)
            Destroy(separators[i].gameObject);

        // Ȯ�� �Է¶� ����
        for (int i = 0; i < num; i++)
        {
            Separator separator = Instantiate(input_prefab, multi_content).GetComponent<Separator>();
            separator.data = i;
        }
    }

    /// <summary>
    /// Single_Rand �ùķ��̼� ����
    /// </summary>
    public void ClickRandButton_Single()
    {
        // ������ ����
        float percent = 0f;
        int true_count = 0;

        try
        {
            percent = 1f / float.Parse(single_input.text);
        }
        catch (System.Exception)
        {
            resultText.text = "�ùٸ� ���� �Է����ּ���.";
            throw;
        }

        // ��� ���
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

        resultText.text = "���� ��� = ( " + Mathf.RoundToInt(slots.Length * percent) + " / " + slots.Length + " ) | ���� ��� = ( " + true_count + " / " + slots.Length + " )";
    }

    /// <summary>
    /// Multi_Rand �ùķ��̼� ����
    /// </summary>
    public void ClickRandButton_Multi()
    {
        // ������ ����
        separators = multi_content.GetComponentsInChildren<Separator>();
        float[] percents = new float[separators.Length];
        int[] results = new int[separators.Length];
        SetMultiPage(2);
        multi_infoText.text = "����� Ȯ���ϼ���.";
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
                resultText.text = "�ùٸ� ���� �Է����ּ���.";
                throw;
            }
        }

        // ��� ���
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
            multi_resultText.text += "[" + (i + 1) + "] �� Ȯ�� - " + results[i] + " �� / Ȯ�� : " + Mathf.RoundToInt((float)results[i] / slots.Length * 100f) + " %\n";
    }

    /// <summary>
    /// Multi_Rand ���â�� ���� �ݴ� �Լ�
    /// </summary>
    public void OnOffResult_Multi()
    {
        isOnOff_multi = !isOnOff_multi;
        multi_result.SetActive(isOnOff_multi);
    }

    /// <summary>
    /// Multi_Rand �ʱ� �������� ���ư��� �Լ�
    /// </summary>
    public void ReSet_Multi()
    {
        SetMultiPage(0);
    }

    /// <summary>
    /// 'Page'�� �ش��ϴ� Page�� �����ϰ� ��� False�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="page">Open�� Multi_Page�� Index</param>
    private void SetMultiPage(int page)
    {
        for (int i = 0; i < multi_pages.Length; i++)
            multi_pages[i].SetActive(false);
        multi_pages[page].SetActive(true);
    }

    /// <summary>
    /// 'percent' Ȯ���� ���� bool�� ��ȯ�ϴ� �Լ� = �ð����⵵ O(1)
    /// </summary>
    /// <param name="percent">Ȯ��</param>
    /// <returns></returns>
    static public bool GetRandFlag(float percent)
    {
        if (Random.value < percent)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 'percent' Ȯ�� �迭�� ���� �迭 Index�� ��ȯ�ϴ� �Լ� = �ð����⵵ O(3n - 1)
    /// </summary>
    /// <param name="percents">Ȯ�� �迭</param>
    /// <returns></returns>
    static public int GetRandFlag(params float[] _percents)
    {
        float[] percents = (float[])_percents.Clone();
        float value = Random.value;

        // ������ ����
        for (int i = 1; i < percents.Length; i++)
            percents[i] += percents[i - 1];

        // �����͸� (0f ~ 1f)�� ����ȭ �۾�
        for (int i = 0; i < percents.Length; i++)
            percents[i] /= percents[percents.Length - 1];

        // Ȯ�� Index ���ϱ�
        for (int i = 1; i < percents.Length; i++)
        {
            if (percents[i - 1] <= value && value < percents[i])
                return i;
        }

        return 0;
    }
}
