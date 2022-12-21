using ScreenRecognition.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRecognition.Desktop.Models
{
    public class PropShieldModel<T> : BaseViewModel
    {
        private T _shield;

        public T Shield
        {
            get => _shield;
            set
            {
                _shield = value;
                OnPropertyChanged(nameof(Shield));
            }
        }
    }
}
