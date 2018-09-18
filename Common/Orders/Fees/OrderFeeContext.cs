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

using QuantConnect.Securities;

namespace QuantConnect.Orders.Fees
{
    /// <summary>
    /// Defines a context for computing order fees
    /// </summary>
    public class OrderFeeContext
    {
        /// <summary>
        /// The security matching the order
        /// </summary>
        public Security Security { get; }

        /// <summary>
        /// The order to compute fees for
        /// </summary>
        public Order Order { get; }

        /// <summary>
        /// The currency converter to be used for converting to the account currency
        /// </summary>
        public ICurrencyConverter CurrencyConverter { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderFeeContext"/> class
        /// </summary>
        /// <param name="security">The security matching the order</param>
        /// <param name="order">The order to compute fees for</param>
        /// <param name="currencyConverter">The currency converter to be used for converting to the account currency</param>
        public OrderFeeContext(Security security, Order order, ICurrencyConverter currencyConverter)
        {
            Security = security;
            Order = order;
            CurrencyConverter = currencyConverter;
        }
    }
}