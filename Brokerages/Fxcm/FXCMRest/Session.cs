/*
 * Copyright (c) 2018: FXCM Group, LLC
 *
 * FXCM Group, LLC and each of its affiliates and subsidiaries are herein referred
 * to as, "FXCM".
 *
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 * 3. FXCM's name may not be used to endorse or promote products derived from
 *    this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY FXCM "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL FXCM BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF
 * THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

using FXCMRest.Models;

// TODO: GetOffers etc: Get snapshot of several tables at once, note that GetModelResponse already supports it.
// TODO: Subscribe/Unsubscribe: probably multiple tables may be specified.
// TODO: SubscribeSymbol/UnsubscribeSymbol: probably multiple symbols may be specified.
// TODO: Probably implement TableManager class that will get and update all tables up-to-date
// TODO: Add logging support

namespace FXCMRest
{
    /// <summary>
    /// The enum with types of available trading tables.
    /// </summary>
    public enum TradingTable
    {
        Offer,
        OpenPosition,
        ClosedPosition,
        Order,
        Account,
        Summary,
// TODO
//        LeverageProfile,
//        Properties
    }

    /// <summary>
    /// The table update action.
    /// </summary>
    public enum UpdateAction
    {
        Insert,
        Update,
        Delete
    }

    /// <summary>
    /// The state of a session.
    /// </summary>
    public enum SessionState
    {
        /// <summary>
        /// The session is disconnected. Call Connect is this state.
        /// </summary>
        Disconnected,
        /// <summary>
        /// The session is connected. Call operations methods in this state.
        /// </summary>
        Connected,
        /// <summary>
        /// The session is reconnecting. Only Close may be called in this state.
        /// </summary>
        Reconnecting
    }

    #region Delegates

    /// <summary>
    /// The delegate used to notify about price update.
    /// It's called in some internal thread context.
    /// </summary>
    /// <param name="priceUpdate">The price update.</param>
    public delegate void Session_PriceUpdate(PriceUpdate priceUpdate);

    /// <summary>
    /// The delegate used to notify about Offers table update.
    /// It's called in some internal thread context.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="obj">The entity.</param>
    public delegate void Session_OfferUpdate(UpdateAction action, Offer obj);

    /// <summary>
    /// The delegate used to notify about Open Positions table update.
    /// It's called in some internal thread context.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="obj">The entity.</param>
    public delegate void Session_OpenPositionUpdate(UpdateAction action, OpenPosition obj);

    /// <summary>
    /// The delegate used to notify about Closed Positions table update.
    /// It's called in some internal thread context.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="obj">The entity.</param>
    public delegate void Session_ClosedPositionUpdate(UpdateAction action, ClosedPosition obj);

    /// <summary>
    /// The delegate used to notify about Order table update.
    /// It's called in some internal thread context.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="obj">The entity.</param>
    public delegate void Session_OrderUpdate(UpdateAction action, Order obj);

    /// <summary>
    /// The delegate used to notify about Accounts table update.
    /// It's called in some internal thread context.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="obj">The entity.</param>
    public delegate void Session_AccountUpdate(UpdateAction action, Account obj);

    /// <summary>
    /// The delegate used to notify about Summary table update.
    /// It's called in some internal thread context.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="obj">The entity.</param>
    public delegate void Session_SummaryUpdate(UpdateAction action, Summary obj);

    /// <summary>
    /// The delegate used to notify about change of a session state.
    /// </summary>
    /// <param name="oldState">The previous session state.</param>
    /// <param name="newState">The new session state.</param>
    public delegate void Session_StateChanged(SessionState oldState, SessionState newState);

    #endregion

    /// <summary>
    /// The FXCM REST API session.
    ///
    /// All methods are synchronous.
    ///
    /// In all entities dates and times are in UTC timezone.
    ///
    /// The class is NOT thread safe. All operations MUST be called in the thread
    /// where the Session instance was created.
    ///
    /// Event callbacks are called in some internal thread context. DO NOT call
    /// class methods from a callback handler.
    /// </summary>
    public class Session
    {
        #region Properties

        /// <summary>
        /// Gets the current session state. You can subscribe for its change using OnStateChanged event.
        /// </summary>
        public SessionState State
        {
            get
            {
                return mSessionState;
            }
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="accessToken">access token for your FXCM account. To create an access token
        ///     visit https://tradingstation.fxcm.com/ </param>
        /// <param name="url">The server host. Use https://api-demo.fxcm.com for Demo accounts and
        ///     https://api.fxcm.com for Real ones.</param>
        public Session(string accessToken, string host)
        {
            mAccessToken = accessToken;
            mHost = host;

            mSessionState = SessionState.Disconnected;
            mEventsLock = new object();

            mSubscribedSymbols = new HashSet<string>();
            mSubscribedTables = new HashSet<TradingTable>();
        }

        /// <summary>
        /// Sets the proxy to be used. Optional.
        /// Note that proxy parameters are global for the application.
        /// Call this method before creating the Session instance.
        /// </summary>
        /// <param name="proxyUrl">The proxy URL. Null to remove the proxy.</param>
        public static void SetProxy(string proxyUrl)
        {
            if (proxyUrl != null)
                WebRequest.DefaultWebProxy = new WebProxy(proxyUrl);
            else
                WebRequest.DefaultWebProxy = null;
        }

        /// <summary>
        /// Opens a connection.
        /// </summary>
        /// <exception cref="InvalidOperationException">If called in an invalid state.</exception>
        /// <exception cref="Exception">If connecting error occurred.</exception>
        public void Connect()
        {
            if (mSessionState != SessionState.Disconnected)
                throw new InvalidOperationException("Bad state");

            // required to use https
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var options = new IO.Options();
            options.Query = new Dictionary<string, string>();
            options.Query["access_token"] = mAccessToken;
            // TODO: you can specify other Socket.IO options here

            var evt = new ManualResetEvent(false);
            string error = null;

            // create Socket.IO connection
            mSocket = IO.Socket(mHost, options);
            mSocket.On(Socket.EVENT_CONNECT, () =>
            {
                Manager mgr = mSocket.Io();
                mSessionId = mgr.EngineSocket.Id;
                mSessionState = SessionState.Connected;

                evt.Set();
            });
            mSocket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
            {
                error = GetErrorText(data);
                mSessionState = SessionState.Disconnected;

                evt.Set();
            });
            mSocket.On(Socket.EVENT_CONNECT_TIMEOUT, (data) =>
            {
                error = "Connect timeout";
                mSessionState = SessionState.Disconnected;

                evt.Set();
            });
            mSocket.On(Socket.EVENT_ERROR, (data) =>
            {
                error = GetErrorText(data);
                mSessionState = SessionState.Disconnected;

                evt.Set();
            });

            // wait for connecting completed
            evt.WaitOne();

            // remove all connect time event listeners
            mSocket.Off();

            if (mSessionState != SessionState.Connected)
                throw new Exception(error ?? "Unknown error");

            InitSocketEventListeners();

            mHttpRequestExecutor = new HttpRequestExecutor(mHost, mAccessToken, mSessionId);
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void Close()
        {
            try
            {
                if (mSocket != null)
                {
                    mSocket.Close();
                    mSocket.Off();

                    mSocket = null;
                }
            }
            catch (Exception)
            {
            }

            mSessionState = SessionState.Disconnected;
        }

        #region Events

        /// <summary>
        /// Event: When the price is updated.
        /// </summary>
        public event Session_PriceUpdate PriceUpdate
        {
            add
            {
                lock (mEventsLock)
                {
                    mPriceUpdate += value;
                }
            }
            remove
            {
                lock (mEventsLock)
                {
                    mPriceUpdate -= value;
                }
            }
        }
        private Session_PriceUpdate mPriceUpdate;

        /// <summary>
        /// Event: When the Offer stream sends new data for the offer.
        /// </summary>
        public event Session_OfferUpdate OfferUpdate
        {
            add
            {
                lock (mEventsLock)
                {
                    mOfferUpdate += value;
                }
            }
            remove
            {
                lock (mEventsLock)
                {
                    mOfferUpdate -= value;
                }
            }
        }
        private Session_OfferUpdate mOfferUpdate;

        /// <summary>
        /// Event: When the Open Positions stream sends new data for the open position.
        /// </summary>
        public event Session_OpenPositionUpdate OpenPositionUpdate
        {
            add
            {
                lock (mEventsLock)
                {
                    mOpenPositionUpdate += value;
                }
            }
            remove
            {
                lock (mEventsLock)
                {
                    mOpenPositionUpdate -= value;
                }
            }
        }
        private Session_OpenPositionUpdate mOpenPositionUpdate;

        /// <summary>
        /// Event: When the Closed Positions stream sends new data for the closed position.
        /// </summary>
        public event Session_ClosedPositionUpdate ClosedPositionUpdate
        {
            add
            {
                lock (mEventsLock)
                {
                    mClosedPositionUpdate += value;
                }
            }
            remove
            {
                lock (mEventsLock)
                {
                    mClosedPositionUpdate -= value;
                }
            }
        }
        private Session_ClosedPositionUpdate mClosedPositionUpdate;

        /// <summary>
        /// Event: When the Order stream sends new data for the order.
        /// </summary>
        public event Session_OrderUpdate OrderUpdate
        {
            add
            {
                lock (mEventsLock)
                {
                    mOrderUpdate += value;
                }
            }
            remove
            {
                lock (mEventsLock)
                {
                    mOrderUpdate -= value;
                }
            }
        }
        private Session_OrderUpdate mOrderUpdate;

        /// <summary>
        /// Event: When the Account stream sends new data for the account.
        /// </summary>
        public event Session_AccountUpdate AccountUpdate
        {
            add
            {
                lock (mEventsLock)
                {
                    mAccountUpdate += value;
                }
            }
            remove
            {
                lock (mEventsLock)
                {
                    mAccountUpdate -= value;
                }
            }
        }
        private Session_AccountUpdate mAccountUpdate;

        /// <summary>
        /// Event: When the Summary stream sends new data for the summary.
        /// </summary>
        public event Session_SummaryUpdate SummaryUpdate
        {
            add
            {
                lock (mEventsLock)
                {
                    mSummaryUpdate += value;
                }
            }
            remove
            {
                lock (mEventsLock)
                {
                    mSummaryUpdate -= value;
                }
            }
        }
        private Session_SummaryUpdate mSummaryUpdate;

        /// <summary>
        /// Event: When the session state is changed.
        /// </summary>
        public event Session_StateChanged StateChanged
        {
            add
            {
                lock (mEventsLock)
                {
                    mStateChanged += value;
                }
            }
            remove
            {
                lock (mEventsLock)
                {
                    mStateChanged -= value;
                }
            }
        }
        private Session_StateChanged mStateChanged;

        #endregion

        #region Market Data

        /// <summary>
        /// Requests a list of all available symbols.
        /// </summary>
        /// <returns>The list of instruments.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public IList<Instrument> GetInstruments()
        {
            CheckSessionState();

            var response = mHttpRequestExecutor.Execute<Json.GetInstrumentsResponse>(
                "GET", "/trading/get_instruments");

            return response.ToEntity();
        }

        /// <summary>
        /// Subscribes to Market Data stream.
        /// After subscribing, market price updates will be pushed to the client via OnPriceUpdate event.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>The last price update.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public PriceUpdate SubscribeSymbol(string symbol)
        {
            CheckSessionState();

            return SubscribeSymbolImpl(symbol);
        }

        private PriceUpdate SubscribeSymbolImpl(string symbol)
        {
            if (symbol == null)
                throw new NullReferenceException("Symbol is null");

            var parameters = new NameValueCollection();
            parameters.Add("pairs", symbol);
            var response = mHttpRequestExecutor.Execute<Json.SubscribeSymbolResponse>(
                "POST", "/subscribe", parameters);

            PriceUpdate priceUpdate = response.ToEntity();

            if (!mSubscribedSymbols.Contains(symbol))
            {
                mSocket.On(symbol, OnPriceUpdate);
                mSubscribedSymbols.Add(symbol);
            }

            return priceUpdate;
        }

        /// <summary>
        /// Unsubscribe from Market Data stream
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <exception cref="Exception">If an error occurred.</exception>
        public void UnsubscribeSymbol(string symbol)
        {
            CheckSessionState();
            if (symbol == null)
                throw new NullReferenceException("Symbol is null");

            var parameters = new NameValueCollection();
            parameters.Add("pairs", symbol);
            var response = mHttpRequestExecutor.Execute<Json.UnsubscribeSymbolResponse>(
                "POST", "/unsubscribe", parameters);

            mSubscribedSymbols.Remove(symbol);
            mSocket.Off(symbol);

            response.CheckExecuted();
        }

        #endregion

        #region Trading Tables

        /// <summary>
        /// Subscribes to the updates of the data models.
        /// Update will be pushed to client via the appropriate event.
        /// </summary>
        /// <param name="table">The table type.</param>
        /// <exception cref="Exception">If an error occurred.</exception>
        public void Subscribe(TradingTable table)
        {
            CheckSessionState();

            SubscribeImpl(table);
        }

        private void SubscribeImpl(TradingTable table)
        {
            string tableParam = ToParameterValue(table);

            var parameters = new NameValueCollection();
            parameters.Add("models", tableParam);
            var response = mHttpRequestExecutor.Execute<Json.BaseResponse>(
                "POST", "/trading/subscribe", parameters);

            response.CheckExecuted();

            if (!mSubscribedTables.Contains(table))
            {
                mSocket.On(tableParam, GetTableUpdateHandler(table));
                mSubscribedTables.Add(table);
            }
        }

        /// <summary>
        /// Unsubscribes from the updates of the data models that are being pushed via the event.
        /// </summary>
        /// <param name="table">The table type.</param>
        /// <exception cref="Exception">If an error occurred.</exception>
        public void Unsubscribe(TradingTable table)
        {
            CheckSessionState();
            string tableParam = ToParameterValue(table);

            var parameters = new NameValueCollection();
            parameters.Add("models", tableParam);
            var response = mHttpRequestExecutor.Execute<Json.BaseResponse>(
                "POST", "/trading/unsubscribe", parameters);

            response.CheckExecuted();

            mSubscribedTables.Remove(table);
            mSocket.Off(tableParam);
        }

        /// <summary>
        /// Changing symbols subscribed to in Offers table.
        /// Offers table will show only symbols that we have subscribed to using UpdateSubscriptions.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="visible">Should the symbol be visible in Offers table.</param>
        /// <exception cref="Exception">If an error occurred.</exception>
        public void UpdateSubscriptions(string symbol, bool visible)
        {
            CheckSessionState();

            var parameters = new NameValueCollection();
            parameters.Add("symbol", symbol);
            parameters.Add("visible", ToParameterValue(visible));
            var response = mHttpRequestExecutor.Execute<Json.BaseResponse>(
                "POST", "/trading/update_subscriptions", parameters);

            response.CheckExecuted();
        }

        /// <summary>
        /// Returns the snapshot of the Offers table.
        /// </summary>
        /// <returns>The table snapshot.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public IList<Offer> GetOffers()
        {
            CheckSessionState();

            var parameters = new NameValueCollection();
            parameters.Add("models", "Offer");
            var response = mHttpRequestExecutor.Execute<Json.GetModelResponse>(
                "GET", "/trading/get_model", parameters);

            Json.GetModelResponse.Models models = response.ToEntity();
            return models.Offers;
        }

        /// <summary>
        /// Returns the snapshot of the Open Positions table.
        /// </summary>
        /// <returns>The table snapshot.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public IList<OpenPosition> GetOpenPositions()
        {
            CheckSessionState();

            var parameters = new NameValueCollection();
            parameters.Add("models", "OpenPosition");
            var response = mHttpRequestExecutor.Execute<Json.GetModelResponse>(
                "GET", "/trading/get_model", parameters);

            Json.GetModelResponse.Models models = response.ToEntity();
            return models.OpenPositions;
        }

        /// <summary>
        /// Returns the snapshot of the Closed Positions table.
        /// </summary>
        /// <returns>The table snapshot.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public IList<ClosedPosition> GetClosedPositions()
        {
            CheckSessionState();

            var parameters = new NameValueCollection();
            parameters.Add("models", "ClosedPosition");
            var response = mHttpRequestExecutor.Execute<Json.GetModelResponse>(
                "GET", "/trading/get_model", parameters);

            Json.GetModelResponse.Models models = response.ToEntity();
            return models.ClosedPositions;
        }

        /// <summary>
        /// Returns the snapshot of the Orders table.
        /// </summary>
        /// <returns>The table snapshot.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public IList<Order> GetOrders()
        {
            CheckSessionState();

            var parameters = new NameValueCollection();
            parameters.Add("models", "Order");
            var response = mHttpRequestExecutor.Execute<Json.GetModelResponse>(
                "GET", "/trading/get_model", parameters);

            Json.GetModelResponse.Models models = response.ToEntity();
            return models.Orders;
        }

        /// <summary>
        /// Returns the snapshot of the Accounts table.
        /// </summary>
        /// <returns>The table snapshot.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public IList<Account> GetAccounts()
        {
            CheckSessionState();

            var parameters = new NameValueCollection();
            parameters.Add("models", "Account");
            var response = mHttpRequestExecutor.Execute<Json.GetModelResponse>(
                "GET", "/trading/get_model", parameters);

            Json.GetModelResponse.Models models = response.ToEntity();
            return models.Accounts;
        }

        /// <summary>
        /// Returns the snapshot of the Summary table.
        /// </summary>
        /// <returns>The table snapshot.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public IList<Summary> GetSummary()
        {
            CheckSessionState();

            var parameters = new NameValueCollection();
            parameters.Add("models", "Summary");
            var response = mHttpRequestExecutor.Execute<Json.GetModelResponse>(
                "GET", "/trading/get_model", parameters);

            Json.GetModelResponse.Models models = response.ToEntity();
            return models.Summary;
        }

        #endregion

        #region Historical Data

        /// <summary>
        /// Allows user to retrieve candles for a given instrument at a given time frame. If time range is specified,
        /// number of candles parameter is ignored.
        /// </summary>
        /// <param name="offerId">ID of requested symbol.</param>
        /// <param name="timeframe">Requested timeframe. One of:
        ///     m1,m5,m15,m30,H1,H2,H3,H4,H6,H8,D1,W1,M1</param>
        /// <param name="num">The number of candles requested (between 1 and 10000).</param>
        /// <param name="from">Beginning of time range. Optional.</param>
        /// <param name="to">End of time range. Optional.</param>
        /// <returns>The list of candles.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public IList<Candle> GetCandles(int offerId, string timeframe, int num,
            DateTime from = default(DateTime), DateTime to = default(DateTime))
        {
            CheckSessionState();

            var parameters = new NameValueCollection();
            parameters.Add("num", num.ToString(CultureInfo.InvariantCulture));
            if (from != DateTime.MinValue)
            {
                parameters.Add("from", Util.ToUnixTimeSeconds(from.ToUniversalTime()).ToString(CultureInfo.InvariantCulture));
            }

            if (to != DateTime.MinValue)
            {
                parameters.Add("to", Util.ToUnixTimeSeconds(to.ToUniversalTime()).ToString(CultureInfo.InvariantCulture));
            }

            var response = mHttpRequestExecutor.Execute<Json.HistoricalDataResponse>(
                "GET", string.Format(CultureInfo.InvariantCulture, "/candles/{0}/{1}", offerId, timeframe), parameters);

            return response.ToEntity();
        }

        #endregion

        #region Trading Orders

        /// <summary>
        /// This command will request immediate opening of a trade at the best available price.
        /// </summary>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>The order identifier.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public string OpenTrade(OpenTradeParams parameters)
        {
            CheckSessionState();

            var valueCollection = new NameValueCollection();
            if (!string.IsNullOrEmpty(parameters.AccountId))
                valueCollection.Add("account_id", parameters.AccountId);

            if (!string.IsNullOrEmpty(parameters.Symbol))
                valueCollection.Add("symbol", parameters.Symbol);
            valueCollection.Add("is_buy", parameters.IsBuy == true ? "true" : "false");

            if (parameters.Rate != null)
                valueCollection.Add("rate", parameters.Rate.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            valueCollection.Add("amount", parameters.Amount.ToString(CultureInfo.InvariantCulture));

            if (parameters.Stop != null)
                valueCollection.Add("stop", parameters.Stop.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.TrailingStep != null)
                valueCollection.Add("trailing_step", parameters.TrailingStep.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.Limit != null)
                valueCollection.Add("limit", parameters.Limit.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.IsInPips != null)
                valueCollection.Add("is_in_pips", parameters.IsInPips == true ? "true" : "false");

            if (parameters.AtMarket != null)
                valueCollection.Add("at_market", parameters.AtMarket.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(parameters.OrderType))
                valueCollection.Add("order_type", parameters.OrderType);

            if (!string.IsNullOrEmpty(parameters.TimeInForce))
                valueCollection.Add("time_in_force", parameters.TimeInForce);

            var response = mHttpRequestExecutor.Execute<Json.TradingBaseResponse>(
                "POST", "/trading/open_trade", valueCollection);

            response.CheckExecuted();

            return response.Data.OrderId;
        }

        /// <summary>
        /// This command will request immediate closure of a trade at the best available price.
        /// </summary>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>The order identifier.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public string CloseTrade(CloseTradeParams parameters)
        {
            CheckSessionState();

            var valueCollection = new NameValueCollection();
            if (parameters.TradeId != null)
                valueCollection.Add("trade_id", parameters.TradeId);

            if (parameters.Rate != null)
                valueCollection.Add("rate", parameters.Rate.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            valueCollection.Add("amount", parameters.Amount.ToString(CultureInfo.InvariantCulture));

            if (parameters.AtMarket != null)
                valueCollection.Add("at_market", parameters.AtMarket.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(parameters.OrderType))
                valueCollection.Add("order_type", parameters.OrderType);

            if (!string.IsNullOrEmpty(parameters.TimeInForce))
                valueCollection.Add("time_in_force", parameters.TimeInForce);

            var response = mHttpRequestExecutor.Execute<Json.TradingBaseResponse>(
                "POST", "/trading/close_trade", valueCollection);

            response.CheckExecuted();

            return response.Data.OrderId;
        }

        /// <summary>
        /// This command will change an existing order that has not been executed.
        /// </summary>
        /// <param name="parameters">The command parameters.</param>
        /// <exception cref="Exception">If an error occurred.</exception>
        public void ChangeOrder(ChangeOrderParams parameters)
        {
            CheckSessionState();

            var valueCollection = new NameValueCollection();
            if (parameters.OrderId != null)
                valueCollection.Add("order_id", parameters.OrderId);

            if (parameters.Rate != null)
                valueCollection.Add("rate", parameters.Rate.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.Range != null)
                valueCollection.Add("range", parameters.Range.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            valueCollection.Add("amount", parameters.Amount.ToString(CultureInfo.InvariantCulture));

            if (parameters.TrailingStep != null)
                valueCollection.Add("trailing_step", parameters.TrailingStep.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            var response = mHttpRequestExecutor.Execute<Json.BaseResponse>(
                "POST", "/trading/change_order", valueCollection);

            response.CheckExecuted();
        }

        /// <summary>
        /// This command will request the removal of an existing order that has not been executed.
        /// </summary>
        /// <param name="parameters">The command parameters.</param>
        /// <exception cref="Exception">If an error occurred.</exception>
        public void DeleteOrder(DeleteOrderParams parameters)
        {
            CheckSessionState();

            var valueCollection = new NameValueCollection();
            if (parameters.OrderId != null)
                valueCollection.Add("order_id", parameters.OrderId);

            var response = mHttpRequestExecutor.Execute<Json.BaseResponse>(
                "POST", "/trading/delete_order", valueCollection);

            response.CheckExecuted();
        }

        /// <summary>
        /// This command will request the creation of a standing order to be filled when market reaches the requested price.
        /// </summary>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>The order identifier.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public string CreateEntryOrder(CreateEntryOrderParams parameters)
        {
            CheckSessionState();

            var valueCollection = new NameValueCollection();
            if (!string.IsNullOrEmpty(parameters.AccountId))
                valueCollection.Add("account_id", parameters.AccountId);

            if (!string.IsNullOrEmpty(parameters.Symbol))
                valueCollection.Add("symbol", parameters.Symbol);
            valueCollection.Add("is_buy", parameters.IsBuy == true ? "true" : "false");

            if (parameters.Rate != null)
                valueCollection.Add("rate", parameters.Rate.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            valueCollection.Add("amount", parameters.Amount.ToString(CultureInfo.InvariantCulture));

            if (parameters.Stop != null)
                valueCollection.Add("stop", parameters.Stop.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.TrailingStep != null)
                valueCollection.Add("trailing_step", parameters.TrailingStep.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.TrailingStopStep != null)
                valueCollection.Add("trailing_stop_step", parameters.TrailingStopStep.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.Limit != null)
                valueCollection.Add("limit", parameters.Limit.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.IsInPips != null)
                valueCollection.Add("is_in_pips", parameters.IsInPips == true ? "true" : "false");

            if (parameters.Range != null)
                valueCollection.Add("range", parameters.Range.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(parameters.OrderType))
                valueCollection.Add("order_type", parameters.OrderType);

            if (!string.IsNullOrEmpty(parameters.TimeInForce))
            {
                valueCollection.Add("time_in_force", parameters.TimeInForce);

                if (parameters.TimeInForce.Equals("GTD") == true)
                {
                    if (parameters.Expiration != null)
                    {
                        DateTime expirationUTC = parameters.Expiration.Value.ToUniversalTime();
                        valueCollection.Add("expiration",
                            expirationUTC.ToString("yyyy-M-dd hh:mm",
                            System.Globalization.CultureInfo.InvariantCulture));
                    }
                }
            }

            var response = mHttpRequestExecutor.Execute<Json.TradingBaseResponse>(
                "POST", "/trading/create_entry_order", valueCollection);

            response.CheckExecuted();

            return response.Data.OrderId;
        }

        /// <summary>
        /// This command will request the creation of a pair of orders. Execution of one of the orders will cancel the other one.
        /// </summary>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>The list of order identifiers.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public IList<string> SimpleOCO(SimpleOCOParams parameters)
        {
            CheckSessionState();

            var valueCollection = new NameValueCollection();
            if (!string.IsNullOrEmpty(parameters.AccountId))
                valueCollection.Add("account_id", parameters.AccountId);

            if (!string.IsNullOrEmpty(parameters.Symbol))
                valueCollection.Add("symbol", parameters.Symbol);

            valueCollection.Add("amount", parameters.Amount.ToString(CultureInfo.InvariantCulture));

            if (parameters.IsInPips != null)
                valueCollection.Add("is_in_pips", parameters.IsInPips == true ? "true" : "false");

            if (!string.IsNullOrEmpty(parameters.TimeInForce))
            {
                valueCollection.Add("time_in_force", parameters.TimeInForce);

                if (parameters.TimeInForce.Equals("GTD") == true)
                {
                    if (parameters.Expiration != null)
                    {
                        DateTime expirationUTC = parameters.Expiration.Value.ToUniversalTime();
                        valueCollection.Add("expiration",
                            expirationUTC.ToString("yyyy-M-dd hh:mm",
                            System.Globalization.CultureInfo.InvariantCulture));
                    }
                }
            }

            valueCollection.Add("is_buy", parameters.IsBuy == true ? "true" : "false");

            valueCollection.Add("rate", parameters.Rate.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.Stop != null)
                valueCollection.Add("stop", parameters.Stop.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.TrailingStep != null)
                valueCollection.Add("trailing_step", parameters.TrailingStep.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.TrailingStopStep != null)
                valueCollection.Add("trailing_stop_step", parameters.TrailingStopStep.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.Limit != null)
                valueCollection.Add("limit", parameters.Limit.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.AtMarket != null)
                valueCollection.Add("at_market", parameters.AtMarket.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(parameters.OrderType))
                valueCollection.Add("order_type", parameters.OrderType);

            valueCollection.Add("is_buy2", parameters.IsBuy2 == true ? "true" : "false");

            valueCollection.Add("rate2", parameters.Rate2.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.Stop2 != null)
                valueCollection.Add("stop2", parameters.Stop2.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.TrailingStep2 != null)
                valueCollection.Add("trailing_step2", parameters.TrailingStep2.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.TrailingStopStep2 != null)
                valueCollection.Add("trailing_stop_step2", parameters.TrailingStopStep2.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.Limit2 != null)
                valueCollection.Add("limit2", parameters.Limit2.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

              var response = mHttpRequestExecutor.Execute<Json.TradingSimpleOCOResponse>(
                "POST", "/trading/simple_oco", valueCollection);

            return response.ToOrdersList();
        }

        /// <summary>
        /// This command will request the addition or removal of an existing orders to/from an OCO group.
        /// </summary>
        /// <param name="parameters">The command parameters.</param>
        /// <exception cref="Exception">If an error occurred.</exception>
        public void EditOCO(EditOCOParams parameters)
        {
            CheckSessionState();

            var valueCollection = new NameValueCollection();

            valueCollection.Add("ocoBulkId", parameters.OcoBulkId.ToString(CultureInfo.InvariantCulture));

            if (parameters.AddOrderId?.Count > 0)
            {
                foreach (var orderid in parameters.AddOrderId)
                {
                    if (!string.IsNullOrEmpty(orderid))
                        valueCollection.Add("addOrderIds", orderid);
                }
            }

            if (parameters.RemoveOrderId?.Count > 0)
            {
                foreach (var orderid in parameters.RemoveOrderId)
                {
                    if (!string.IsNullOrEmpty(orderid))
                        valueCollection.Add("removeOrderIds", orderid);
                }
            }

            var response = mHttpRequestExecutor.Execute<Json.BaseResponse>(
              "POST", "/trading/edit_oco", valueCollection);

            response.CheckExecuted();
        }

        /// <summary>
        /// This command will request the change of a stop loss or limit profit order attached to a trade.
        /// </summary>
        /// <param name="parameters">The command parameters.</param>
        /// <exception cref="Exception">If an error occurred.</exception>
        public void ChangeTradeStopLimit(ChangeTradeStopLimitParams parameters)
        {
            CheckSessionState();

            var valueCollection = new NameValueCollection();
            if (parameters.TradeId != null)
                valueCollection.Add("trade_id", parameters.TradeId);

            valueCollection.Add("is_stop", parameters.IsStop == true ? "true" : "false");

            if (parameters.Rate != null)
                valueCollection.Add("rate", parameters.Rate.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            if (parameters.IsInPips != null)
                valueCollection.Add("is_in_pips", parameters.IsInPips == true ? "true" : "false");

            if (parameters.TrailingStep != null)
                valueCollection.Add("trailing_step", parameters.TrailingStep.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            var response = mHttpRequestExecutor.Execute<Json.BaseResponse>(
                "POST", "/trading/change_trade_stop_limit", valueCollection);

            response.CheckExecuted();
        }

        /// <summary>
        /// This command will request the change of a stop loss or limit profit order attached to an entry order.
        /// </summary>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>The list of order identifiers.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public IList<string> ChangeOrderStopLimit(ChangeOrderStopLimitParams parameters)
        {
            CheckSessionState();

            var valueCollection = new NameValueCollection();
            if (parameters.OrderId != null)
                valueCollection.Add("order_id", parameters.OrderId);

            if (parameters.Limit != null)
                valueCollection.Add("limit", parameters.Limit.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            valueCollection.Add("is_limit_in_pips", parameters.IsLimitInPips == true ? "true" : "false");

            if (parameters.Stop != null)
                valueCollection.Add("stop", parameters.Stop.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            valueCollection.Add("is_stop_in_pips", parameters.IsStopInPips== true ? "true" : "false");

            if (parameters.TrailingStep != null)
                valueCollection.Add("trailing_step", parameters.TrailingStep.Value.ToString("G",
                    System.Globalization.CultureInfo.InvariantCulture));

            var response = mHttpRequestExecutor.Execute<Json.ChangeOrderStopLimitResponse>(
                "POST", "/trading/change_order_stop_limit", valueCollection);

            return response.ToOrdersList();
        }

        /// <summary>
        /// This command will request the closure of all positions for a specified security.
        /// </summary>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>The list of order identifiers.</returns>
        /// <exception cref="Exception">If an error occurred.</exception>
        public IList<string> CloseAllForSymbol(CloseAllForSymbolParams parameters)
        {
            CheckSessionState();

            var valueCollection = new NameValueCollection();
            if (!string.IsNullOrEmpty(parameters.AccountId))
                valueCollection.Add("account_id", parameters.AccountId);

            valueCollection.Add("forSymbol", parameters.ForSymbol == true ? "true" : "false");

            if (!string.IsNullOrEmpty(parameters.Symbol))
                valueCollection.Add("symbol", parameters.Symbol);

            if (!string.IsNullOrEmpty(parameters.OrderType))
                valueCollection.Add("order_type", parameters.OrderType);

            if (!string.IsNullOrEmpty(parameters.TimeInForce))
                valueCollection.Add("time_in_force", parameters.TimeInForce);

            var response = mHttpRequestExecutor.Execute<Json.TradingCloseAllForSymbolResponse>(
                "POST", "/trading/close_all_for_symbol", valueCollection);

            return response.ToOrdersList();
        }

        #endregion

        #region Socket.IO Event Handlers

        /// <summary>
        /// Called when Socket.IO starts reconnecting cycle.
        /// </summary>
        /// <param name="data">The reconnect attempt.</param>
        private void OnReconnecting(object data)
        {
            ChangeSessionState(SessionState.Reconnecting);
        }

        /// <summary>
        /// Called when Socket.IO failed to reconnect.
        /// We move to Disconnected state.
        /// </summary>
        private void OnReconnectFailed()
        {
            ChangeSessionState(SessionState.Disconnected);
        }

        /// <summary>
        /// Called when the connection is opened after reconnecting.
        /// </summary>
        private void OnConnect()
        {
            Manager mgr = mSocket.Io();
            mSessionId = mgr.EngineSocket.Id;

            mHttpRequestExecutor = new HttpRequestExecutor(mHost, mAccessToken, mSessionId);

            ResubscribeAllSocketEvents();

            ChangeSessionState(SessionState.Connected);
        }

        /// <summary>
        /// Called when a price update is received.
        /// </summary>
        /// <param name="data">The PriceUpdate Json.</param>
        private void OnPriceUpdate(object data)
        {
            string objJson = data as string;
            if (objJson == null)
                return;

            try
            {
                var obj = JsonConvert.DeserializeObject<Json.PriceUpdate>(objJson);
                PriceUpdate priceUpdate = obj.ToEntity();

                Session_PriceUpdate temp = mPriceUpdate;
                if (temp != null)
                    temp(priceUpdate);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Called when the offer table update is received.
        /// </summary>
        /// <param name="data">The Model Json.</param>
        private void OnOfferUpdate(object data)
        {
            string objJson = data as string;
            if (objJson == null)
                return;

            try
            {
                var obj = JsonConvert.DeserializeObject<Json.Offer>(objJson);
                Offer offer = obj.ToEntity();
                UpdateAction action = obj.Action ?? UpdateAction.Update;

                Session_OfferUpdate temp = mOfferUpdate;
                if (temp != null)
                    temp(action, offer);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Called when the order table update is received.
        /// </summary>
        /// <param name="data">The Model Json.</param>
        private void OnOrderUpdate(object data)
        {
            string objJson = data as string;
            if (objJson == null)
                return;

            try
            {
                var obj = JsonConvert.DeserializeObject<Json.Order>(objJson);
                Order order = obj.ToEntity();
                UpdateAction action = obj.Action ?? UpdateAction.Update;

                Session_OrderUpdate temp = mOrderUpdate;
                if (temp != null)
                    temp(action, order);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Called when the open position table update is received.
        /// </summary>
        /// <param name="data">The Model Json.</param>
        private void OnOpenPositionUpdate(object data)
        {
            string objJson = data as string;
            if (objJson == null)
                return;

            try
            {
                var obj = JsonConvert.DeserializeObject<Json.OpenPosition>(objJson);
                OpenPosition openPosition = obj.ToEntity();
                UpdateAction action = obj.Action ?? UpdateAction.Update;

                Session_OpenPositionUpdate temp = mOpenPositionUpdate;
                if (temp != null)
                    temp(action, openPosition);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Called when the closed position table update is received.
        /// </summary>
        /// <param name="data">The Model Json.</param>
        private void OnClosedPositionUpdate(object data)
        {
            string objJson = data as string;
            if (objJson == null)
                return;

            try
            {
                var obj = JsonConvert.DeserializeObject<Json.ClosedPosition>(objJson);
                ClosedPosition closedPosition = obj.ToEntity();
                UpdateAction action = obj.Action ?? UpdateAction.Update;

                Session_ClosedPositionUpdate temp = mClosedPositionUpdate;
                if (temp != null)
                    temp(action, closedPosition);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Called when the account table update is received.
        /// </summary>
        /// <param name="data">The Model Json.</param>
        private void OnAccountUpdate(object data)
        {
            string objJson = data as string;
            if (objJson == null)
                return;

            try
            {
                var obj = JsonConvert.DeserializeObject<Json.Account>(objJson);
                Account account = obj.ToEntity();
                UpdateAction action = obj.Action ?? UpdateAction.Update;

                Session_AccountUpdate temp = mAccountUpdate;
                if (temp != null)
                    temp(action, account);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Called when the summary table update is received.
        /// </summary>
        /// <param name="data">The Model Json.</param>
        private void OnSummaryUpdate(object data)
        {
            string objJson = data as string;
            if (objJson == null)
                return;

            try
            {
                var obj = JsonConvert.DeserializeObject<Json.Summary>(objJson);
                Summary summary = obj.ToEntity();
                UpdateAction action = obj.Action ?? UpdateAction.Update;

                Session_SummaryUpdate temp = mSummaryUpdate;
                if (temp != null)
                    temp(action, summary);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        #endregion

        #region Implementation

        /// <summary>
        /// Changes the session state and fires OnStateChanged event.
        /// </summary>
        /// <param name="newState">The new state.</param>
        private void ChangeSessionState(SessionState newState)
        {
            if (mSessionState == newState)
                return;

            SessionState oldSessionState = mSessionState;
            mSessionState = newState;

            Session_StateChanged temp = mStateChanged;
            if (temp != null)
                temp(oldSessionState, mSessionState);
        }

        /// <summary>
        /// Returns string representation of an error received from Socket.IO
        /// as a data object.
        /// </summary>
        private static string GetErrorText(object data)
        {
            if (data == null)
                return "Unknown error";

            string s = data as string;
            if (s != null)
                return s;

            Exception e = data as Exception;
            if (e != null)
            {
                s = e.Message;
                if (e.InnerException != null)
                {
                    // if message is "xhr poll error" then actual message is in InnerException;
                    // clear message in this case
                    if (s.Contains("xhr poll error"))
                        s = "";
                    else
                        s += "\n";
                    s += GetErrorText(e.InnerException);
                }
                return s;
            }

            return data.ToString();
        }

        /// <summary>
        /// Adds event listeners of Socket used after connection completed.
        /// </summary>
        private void InitSocketEventListeners()
        {
            mSocket.On(Socket.EVENT_RECONNECTING, OnReconnecting);
            mSocket.On(Socket.EVENT_RECONNECT_FAILED, OnReconnectFailed);
            mSocket.On(Socket.EVENT_CONNECT, OnConnect);
        }

        /// <summary>
        /// Checks that the session is in a valid state for operations.
        /// </summary>
        /// <exception cref="InvalidOperationException">If called in an invalid state.</exception>
        private void CheckSessionState()
        {
            if (mSessionState != SessionState.Connected)
                throw new InvalidOperationException("Bad state");
        }

        /// <summary>
        /// Converts TradingTable to string parameter value.
        /// </summary>
        private string ToParameterValue(TradingTable table)
        {
            switch (table)
            {
                case TradingTable.Offer:
                    return "Offer";
                case TradingTable.OpenPosition:
                    return "OpenPosition";
                case TradingTable.ClosedPosition:
                    return "ClosedPosition";
                case TradingTable.Order:
                    return "Order";
                case TradingTable.Account:
                    return "Account";
                case TradingTable.Summary:
                    return "Summary";
// TODO
//                case TradingTable.LeverageProfile:
//                    return "LeverageProfile";
//                case TradingTable.Properties:
//                    return "Properties";
            }
            throw new Exception("Unknown table type");
        }

        /// <summary>
        /// Converts bool to string parameter value.
        /// </summary>
        private string ToParameterValue(bool value)
        {
            return value.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the handler to process update events of a specified table.
        /// </summary>
        private Action<object> GetTableUpdateHandler(TradingTable table)
        {
            switch (table)
            {
                case TradingTable.Offer:
                    return OnOfferUpdate;
                case TradingTable.OpenPosition:
                    return OnOpenPositionUpdate;
                case TradingTable.ClosedPosition:
                    return OnClosedPositionUpdate;
                case TradingTable.Order:
                    return OnOrderUpdate;
                case TradingTable.Account:
                    return OnAccountUpdate;
                case TradingTable.Summary:
                    return OnSummaryUpdate;
// TODO
//                case TradingTable.LeverageProfile:
//                    return null;
//                case TradingTable.Properties:
//                    return null;
            }
            throw new Exception("Unknown table type");
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void ResubscribeAllSocketEvents()
        {
            HashSet<string> subscribedSymbols = mSubscribedSymbols;
            mSubscribedSymbols = new HashSet<string>();
            foreach (string symbol in subscribedSymbols)
            {
                try
                {
                    mSocket.Off(symbol);
                    SubscribeSymbolImpl(symbol);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            HashSet<TradingTable> subscribedTables = mSubscribedTables;
            mSubscribedTables = new HashSet<TradingTable>();
            foreach (TradingTable table in subscribedTables)
            {
                try
                {
                    mSocket.Off(ToParameterValue(table));
                    SubscribeImpl(table);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

        #endregion

        #region Data members

        /// <summary>
        /// The access token.
        /// </summary>
        private string mAccessToken;

        /// <summary>
        /// The server host.
        /// </summary>
        private string mHost;

        /// <summary>
        /// The current session state.
        /// </summary>
        private SessionState mSessionState;

        /// <summary>
        /// Socket.IO socket.
        /// </summary>
        private Socket mSocket;

        /// <summary>
        /// Socket.IO session id.
        /// </summary>
        private string mSessionId;

        /// <summary>
        /// The lock object which guards access to object's events.
        /// </summary>
        private object mEventsLock;

        /// <summary>
        /// The Http request executor.
        /// </summary>
        private HttpRequestExecutor mHttpRequestExecutor;

        /// <summary>
        /// The list of already subscribed symbols.
        /// </summary>
        private HashSet<string> mSubscribedSymbols;

        /// <summary>
        /// The list of already subscribed tables.
        /// </summary>
        private HashSet<TradingTable> mSubscribedTables;

        #endregion
    }
}
