using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Bludoku.Scripts.PowerUps
{
    public class PowerUpButtonView : MonoBehaviour
    {
        public event Action OnClicked;

        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private Image cooldownFill;
        [SerializeField] private TMP_Text cooldownText;
        [SerializeField] private Color readyColor = Color.white;
        [SerializeField] private Color cooldownColor = new(0.4f, 0.4f, 0.4f, 1f);

        private const float HintScale = 1.08f;
        private const float HintDuration = 0.5f;
        private const float RechargePunchDuration = 0.3f;

        private void Awake()
        {
            button.onClick.AddListener(() => OnClicked?.Invoke());
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public void SetState(bool isReady, int cooldownLeft, int cooldownMoves)
        {
            button.interactable = isReady;
            icon.color = isReady ? readyColor : cooldownColor;

            cooldownFill.gameObject.SetActive(!isReady);
            cooldownText.gameObject.SetActive(!isReady);

            if (isReady)
                return;

            cooldownFill.fillAmount = (float)cooldownLeft / cooldownMoves;
            cooldownText.text = cooldownLeft.ToString();
        }

        public void SetHintActive(bool active)
        {
            ResetScale();

            if (active)
            {
                transform.DOScale(HintScale, HintDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }

        public void PlayRecharged()
        {
            ResetScale();
            transform.DOPunchScale(Vector3.one * 0.2f, RechargePunchDuration, 1, 0.5f);
        }

        private void ResetScale()
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
        }
    }
}
