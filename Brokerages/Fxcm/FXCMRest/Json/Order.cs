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

using Newtonsoft.Json;

namespace FXCMRest.Json
{
    /// <summary>
    /// Order model Json entity.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The update action. Optional. It's defined only in updates.
        /// </summary>
        [JsonProperty("action")]
        [JsonConverter(typeof(Converters.UpdateActionConverter))]
        public FXCMRest.UpdateAction? Action { get; set; }

        /// <summary>
        /// The price precision of the instrument. It defines number of digits after the decimal point in the instrument price quote.
        /// </summary>
        [JsonProperty("ratePrecision")]
        public int? RatePrecision { get; set; }

        /// <summary>
        /// The unique identification number of the order. The number is unique within the same database that stores the account the order is placed on.
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// The time when the order was created
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(Converters.TimeConverter))]
        public DateTime? Time { get; set; }

        /// <summary>
        /// The unique name of the account the position is opened on. The name is unique within the database where the account is stored.
        /// </summary>
        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        /// <summary>
        /// The unique identification number of the account the position is opened on. The number is unique within the database where the account is stored.
        /// </summary>
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        /// <summary>
        /// The time-in-force option of the order. The possible values are:
        /// GTC - Good Till Cancelled
        /// IOC - Immediate Or Cancel
        /// FOK - Fill Or Kill
        /// DAY - Day Order
        /// GTD - Good Till Date
        /// </summary>
        [JsonProperty("timeInForce")]
        public string TimeInForce { get; set; }

        /// <summary>
        /// The symbol of the instrument.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// The trade operation the position is opened by.
        /// </summary>
        [JsonProperty("isBuy")]
        public bool? IsBuy { get; set; }

        /// <summary>
        /// The price the order is placed at.
        /// </summary>
        [JsonProperty("buy")]
        public double? Buy { get; set; }

        /// <summary>
        /// The price the order is placed at.
        /// </summary>
        [JsonProperty("sell")]
        public double? Sell { get; set; }

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
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The state of the order. The possible values are
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// The amount of the position in thousand units.
        /// </summary>
        [JsonProperty("amountK")]
        public int? AmountK { get; set; }

        /// <summary>
        /// unknown
        /// </summary>
        [JsonProperty("currencyPoint")]
        public double? CurrencyPoint { get; set; }

        /// <summary>
        /// The number of pips the market should move before the stop order moves the same number of pips after it. If the trailing order is dynamic (automatically updates every 0.1 of a pip), then the value of this field is 1. If the order is not trailing, the value of this field is 0.
        /// </summary>
        [JsonProperty("stopMove")]
        public int? StopMove { get; set; }

        /// <summary>
        /// The price of the associated stop order (loss limit level).
        /// </summary>
        [JsonProperty("stop")]
        public double? Stop { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("stopRate")]
        public double? StopRate { get; set; }

        /// <summary>
        /// The price of the associated limit order (profit limit level).
        /// </summary>
        [JsonProperty("limit")]
        public double? Limit { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("limitRate")]
        public double? LimitRate { get; set; }

        /// <summary>
        /// Indicates if the order is of Entry type (resting order).
        /// </summary>
        [JsonProperty("isEntryOrder")]
        public bool? IsEntryOrder { get; set; }

        /// <summary>
        /// The unique identifier of an existing OCO group which the order is linked to. The number is unique within the same database that stores the account the contingent order is placed on.
        /// </summary>
        [JsonProperty("ocoBulkId")]
        public int? OcoBulkId { get; set; }

        /// <summary>
        /// Indicates if the order is of Net Amount type.
        /// </summary>
        [JsonProperty("isNetQuantity")]
        public bool? IsNetQuantity { get; set; }

        /// <summary>
        /// Indicates if the order is of Limit type.
        /// </summary>
        [JsonProperty("isLimitOrder")]
        public bool? IsLimitOrder { get; set; }

        /// <summary>
        /// Indicates if the order is of Stop type.
        /// </summary>
        [JsonProperty("isStopOrder")]
        public bool? IsStopOrder { get; set; }

        /// <summary>
        /// Indicates if the order is of Entry with Limit and Stop type.
        /// </summary>
        [JsonProperty("isELSOrder")]
        public bool? IsELSOrder { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("stopPegBaseType")]
        public int? StopPegBaseType { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("limitPegBaseType")]
        public int? LimitPegBaseType { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("range")]
        public double? Range { get; set; }

        /// <summary>
        /// Converts Json object to a model entity.
        /// </summary>
        /// <returns>The entity to return to a user.</returns>
        /// <exception cref="Exception">If a response is an error response.</exception>
        public FXCMRest.Models.Order ToEntity()
        {
            var entity = new FXCMRest.Models.Order();
            if (RatePrecision != null)
            {
                entity.RatePrecision = RatePrecision.Value;
                entity.IsRatePrecisionValid = true;
            }
            if (OrderId != null)
            {
                entity.OrderId = OrderId;
                entity.IsOrderIdValid = true;
            }
            if (Time != null)
            {
                entity.Time = Time.Value;
                entity.IsTimeValid = true;
            }
            if (AccountName != null)
            {
                entity.AccountName = AccountName;
                entity.IsAccountNameValid = true;
            }
            if (AccountId != null)
            {
                entity.AccountId = AccountId;
                entity.IsAccountIdValid = true;
            }
            if (TimeInForce != null)
            {
                entity.TimeInForce = TimeInForce;
                entity.IsTimeInForceValid = true;
            }
            if (Currency != null)
            {
                entity.Currency = Currency;
                entity.IsCurrencyValid = true;
            }
            if (IsBuy != null)
            {
                entity.IsBuy = IsBuy.Value;
                entity.IsIsBuyValid = true;
            }
            if (Buy != null)
            {
                entity.Buy = Buy.Value;
                entity.IsBuyValid = true;
            }
            if (Sell != null)
            {
                entity.Sell = Sell.Value;
                entity.IsSellValid = true;
            }
            if (Type != null)
            {
                entity.Type = Type;
                entity.IsTypeValid = true;
            }
            if (Status != null)
            {
                entity.Status = Status;
                entity.IsStatusValid = true;
            }
            if (AmountK != null)
            {
                entity.AmountK = AmountK.Value;
                entity.IsAmountKValid = true;
            }
            if (CurrencyPoint != null)
            {
                entity.CurrencyPoint = CurrencyPoint.Value;
                entity.IsCurrencyPointValid = true;
            }
            if (StopMove != null)
            {
                entity.StopMove = StopMove.Value;
                entity.IsStopMoveValid = true;
            }
            if (Stop != null)
            {
                entity.Stop = Stop.Value;
                entity.IsStopValid = true;
            }
            if (StopRate != null)
            {
                entity.StopRate = StopRate.Value;
                entity.IsStopRateValid = true;
            }
            if (Limit != null)
            {
                entity.Limit = Limit.Value;
                entity.IsLimitValid = true;
            }
            if (LimitRate != null)
            {
                entity.LimitRate = LimitRate.Value;
                entity.IsLimitRateValid = true;
            }
            if (IsEntryOrder != null)
            {
                entity.IsEntryOrder = IsEntryOrder.Value;
                entity.IsIsEntryOrderValid = true;
            }
            if (OcoBulkId != null)
            {
                entity.OcoBulkId = OcoBulkId.Value;
                entity.IsOcoBulkIdValid = true;
            }
            if (IsNetQuantity != null)
            {
                entity.IsNetQuantity = IsNetQuantity.Value;
                entity.IsIsNetQuantityValid = true;
            }
            if (IsLimitOrder != null)
            {
                entity.IsLimitOrder = IsLimitOrder.Value;
                entity.IsIsLimitOrderValid = true;
            }
            if (IsStopOrder != null)
            {
                entity.IsStopOrder = IsStopOrder.Value;
                entity.IsIsStopOrderValid = true;
            }
            if (IsELSOrder != null)
            {
                entity.IsELSOrder = IsELSOrder.Value;
                entity.IsIsELSOrderValid = true;
            }
            if (StopPegBaseType != null)
            {
                entity.StopPegBaseType = StopPegBaseType.Value;
                entity.IsStopPegBaseTypeValid = true;
            }
            if (LimitPegBaseType != null)
            {
                entity.LimitPegBaseType = LimitPegBaseType.Value;
                entity.IsLimitPegBaseTypeValid = true;
            }
            if (Range != null)
            {
                entity.Range = Range.Value;
                entity.IsRangeValid = true;
            }
            return entity;
        }
    }
}
