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
    /// Wrapper class for fee models
    /// </summary>
    public class FeeModelWrapper : BaseFeeModel
    {
        private readonly IFeeModel _feeModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeModelWrapper"/> class
        /// </summary>
        /// <param name="feeModel">The fee model</param>
        public FeeModelWrapper(IFeeModel feeModel)
        {
            _feeModel = feeModel;
        }

        /// <summary>
        /// Gets the order fee associated with the specified order. This returns the cost
        /// of the transaction, including the currency
        /// </summary>
        /// <param name="security">The security matching the order</param>
        /// <param name="order">The order to compute fees for</param>
        /// <returns>The cost of the order, including the currency</returns>
        public override CashAmount GetOrderFee(Security security, Order order)
        {
            var model = _feeModel as IOrderFeeModel;
            return model != null
                ? model.GetOrderFee(security, order)
                : new CashAmount(_feeModel.GetOrderFee(security, order), CashBook.AccountCurrency);
        }
    }
}
