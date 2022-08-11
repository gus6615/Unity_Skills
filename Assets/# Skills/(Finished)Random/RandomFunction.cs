using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFunction
{
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
