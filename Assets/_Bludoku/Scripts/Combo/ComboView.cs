using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Bludoku.Scripts.Combo
{
    public class ComboView : MonoBehaviour
    {
        [SerializeField] private Transform booster;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Image[] moveIndicators;
        [SerializeField] private Color moveAvailableColor = Color.white;
        [SerializeField] private Color moveSpentColor = new(1f, 1f, 1f, 0.25f);

        private const string LevelFormat = "x{0}";
        private const float LevelPunchDuration = 0.3f;

        private bool _isActive;
        private Tween _pulseTween;

        private void Awake()
        {
            booster.localScale = Vector3.zero;
        }

        public void SetState(bool isActive, int level, int movesLeft)
        {
            if (isActive)
                levelText.text = string.Format(LevelFormat, level);

            UpdateMoveIndicators(isActive ? movesLeft : 0);
            SetComboActive(isActive);
        }

        public void PlayLevelUp()
        {
            levelText.transform.DOKill(true);
            levelText.transform.DOPunchScale(Vector3.one * 0.4f, LevelPunchDuration, 1, 0.5f);
        }

        private void UpdateMoveIndicators(int movesLeft)
        {
            for (var i = 0; i < moveIndicators.Length; i++)
                moveIndicators[i].color = i < movesLeft ? moveAvailableColor : moveSpentColor;
        }

        private void SetComboActive(bool isActive)
        {
            if (_isActive == isActive)
                return;

            booster.DOKill();
            _pulseTween?.Kill();
            _pulseTween = null;

            if (isActive)
            {
                booster.DOScale(Vector3.one, 0.8f)
                    .SetEase(Ease.OutElastic)
                    .OnComplete(StartPulse);
            }
            else
            {
                booster.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            }

            _isActive = isActive;
        }

        private void StartPulse()
        {
            if (!_isActive)
                return;

            _pulseTween = booster.DOScale(1.08f, 0.45f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable()
        {
            booster.DOKill();
            _pulseTween?.Kill();
            _pulseTween = null;
        }
    }
}
