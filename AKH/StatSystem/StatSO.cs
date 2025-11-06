using System;
using System.Collections.Generic;
using Scripts.Network;
using UnityEngine;

namespace Scripts.StatSystem
{

    [CreateAssetMenu(fileName = "StatSO", menuName = "SO/Stat/Stat", order = 0)]
    public class StatSO : ScriptableObject, ICloneable
    {
        public delegate void ValueChangeHandler(StatSO stat, float currentValue, float prevValue);
        public event ValueChangeHandler OnValueChanged;

        public string statName;
        public string description;
        public StatType statType;
        
        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;
        [SerializeField] private float baseValue, minValue, maxValue; 

        private Dictionary<object, float> _modifyValueByKey = new Dictionary<object, float>();
        private Dictionary<object, float> _multiplierValueByKey = new Dictionary<object, float>();

        [field: SerializeField] public bool IsPercent { get; private set; }
        public float ModifiedValue { get; private set; } = 0;
        public float MultiplierValue { get; private set; } = 0;
        public Sprite Icon => icon;
        public float MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }
        public float MinValue
        {
            get => minValue;
            set => minValue = value;
        }
        public float Value => Mathf.Clamp((baseValue + ModifiedValue) * (1+MultiplierValue), MinValue, MaxValue);
        public float DivideValue => Mathf.Clamp((baseValue + ModifiedValue) / (1+MultiplierValue), MinValue, MaxValue);
        public bool IsMax => Mathf.Approximately(Value, MaxValue);
        public bool IsMin => Mathf.Approximately(Value, MinValue);

        public float BaseValue
        {
            get => baseValue;
            set
            {
                float prevValue = Value;
                baseValue = Mathf.Clamp(value, MinValue, MaxValue);
                TryInvokeValueChangeEvent(value, prevValue);
            }
        }
        public float AddMultiplierOneTime(float value)
        {
            float multiplier = MultiplierValue;
            multiplier += value;
            return Mathf.Clamp((baseValue + ModifiedValue) * (1 + multiplier), MinValue, MaxValue);
        }
        public void AddMultiplier(object key,float value)
        {
            if (_multiplierValueByKey.ContainsKey(key)) return;
            float prevValue = Value;
            MultiplierValue += value;
            _multiplierValueByKey.Add(key, value);
            TryInvokeValueChangeEvent(Value, prevValue);
        }
        public void RemoveMultiplier(object key)
        {
            if (_multiplierValueByKey.TryGetValue(key, out float value))
            {
                float prevValue = Value;
                MultiplierValue -= value;
                _multiplierValueByKey.Remove(key);
                TryInvokeValueChangeEvent(Value, prevValue);
            }
        }
        public void AddModifier(object key, float value)
        {
            if (_modifyValueByKey.ContainsKey(key)) return;

            float prevValue = Value;
            ModifiedValue += value;
            _modifyValueByKey.Add(key, value);
            TryInvokeValueChangeEvent(Value, prevValue);
        }
        public void RemoveModifier(object key)
        {
            if (_modifyValueByKey.TryGetValue(key, out float value))
            {
                float prevValue = Value;
                ModifiedValue -= value;
                _modifyValueByKey.Remove(key);
                TryInvokeValueChangeEvent(Value, prevValue);
            }
        }
        public void ClearModifier()
        {
            float prevValue = Value;
            _modifyValueByKey.Clear();
            ModifiedValue = 0;
            TryInvokeValueChangeEvent(Value, prevValue);
        }
        public void ClearMultiplier()
        {
            float prevValue = Value;
            _multiplierValueByKey.Clear();
            MultiplierValue = 0;
            TryInvokeValueChangeEvent(Value, prevValue);
        }
        private void TryInvokeValueChangeEvent(float value, float prevValue)
        {
            if (Mathf.Approximately(value, prevValue) == false)
            {
                OnValueChanged?.Invoke(this, value, prevValue);
            }
        }
        public object Clone()
        {
            return Instantiate(this);
        }
    }
}
