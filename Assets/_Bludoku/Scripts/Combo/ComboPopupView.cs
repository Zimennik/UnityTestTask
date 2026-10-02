using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Bludoku.Scripts.Combo
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ComboPopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private float riseDistance = 80f;

        private const string LabelFormat = "COMBO x{0}";
        private const float AppearDuration = 0.25f;
        private const float HoldDuration = 0.5f;
        private const float DisappearDuration = 0.3f;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Vector2 _startPosition;
        private Sequence _sequence;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = (RectTransform)transform;
            _startPosition = _rectTransform.anchoredPosition;
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

        public void SetColor(Color color)
        {
            label.color = color;
        }

        public void Show(int level)
        {
            label.text = string.Format(LabelFormat, level);

            _sequence?.Kill();
            _rectTransform.anchoredPosition = _startPosition;
            _rectTransform.localScale = Vector3.zero;
            _canvasGroup.alpha = 1f;

            _sequence = DOTween.Sequence()
                .Append(_rectTransform.DOScale(Vector3.one, AppearDuration).SetEase(Ease.OutBack))
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
