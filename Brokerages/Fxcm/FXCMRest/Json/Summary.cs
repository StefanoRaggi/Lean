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
    /// Summary model Json entity.
    /// </summary>
    public class Summary
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
        /// The unique identification number of the instrument.
        /// </summary>
        [JsonProperty("offerId")]
        public int? OfferId { get; set; }

        /// <summary>
        /// The symbol of the instrument.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// The current profit/loss of all Sell positions. It does not include commissions and interests.
        /// </summary>
        [JsonProperty("plSell")]
        public double? PlSell { get; set; }

        /// <summary>
        /// The sum of amounts of Sell positions in thousand units.
        /// </summary>
        [JsonProperty("amountKSell")]
        public int? AmountKSell { get; set; }

        /// <summary>
        /// The average open price of Sell positions.
        /// </summary>
        [JsonProperty("avgSell")]
        public double? AvgSell { get; set; }

        /// <summary>
        /// The current market price, at which Sell positions can be closed.
        /// </summary>
        [JsonProperty("closeBuy")]
        public double? CloseBuy { get; set; }

        /// <summary>
        /// The current market price, at which Buy positions can be closed.
        /// </summary>
        [JsonProperty("closeSell")]
        public double? CloseSell { get; set; }

        /// <summary>
        /// The average open price of Buy positions.
        /// </summary>
        [JsonProperty("avgBuy")]
        public double? AvgBuy { get; set; }

        /// <summary>
        /// The sum of amounts of Buy positions in thousand units.
        /// </summary>
        [JsonProperty("amountKBuy")]
        public int? AmountKBuy { get; set; }

        /// <summary>
        /// The cumulative amount of funds that is added the account balance for holding the positions overnight.
        /// </summary>
        [JsonProperty("rollSum")]
        public double? RollSum { get; set; }

        /// <summary>
        /// The amount of funds currently committed to maintain Sell positions.
        /// </summary>
        [JsonProperty("usedMarginSell")]
        public double? UsedMarginSell { get; set; }

        /// <summary>
        /// The amount of funds currently committed to maintain Buy positions.
        /// </summary>
        [JsonProperty("usedMarginBuy")]
        public double? UsedMarginBuy { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("isSellDisabled")]
        public bool? IsSellDisabled { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("isBuyDisabled")]
        public bool? IsBuyDisabled { get; set; }

        /// <summary>
        /// The current profit/loss of all Buy positions. It does not include commissions and interests.
        /// </summary>
        [JsonProperty("plBuy")]
        public double? PlBuy { get; set; }

        /// <summary>
        /// The sum of amounts of all positions in thousand units.
        /// </summary>
        [JsonProperty("amountK")]
        public int? AmountK { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("currencyPoint")]
        public double? CurrencyPoint { get; set; }

        /// <summary>
        /// The current profit/loss of all positions. It does not include commissions and interests.
        /// </summary>
        [JsonProperty("grossPL")]
        public double? GrossPL { get; set; }

        /// <summary>
        /// The current profit/loss of all positions. It includes commissions and interests.
        /// </summary>
        [JsonProperty("netPL")]
        public double? NetPL { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("netStop")]
        public double? NetStop { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("netStopMove")]
        public double? NetStopMove { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("netLimit")]
        public double? NetLimit { get; set; }

        /// <summary>
        /// Indicates the row is a summary of for whole table.
        /// </summary>
        [JsonProperty("isTotal")]
        public bool? IsTotal { get; set; }

        /// <summary>
        /// Converts Json object to a model entity.
        /// </summary>
        /// <returns>The entity to return to a user.</returns>
        /// <exception cref="Exception">If a response is an error response.</exception>
        public FXCMRest.Models.Summary ToEntity()
        {
            var entity = new FXCMRest.Models.Summary();
            if (RatePrecision != null)
            {
                entity.RatePrecision = RatePrecision.Value;
                entity.IsRatePrecisionValid = true;
            }
            if (OfferId != null)
            {
                entity.OfferId = OfferId.Value;
                entity.IsOfferIdValid = true;
            }
            if (Currency != null)
            {
                entity.Currency = Currency;
                entity.IsCurrencyValid = true;
            }
            if (PlSell != null)
            {
                entity.PlSell = PlSell.Value;
                entity.IsPlSellValid = true;
            }
            if (AmountKSell != null)
            {
                entity.AmountKSell = AmountKSell.Value;
                entity.IsAmountKSellValid = true;
            }
            if (AvgSell != null)
            {
                entity.AvgSell = AvgSell.Value;
                entity.IsAvgSellValid = true;
            }
            if (CloseBuy != null)
            {
                entity.CloseBuy = CloseBuy.Value;
                entity.IsCloseBuyValid = true;
            }
            if (CloseSell != null)
            {
                entity.CloseSell = CloseSell.Value;
                entity.IsCloseSellValid = true;
            }
            if (AvgBuy != null)
            {
                entity.AvgBuy = AvgBuy.Value;
                entity.IsAvgBuyValid = true;
            }
            if (AmountKBuy != null)
            {
                entity.AmountKBuy = AmountKBuy.Value;
                entity.IsAmountKBuyValid = true;
            }
            if (RollSum != null)
            {
                entity.RollSum = RollSum.Value;
                entity.IsRollSumValid = true;
            }
            if (UsedMarginSell != null)
            {
                entity.UsedMarginSell = UsedMarginSell.Value;
                entity.IsUsedMarginSellValid = true;
            }
            if (UsedMarginBuy != null)
            {
                entity.UsedMarginBuy = UsedMarginBuy.Value;
                entity.IsUsedMarginBuyValid = true;
            }
            if (IsSellDisabled != null)
            {
                entity.IsSellDisabled = IsSellDisabled.Value;
                entity.IsIsSellDisabledValid = true;
            }
            if (IsBuyDisabled != null)
            {
                entity.IsBuyDisabled = IsBuyDisabled.Value;
                entity.IsIsBuyDisabledValid = true;
            }
            if (PlBuy != null)
            {
                entity.PlBuy = PlBuy.Value;
                entity.IsPlBuyValid = true;
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
            if (GrossPL != null)
            {
                entity.GrossPL = GrossPL.Value;
                entity.IsGrossPLValid = true;
            }
            if (NetPL != null)
            {
                entity.NetPL = NetPL.Value;
                entity.IsNetPLValid = true;
            }
            if (NetStop != null)
            {
                entity.NetStop = NetStop.Value;
                entity.IsNetStopValid = true;
            }
            if (NetStopMove != null)
            {
                entity.NetStopMove = NetStopMove.Value;
                entity.IsNetStopMoveValid = true;
            }
            if (NetLimit != null)
            {
                entity.NetLimit = NetLimit.Value;
                entity.IsNetLimitValid = true;
            }
            if (IsTotal != null)
            {
                entity.IsTotal = IsTotal.Value;
                entity.IsIsTotalValid = true;
            }
            return entity;
        }
    }
}
