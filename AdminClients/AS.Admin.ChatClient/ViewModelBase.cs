using AS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Admin.ChatClient
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
