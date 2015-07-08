using AS.Common;
using System;
using System.ComponentModel;

namespace AS.Client.Core.WPF
{
    public class ViewModelBase : ObjectBase, IChangeTracking
    {
        protected override void OnPropertyChanged<T>(System.Linq.Expressions.Expression<Func<T>> propertyExpression)
        {
            this.IsChanged = true;
            base.OnPropertyChanged<T>(propertyExpression);
        }
    }
}
