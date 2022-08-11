# GetRandFlag - Single
<pre><code> // 'percent' 확률에 따라 bool을 반환하는 함수입니다.
    static public bool GetRandFlag(float percent)
    {
        if (Random.value < percent)
            return true;
        else
            return false;
    }
</code></pre>
위 함수는 랜덤 함수의 기본형입니다.<br>
인자 값, 'percent'에 따라 Bool 값을 반환합니다. <br><br>
Ex) percent = 0.2f 라면, 20% 확률로 True를 반환합니다.<br><br><br>

# 시뮬레이션 결과
![1](https://user-images.githubusercontent.com/57510872/183857390-34f1d9db-1624-4f36-9d56-afb99b4e33ae.PNG)<br>
