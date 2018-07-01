using CoreTestApp.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace CoreTestApp.Hubs
{
    /// <summary>
    /// Gpioピンの状態変更を保持・通知するハブクラス
    /// </summary>
    public class GpioHub : Hub
    {
        /// <summary>
        /// ピンの値リスト
        /// </summary>
        private List<GpioPinValue> GpioValueList;

        /// <summary>
        /// Gpioピンリスナー
        /// </summary>
        private IDisposable GpioPinListener;

        /// <summary>
        /// スイッチのON回数
        /// </summary>
        private int SwitchCount;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GpioHub()
        {
            // 初期化
            init();

            // リスナーにイベントを登録
            GpioPinListener = Observable.Interval(TimeSpan.FromMilliseconds(100))
                // ピンのステータス変更を検知
                .Scan((GpioValueList.ToReadOnlyCollection(), GpioValueList.ToReadOnlyCollection()),
                        (x, y) => _ = (x.Item2, Pi.Gpio.Pins.Select(z => z.ReadValue()).ToList().ToReadOnlyCollection()))
                // ピンの値変更があった場合のみ動作させる
                .Where(x => !x.Item1.SequenceEqual(x.Item2))
                // 実行
                .Subscribe(x => {
                    // GPIOピン4番（タクトスイッチ）のONを検出する
                    if(x.Item2[4] == GpioPinValue.High)
                    {
                        SwitchCount++;
                    }
                });

            
            void init()
            {
                // ピンリストの初期状態をセット
                this.GpioValueList = Pi.Gpio.Pins.Select(x => x.ReadValue()).ToList();
                // タクトスイッチのON回数をリセット
                this.SwitchCount = 0;
            }
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public new void Dispose()
        {
            // ハブを解放
            base.Dispose();
            // ピンリスナーを解放
            GpioPinListener.Dispose();
        }

        /// <summary>
        /// スイッチの押下回数を取得
        /// </summary>
        /// <returns></returns>
        public int GetCurrentVal() => SwitchCount;
    }

    /// <summary>
    /// 拡張クラス
    /// </summary>
    static class Ex
    {
        /// <summary>
        /// ReadOnlyCollectionへの変換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> self) => new ReadOnlyCollection<T>(self.ToList());
    }
}