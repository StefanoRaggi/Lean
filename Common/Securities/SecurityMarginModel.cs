﻿/*
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
using QuantConnect.Logging;
using QuantConnect.Orders;
using QuantConnect.Util;

namespace QuantConnect.Securities
{
    /// <summary>
    /// Represents a simple, constant margining model by specifying the percentages of required margin.
    /// </summary>
    public class SecurityMarginModel : IBuyingPowerModel
    {
        private decimal _initialMarginRequirement;
        private decimal _maintenanceMarginRequirement;
        /// <summary>
        /// The percentage used to determine the required unused buying power for the account.
        /// </summary>
        protected decimal RequiredFreeBuyingPowerPercent;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityMarginModel"/> with no leverage (1x)
        /// </summary>
        public SecurityMarginModel() : this(1m)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityMarginModel"/>
        /// </summary>
        /// <param name="initialMarginRequirement">The percentage of an order's absolute cost
        /// that must be held in free cash in order to place the order</param>
        /// <param name="maintenanceMarginRequirement">The percentage of the holding's absolute
        /// cost that must be held in free cash in order to avoid a margin call</param>
        /// <param name="requiredFreeBuyingPowerPercent">The percentage used to determine the required unused buying power for the account.</param>
        public SecurityMarginModel(decimal initialMarginRequirement, decimal maintenanceMarginRequirement, decimal requiredFreeBuyingPowerPercent)
        {
            if (initialMarginRequirement < 0 || initialMarginRequirement > 1)
            {
                throw new ArgumentException("Initial margin requirement must be between 0 and 1");
            }

            if (maintenanceMarginRequirement < 0 || maintenanceMarginRequirement > 1)
            {
                throw new ArgumentException("Maintenance margin requirement must be between 0 and 1");
            }
            if (requiredFreeBuyingPowerPercent < 0 || requiredFreeBuyingPowerPercent > 1)
            {
                throw new ArgumentException("Free Buying Power Percent requirement must be between 0 and 1");
            }

            _initialMarginRequirement = initialMarginRequirement;
            _maintenanceMarginRequirement = maintenanceMarginRequirement;
            RequiredFreeBuyingPowerPercent = requiredFreeBuyingPowerPercent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityMarginModel"/>
        /// </summary>
        /// <param name="leverage">The leverage</param>
        /// <param name="requiredFreeBuyingPowerPercent">The percentage used to determine the required unused buying power for the account.</param>
        public SecurityMarginModel(decimal leverage, decimal requiredFreeBuyingPowerPercent = 0)
        {
            if (leverage < 1)
            {
                throw new ArgumentException("Leverage must be greater than or equal to 1.");
            }
            if (requiredFreeBuyingPowerPercent < 0 || requiredFreeBuyingPowerPercent > 1)
            {
                throw new ArgumentException("Free Buying Power Percent requirement must be between 0 and 1");
            }

            _initialMarginRequirement = 1 / leverage;
            _maintenanceMarginRequirement = 1 / leverage;
            RequiredFreeBuyingPowerPercent = requiredFreeBuyingPowerPercent;
        }

        /// <summary>
        /// Gets the current leverage of the security
        /// </summary>
        /// <param name="security">The security to get leverage for</param>
        /// <returns>The current leverage in the security</returns>
        public virtual decimal GetLeverage(Security security)
        {
            return 1 / GetMaintenanceMarginRequirement(security);
        }

        /// <summary>
        /// Sets the leverage for the applicable securities, i.e, equities
        /// </summary>
        /// <remarks>
        /// This is added to maintain backwards compatibility with the old margin/leverage system
        /// </remarks>
        /// <param name="security"></param>
        /// <param name="leverage">The new leverage</param>
        public virtual void SetLeverage(Security security, decimal leverage)
        {
            if (leverage < 1)
            {
                throw new ArgumentException("Leverage must be greater than or equal to 1.");
            }

            decimal margin = 1 / leverage;
            _initialMarginRequirement = margin;
            _maintenanceMarginRequirement = margin;
        }

        /// <summary>
        /// Gets the total margin required to execute the specified order in units of the account currency including fees
        /// </summary>
        /// <param name="security">The security to compute initial margin for</param>
        /// <param name="order">The order to be executed</param>
        /// <returns>The total margin in terms of the currency quoted in the order</returns>
        protected virtual decimal GetInitialMarginRequiredForOrder(Security security, Order order)
        {
            //Get the order value from the non-abstract order classes (MarketOrder, LimitOrder, StopMarketOrder)
            //Market order is approximated from the current security price and set in the MarketOrder Method in QCAlgorithm.
            var orderFees = security.GetFeeModel().GetOrderFee(security, order).Amount;

            var orderValue = order.GetValue(security) * GetInitialMarginRequirement(security);
            return orderValue + Math.Sign(orderValue) * orderFees;
        }

        /// <summary>
        /// Gets the margin currently alloted to the specified holding
        /// </summary>
        /// <param name="security">The security to compute maintenance margin for</param>
        /// <returns>The maintenance margin required for the </returns>
        protected virtual decimal GetMaintenanceMargin(Security security)
        {
            return security.Holdings.AbsoluteHoldingsCost * GetMaintenanceMarginRequirement(security);
        }

        /// <summary>
        /// Gets the margin cash available for a trade
        /// </summary>
        /// <param name="portfolio">The algorithm's portfolio</param>
        /// <param name="security">The security to be traded</param>
        /// <param name="direction">The direction of the trade</param>
        /// <returns>The margin available for the trade</returns>
        protected virtual decimal GetMarginRemaining(SecurityPortfolioManager portfolio, Security security, OrderDirection direction)
        {
            var result = portfolio.MarginRemaining;

            if (direction != OrderDirection.Hold)
            {
                var holdings = security.Holdings;
                //If the order is in the same direction as holdings, our remaining cash is our cash
                //In the opposite direction, our remaining cash is 2 x current value of assets + our cash
                if (holdings.IsLong)
                {
                    switch (direction)
                    {
                        case OrderDirection.Sell:
                            result +=
                                // portion of margin to close the existing position
                                GetMaintenanceMargin(security) +
                                // portion of margin to open the new position
                                security.Holdings.AbsoluteHoldingsValue * GetInitialMarginRequirement(security);
                            break;
                    }
                }
                else if (holdings.IsShort)
                {
                    switch (direction)
                    {
                        case OrderDirection.Buy:
                            result +=
                                // portion of margin to close the existing position
                                GetMaintenanceMargin(security) +
                                // portion of margin to open the new position
                                security.Holdings.AbsoluteHoldingsValue * GetInitialMarginRequirement(security);
                            break;
                    }
                }
            }

            result -= portfolio.TotalPortfolioValue * RequiredFreeBuyingPowerPercent;
            return result < 0 ? 0 : result;
        }

        /// <summary>
        /// The percentage of an order's absolute cost that must be held in free cash in order to place the order
        /// </summary>
        protected virtual decimal GetInitialMarginRequirement(Security security)
        {
            return _initialMarginRequirement;
        }

        /// <summary>
        /// The percentage of the holding's absolute cost that must be held in free cash in order to avoid a margin call
        /// </summary>
        public virtual decimal GetMaintenanceMarginRequirement(Security security)
        {
            return _maintenanceMarginRequirement;
        }

        /// <summary>
        /// Check if there is sufficient buying power to execute this order.
        /// </summary>
        /// <param name="portfolio">The algorithm's portfolio</param>
        /// <param name="security">The security to be traded</param>
        /// <param name="order">The order to be checked</param>
        /// <returns>Returns buying power information for an order</returns>
        public HasSufficientBuyingPowerForOrderResult HasSufficientBuyingPowerForOrder(SecurityPortfolioManager portfolio, Security security, Order order)
        {
            // short circuit the div 0 case
            if (order.Quantity == 0)
            {
                return new HasSufficientBuyingPowerForOrderResult(true);
            }

            var ticket = portfolio.Transactions.GetOrderTicket(order.Id);
            if (ticket == null)
            {
                var reason = $"Null order ticket for id: {order.Id}";
                Log.Error($"SecurityMarginModel.HasSufficientBuyingPowerForOrder(): {reason}");
                return new HasSufficientBuyingPowerForOrderResult(false, reason);
            }

            if (order.Type == OrderType.OptionExercise)
            {
                // for option assignment and exercise orders we look into the requirements to process the underlying security transaction
                var option = (Option.Option)security;
                var underlying = option.Underlying;

                if (option.IsAutoExercised(underlying.Close))
                {
                    var quantity = option.GetExerciseQuantity(order.Quantity);

                    var newOrder = new LimitOrder
                    {
                        Id = order.Id,
                        Time = order.Time,
                        LimitPrice = option.StrikePrice,
                        Symbol = underlying.Symbol,
                        Quantity = option.Symbol.ID.OptionRight == OptionRight.Call ? quantity : -quantity
                    };

                    // we continue with this call for underlying
                    return underlying.BuyingPowerModel.HasSufficientBuyingPowerForOrder(portfolio, underlying, newOrder);
                }

                return new HasSufficientBuyingPowerForOrderResult(true);
            }

            // When order only reduces or closes a security position, capital is always sufficient
            if (security.Holdings.Quantity * order.Quantity < 0 && Math.Abs(security.Holdings.Quantity) >= Math.Abs(order.Quantity))
            {
                return new HasSufficientBuyingPowerForOrderResult(true);
            }

            var freeMargin = GetMarginRemaining(portfolio, security, order.Direction);
            var initialMarginRequiredForOrder = GetInitialMarginRequiredForOrder(security, order);

            // pro-rate the initial margin required for order based on how much has already been filled
            var percentUnfilled = (Math.Abs(order.Quantity) - Math.Abs(ticket.QuantityFilled)) / Math.Abs(order.Quantity);
            var initialMarginRequiredForRemainderOfOrder = percentUnfilled * initialMarginRequiredForOrder;

            if (Math.Abs(initialMarginRequiredForRemainderOfOrder) > freeMargin)
            {
                var reason = $"Id: {order.Id}, Initial Margin: {initialMarginRequiredForRemainderOfOrder.Normalize()}, Free Margin: {freeMargin.Normalize()}";
                Log.Error($"SecurityMarginModel.HasSufficientBuyingPowerForOrder(): {reason}");
                return new HasSufficientBuyingPowerForOrderResult(false, reason);
            }

            return new HasSufficientBuyingPowerForOrderResult(true);
        }

        /// <summary>
        /// Get the maximum market order quantity to obtain a position with a given value in account currency. Will not take into account buying power.
        /// </summary>
        /// <param name="portfolio">The algorithm's portfolio</param>
        /// <param name="security">The security to be traded</param>
        /// <param name="target">Target percentage holdings</param>
        /// <returns>Returns the maximum allowed market order quantity and if zero, also the reason</returns>
        public GetMaximumOrderQuantityForTargetValueResult GetMaximumOrderQuantityForTargetValue(SecurityPortfolioManager portfolio, Security security, decimal target)
        {
            // adjust target portfolio value to comply with required Free Buying Power Percent
            var targetPortfolioValue = target * (portfolio.TotalPortfolioValue - portfolio.TotalPortfolioValue * RequiredFreeBuyingPowerPercent);

            // if targeting zero, simply return the negative of the quantity
            if (targetPortfolioValue == 0)
            {
                return new GetMaximumOrderQuantityForTargetValueResult(-security.Holdings.Quantity, string.Empty, false);
            }

            var currentHoldingsValue = security.Holdings.HoldingsValue;

            // remove directionality, we'll work in the land of absolutes
            var targetOrderValue = Math.Abs(targetPortfolioValue - currentHoldingsValue);
            var direction = targetPortfolioValue > currentHoldingsValue ? OrderDirection.Buy : OrderDirection.Sell;

            // determine the unit price in terms of the account currency
            var unitPrice = new MarketOrder(security.Symbol, 1, DateTime.UtcNow).GetValue(security);
            if (unitPrice == 0)
            {
                return new GetMaximumOrderQuantityForTargetValueResult(0, $"The price of the {security.Symbol.Value} security is zero because it does not have any market data yet. When the security price is set this security will be ready for trading.");
            }

            // calculate the total margin available
            var marginRemaining = GetMarginRemaining(portfolio, security, direction);
            if (marginRemaining <= 0)
            {
                return new GetMaximumOrderQuantityForTargetValueResult(0, $"The portfolio does not have enough margin available.");
            }

            // continue iterating while we do not have enough margin for the order
            decimal orderValue = 0;
            decimal orderFees = 0;
            // compute the initial order quantity
            var orderQuantity = targetOrderValue / unitPrice;

            // rounding off Order Quantity to the nearest multiple of Lot Size
            orderQuantity -= orderQuantity % security.SymbolProperties.LotSize;
            if (orderQuantity == 0)
            {
                return new GetMaximumOrderQuantityForTargetValueResult(0, $"The order quantity is less than the lot size of {security.SymbolProperties.LotSize} and has been rounded to zero.", false);
            }

            var loopCount = 0;
            // Just in case...
            var lastOrderQuantity = 0m;
            do
            {
                // Each loop will reduce the order quantity based on the difference between orderValue and targetOrderValue
                if (orderValue > targetOrderValue)
                {
                    var currentOrderValuePerUnit = orderValue / orderQuantity;
                    var amountOfOrdersToRemove = (orderValue - targetOrderValue) / currentOrderValuePerUnit;
                    if (amountOfOrdersToRemove < security.SymbolProperties.LotSize)
                    {
                        // we will always substract at leat 1 LotSize
                        amountOfOrdersToRemove = security.SymbolProperties.LotSize;
                    }
                    orderQuantity -= amountOfOrdersToRemove;
                    orderQuantity -= orderQuantity % security.SymbolProperties.LotSize;
                }

                if (orderQuantity <= 0)
                {
                    return new GetMaximumOrderQuantityForTargetValueResult(0, $"The order quantity is less than the lot size of {security.SymbolProperties.LotSize} and has been rounded to zero." +
                                                                              $"Target order value {targetOrderValue}. Order fees {orderFees}. Order quantity {orderQuantity}.");
                }

                // generate the order
                var order = new MarketOrder(security.Symbol, orderQuantity, DateTime.UtcNow);
                orderFees = security.GetFeeModel().GetOrderFee(security, order).Amount;

                // The TPV, take out the fees(unscaled) => yields available value for trading(less fees) -- then scale that by the target
                // finally remove currentHoldingsValue to get targetOrderValue
                targetOrderValue = Math.Abs((portfolio.TotalPortfolioValue - orderFees - portfolio.TotalPortfolioValue * RequiredFreeBuyingPowerPercent)
                                            * target - currentHoldingsValue);

                // After the first loop we need to recalculate order quantity since now we have fees included
                if (loopCount == 0)
                {
                    // re compute the initial order quantity
                    orderQuantity = targetOrderValue / unitPrice;
                    orderQuantity -= orderQuantity % security.SymbolProperties.LotSize;
                }
                else
                {
                    // Start safe check after first loop
                    if (lastOrderQuantity == orderQuantity)
                    {
                        throw new Exception($"GetMaximumOrderQuantityForTargetValue failed to converge to target order value {targetOrderValue}. " +
                                            $"Current order value is {orderValue}. Order quantity {orderQuantity}. Lot size is " +
                                            $"{security.SymbolProperties.LotSize}. Order fees {orderFees}. Security symbol {security.Symbol}");
                    }
                    lastOrderQuantity = orderQuantity;
                }

                orderValue = orderQuantity * unitPrice;
                loopCount++;
                // we always have to loop at least twice
            } while (loopCount < 2 || orderValue > targetOrderValue);

            // add directionality back in
            return new GetMaximumOrderQuantityForTargetValueResult((direction == OrderDirection.Sell ? -1 : 1) * orderQuantity);
        }

        /// <summary>
        /// Gets the amount of buying power reserved to maintain the specified position
        /// </summary>
        /// <param name="security">The security for the position</param>
        /// <returns>The reserved buying power in account currency</returns>
        public decimal GetReservedBuyingPowerForPosition(Security security)
        {
            return GetMaintenanceMargin(security);
        }

        /// <summary>
        /// Gets the buying power available for a trade
        /// </summary>
        /// <param name="portfolio">The algorithm's portfolio</param>
        /// <param name="security">The security to be traded</param>
        /// <param name="direction">The direction of the trade</param>
        /// <returns>The buying power available for the trade</returns>
        public decimal GetBuyingPower(SecurityPortfolioManager portfolio, Security security, OrderDirection direction)
        {
            return GetMarginRemaining(portfolio, security, direction);
        }
    }
}