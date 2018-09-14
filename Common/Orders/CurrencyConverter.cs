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

using QuantConnect.Interfaces;
using QuantConnect.Securities;

namespace QuantConnect.Orders
{
    /// <summary>
    /// Provides the ability to convert cash amounts between different currencies
    /// </summary>
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly CashBook _cashBook;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyConverter"/> class
        /// </summary>
        /// <param name="cashBook">The cash book instance</param>
        public CurrencyConverter(CashBook cashBook)
        {
            _cashBook = cashBook;
        }

        /// <summary>
        /// Converts a cash amount between two different currencies
        /// </summary>
        /// <param name="cashAmount">The <see cref="CashAmount"/> instance to convert</param>
        /// <param name="destinationCurrency">The destination currency</param>
        /// <returns>A new <see cref="CashAmount"/> instance denominated in the destination currency</returns>
        public CashAmount Convert(CashAmount cashAmount, string destinationCurrency)
        {
            var amount = _cashBook.Convert(cashAmount.Amount, cashAmount.Currency, destinationCurrency);
            return new CashAmount(amount, destinationCurrency);
        }

        /// <summary>
        /// Converts a cash amount to the account currency
        /// </summary>
        /// <param name="cashAmount">The <see cref="CashAmount"/> instance to convert</param>
        /// <returns>A new <see cref="CashAmount"/> instance denominated in the account currency</returns>
        public CashAmount ConvertToAccountCurrency(CashAmount cashAmount)
        {
            return Convert(cashAmount, CashBook.AccountCurrency);
        }
    }
}
