using System;
using System.Collections;
using SidiaGameJam.Events;
using CookinVaniaController = SidiaGameJam.Controller.Controller;
using SidiaGameJam.Events.EventConfig;
using UnityEngine;

namespace SidiaGameJam.Components
{
    public class LifeComponentBase : ComponentBase
    {
        public EventOneBoolParamRelated onPlayerDead;
        public SoEvent onPlayerDestroyed;
        public SoEventRelated onPlayerTakeHit;
        public bool canTakeDamage = true;
        private float _currentLife;

        private float _maxLife = 100f;

        public override void Initialize()
        {
            base.Initialize();
            _currentLife = _maxLife;
        }

        public void SetMaxLife(float maxLife)
        {
            _maxLife = maxLife;
            _currentLife = _maxLife;
        }

        public void Heal(float amount)
        {
            if (_currentLife + amount > _maxLife)
                _currentLife = _maxLife;

            else
                _currentLife += amount;
        }

        public void ReceiveDamage(float amount)
        {
            if (amount <= 0 || _currentLife <= 0 || !canTakeDamage) return;

            if (_currentLife - amount <= 0)
            {
                _currentLife = 0;
                Death();
            }

            else
            {
                _currentLife -= amount;
                onPlayerTakeHit.InvokeAction(gameObject);
            }
        }

        protected virtual void Death()
        {
            onPlayerDead.InvokeAction(true, gameObject);
            GetOwner().GetComponent<Controller.Controller>().DisableInput();
            StartCoroutine(nameof(DestroyPlayerByTime));
        }

        private IEnumerator DestroyPlayerByTime()
        {
            yield return new WaitForSeconds(2);
            if (onPlayerDestroyed)
            {
                onPlayerDestroyed.InvokeAction();
            }

            Destroy(gameObject);
        }
    }
}