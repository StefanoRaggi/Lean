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

namespace QuantConnect.Orders.Fees
{
    /// <summary>
    /// Represents a model that simulates order fees
    /// </summary>
    public interface IOrderFeeModel : IFeeModel
    {
        /// <summary>
        /// Gets the order fee associated with the specified order. This returns the cost
        /// of the transaction, including the currency
        /// </summary>
        /// <param name="context">The order fee context instance</param>
        /// <returns>A new <see cref="OrderFee"/> instance</returns>
        OrderFee GetOrderFee(OrderFeeContext context);
    }
}