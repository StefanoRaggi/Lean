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
    /// The Account entity.
    /// </summary>
    public class Account
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
        /// The unique identification number of the account the position is opened on. The number is unique within the database where the account is stored.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Returns true if AccountId is valid.
        /// </summary>
        public bool IsAccountIdValid { get; set; }

        /// <summary>
        /// The amount of funds on the account. This amount does not include floating profit and loss
        /// </summary>
        public double Balance { get; set; }

        /// <summary>
        /// Returns true if Balance is valid.
        /// </summary>
        public bool IsBalanceValid { get; set; }

        /// <summary>
        /// The amount of funds used to maintain all open positions on the account.
        /// </summary>
        public double UsdMr { get; set; }

        /// <summary>
        /// Returns true if UsdMr is valid.
        /// </summary>
        public bool IsUsdMrValid { get; set; }

        /// <summary>
        /// The limitation state of the account. Each state defines the operations that can be performed on the account. The possible values are:
        /// Y - Margin call (all positions are liquidated, new positions cannot be opened).
        /// W - Warning of a possible margin call (positions may be closed, new positions cannot be opened).
        /// Q - Equity stop (all positions are liquidated, new positions cannot be opened up to the end of the trading day).
        /// A - Equity alert (positions may be closed, new positions cannot be opened up to the end of the trading day).
        /// N - No limitations (no limitations are imposed on the account operations).
        /// </summary>
        public string Mc { get; set; }

        /// <summary>
        /// Returns true if Mc is valid.
        /// </summary>
        public bool IsMcValid { get; set; }

        /// <summary>
        /// The unique name of the account the position is opened on. The name is unique within the database where the account is stored.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Returns true if AccountName is valid.
        /// </summary>
        public bool IsAccountNameValid { get; set; }

        /// <summary>
        /// The amount of funds used to maintain all open positions on the account with the three-level margin policy.
        /// </summary>
        public double UsdMr3 { get; set; }

        /// <summary>
        /// Returns true if UsdMr3 is valid.
        /// </summary>
        public bool IsUsdMr3Valid { get; set; }

        /// <summary>
        /// The type of the position maintenance. It defines how trade operations can be performed on the account. The possible values are:
        /// Y - Hedging is allowed. In other words, both buy and sell positions can be opened for the same instrument at the same time. To close each buy or sell position, an individual order is required.
        /// N - Hedging is not allowed. In other words, either a buy or a sell position can be opened for the same instrument at a time. Opening a position for the instrument that already has open position(s) of the opposite trade operation always causes closing or partial closing of the open position(s).
        /// 0 - Netting only. In other words, for each instrument there exists only one open position. The amount of the position is the total amount of the instrument, either bought or sold, that has not yet been offset by opposite trade operations.
        /// D - Day netting. In other words, for each instrument there exists only one open position. Same as Netting only, but within a trading day. If the position is not offset during the same trading day it is opened, it is closed automatically on simulated delivery date.
        /// F - FIFO. Positions open and close in accordance with the FIFO (First-in, First-out) rule. Hedging is not allowed.
        /// </summary>
        public string Hedging { get; set; }

        /// <summary>
        /// Returns true if Hedging is valid.
        /// </summary>
        public bool IsHedgingValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public double UsableMargin3 { get; set; }

        /// <summary>
        /// Returns true if UsableMargin3 is valid.
        /// </summary>
        public bool IsUsableMargin3Valid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public double UsableMarginPerc { get; set; }

        /// <summary>
        /// Returns true if UsableMarginPerc is valid.
        /// </summary>
        public bool IsUsableMarginPercValid { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        public double UsableMargin3Perc { get; set; }

        /// <summary>
        /// Returns true if UsableMargin3Perc is valid.
        /// </summary>
        public bool IsUsableMargin3PercValid { get; set; }

        /// <summary>
        /// The amount of funds on the account, including profits and losses of all open positions (the floating balance of the account).
        /// </summary>
        public double Equity { get; set; }

        /// <summary>
        /// Returns true if Equity is valid.
        /// </summary>
        public bool IsEquityValid { get; set; }

        /// <summary>
        /// The amount of funds available to open new positions or to absorb losses of the existing positions.
        /// </summary>
        public double UsableMargin { get; set; }

        /// <summary>
        /// Returns true if UsableMargin is valid.
        /// </summary>
        public bool IsUsableMarginValid { get; set; }

        /// <summary>
        /// The amount of profits and losses (both floating and realized) of the current trading day.
        /// </summary>
        public double DayPL { get; set; }

        /// <summary>
        /// Returns true if DayPL is valid.
        /// </summary>
        public bool IsDayPLValid { get; set; }

        /// <summary>
        /// The amount of profits and losses of all open positions on the account.
        /// </summary>
        public double GrossPL { get; set; }

        /// <summary>
        /// Returns true if GrossPL is valid.
        /// </summary>
        public bool IsGrossPLValid { get; set; }

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
            StringBuilder sb = new StringBuilder("Account: ");
            if (IsRatePrecisionValid)
                sb.Append("RatePrecision: ").Append(RatePrecision).Append(", ");
            if (IsAccountIdValid)
                sb.Append("AccountId: ").Append(AccountId).Append(", ");
            if (IsBalanceValid)
                sb.Append("Balance: ").Append(Balance).Append(", ");
            if (IsUsdMrValid)
                sb.Append("UsdMr: ").Append(UsdMr).Append(", ");
            if (IsMcValid)
                sb.Append("Mc: ").Append(Mc).Append(", ");
            if (IsAccountNameValid)
                sb.Append("AccountName: ").Append(AccountName).Append(", ");
            if (IsUsdMr3Valid)
                sb.Append("UsdMr3: ").Append(UsdMr3).Append(", ");
            if (IsHedgingValid)
                sb.Append("Hedging: ").Append(Hedging).Append(", ");
            if (IsUsableMargin3Valid)
                sb.Append("UsableMargin3: ").Append(UsableMargin3).Append(", ");
            if (IsUsableMarginPercValid)
                sb.Append("UsableMarginPerc: ").Append(UsableMarginPerc).Append(", ");
            if (IsUsableMargin3PercValid)
                sb.Append("UsableMargin3Perc: ").Append(UsableMargin3Perc).Append(", ");
            if (IsEquityValid)
                sb.Append("Equity: ").Append(Equity).Append(", ");
            if (IsUsableMarginValid)
                sb.Append("UsableMargin: ").Append(UsableMargin).Append(", ");
            if (IsDayPLValid)
                sb.Append("DayPL: ").Append(DayPL).Append(", ");
            if (IsGrossPLValid)
                sb.Append("GrossPL: ").Append(GrossPL).Append(", ");
            if (IsIsTotalValid)
                sb.Append("IsTotal: ").Append(IsTotal).Append(", ");
            return sb.ToString();
        }
    }
}
