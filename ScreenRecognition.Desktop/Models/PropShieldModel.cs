﻿using ScreenRecognition.Desktop.ViewModel;

namespace ScreenRecognition.Desktop.Models
{
    public class PropShieldModel<T> : BaseViewModel
    {
        private T _shield;

        public T Property
        {
            get => _shield;
            set
            {
                _shield = value;
                OnPropertyChanged(nameof(Property));
            }
        }

        public PropShieldModel()
        {

        }

        public PropShieldModel(T resource) : this()
        {
            Property = resource;
        }
    }
}
