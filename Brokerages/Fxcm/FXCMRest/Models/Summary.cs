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
    /// The Summary entity.
    /// </summary>
    public class Summary
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
        /// The unique identification number of the instrument.
        /// </summary>
        public int OfferId { get; set; }

        /// <summary>
        /// Returns true if OfferId is valid.
        /// </summary>
        public bool IsOfferIdValid { get; set; }

        /// <summary>
        /// The symbol of the instrument.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Returns true if Currency is valid.
        /// </summary>
        public bool IsCurrencyValid { get; set; }

        /// <summary>
        /// The current profit/loss of all Sell positions. It does not include commissions and interests.
        /// </summary>
        public double PlSell { get; set; }

        /// <summary>
        /// Returns true if PlSell is valid.
        /// </summary>
        public bool IsPlSellValid { get; set; }

        /// <summary>
        /// The sum of amounts of Sell positions in thousand units.
        /// </summary>
        public int AmountKSell { get; set; }

        /// <summary>
        /// Returns true if AmountKSell is valid.
        /// </summary>
        public bool IsAmountKSellValid { get; set; }

        /// <summary>
        /// The average open price of Sell positions.
        /// </summary>
        public double AvgSell { get; set; }

        /// <summary>
        /// Returns true if AvgSell is valid.
        /// </summary>
        public bool IsAvgSellValid { get; set; }

        /// <summary>
        /// The current market price, at which Sell positions can be closed.
        /// </summary>
        public double CloseBuy { get; set; }

        /// <summary>
        /// Returns true if CloseBuy is valid.
        /// </summary>
        public bool IsCloseBuyValid { get; set; }

        /// <summary>
        /// The current market price, at which Buy positions can be closed.
        /// </summary>
        public double CloseSell { get; set; }

        /// <summary>
        /// Returns true if CloseSell is valid.
        /// </summary>
        public bool IsCloseSellValid { get; set; }

        /// <summary>
        /// The average open price of Buy positions.
        /// </summary>
        public double AvgBuy { get; set; }

        /// <summary>
        /// Returns true if AvgBuy is valid.
        /// </summary>
        public bool IsAvgBuyValid { get; set; }

        /// <summary>
        /// The sum of amounts of Buy positions in thousand units.
        /// </summary>
        public int AmountKBuy { get; set; }

        /// <summary>
        /// Returns true if AmountKBuy is valid.
        /// </summary>
        public bool IsAmountKBuyValid { get; set; }

        /// <summary>
        /// The cumulative amount of funds that is added the account balance for holding the positions overnight.
        /// </summary>
        public double RollSum { get; set; }

        /// <summary>
        /// Returns true if RollSum is valid.
        /// </summary>
        public bool IsRollSumValid { get; set; }

        /// <summary>
        /// The amount of funds currently committed to maintain Sell positions.
        /// </summary>
        public double UsedMarginSell { get; set; }

        /// <summary>
        /// Returns true if UsedMarginSell is valid.
        /// </summary>
        public bool IsUsedMarginSellValid { get; set; }

        /// <summary>
        /// The amount of funds currently committed to maintain Buy positions.
        /// </summary>
        public double UsedMarginBuy { get; set; }

        /// <summary>
        /// Returns true if UsedMarginBuy is valid.
        /// </summary>
        public bool IsUsedMarginBuyValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public bool IsSellDisabled { get; set; }

        /// <summary>
        /// Returns true if IsSellDisabled is valid.
        /// </summary>
        public bool IsIsSellDisabledValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public bool IsBuyDisabled { get; set; }

        /// <summary>
        /// Returns true if IsBuyDisabled is valid.
        /// </summary>
        public bool IsIsBuyDisabledValid { get; set; }

        /// <summary>
        /// The current profit/loss of all Buy positions. It does not include commissions and interests.
        /// </summary>
        public double PlBuy { get; set; }

        /// <summary>
        /// Returns true if PlBuy is valid.
        /// </summary>
        public bool IsPlBuyValid { get; set; }

        /// <summary>
        /// The sum of amounts of all positions in thousand units.
        /// </summary>
        public int AmountK { get; set; }

        /// <summary>
        /// Returns true if AmountK is valid.
        /// </summary>
        public bool IsAmountKValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public double CurrencyPoint { get; set; }

        /// <summary>
        /// Returns true if CurrencyPoint is valid.
        /// </summary>
        public bool IsCurrencyPointValid { get; set; }

        /// <summary>
        /// The current profit/loss of all positions. It does not include commissions and interests.
        /// </summary>
        public double GrossPL { get; set; }

        /// <summary>
        /// Returns true if GrossPL is valid.
        /// </summary>
        public bool IsGrossPLValid { get; set; }

        /// <summary>
        /// The current profit/loss of all positions. It includes commissions and interests.
        /// </summary>
        public double NetPL { get; set; }

        /// <summary>
        /// Returns true if NetPL is valid.
        /// </summary>
        public bool IsNetPLValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public double NetStop { get; set; }

        /// <summary>
        /// Returns true if NetStop is valid.
        /// </summary>
        public bool IsNetStopValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public double NetStopMove { get; set; }

        /// <summary>
        /// Returns true if NetStopMove is valid.
        /// </summary>
        public bool IsNetStopMoveValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public double NetLimit { get; set; }

        /// <summary>
        /// Returns true if NetLimit is valid.
        /// </summary>
        public bool IsNetLimitValid { get; set; }

        /// <summary>
        /// Indicates the row is a summary of for whole table.
        /// </summary>
        public bool IsTotal { get; set; }

        /// <summary>
        /// Returns true if IsTotal is valid.
        /// </summary>
        public bool IsIsTotalValid { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Summary: ");
            if (IsRatePrecisionValid)
                sb.Append("RatePrecision: ").Append(RatePrecision).Append(", ");
            if (IsOfferIdValid)
                sb.Append("OfferId: ").Append(OfferId).Append(", ");
            if (IsCurrencyValid)
                sb.Append("Currency: ").Append(Currency).Append(", ");
            if (IsPlSellValid)
                sb.Append("PlSell: ").Append(PlSell).Append(", ");
            if (IsAmountKSellValid)
                sb.Append("AmountKSell: ").Append(AmountKSell).Append(", ");
            if (IsAvgSellValid)
                sb.Append("AvgSell: ").Append(AvgSell).Append(", ");
            if (IsCloseBuyValid)
                sb.Append("CloseBuy: ").Append(CloseBuy).Append(", ");
            if (IsCloseSellValid)
                sb.Append("CloseSell: ").Append(CloseSell).Append(", ");
            if (IsAvgBuyValid)
                sb.Append("AvgBuy: ").Append(AvgBuy).Append(", ");
            if (IsAmountKBuyValid)
                sb.Append("AmountKBuy: ").Append(AmountKBuy).Append(", ");
            if (IsRollSumValid)
                sb.Append("RollSum: ").Append(RollSum).Append(", ");
            if (IsUsedMarginSellValid)
                sb.Append("UsedMarginSell: ").Append(UsedMarginSell).Append(", ");
            if (IsUsedMarginBuyValid)
                sb.Append("UsedMarginBuy: ").Append(UsedMarginBuy).Append(", ");
            if (IsIsSellDisabledValid)
                sb.Append("IsSellDisabled: ").Append(IsSellDisabled).Append(", ");
            if (IsIsBuyDisabledValid)
                sb.Append("IsBuyDisabled: ").Append(IsBuyDisabled).Append(", ");
            if (IsPlBuyValid)
                sb.Append("PlBuy: ").Append(PlBuy).Append(", ");
            if (IsAmountKValid)
                sb.Append("AmountK: ").Append(AmountK).Append(", ");
            if (IsCurrencyPointValid)
                sb.Append("CurrencyPoint: ").Append(CurrencyPoint).Append(", ");
            if (IsGrossPLValid)
                sb.Append("GrossPL: ").Append(GrossPL).Append(", ");
            if (IsNetPLValid)
                sb.Append("NetPL: ").Append(NetPL).Append(", ");
            if (IsNetStopValid)
                sb.Append("NetStop: ").Append(NetStop).Append(", ");
            if (IsNetStopMoveValid)
                sb.Append("NetStopMove: ").Append(NetStopMove).Append(", ");
            if (IsNetLimitValid)
                sb.Append("NetLimit: ").Append(NetLimit).Append(", ");
            if (IsIsTotalValid)
                sb.Append("IsTotal: ").Append(IsTotal).Append(", ");
            return sb.ToString();
        }
    }
}
