using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Bludoku.Scripts.Score
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ScoreGainView : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private float riseDistance = 40f;

        private const string LabelFormat = "+{0}";
        private const float AppearDuration = 0.25f;
        private const float HoldDuration = 0.35f;
        private const float DisappearDuration = 0.3f;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Vector2 _startPosition;
        private Sequence _sequence;
        private int _pendingPoints;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = (RectTransform)transform;
            _startPosition = _rectTransform.anchoredPosition;
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

        private void LateUpdate()
        {
            if (_pendingPoints <= 0)
                return;

            Show(_pendingPoints);
            _pendingPoints = 0;
        }

        public void AddPoints(int points)
        {
            _pendingPoints += points;
        }

        private void Show(int points)
        {
            label.text = string.Format(LabelFormat, points);

            _sequence?.Kill();
            _rectTransform.anchoredPosition = _startPosition;
            _rectTransform.localScale = Vector3.zero;
            _canvasGroup.alpha = 1f;

            _sequence = DOTween.Sequence()
                .Append(_rectTransform.DOScale(Vector3.one, AppearDuration).SetEase(Ease.OutCubic))
                .AppendInterval(HoldDuration)
                .Append(_rectTransform.DOAnchorPosY(_startPosition.y + riseDistance, DisappearDuration).SetEase(Ease.InQuad))
                .Join(_canvasGroup.DOFade(0f, DisappearDuration));
        }

        private void OnDisable()
        {
            _sequence?.Kill();
        }
    }
}
