# RandomFunction Script
## 이 Script는 단일(Single) 혹은 다수(Multi) 확률 구현에 필요한 함수를 포함합니다.
<br>

<pre><code>
    static public bool GetRandFlag(float percent)
    {
        if (Random.value < percent)
            return true;
        else
            return false;
    }
</code></pre>
### GetRandFlag 함수 (단일 확률)<br><br>
위 함수는 **단일 확률**에 대한 랜덤 기능을 수행하는 함수입니다.<br>
인자 값, 'percent'에 따라 Bool 값을 반환합니다. <br><br>
예를 들어, percent = 0.2f 라면 20% 확률로 True를 반환합니다.
<br><br>

<pre><code>
    static public int GetRandFlag(params float[] _percents)
    {
        float[] percents = (float[])_percents.Clone();
        float value = Random.value;

        // 구간 설정
        for (int i = 1; i < percents.Length; i++)
            percents[i] += percents[i - 1];

        // 정규화 작업 (0f ~ 1f)
        for (int i = 0; i < percents.Length; i++)
            percents[i] /= percents[percents.Length - 1];

        // value(난수)가 어느 구간에 있는지 확인
        for (int i = 1; i < percents.Length; i++)
        {
            if (percents[i - 1] <= value && value < percents[i])
                return i;
        }

        return 0;
    }
</code></pre>
### GetRandFlag 함수(다수 확률)<br><br>
위 함수는 **다수의 확률**에 대한 랜덤 기능을 수행하는 함수입니다.<br>
이때 인자 값, 'percents'를 통해 구간을 나누고 정규화 하는 과정이 필요합니다.<br>
그 이유는 Random.value 값이 0 ~ 1 사이의 Float이기 때문입니다.<br><br>
정규화 과정을 거쳤다면 미리 선언한 value (= Random.value, 난수)가 어떤 구간에 존재하는지 확인하고 구간의 정보를 반환합니다.
 <br><br>
예를 들어, percents = { 1f, 1f, 2f, 4f } 이라면, 구간 작업 및 정규화를 통해 percents = { 0.125f, 0.25f. 0.5f, 1f } 가 됩니다. 이때, value (0 ~ 1) 값이 0 ~ 0.125f 라면 0을, 0.125f ~ 0.25f 라면 1을, 0.25f ~ 0.5f 라면 2를, 0.5f ~ 1f 라면 3을 반환합니다.

<br><br><br><br><br>

# Single 확률 시뮬레이션 결과
![1](https://user-images.githubusercontent.com/57510872/183857390-34f1d9db-1624-4f36-9d56-afb99b4e33ae.PNG)<br>

20% 확률에 대한 시뮬레이션 결과입니다.<br>
보통의 경우 위와 같이 5번 ~ 7번의 성공 결과가 보여졌습니다.<br><br>
하지만 아래 결과와 같이 큰 차이가 나는 경우도 나타났습니다.
<br><br><br><br>

![6](https://user-images.githubusercontent.com/57510872/184073739-8dc4cac9-b771-427e-b0bb-4a467c349dbf.PNG)
![7](https://user-images.githubusercontent.com/57510872/184073742-21b80ade-1e39-4ae1-85ef-012151bf2b11.PNG)
~~(이래서 운빨망겜 이라는 소리가 나오나 봅니다.)~~
<br><br><br><br>

# Multi 확률 시뮬레이션 결과

![2](https://user-images.githubusercontent.com/57510872/184073898-c4746214-efdb-4f19-90bf-99e0426f615a.PNG)
![3](https://user-images.githubusercontent.com/57510872/184073902-63479f49-6306-4841-b258-90b25f90d9df.PNG)
![4](https://user-images.githubusercontent.com/57510872/184073903-ac702855-815a-4af4-bcd1-813b5b103675.PNG)
![5](https://user-images.githubusercontent.com/57510872/184073904-c565f9af-f52a-4d66-819c-11ce576b8fe4.PNG)


각각 70%, 20%, 10% 확률에 대한 시뮬레이션 결과입니다.<br>
Multi 확률 또한 극단적인 경우 50%, 20%, 30% 가 나오기도 했습니다.<br>
하지만 대부분은 정상적으로 나타났습니다.
<br><br>

이상으로 랜덤 확률 구현에 대한 기록을 마치도록 하겠습니다.<br>
긴 글을 읽어 주셔서 감사합니다.<br><br>

※ 시뮬레이션이 궁금하시면 프로젝트를 받아 수행해주시면 됩니다.
