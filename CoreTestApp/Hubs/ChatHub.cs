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
                    // 状態変更にピンのNoと値をメンバ変数として書き換えたい
                    var timestamp = DateTime.Now.ToString();
                    Clients.All.SendAsync("Receive", "xvideos" ,timestamp);
                });
        }

        /// <summary>
        /// newは基本禁じ手やでな
        /// TODO: または、disposedイベント持ってればそれ登録してその中で解放すればいいかな
        /// </summary>
        public new void Dispose()
        {
            base.Dispose();
            _listener.Dispose();
        }

        /// <summary>
        /// ボタン押下時のハンドラ
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task Broadcast(string message)
        {
            // ピンの状態変更を発生させてみる
            list[0] = !list[0];

            return null;
        }
    }

    static class Ex
    {
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> self) => new ReadOnlyCollection<T>(self.ToList());
    }
}