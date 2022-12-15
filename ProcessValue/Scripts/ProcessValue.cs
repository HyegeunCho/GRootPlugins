using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

namespace GRootPlugins.ProcessValue
{
    public class ProcessValue<T>
    {
        public T InitialValue { private set; get; }
        public T CurrentValue { private set; get; }
        public T ProcessedValue { private set; get; }

        public bool IsProcessed => CurrentValue.Equals(ProcessedValue);

        public delegate void ProcessValueEventHandler(object sender, T value);
        public event ProcessValueEventHandler CurrentValueUpdateEvent;
        public event ProcessValueEventHandler ProcessedValueUpdateEvent;

        public void ClearEvents()
        {
            CurrentValueUpdateEvent = null;
            ProcessedValueUpdateEvent = null;
        }
        
        public ProcessValue(T inValue)
        {
            InitialValue = inValue;
            CurrentValue = inValue;
            ProcessedValue = inValue;
        }

        public ProcessValue<T> Init(T inValue)
        {
            InitialValue = inValue;
            CurrentValue = inValue;
            ProcessedValue = inValue;
            
            CurrentValueUpdateEvent?.Invoke(this, CurrentValue);
            ProcessedValueUpdateEvent?.Invoke(this, ProcessedValue);
            
            return this;
        }
        
        public ProcessValue<T> Reset()
        {
            CurrentValue = InitialValue;
            ProcessedValue = InitialValue;
            
            CurrentValueUpdateEvent?.Invoke(this, CurrentValue);
            ProcessedValueUpdateEvent?.Invoke(this, ProcessedValue);
            
            return this;
        }

        public ProcessValue<T> Update(Func<T, T> inUpdateFunction)
        {
            CurrentValue = inUpdateFunction(CurrentValue);
            
            CurrentValueUpdateEvent?.Invoke(this, CurrentValue);

            return this;
        }
        
        public ProcessValue<T> Set(T inValue)
        {
            CurrentValue = inValue;
            
            CurrentValueUpdateEvent?.Invoke(this, CurrentValue);
            
            return this;
        }

        public ProcessValue<T> UnSet()
        {
            CurrentValue = ProcessedValue;
            
            CurrentValueUpdateEvent?.Invoke(this, CurrentValue);
            
            return this;
        }
        
        public ProcessValue<T> Process()
        {
            ProcessedValue = CurrentValue;
            
            ProcessedValueUpdateEvent?.Invoke(this, ProcessedValue);
            
            return this;
        }

        public new string ToString()
        {
            return $"{CurrentValue}/{ProcessedValue}";
        }
        
    }
    
}
