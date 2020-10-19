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
using System.Text;
using System.Threading.Tasks;

namespace FXCMRest
{
    /// <summary>
    /// OpenTrade command parameters struct.
    /// </summary>
    public struct OpenTradeParams
    {
        /// <summary>
        /// The trade's account identifier.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// The trade's symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Defines the trade's market side (if true, then buy trade, otherwise sell trade). Temporarily not required by the server and defaults to true but this will change.
        /// </summary>
        public bool IsBuy { get; set; }

        /// <summary>
        /// The trade's rate. Optional.
        /// </summary>
        public double? Rate { get; set; }

        /// <summary>
        /// The trade's amount in lots.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// The trade's stop rate. Optional.
        /// </summary>
        public double? Stop { get; set; }

        /// <summary>
        /// The trailing step for the stop rate. Optional.
        /// </summary>
        public double? TrailingStep { get; set; }

        /// <summary>
        /// The trade's limit rate. Optional.
        /// </summary>
        public double? Limit { get; set; }

        /// <summary>
        /// Defines if the trade's stop/limit rate is in pips. Optional.
        /// </summary>
        public bool? IsInPips { get; set; }

        /// <summary>
        /// Defines the market range. Required if OrderType is set to "MarketRange". Optional.
        /// </summary>
        public double? AtMarket { get; set; }

        /// <summary>
        /// The type of the order execution. Market Order type choices "AtMarket", "MarketRange".
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// Time in force choices "IOC", "GTC", "FOK", "DAY", "GTD".
        /// </summary>
        public string TimeInForce { get; set; }

    }

    /// <summary>
    /// CloseTrade command parameters struct.
    /// </summary>
    public struct CloseTradeParams
    {
        /// <summary>
        /// The trade identifier.
        /// </summary>
        public string TradeId { get; set; }

        /// <summary>
        /// The trade's rate. Optional.
        /// </summary>
        public double? Rate { get; set; }

        /// <summary>
        /// The trade's amount in lots.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Defines the market range. Required if OrderType is set to "MarketRange". Optional.
        /// </summary>
        public double? AtMarket { get; set; }

        /// <summary>
        /// The type of the order execution. Market Order type choices "AtMarket", "MarketRange".
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// Time in force choices "IOC", "GTC", "FOK", "DAY", "GTD".
        /// </summary>
        public string TimeInForce { get; set; }

    }

    /// <summary>
    /// ChangeOrder command parameters struct.
    /// </summary>
    public struct ChangeOrderParams
    {
        /// <summary>
        /// The order identifier.
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// The trade's rate. Optional.
        /// </summary>
        public double? Rate { get; set; }

        /// <summary>
        /// The order's range (is used for "RangeEntry" orders only). Optional.
        /// </summary>
        public double? Range { get; set; }

        /// <summary>
        /// The trade's amount in lots.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// The trailing step for the stop rate. Optional.
        /// </summary>
        public double? TrailingStep { get; set; }

    }

    /// <summary>
    /// DeleteOrder command parameters struct.
    /// </summary>
    public struct DeleteOrderParams
    {
        /// <summary>
        /// The order identifier.
        /// </summary>
        public string OrderId { get; set; }

    }

    /// <summary>
    /// CreateEntryOrder command parameters struct.
    /// </summary>
    public struct CreateEntryOrderParams
    {
        /// <summary>
        /// The trade's account identifier.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// The trade's symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Defines the trade's market side (if true, then buy trade, otherwise sell trade). Temporarily not required by the server and defaults to true but this will change. Optional.
        /// </summary>
        public bool? IsBuy { get; set; }

        /// <summary>
        /// The trade's rate. Optional.
        /// </summary>
        public double? Rate { get; set; }

        /// <summary>
        /// The trade's amount in lots.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// The trade's stop rate. Optional.
        /// </summary>
        public double? Stop { get; set; }

        /// <summary>
        /// The trailing step for the stop rate. Optional.
        /// </summary>
        public double? TrailingStep { get; set; }

        /// <summary>
        /// The trailing step for the order stop rate. Optional.
        /// </summary>
        public double? TrailingStopStep { get; set; }

        /// <summary>
        /// The trade's limit rate. Optional.
        /// </summary>
        public double? Limit { get; set; }

        /// <summary>
        /// Defines if the trade's stop/limit rate is in pips. Optional.
        /// </summary>
        public bool? IsInPips { get; set; }

        /// <summary>
        /// The type of the order execution. Market Order type choices "Entry", "RangeEntry".
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// Time in force choices "GTC", "DAY", "GTD".
        /// </summary>
        public string TimeInForce { get; set; }

        /// <summary>
        /// Defines the market range. Required if OrderType is set to "MarketRange". Optional.
        /// </summary>
        public double? Range { get; set; }

        /// <summary>
        /// The order's expiration date for GTD. Not including time sets the expiration at start of trading day. Optional.
        /// </summary>
        public DateTime? Expiration { get; set; }
    }

    /// <summary>
    /// SimpleOCO command parameters struct.
    /// </summary>
    public struct SimpleOCOParams
    {
        /// <summary>
        /// The trade's account identifier.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// The trade's symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The trade's amount in lots.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Defines if the trade's stop/limit rate is in pips. Optional.
        /// </summary>
        public bool? IsInPips { get; set; }

        /// <summary>
        /// Time in force choices "IOC", "GTC", "FOK", "DAY", "GTD".
        /// </summary>
        public string TimeInForce { get; set; }

        /// <summary>
        /// The order's expiration date. Optional.
        /// </summary>
        public DateTime? Expiration { get; set; }

        /// <summary>
        /// Defines the trade's market side (if true, then buy trade, otherwise sell trade). Temporarily not required by the server and defaults to true but this will change. Optional.
        /// </summary>
        public bool? IsBuy { get; set; }

        /// <summary>
        /// The trade's rate.
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// The trade's stop rate. Optional.
        /// </summary>
        public double? Stop { get; set; }

        /// <summary>
        /// The trailing step for the stop rate. Optional.
        /// </summary>
        public double? TrailingStep { get; set; }

        /// <summary>
        /// The trailing step for the stop rate. Optional.
        /// </summary>
        public int? TrailingStopStep { get; set; }

        /// <summary>
        /// The trade's limit rate. Optional.
        /// </summary>
        public double? Limit { get; set; }

        /// <summary>
        /// Defines the market range. Required if OrderType is set to "MarketRange". Optional.
        /// </summary>
        public double? AtMarket { get; set; }

        /// <summary>
        /// The type of the order execution. Market Order type choices "AtMarket", "MarketRange".
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// Defines the second trade's market side (if true, then buy trade, otherwise sell trade). Optional.
        /// </summary>
        public bool IsBuy2 { get; set; }

        /// <summary>
        /// The second trade's rate.
        /// </summary>
        public double Rate2 { get; set; }

        /// <summary>
        /// The secondtrade's stop rate. Optional.
        /// </summary>
        public double? Stop2 { get; set; }

        /// <summary>
        /// The second trailing step for the stop rate. Optional.
        /// </summary>
        public int? TrailingStep2 { get; set; }

        /// <summary>
        /// The second trailing step for the stop rate. Optional.
        /// </summary>
        public int? TrailingStopStep2 { get; set; }

        /// <summary>
        /// The second trade's limit rate. Optional.
        /// </summary>
        public double? Limit2 { get; set; }

    }

    /// <summary>
    /// EditOCO command parameters struct.
    /// </summary>
    public struct EditOCOParams
    {
        /// <summary>
        /// The oco bulk identifier (if equals zero then new oco order will be created).
        /// </summary>
        public int OcoBulkId { get; set; }

        /// <summary>
        /// The list orders identifiers to add to the oco order.
        /// </summary>
        public IList<string> AddOrderId { get; set; }

        /// <summary>
        /// The list orders identifiers to remove from the oco order.
        /// </summary>
        public IList<string> RemoveOrderId { get; set; }

    }

    /// <summary>
    /// ChangeTradeStopLimit command parameters struct.
    /// </summary>
    public struct ChangeTradeStopLimitParams
    {
        /// <summary>
        /// The trade identifier.
        /// </summary>
        public string TradeId { get; set; }

        /// <summary>
        /// Defines stop or limit should be changed (if true, then stop should be changed, otherwise limit).
        /// </summary>
        public bool IsStop { get; set; }

        /// <summary>
        /// The trade's rate. Optional.
        /// </summary>
        public double? Rate { get; set; }

        /// <summary>
        /// Defines if the trade's stop/limit rate is in pips. Optional.
        /// </summary>
        public bool? IsInPips { get; set; }

        /// <summary>
        /// The trailing step for the stop rate. Optional.
        /// </summary>
        public double? TrailingStep { get; set; }

    }

    /// <summary>
    /// ChangeOrderStopLimit command parameters struct.
    /// </summary>
    public struct ChangeOrderStopLimitParams
    {
        /// <summary>
        /// The order identifier.
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// The trade's limit rate. Optional.
        /// </summary>
        public double? Limit { get; set; }

        /// <summary>
        /// Define if the trade's limit rate is in pips.
        /// </summary>
        public bool IsLimitInPips { get; set; }

        /// <summary>
        /// The trade's stop rate. Optional.
        /// </summary>
        public double? Stop { get; set; }

        /// <summary>
        /// Define if the trade's stop rate is in pips.
        /// </summary>
        public bool IsStopInPips { get; set; }

        /// <summary>
        /// The trailing step for the stop rate. Optional.
        /// </summary>
        public double? TrailingStep { get; set; }
    }

    /// <summary>
    /// CloseAllForSymbol command parameters struct.
    /// </summary>
    public struct CloseAllForSymbolParams
    {
        /// <summary>
        /// The trade's account identifier.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Defines if trades should be closed for the specified symbol.
        /// </summary>
        public bool ForSymbol { get; set; }

        /// <summary>
        /// The trade's symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The type of the order execution. Market Order type choices "AtMarket", "MarketRange".
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// Time in force choices "IOC", "GTC", "FOK", "DAY", "GTD".
        /// </summary>
        public string TimeInForce { get; set; }

    }

}
