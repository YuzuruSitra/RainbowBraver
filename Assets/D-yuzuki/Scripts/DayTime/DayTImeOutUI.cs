using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ŽžŠÔ‚ÌUI•`‰æ(‰¼’u‚«)
public class DayTImeOutUI : MonoBehaviour
{
    [SerializeField]
    private DayTimeKeeper _dayTimeKeeper;
    [SerializeField]
    private TextMeshProUGUI _timeText;
    [SerializeField]
    private Slider _slider;

    void Update()
    {
        _timeText.text = "Time " + _dayTimeKeeper.CurrentTime.Hours.ToString("00") + " : " + _dayTimeKeeper.CurrentTime.Minutes.ToString("00");
        _slider.value = _dayTimeKeeper.CurrentHourRatio;
    }
}
