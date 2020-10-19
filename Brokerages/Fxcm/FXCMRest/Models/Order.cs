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

namespace FXCMRest.Models
{
    /// <summary>
    /// The Order entity.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The price precision of the instrument. It defines number of digits after the decimal point in the instrument price quote.
        /// </summary>
        public int RatePrecision { get; set; }

        /// <summary>
        /// Returns true if RatePrecision is valid.
        /// </summary>
        public bool IsRatePrecisionValid { get; set; }

        /// <summary>
        /// The unique identification number of the order. The number is unique within the same database that stores the account the order is placed on.
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Returns true if OrderId is valid.
        /// </summary>
        public bool IsOrderIdValid { get; set; }

        /// <summary>
        /// The time when the order was created
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Returns true if Time is valid.
        /// </summary>
        public bool IsTimeValid { get; set; }

        /// <summary>
        /// The unique name of the account the position is opened on. The name is unique within the database where the account is stored.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Returns true if AccountName is valid.
        /// </summary>
        public bool IsAccountNameValid { get; set; }

        /// <summary>
        /// The unique identification number of the account the position is opened on. The number is unique within the database where the account is stored.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Returns true if AccountId is valid.
        /// </summary>
        public bool IsAccountIdValid { get; set; }

        /// <summary>
        /// The time-in-force option of the order. The possible values are:
        /// GTC - Good Till Cancelled
        /// IOC - Immediate Or Cancel
        /// FOK - Fill Or Kill
        /// DAY - Day Order
        /// GTD - Good Till Date
        /// </summary>
        public string TimeInForce { get; set; }

        /// <summary>
        /// Returns true if TimeInForce is valid.
        /// </summary>
        public bool IsTimeInForceValid { get; set; }

        /// <summary>
        /// The symbol of the instrument.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Returns true if Currency is valid.
        /// </summary>
        public bool IsCurrencyValid { get; set; }

        /// <summary>
        /// The trade operation the position is opened by.
        /// </summary>
        public bool IsBuy { get; set; }

        /// <summary>
        /// Returns true if IsBuy is valid.
        /// </summary>
        public bool IsIsBuyValid { get; set; }

        /// <summary>
        /// The price the order is placed at.
        /// </summary>
        public double Buy { get; set; }

        /// <summary>
        /// Returns true if Buy is valid.
        /// </summary>
        public bool IsBuyValid { get; set; }

        /// <summary>
        /// The price the order is placed at.
        /// </summary>
        public double Sell { get; set; }

        /// <summary>
        /// Returns true if Sell is valid.
        /// </summary>
        public bool IsSellValid { get; set; }

        /// <summary>
        /// The order type. The possible values are:
        /// S - Stop
        /// ST - Trailing Stop
        /// L - Limit
        /// SE - Entry Stop
        /// LE - Entry Limit
        /// STE - Trailing Entry Stop
        /// LTE - Trailing Entry Limit
        /// C - Close
        /// CM - Close Market
        /// CR - Close Range
        /// O - Open
        /// OM - Open Market
        /// OR - Open Range
        /// M - Margin Call
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Returns true if Type is valid.
        /// </summary>
        public bool IsTypeValid { get; set; }

        /// <summary>
        /// The state of the order. The possible values are
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Returns true if Status is valid.
        /// </summary>
        public bool IsStatusValid { get; set; }

        /// <summary>
        /// The amount of the position in thousand units.
        /// </summary>
        public int AmountK { get; set; }

        /// <summary>
        /// Returns true if AmountK is valid.
        /// </summary>
        public bool IsAmountKValid { get; set; }

        /// <summary>
        /// unknown
        /// </summary>
        public double CurrencyPoint { get; set; }

        /// <summary>
        /// Returns true if CurrencyPoint is valid.
        /// </summary>
        public bool IsCurrencyPointValid { get; set; }

        /// <summary>
        /// The number of pips the market should move before the stop order moves the same number of pips after it. If the trailing order is dynamic (automatically updates every 0.1 of a pip), then the value of this field is 1. If the order is not trailing, the value of this field is 0.
        /// </summary>
        public int StopMove { get; set; }

        /// <summary>
        /// Returns true if StopMove is valid.
        /// </summary>
        public bool IsStopMoveValid { get; set; }

        /// <summary>
        /// The price of the associated stop order (loss limit level).
        /// </summary>
        public double Stop { get; set; }

        /// <summary>
        /// Returns true if Stop is valid.
        /// </summary>
        public bool IsStopValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public double StopRate { get; set; }

        /// <summary>
        /// Returns true if StopRate is valid.
        /// </summary>
        public bool IsStopRateValid { get; set; }

        /// <summary>
        /// The price of the associated limit order (profit limit level).
        /// </summary>
        public double Limit { get; set; }

        /// <summary>
        /// Returns true if Limit is valid.
        /// </summary>
        public bool IsLimitValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public double LimitRate { get; set; }

        /// <summary>
        /// Returns true if LimitRate is valid.
        /// </summary>
        public bool IsLimitRateValid { get; set; }

        /// <summary>
        /// Indicates if the order is of Entry type (resting order).
        /// </summary>
        public bool IsEntryOrder { get; set; }

        /// <summary>
        /// Returns true if IsEntryOrder is valid.
        /// </summary>
        public bool IsIsEntryOrderValid { get; set; }

        /// <summary>
        /// The unique identifier of an existing OCO group which the order is linked to. The number is unique within the same database that stores the account the contingent order is placed on.
        /// </summary>
        public int OcoBulkId { get; set; }

        /// <summary>
        /// Returns true if OcoBulkId is valid.
        /// </summary>
        public bool IsOcoBulkIdValid { get; set; }

        /// <summary>
        /// Indicates if the order is of Net Amount type.
        /// </summary>
        public bool IsNetQuantity { get; set; }

        /// <summary>
        /// Returns true if IsNetQuantity is valid.
        /// </summary>
        public bool IsIsNetQuantityValid { get; set; }

        /// <summary>
        /// Indicates if the order is of Limit type.
        /// </summary>
        public bool IsLimitOrder { get; set; }

        /// <summary>
        /// Returns true if IsLimitOrder is valid.
        /// </summary>
        public bool IsIsLimitOrderValid { get; set; }

        /// <summary>
        /// Indicates if the order is of Stop type.
        /// </summary>
        public bool IsStopOrder { get; set; }

        /// <summary>
        /// Returns true if IsStopOrder is valid.
        /// </summary>
        public bool IsIsStopOrderValid { get; set; }

        /// <summary>
        /// Indicates if the order is of Entry with Limit and Stop type.
        /// </summary>
        public bool IsELSOrder { get; set; }

        /// <summary>
        /// Returns true if IsELSOrder is valid.
        /// </summary>
        public bool IsIsELSOrderValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public int StopPegBaseType { get; set; }

        /// <summary>
        /// Returns true if StopPegBaseType is valid.
        /// </summary>
        public bool IsStopPegBaseTypeValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public int LimitPegBaseType { get; set; }

        /// <summary>
        /// Returns true if LimitPegBaseType is valid.
        /// </summary>
        public bool IsLimitPegBaseTypeValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public double Range { get; set; }

        /// <summary>
        /// Returns true if Range is valid.
        /// </summary>
        public bool IsRangeValid { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Order: ");
            if (IsRatePrecisionValid)
                sb.Append("RatePrecision: ").Append(RatePrecision).Append(", ");
            if (IsOrderIdValid)
                sb.Append("OrderId: ").Append(OrderId).Append(", ");
            if (IsTimeValid)
                sb.Append("Time: ").Append(Time).Append(", ");
            if (IsAccountNameValid)
                sb.Append("AccountName: ").Append(AccountName).Append(", ");
            if (IsAccountIdValid)
                sb.Append("AccountId: ").Append(AccountId).Append(", ");
            if (IsTimeInForceValid)
                sb.Append("TimeInForce: ").Append(TimeInForce).Append(", ");
            if (IsCurrencyValid)
                sb.Append("Currency: ").Append(Currency).Append(", ");
            if (IsIsBuyValid)
                sb.Append("IsBuy: ").Append(IsBuy).Append(", ");
            if (IsBuyValid)
                sb.Append("Buy: ").Append(Buy).Append(", ");
            if (IsSellValid)
                sb.Append("Sell: ").Append(Sell).Append(", ");
            if (IsTypeValid)
                sb.Append("Type: ").Append(Type).Append(", ");
            if (IsStatusValid)
                sb.Append("Status: ").Append(Status).Append(", ");
            if (IsAmountKValid)
                sb.Append("AmountK: ").Append(AmountK).Append(", ");
            if (IsCurrencyPointValid)
                sb.Append("CurrencyPoint: ").Append(CurrencyPoint).Append(", ");
            if (IsStopMoveValid)
                sb.Append("StopMove: ").Append(StopMove).Append(", ");
            if (IsStopValid)
                sb.Append("Stop: ").Append(Stop).Append(", ");
            if (IsStopRateValid)
                sb.Append("StopRate: ").Append(StopRate).Append(", ");
            if (IsLimitValid)
                sb.Append("Limit: ").Append(Limit).Append(", ");
            if (IsLimitRateValid)
                sb.Append("LimitRate: ").Append(LimitRate).Append(", ");
            if (IsIsEntryOrderValid)
                sb.Append("IsEntryOrder: ").Append(IsEntryOrder).Append(", ");
            if (IsOcoBulkIdValid)
                sb.Append("OcoBulkId: ").Append(OcoBulkId).Append(", ");
            if (IsIsNetQuantityValid)
                sb.Append("IsNetQuantity: ").Append(IsNetQuantity).Append(", ");
            if (IsIsLimitOrderValid)
                sb.Append("IsLimitOrder: ").Append(IsLimitOrder).Append(", ");
            if (IsIsStopOrderValid)
                sb.Append("IsStopOrder: ").Append(IsStopOrder).Append(", ");
            if (IsIsELSOrderValid)
                sb.Append("IsELSOrder: ").Append(IsELSOrder).Append(", ");
            if (IsStopPegBaseTypeValid)
                sb.Append("StopPegBaseType: ").Append(StopPegBaseType).Append(", ");
            if (IsLimitPegBaseTypeValid)
                sb.Append("LimitPegBaseType: ").Append(LimitPegBaseType).Append(", ");
            if (IsRangeValid)
                sb.Append("Range: ").Append(Range).Append(", ");
            return sb.ToString();
        }
    }
}
