using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DemThoiGian : MonoBehaviour
{
    public static DemThoiGian Instance;
    public TextMeshProUGUI timerText; // Text để hiển thị thời gian
    public Animator countdownAnimator;
    public float countdownTime; // Thời gian đếm ngược ban đầu (giây)

    private bool isCountingDown;
    private bool isStopCountdown;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        // Bắt đầu đếm ngược khi game khởi chạy
        StartCountdown();
        isCountingDown = isStopCountdown = false;
    }
    public void SetCountdownTime(float value)
    {
        countdownTime = value;
    }
    public void StartCountdown()
    {
        if (!isCountingDown)
        {
            StartCoroutine(CountdownCoroutine());
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        isCountingDown = true;

        float remainingTime = countdownTime;
        bool sapHetThoiGian = false;
        while (remainingTime > 0)
        {
            // Nếu game đang tạm dừng, đợi cho đến khi game tiếp tục
            while (isStopCountdown)
            {
                yield return null; // Đợi 1 frame trước khi kiểm tra lại
            }

            // Cập nhật Text với thời gian còn lại ở định dạng phút:giây
            timerText.text = FormatTime(remainingTime);

            // Đợi 1 giây thực tế (thời gian không bị ảnh hưởng bởi Time.timeScale)
            yield return new WaitForSecondsRealtime(1f);

            // Giảm thời gian còn lại
            remainingTime--;
            if (remainingTime < 10f && !sapHetThoiGian)
            {
                sapHetThoiGian = true;
                timerText.color = Color.red;
                countdownAnimator.enabled = true;
            }
        }

        // Khi đếm ngược kết thúc
        timerText.text = "0:00";
        PlayerControl.Instance.GetComponent<TrangThai>().Hurt(float.MaxValue, Vector2.zero, 0f);
        isCountingDown = false;
    }
    public void StopCountdown()
    {
        isStopCountdown = true;
    }
    public void ContinueCountDown()
    {
        isStopCountdown = false;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60); // Chia lấy phút
        int seconds = Mathf.FloorToInt(time % 60); // Lấy số giây còn lại
        return string.Format("{0:0}:{1:00}", minutes, seconds); // Định dạng MM:SS
    }
}
