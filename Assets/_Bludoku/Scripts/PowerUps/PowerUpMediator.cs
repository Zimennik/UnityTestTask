using System;
using System.Collections.Generic;
using _Bludoku.Scripts.Boards;
using _Bludoku.Scripts.Core;
using UnityEngine;

namespace _Bludoku.Scripts.PowerUps
{
    public class PowerUpMediator : MonoBehaviour
    {
        public event Action<string> OnPowerUpUsed;

        [SerializeField] private Board board;
        [SerializeField] private FiguresController figuresController;
        [SerializeField] private PowerUpSlot[] slots;

        private readonly List<(PowerUpSlot slot, PowerUpCharges charges)> _powerUps = new();
        private PowerUpContext _context;

        public bool HasReadyPowerUp => _powerUps.Exists(powerUp => powerUp.charges.IsReady);

        private void Awake()
        {
            _context = new PowerUpContext(figuresController);

            foreach (var slot in slots)
            {
                var charges = new PowerUpCharges(slot.definition.MaxCharges, slot.definition.CooldownMoves);
                PowerUpSaveLoad.Load(slot.definition.Id, charges);

                slot.button.SetIcon(slot.definition.Icon);
                charges.OnChanged += () => UpdateView(slot, charges);
                charges.OnRecharged += slot.button.PlayRecharged;
                slot.button.OnClicked += () => Use(slot, charges);

                _powerUps.Add((slot, charges));
            }

            board.OnFigurePlaced += FigurePlaced;
        }

        private void Start()
        {
            foreach (var (slot, charges) in _powerUps)
                UpdateView(slot, charges);
        }

        private void OnDestroy()
        {
            board.OnFigurePlaced -= FigurePlaced;
        }

        public void ShowHint()
        {
            foreach (var (slot, charges) in _powerUps)
                slot.button.SetHintActive(charges.IsReady);
        }

        public void ResetPowerUps()
        {
            HideHint();

            foreach (var (slot, charges) in _powerUps)
            {
                charges.Reset();
                PowerUpSaveLoad.Save(slot.definition.Id, charges);
            }
        }

        private void Use(PowerUpSlot slot, PowerUpCharges charges)
        {
            if (!charges.TryUse())
                return;

            HideHint();
            PowerUpSaveLoad.Save(slot.definition.Id, charges);

            OnPowerUpUsed?.Invoke(slot.definition.Id);
            slot.definition.Apply(_context);
        }

        private void FigurePlaced(ClearResult result)
        {
            foreach (var (slot, charges) in _powerUps)
            {
                charges.RegisterMove();
                PowerUpSaveLoad.Save(slot.definition.Id, charges);
            }
        }

        private void HideHint()
        {
            foreach (var (slot, _) in _powerUps)
                slot.button.SetHintActive(false);
        }

        private static void UpdateView(PowerUpSlot slot, PowerUpCharges charges)
        {
            slot.button.SetState(charges.IsReady, charges.CooldownLeft, charges.CooldownMoves);
        }
    }
}
