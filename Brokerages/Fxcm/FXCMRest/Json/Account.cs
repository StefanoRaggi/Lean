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
    /// Account model Json entity.
    /// </summary>
    public class Account
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
        /// The unique identification number of the account the position is opened on. The number is unique within the database where the account is stored.
        /// </summary>
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        /// <summary>
        /// The amount of funds on the account. This amount does not include floating profit and loss
        /// </summary>
        [JsonProperty("balance")]
        public double? Balance { get; set; }

        /// <summary>
        /// The amount of funds used to maintain all open positions on the account.
        /// </summary>
        [JsonProperty("usdMr")]
        public double? UsdMr { get; set; }

        /// <summary>
        /// The limitation state of the account. Each state defines the operations that can be performed on the account. The possible values areY - Margin call (all positions are liquidated, new positions cannot be opened).W - Warning of a possible margin call (positions may be closed, new positions cannot be opened).Q - Equity stop (all positions are liquidated, new positions cannot be opened up to the end of the trading day).A - Equity alert (positions may be closed, new positions cannot be opened up to the end of the trading day).N - No limitations (no limitations are imposed on the account operations).
        /// </summary>
        [JsonProperty("mc")]
        public string Mc { get; set; }

        /// <summary>
        /// The unique name of the account the position is opened on. The name is unique within the database where the account is stored.
        /// </summary>
        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        /// <summary>
        /// The amount of funds used to maintain all open positions on the account with the three-level margin policy.
        /// </summary>
        [JsonProperty("usdMr3")]
        public double? UsdMr3 { get; set; }

        /// <summary>
        /// The type of the position maintenance. It defines how trade operations can be performed on the account. The possible values areY - Hedging is allowed. In other words, both buy and sell positions can be opened for the same instrument at the same time. To close each buy or sell position, an individual order is required.N - Hedging is not allowed. In other words, either a buy or a sell position can be opened for the same instrument at a time. Opening a position for the instrument that already has open position(s) of the opposite trade operation always causes closing or partial closing of the open position(s).0 - Netting only. In other words, for each instrument there exists only one open position. The amount of the position is the total amount of the instrument, either bought or sold, that has not yet been offset by opposite trade operations.D - Day netting. In other words, for each instrument there exists only one open position. Same as Netting only, but within a trading day. If the position is not offset during the same trading day it is opened, it is closed automatically on simulated delivery date.F - FIFO. Positions open and close in accordance with the FIFO (First-in, First-out) rule. Hedging is not allowed.
        /// </summary>
        [JsonProperty("hedging")]
        public string Hedging { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("usableMargin3")]
        public double? UsableMargin3 { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("usableMarginPerc")]
        public double? UsableMarginPerc { get; set; }

        /// <summary>
        /// UNKNOWN
        /// </summary>
        [JsonProperty("usableMargin3Perc")]
        public double? UsableMargin3Perc { get; set; }

        /// <summary>
        /// The amount of funds on the account, including profits and losses of all open positions (the floating balance of the account).
        /// </summary>
        [JsonProperty("equity")]
        public double? Equity { get; set; }

        /// <summary>
        /// The amount of funds available to open new positions or to absorb losses of the existing positions.
        /// </summary>
        [JsonProperty("usableMargin")]
        public double? UsableMargin { get; set; }

        /// <summary>
        /// The amount of profits and losses (both floating and realized) of the current trading day.
        /// </summary>
        [JsonProperty("dayPL")]
        public double? DayPL { get; set; }

        /// <summary>
        /// The amount of profits and losses of all open positions on the account.
        /// </summary>
        [JsonProperty("grossPL")]
        public double? GrossPL { get; set; }

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
        public FXCMRest.Models.Account ToEntity()
        {
            var entity = new FXCMRest.Models.Account();
            if (RatePrecision != null)
            {
                entity.RatePrecision = RatePrecision.Value;
                entity.IsRatePrecisionValid = true;
            }
            if (AccountId != null)
            {
                entity.AccountId = AccountId;
                entity.IsAccountIdValid = true;
            }
            if (Balance != null)
            {
                entity.Balance = Balance.Value;
                entity.IsBalanceValid = true;
            }
            if (UsdMr != null)
            {
                entity.UsdMr = UsdMr.Value;
                entity.IsUsdMrValid = true;
            }
            if (Mc != null)
            {
                entity.Mc = Mc;
                entity.IsMcValid = true;
            }
            if (AccountName != null)
            {
                entity.AccountName = AccountName;
                entity.IsAccountNameValid = true;
            }
            if (UsdMr3 != null)
            {
                entity.UsdMr3 = UsdMr3.Value;
                entity.IsUsdMr3Valid = true;
            }
            if (Hedging != null)
            {
                entity.Hedging = Hedging;
                entity.IsHedgingValid = true;
            }
            if (UsableMargin3 != null)
            {
                entity.UsableMargin3 = UsableMargin3.Value;
                entity.IsUsableMargin3Valid = true;
            }
            if (UsableMarginPerc != null)
            {
                entity.UsableMarginPerc = UsableMarginPerc.Value;
                entity.IsUsableMarginPercValid = true;
            }
            if (UsableMargin3Perc != null)
            {
                entity.UsableMargin3Perc = UsableMargin3Perc.Value;
                entity.IsUsableMargin3PercValid = true;
            }
            if (Equity != null)
            {
                entity.Equity = Equity.Value;
                entity.IsEquityValid = true;
            }
            if (UsableMargin != null)
            {
                entity.UsableMargin = UsableMargin.Value;
                entity.IsUsableMarginValid = true;
            }
            if (DayPL != null)
            {
                entity.DayPL = DayPL.Value;
                entity.IsDayPLValid = true;
            }
            if (GrossPL != null)
            {
                entity.GrossPL = GrossPL.Value;
                entity.IsGrossPLValid = true;
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
