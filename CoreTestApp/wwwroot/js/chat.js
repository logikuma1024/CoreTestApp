//--- ChatHub とのコネクションを生成
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .configureLogging(signalR.LogLevel.Information)
    .build();

//--- 受信したときの処理
connection.on('Receive', (message, timestamp) => {
    const item = document.createElement('li');
    item.innerHTML = "<div>" + timestamp + " - " + message + "</div>";
    document.getElementById('messages').appendChild(item);
});

//--- ボタンをクリックしたらデータを送信
document.getElementById('button').addEventListener('click', event => {
    const message = document.getElementById('message').value;
    connection.invoke('Broadcast', message).catch(e => console.log(e));
    event.preventDefault();
});

//--- 接続を確立
connection.start().catch(e => console.log(e));