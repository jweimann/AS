using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AS.Common
{
    public class ObjectBase : INotifyPropertyChanged, IChangeTracking
    {
        #region INotifyPropertyChanged Members

        private event PropertyChangedEventHandler _PropertyChanged;

        private List<PropertyChangedEventHandler> _PropertyChangedSubscribers = new List<PropertyChangedEventHandler>();

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!_PropertyChangedSubscribers.Contains(value))
                {
                    _PropertyChanged += value;
                    _PropertyChangedSubscribers.Add(value);
                }
            }
            remove
            {
                _PropertyChanged -= value;
                _PropertyChangedSubscribers.Remove(value);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (_PropertyChanged != null)
            {
                this.IsChanged = true;
                _PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName);
        }

        #endregion INotifyPropertyChanged Members

        #region ValidatePropertyChanged Implementation

        /*
        private event ValidatePropertyChangedEventHandler _ValidatePropertyChanged;

        List<ValidatePropertyChangedEventHandler> _ValidatePropertyChangedSubscribers = new List<ValidatePropertyChangedEventHandler>();

        public event ValidatePropertyChangedEventHandler ValidatePropertyChanged
        {
            add
            {
                if (!_ValidatePropertyChangedSubscribers.Contains(value))
                {
                    _ValidatePropertyChanged += value;
                    _ValidatePropertyChangedSubscribers.Add(value);
                }
            }
            remove
            {
                _ValidatePropertyChanged -= value;
                _ValidatePropertyChangedSubscribers.Remove(value);
            }
        }

        protected void OnValidatePropertyChanged(string propertyName)
        {
            if (_ValidatePropertyChanged != null)
            {
                _ValidatePropertyChanged(this, new ValidatePropertyChangedEventHandler(propertyName));
            }
        }

        protected virtual void OnValidatePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnValidatePropertyChanged(propertyName);
        }
        */

        #endregion ValidatePropertyChanged Implementation

        #region IChangeTracking Implementation

        public virtual void AcceptChanges()
        {
            this.IsChanged = false;
        }

        private bool _isChanged;

        public virtual bool IsChanged
        {
            get { return _isChanged; }
            protected set
            {
                if (_isChanged != value)
                {
                    _isChanged = value;
                    OnPropertyChanged(() => IsChanged);
                }
            }
        }

        #endregion IChangeTracking Implementation
    }
}