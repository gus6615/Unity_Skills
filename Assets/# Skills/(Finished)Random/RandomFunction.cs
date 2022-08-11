using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFunction
{
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
