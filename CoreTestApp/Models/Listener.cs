using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTestApp.Models
{
    /// <summary>
    /// GPIOピンの状態変更イベント
    /// </summary>
    class ChangePinsStateEventArgs : EventArgs
    {
        public ReadOnlyDictionary<int, bool> Pins { get; }

        public ChangePinsStateEventArgs(IDictionary<int, bool> pins)
        {
            Pins = new ReadOnlyDictionary<int, bool>(pins);
        }
    }

    /// <summary>
    /// GPIOピンの入力を待機するリスナークラス
    /// </summary>
    class Listener
    {
        // 
        public event EventHandler<EventArgs> Started;

        public event EventHandler<ChangePinsStateEventArgs> PinsStateChanged;

        bool IsDisposed { get; set; }

        ConcurrentDictionary<int, bool> _Pins { get; set; }

        ConcurrentDictionary<int, bool> _PrevPins { get; set; }

        public ReadOnlyDictionary<int, bool> Pins => new ReadOnlyDictionary<int, bool>(_Pins);

        public async void Run()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(CoreTestApp));

            OnStart();

            await Task.Run(() =>
            {
                while (!IsDisposed)
                {
                    if (!_PrevPins.SequenceEqual(_Pins))
                    {
                        _PrevPins = new ConcurrentDictionary<int, bool>(_Pins);
                        PinsStateChanged?.Invoke(this, new ChangePinsStateEventArgs(_Pins));
                    }
                }
            });
        }

        protected virtual void OnStart()
        {
            _Pins = new ConcurrentDictionary<int, bool>(Enumerable.Range(1, 10).ToDictionary(x => x, _ => false));
            _PrevPins = new ConcurrentDictionary<int, bool>(_Pins);

            Started?.Invoke(this, new EventArgs());
        }

        public void ChangePinState(int addr)
        {
            if (_Pins?.Count <= addr)
                throw new ArgumentOutOfRangeException();

            _Pins[addr] = !_Pins[addr];
        }

        public void Dispose()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(CoreTestApp));
            IsDisposed = true;
        }
    }
}
