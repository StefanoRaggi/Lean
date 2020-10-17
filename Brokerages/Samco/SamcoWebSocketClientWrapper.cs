﻿using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QuantConnect.Logging;
using QuantConnect.Util;

namespace QuantConnect.Brokerages.Samco
{
    public class SamcoWebSocketClientWrapper : IWebSocket
    {
        private const int ReceiveBufferSize = 8192;

        private string _url;
        private string _sessionToken;
        private CancellationTokenSource _cts;
        private ClientWebSocket _client;
        private Task _taskConnect;
        private readonly object _locker = new object();

        /// <summary>
        /// Wraps constructor
        /// </summary>
        /// <param name="url"></param>
        public void Initialize(string url)
        {
            _url = url;
        }

        public void SetAuthTokenHeader(string sessionToken)
        {

            _sessionToken = sessionToken;
        }

        /// <summary>
        /// Wraps send method
        /// </summary>
        /// <param name="data"></param>
        public void Send(string data)
        {
            lock (_locker)
            {
                var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(data));
                _client.SendAsync(buffer, WebSocketMessageType.Text, true, _cts.Token).SynchronouslyAwaitTask();
            }
        }

        /// <summary>
        /// Wraps Connect method
        /// </summary>
        public void Connect()
        {
            lock (_locker)
            {
                if (_cts == null)
                {
                    _cts = new CancellationTokenSource();

                    _taskConnect = Task.Factory.StartNew(
                        () =>
                        {
                            Log.Trace("SamcoWebSocketClientWrapper connection task started.");

                            try
                            {
                                while (!_cts.IsCancellationRequested)
                                {
                                    using (var connectionCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token))
                                    {
                                        HandleConnection(connectionCts).SynchronouslyAwaitTask();
                                        connectionCts.Cancel();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Log.Error(e, "Error in SamcoWebSocketClientWrapper connection task");
                            }

                            Log.Trace("SamcoWebSocketClientWrapper connection task ended.");
                        },
                        _cts.Token);
                }
            }
        }

        /// <summary>
        /// Wraps Close method
        /// </summary>
        public void Close()
        {
            try
            {
                _client?.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", _cts.Token).SynchronouslyAwaitTask();

                _cts?.Cancel();

                _taskConnect?.Wait(TimeSpan.FromSeconds(5));

                _cts.DisposeSafely();
            }
            catch (Exception e)
            {
                Log.Error($"SamcoWebSocketClientWrapper.Close(): {e}");
            }

            _cts = null;

            OnClose(new WebSocketCloseData(0, string.Empty, true));
        }

        /// <summary>
        /// Wraps IsAlive
        /// </summary>
        public bool IsOpen => _client != null && _client.State == WebSocketState.Open;

        /// <summary>
        /// Wraps message event
        /// </summary>
        public event EventHandler<WebSocketMessage> Message;

        /// <summary>
        /// Wraps error event
        /// </summary>
        public event EventHandler<WebSocketError> Error;

        /// <summary>
        /// Wraps open method
        /// </summary>
        public event EventHandler Open;

        /// <summary>
        /// Wraps close method
        /// </summary>
        public event EventHandler<WebSocketCloseData> Closed;

        /// <summary>
        /// Wraps ReadyState
        /// </summary>
        public WebSocketState ReadyState => _client.State;

        /// <summary>
        /// Event invocator for the <see cref="Message"/> event
        /// </summary>
        protected virtual void OnMessage(WebSocketMessage e)
        {
            //Log.Trace("SamcoWebSocketClientWrapper.OnMessage(): " + e.Message);
            Message?.Invoke(this, e);
        }

        /// <summary>
        /// Event invocator for the <see cref="Error"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnError(WebSocketError e)
        {
            Log.Error(e.Exception, $"SamcoWebSocketClientWrapper.OnError(): (IsOpen:{IsOpen}, State:{_client.State}): {e.Message}");
            Error?.Invoke(this, e);
        }

        /// <summary>
        /// Event invocator for the <see cref="Open"/> event
        /// </summary>
        protected virtual void OnOpen()
        {
            Log.Trace($"SamcoWebSocketClientWrapper.OnOpen(): Connection opened (IsOpen:{IsOpen}, State:{_client.State}): {_url}");
            Open?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event invocator for the <see cref="Close"/> event
        /// </summary>
        protected virtual void OnClose(WebSocketCloseData e)
        {
            Log.Trace($"SamcoWebSocketClientWrapper.OnClose(): Connection closed (IsOpen:{IsOpen}, State:{_client.State}): {_url}");
            Closed?.Invoke(this, e);
        }

        private async Task HandleConnection(CancellationTokenSource connectionCts)
        {
            using (_client = new ClientWebSocket())
            {
                Log.Trace("SamcoWebSocketClientWrapper.HandleConnection(): Auth token " + _sessionToken + " Connecting to " + _url + " ....");

                try
                {
                    if (_sessionToken == null)
                    {
                        Log.Error("Error in SamcoWebSocketClientWrapper.SetAuthTokenHeader(): websocket auth session token is empty.");
                        return;
                    }

                    _client.Options.SetRequestHeader("x-session-token", _sessionToken);
                    await _client.ConnectAsync(new Uri(_url), connectionCts.Token);
                    OnOpen();

                    while ((_client.State == WebSocketState.Open || _client.State == WebSocketState.CloseSent) &&
                        !connectionCts.IsCancellationRequested)
                    {
                        var messageData = await ReceiveMessage(_client, connectionCts.Token);

                        if (messageData.MessageType == WebSocketMessageType.Close)
                        {
                            Log.Trace("SamcoWebSocketClientWrapper.HandleConnection(): WebSocketMessageType.Close");
                            return;
                        }

                        var message = Encoding.UTF8.GetString(messageData.Data);
                        OnMessage(new WebSocketMessage(message));
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
                    OnError(new WebSocketError(ex.Message, ex));
                }
            }
        }

        private static async Task<MessageData> ReceiveMessage(
            WebSocket webSocket,
            CancellationToken ct,
            long maxSize = long.MaxValue)
        {
            var buffer = new ArraySegment<byte>(new byte[ReceiveBufferSize]);

            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;

                do
                {
                    result = await webSocket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                    if (ms.Length > maxSize)
                    {
                        throw new InvalidOperationException("Maximum size of the message was exceeded.");
                    }
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);

                return new MessageData
                {
                    Data = ms.ToArray(),
                    MessageType = result.MessageType
                };
            }
        }

        private class MessageData
        {
            public byte[] Data { get; set; }
            public WebSocketMessageType MessageType { get; set; }
        }
    }
}