using System.Collections.Generic;
using _Bludoku.Scripts.Boards;
using _Bludoku.Scripts.Combo;
using DG.Tweening;
using UnityEngine;

namespace _Bludoku.Scripts.Effects
{
    public class ComboVfxController : MonoBehaviour
    {
        [SerializeField] private Board board;
        [SerializeField] private ComboMediator comboMediator;
        [SerializeField] private ComboPopupView popupView;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private SpriteRenderer boardGlow;
        [SerializeField] private ParticleSystem sparks;
        [SerializeField] private ParticleSystem burstPrefab;

        [SerializeField] private int maxIntensityLevel = 6;
        [SerializeField] private Color lowIntensityColor = new(0.6f, 0.8f, 1f);
        [SerializeField] private Color highIntensityColor = new(0.1f, 0.45f, 1f);
        [SerializeField] private float glowMinAlpha = 0.4f;
        [SerializeField] private float glowMaxAlpha = 0.7f;
        [SerializeField] private float glowPulseDuration = 1.2f;
        [SerializeField] private float minSparksRate = 5f;
        [SerializeField] private float maxSparksRate = 25f;
        [SerializeField] private int minBurstPerCell = 2;
        [SerializeField] private int maxBurstPerCell = 5;
        [SerializeField] private float maxShakeStrength = 0.15f;

        private const float GlowFadeDuration = 0.8f;
        private const float GlowColorDuration = 0.6f;
        private const float ShakeDuration = 0.3f;
        private const int ShakeVibrato = 20;

        private ParticleSystem _burst;
        private Tween _glowAlphaTween;
        private Tween _glowColorTween;
        private Color _glowColor;
        private float _glowAlpha;
        private Vector3 _cameraPosition;
        private List<Vector3> _clearedPositions;
        private float? _pendingBurstIntensity;

        private void Awake()
        {
            _cameraPosition = cameraTransform.localPosition;
            _glowColor = boardGlow.color;
            SetGlowAlpha(0f);

            board.OnFigurePlaced += FigurePlaced;
            comboMediator.Combo.OnComboIncreased += ComboIncreased;
            comboMediator.Combo.OnComboEnded += ComboEnded;
        }

        private void Start()
        {
            if (comboMediator.Combo.IsActive)
                ApplyIntensity(GetIntensity(comboMediator.Combo.Level));
        }

        private void LateUpdate()
        {
            if (_pendingBurstIntensity.HasValue && _clearedPositions != null)
                PlayBurst(_pendingBurstIntensity.Value, _clearedPositions);

            _pendingBurstIntensity = null;
            _clearedPositions = null;
        }

        private void OnDestroy()
        {
            board.OnFigurePlaced -= FigurePlaced;
            comboMediator.Combo.OnComboIncreased -= ComboIncreased;
            comboMediator.Combo.OnComboEnded -= ComboEnded;

            _glowAlphaTween?.Kill();
            _glowColorTween?.Kill();
            cameraTransform.DOKill();
        }

        private void FigurePlaced(ClearResult result)
        {
            _clearedPositions = result.ClearedPositions;
        }

        private void ComboIncreased(int level)
        {
            var intensity = GetIntensity(level);

            ApplyIntensity(intensity);
            Shake(intensity);
            _pendingBurstIntensity = intensity;
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

        private void PlayBurst(float intensity, List<Vector3> positions)
        {
            var burst = GetBurst();
            var emitParams = new ParticleSystem.EmitParams
            {
                startColor = GetColor(intensity),
                applyShapeToPosition = true
            };
            var countPerCell = Mathf.RoundToInt(Mathf.Lerp(minBurstPerCell, maxBurstPerCell, intensity));

            foreach (var position in positions)
            {
                emitParams.position = position;
                burst.Emit(emitParams, countPerCell);
            }
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

        private ParticleSystem GetBurst()
        {
            if (_burst == null)
                _burst = Instantiate(burstPrefab, board.transform);

            return _burst;
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
