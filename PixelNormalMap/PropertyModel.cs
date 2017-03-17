using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyFrameWork
{
     [SerializableAttribute()]
    public class PropertyModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        protected void SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (object.Equals(field, value)) { return; }

            field = value;
            var h = this.PropertyChanged;
            if (h != null) { h(this, new PropertyChangedEventArgs(propertyName)); }
        }
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
