using System.ComponentModel;
using System.Runtime.CompilerServices;
using AS.Common;

namespace AS.Client.Core.WPF
{
    public class WPFClientEntity : ClientEntity, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public override long EntityId { get { return _entityId; } protected set { _entityId = value; OnPropertyChanged(); } }
        public override Vector3 Position { get { return _position; } set { _position = value; OnPropertyChanged(); } }

        public WPFClientEntity(long entityId) : base(entityId)
        {
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            return;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
