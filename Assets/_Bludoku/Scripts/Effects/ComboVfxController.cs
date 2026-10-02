using _Bludoku.Scripts.Combo;
using DG.Tweening;
using UnityEngine;

namespace _Bludoku.Scripts.Effects
{
    public class ComboVfxController : MonoBehaviour
    {
        [SerializeField] private ComboMediator comboMediator;
        [SerializeField] private ComboPopupView popupView;
        [SerializeField] private SpriteRenderer boardGlow;
        [SerializeField] private ParticleSystem sparks;
        [SerializeField] private ParticleSystem burst;
        [SerializeField] private Transform cameraTransform;

        [SerializeField] private int maxIntensityLevel = 6;
        [SerializeField] private Color lowIntensityColor = new(0.6f, 0.8f, 1f);
        [SerializeField] private Color highIntensityColor = new(0.1f, 0.45f, 1f);
        [SerializeField] private float glowMinAlpha = 0.4f;
        [SerializeField] private float glowMaxAlpha = 0.7f;
        [SerializeField] private float glowPulseDuration = 1.2f;
        [SerializeField] private float minSparksRate = 5f;
        [SerializeField] private float maxSparksRate = 25f;
        [SerializeField] private int minBurstCount = 15;
        [SerializeField] private int maxBurstCount = 50;
        [SerializeField] private float maxShakeStrength = 0.15f;

        private const float GlowFadeDuration = 0.8f;
        private const float GlowColorDuration = 0.6f;
        private const float ShakeDuration = 0.3f;
        private const int ShakeVibrato = 20;

        private Tween _glowAlphaTween;
        private Tween _glowColorTween;
        private Color _glowColor;
        private float _glowAlpha;
        private Vector3 _cameraPosition;

        private void Awake()
        {
            _cameraPosition = cameraTransform.localPosition;
            _glowColor = boardGlow.color;
            SetGlowAlpha(0f);

            comboMediator.Combo.OnComboIncreased += ComboIncreased;
            comboMediator.Combo.OnComboEnded += ComboEnded;
        }

        private void Start()
        {
            if (comboMediator.Combo.IsActive)
                ApplyIntensity(GetIntensity(comboMediator.Combo.Level));
        }

        private void OnDestroy()
        {
            comboMediator.Combo.OnComboIncreased -= ComboIncreased;
            comboMediator.Combo.OnComboEnded -= ComboEnded;

            _glowAlphaTween?.Kill();
            _glowColorTween?.Kill();
            cameraTransform.DOKill();
        }

        private void ComboIncreased(int level)
        {
            var intensity = GetIntensity(level);

            ApplyIntensity(intensity);
            PlayBurst(intensity);
            Shake(intensity);
        }

        private void ComboEnded(int level)
        {
            _glowAlphaTween?.Kill();
            _glowAlphaTween = DOTween.To(() => _glowAlpha, SetGlowAlpha, 0f, GlowFadeDuration)
                .SetEase(Ease.InOutSine);

            sparks.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        private void ApplyIntensity(float intensity)
        {
            var color = GetColor(intensity);

            popupView.SetColor(color);
            StartGlow(color);

            var main = sparks.main;
            main.startColor = color;

            var emission = sparks.emission;
            emission.rateOverTime = Mathf.Lerp(minSparksRate, maxSparksRate, intensity);

            if (!sparks.isPlaying)
                sparks.Play();
        }

        private void StartGlow(Color color)
        {
            _glowColorTween?.Kill();

            if (_glowAlpha <= 0f)
                SetGlowColor(color);
            else
                _glowColorTween = DOTween.To(() => _glowColor, SetGlowColor, color, GlowColorDuration)
                    .SetEase(Ease.InOutSine);

            _glowAlphaTween?.Kill();
            _glowAlphaTween = DOTween.To(() => _glowAlpha, SetGlowAlpha, glowMaxAlpha, GlowFadeDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(StartGlowPulse);
        }

        private void StartGlowPulse()
        {
            _glowAlphaTween = DOTween.To(() => _glowAlpha, SetGlowAlpha, glowMinAlpha, glowPulseDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void PlayBurst(float intensity)
        {
            var main = burst.main;
            main.startColor = GetColor(intensity);

            burst.Emit(Mathf.RoundToInt(Mathf.Lerp(minBurstCount, maxBurstCount, intensity)));
        }

        private void Shake(float intensity)
        {
            var strength = maxShakeStrength * intensity;
            if (strength <= 0f)
                return;

            cameraTransform.DOKill();
            cameraTransform.localPosition = _cameraPosition;
            cameraTransform.DOShakePosition(ShakeDuration, new Vector3(strength, strength, 0f), ShakeVibrato)
                .OnComplete(() => cameraTransform.localPosition = _cameraPosition);
        }

        private void SetGlowColor(Color color)
        {
            _glowColor = color;
            ApplyGlow();
        }

        private void SetGlowAlpha(float alpha)
        {
            _glowAlpha = alpha;
            ApplyGlow();
        }

        private void ApplyGlow()
        {
            boardGlow.color = new Color(_glowColor.r, _glowColor.g, _glowColor.b, _glowAlpha);
        }

        private float GetIntensity(int level)
        {
            return Mathf.InverseLerp(ComboSystem.ActivationLevel, maxIntensityLevel, level);
        }

        private Color GetColor(float intensity)
        {
            return Color.Lerp(lowIntensityColor, highIntensityColor, intensity);
        }
    }
}
