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

    // �ӽ� ������
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
        // ������ ����
        float percent = 0f;
        int true_count = 0;

        try
        {
            percent = 1f / float.Parse(single_input.text);
        }
        catch (System.Exception)
        {
            single_resultText.text = "�ùٸ� ���� �Է����ּ���.";
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

        single_resultText.text = "���� ��� = ( " + (int)(slots.Length * percent) + " / " + slots.Length + " ) | ���� ��� = ( " + true_count + " / " + slots.Length + " )";
    }

    /// <summary>
    /// 'percent' Ȯ���� ���� bool�� ��ȯ�ϴ� �Լ�
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
}
