using CoreTestApp.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace CoreTestApp.Hubs
{
    /// <summary>
    /// SignalRハブテストクラス
    /// </summary>
    public class ChatHub : Hub
    {
        /// <summary>
        /// 状態変更検知テストのリスト
        /// </summary>
        private List<bool> list = Enumerable.Repeat(false, 10).ToList();

        private IDisposable _listener;

        private String _message = String.Empty;

        public ChatHub()
        {
            // リスナーを登録
            _listener = Observable.Interval(TimeSpan.FromMilliseconds(1000))
                // 一個前と今の値の状態をタプルで保持
                .Scan((list.ToReadOnlyCollection(), list.ToReadOnlyCollection()), (x, y) => _ = (x.Item2, list.ToReadOnlyCollection()))
                // 変更された時だけ実行する
                .Where(x => !x.Item1.SequenceEqual(x.Item2))
                .Subscribe(_ => {
                    // TODO: 状態変更にピンのNoと値をメンバ変数として書き換えたい
                    _message = "xvideos";
                });
        }

        /// <summary>
        /// 
        /// </summary>
        ~ChatHub()
        {
            // デストラクタは誤り？
            _listener.Dispose();
        }

        public Task Broadcast(string message)
        {
            // ピンの状態変更を発生させてみる
            list[0] = !list[0];
           
            var timestamp = DateTime.Now.ToString();

            // メッセージを画面にSignalRで描画
            return Clients.All.SendAsync("Receive", _message, timestamp);
        }
    }

    static class Ex
    {
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> self) => new ReadOnlyCollection<T>(self.ToList());
    }
}