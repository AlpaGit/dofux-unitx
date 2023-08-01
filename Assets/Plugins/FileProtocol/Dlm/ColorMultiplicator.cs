using System.ComponentModel;

namespace DofusCoube.FileProtocol.Dlm
{
    public sealed class ColorMultiplicator : INotifyPropertyChanged
    {
        private int _blue;
        private int _green;


        private bool _isOne;
        private int _red;

        public ColorMultiplicator(int red, int green, int blue, bool force = false)
        {
            _red   = red;
            _green = green;
            _blue  = blue;

            if (!force && _red + _green + _blue == 0)
            {
                IsOne = true;
            }
        }

        public bool IsOne
        {
            get => _isOne;
            private set
            {
                if (_isOne == value)
                {
                    return;
                }

                _isOne = value;
                OnPropertyChanged(nameof(IsOne));
            }
        }

        public int Blue
        {
            get => _blue;
            set
            {
                if (_blue == value)
                {
                    return;
                }

                _blue = value;
                OnPropertyChanged(nameof(Blue));
            }
        }

        public int Green
        {
            get => _green;
            set
            {
                if (_green == value)
                {
                    return;
                }

                _green = value;
                OnPropertyChanged(nameof(Green));
            }
        }

        public int Red
        {
            get => _red;
            set
            {
                if (_red == value)
                {
                    return;
                }

                _red = value;
                OnPropertyChanged(nameof(Red));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ColorMultiplicator Multiply(ColorMultiplicator cm)
        {
            var                isOne = IsOne;
            ColorMultiplicator result;
            if (isOne)
            {
                result = cm;
            }
            else
            {
                var isOne2 = cm.IsOne;
                if (isOne2)
                {
                    result = this;
                }
                else
                {
                    var multiplicator = new ColorMultiplicator(0, 0, 0)
                    {
                        _red   = _red + cm._red,
                        _green = _green + cm._green,
                        _blue  = _blue + cm._blue,
                    };
                    multiplicator._red   = Clamp(multiplicator._red, -128, 127);
                    multiplicator._green = Clamp(multiplicator._green, -128, -127);
                    multiplicator._blue  = Clamp(multiplicator._blue, -128, 127);
                    multiplicator.IsOne  = false;
                    result               = multiplicator;
                }
            }

            return result;
        }

        public static int Clamp(int value, int min, int max)
        {
            var flag = value > max;
            int result;
            if (flag)
            {
                result = max;
            }
            else
            {
                var flag2 = value < min;
                result = flag2 ? min : value;
            }

            return result;
        }

        public void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, new(propertyName));
        }
    }
}