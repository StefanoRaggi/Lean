/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using FXCMRest;
using NodaTime;
using QuantConnect.Data;
using QuantConnect.Data.Market;
using QuantConnect.Logging;
using QuantConnect.Packets;
using QuantConnect.Securities;

namespace QuantConnect.Brokerages.Fxcm
{
    /// <summary>
    /// FXCM brokerage - implementation of IDataQueueHandler interface
    /// </summary>
    public partial class FxcmBrokerage
    {
        #region IDataQueueHandler implementation

        /// <summary>
        /// Sets the job we're subscribing for
        /// </summary>
        /// <param name="job">Job we're subscribing for</param>
        public void SetJob(LiveNodePacket job)
        {
        }

        /// <summary>
        /// Subscribe to the specified configuration
        /// </summary>
        /// <param name="dataConfig">defines the parameters to subscribe to a data feed</param>
        /// <param name="newDataAvailableHandler">handler to be fired on new data available</param>
        /// <returns>The new enumerator for this subscription request</returns>
        public IEnumerator<BaseData> Subscribe(SubscriptionDataConfig dataConfig, EventHandler newDataAvailableHandler)
        {
            if (!CanSubscribe(dataConfig.Symbol))
            {
                return Enumerable.Empty<BaseData>().GetEnumerator();
            }

            var enumerator = _aggregator.Add(dataConfig, newDataAvailableHandler);
            _subscriptionManager.Subscribe(dataConfig);

            return enumerator;
        }

        /// <summary>
        /// Adds the specified symbols to the subscription
        /// </summary>
        /// <param name="symbols">The symbols to be added keyed by SecurityType</param>
        private bool Subscribe(IEnumerable<Symbol> symbols)
        {
            foreach (var symbol in symbols)
            {
                Log.Trace($"FxcmBrokerage.Subscribe(): {symbol}");

                var brokerageTicker = _symbolMapper.GetBrokerageSymbol(symbol);

                _session.SubscribeSymbol(brokerageTicker);

                // cache exchange time zone for symbol
                DateTimeZone exchangeTimeZone;
                if (!_symbolExchangeTimeZones.TryGetValue(symbol, out exchangeTimeZone))
                {
                    exchangeTimeZone = MarketHoursDatabase.FromDataFolder().GetExchangeHours(Market.FXCM, symbol, symbol.SecurityType).TimeZone;
                    _symbolExchangeTimeZones.Add(symbol, exchangeTimeZone);
                }
            }

            return true;
        }

        /// <summary>
        /// Removes the specified configuration
        /// </summary>
        /// <param name="dataConfig">Subscription config to be removed</param>
        public void Unsubscribe(SubscriptionDataConfig dataConfig)
        {
            _subscriptionManager.Unsubscribe(dataConfig);
            _aggregator.Remove(dataConfig);
        }

        /// <summary>
        /// Removes the specified symbols to the subscription
        /// </summary>
        /// <param name="symbols">The symbols to be removed keyed by SecurityType</param>
        private bool Unsubscribe(IEnumerable<Symbol> symbols)
        {
            foreach (var symbol in symbols)
            {
                Log.Trace($"FxcmBrokerage.Unsubscribe(): {symbol}");

                var brokerageTicker = _symbolMapper.GetBrokerageSymbol(symbol);

                _session.UnsubscribeSymbol(brokerageTicker);
            }

            return true;
        }

        /// <summary>
        /// Returns true if this brokerage supports the specified symbol
        /// </summary>
        private static bool CanSubscribe(Symbol symbol)
        {
            // ignore unsupported security types
            if (symbol.ID.SecurityType != SecurityType.Forex && symbol.ID.SecurityType != SecurityType.Cfd)
            {
                return false;
            }

            // ignore universe symbols
            return !symbol.Value.Contains("-UNIVERSE-");
        }

        private void OnPriceUpdate(PriceUpdate priceUpdate)
        {
            //Log.Trace(priceUpdate.ToString());

            var securityType = _symbolMapper.GetBrokerageSecurityType(priceUpdate.Symbol);
            var symbol = _symbolMapper.GetLeanSymbol(priceUpdate.Symbol, securityType, Market.FXCM);

            // if instrument is subscribed, add ticks to list
            if (_subscriptionManager.IsSubscribed(symbol, TickType.Quote))
            {
                var time = priceUpdate.Updated;

                // live ticks timestamps must be in exchange time zone
                DateTimeZone exchangeTimeZone;
                if (_symbolExchangeTimeZones.TryGetValue(symbol, out exchangeTimeZone))
                {
                    time = time.ConvertFromUtc(exchangeTimeZone);
                }

                var bidPrice = Convert.ToDecimal(priceUpdate.Bid);
                var askPrice = Convert.ToDecimal(priceUpdate.Ask);
                var tick = new Tick(time, symbol, bidPrice, askPrice);

                _aggregator.Update(tick);
            }
        }

        #endregion

    }
}
