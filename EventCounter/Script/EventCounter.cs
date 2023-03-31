using System;

namespace GRootPlugins.EventCounter
{
    public class EventCounter<T> 
    {
        public T CurrentValue { private set; get; }

        private T _initialValue;
        private Func<T, T, T> _updater;
        private Predicate<T> _predicator;
        private bool _isAutoExecute;

        public event Action OnCountComplete;
        
        public EventCounter(T inInitialValue, Func<T, T, T> inUpdater, Predicate<T> inPredicator, bool inAutoExecute = true)
        {
            _initialValue = inInitialValue;
            _updater = inUpdater;
            _predicator = inPredicator;
            _isAutoExecute = inAutoExecute;
            
            ResetValue();
        }

        public void ResetValue()
        {
            CurrentValue = _initialValue;
        }
        
        public void UpdateValue(T inDiffValue)
        {
            if (!_isActivate) return;
            
            if (_updater == null) throw new Exception("No Update function");
            CurrentValue = _updater(CurrentValue, inDiffValue);

            if (!_isAutoExecute) return;
            
            if (IsCountComplete())
            {
                Execute();
                ResetValue();
            }
        }

        public bool IsCountComplete()
        {
            if (!_isActivate) return false;
            
            if (_predicator == null) throw new Exception("No Predicator function");
            return _predicator(CurrentValue);
        }

        public void Execute()
        {
            OnCountComplete?.Invoke();
        }
        
        public void ClearEvents()
        {
            OnCountComplete = null;
        }

        private bool _isActivate = false;

        public void Pause()
        {
            _isActivate = false;
        }

        public void Start()
        {
            _isActivate = true;
        }
    }
    
}
