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
    /// The Closed Position entity.
    /// </summary>
    public class ClosedPosition
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
        /// The unique identification number of the open position. The number is unique within the same database that stores the account the position is opened on.
        /// </summary>
        public string TradeId { get; set; }

        /// <summary>
        /// Returns true if TradeId is valid.
        /// </summary>
        public bool IsTradeIdValid { get; set; }

        /// <summary>
        /// The unique name of the account the position is opened on. The name is unique within the database where the account is stored.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Returns true if AccountName is valid.
        /// </summary>
        public bool IsAccountNameValid { get; set; }

        /// <summary>
        /// The cumulative amount of funds that is added the account balance for holding the position overnight.
        /// </summary>
        public double Roll { get; set; }

        /// <summary>
        /// Returns true if Roll is valid.
        /// </summary>
        public bool IsRollValid { get; set; }

        /// <summary>
        /// The amount of funds subtracted from the account balance to pay for the broker's service in accordance with the terms and conditions of the account trading agreement.
        /// </summary>
        public double Com { get; set; }

        /// <summary>
        /// Returns true if Com is valid.
        /// </summary>
        public bool IsComValid { get; set; }

        /// <summary>
        /// The price the position is opened at.
        /// </summary>
        public double Open { get; set; }

        /// <summary>
        /// Returns true if Open is valid.
        /// </summary>
        public bool IsOpenValid { get; set; }

        /// <summary>
        /// The simulated delivery date. The date when the position could be automatically closed. The date is provided in the yyyyMMdd format. It is applicable only for positions opened on accounts with the day netting trading mode. Otherwise, the default DateTime.
        /// </summary>
        public DateTime ValueDate { get; set; }

        /// <summary>
        /// Returns true if ValueDate is valid.
        /// </summary>
        public bool IsValueDateValid { get; set; }

        /// <summary>
        /// The current profit/loss of the position. It is expressed in the account currency.
        /// </summary>
        public double GrossPL { get; set; }

        /// <summary>
        /// Returns true if GrossPL is valid.
        /// </summary>
        public bool IsGrossPLValid { get; set; }

        /// <summary>
        /// The price at which the position can be closed at the moment.
        /// </summary>
        public double Close { get; set; }

        /// <summary>
        /// Returns true if Close is valid.
        /// </summary>
        public bool IsCloseValid { get; set; }

        /// <summary>
        /// The current profit/loss per one lot of the position. It is expressed in the account currency.
        /// </summary>
        public double VisiblePL { get; set; }

        /// <summary>
        /// Returns true if VisiblePL is valid.
        /// </summary>
        public bool IsVisiblePLValid { get; set; }

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
        /// The date and time when the position was opened.
        /// </summary>
        public DateTime OpenTime { get; set; }

        /// <summary>
        /// Returns true if OpenTime is valid.
        /// </summary>
        public bool IsOpenTimeValid { get; set; }

        /// <summary>
        /// The date and time when the position was closed.
        /// </summary>
        public DateTime CloseTime { get; set; }

        /// <summary>
        /// Returns true if CloseTime is valid.
        /// </summary>
        public bool IsCloseTimeValid { get; set; }

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
            StringBuilder sb = new StringBuilder("ClosedPosition: ");
            if (IsRatePrecisionValid)
                sb.Append("RatePrecision: ").Append(RatePrecision).Append(", ");
            if (IsTradeIdValid)
                sb.Append("TradeId: ").Append(TradeId).Append(", ");
            if (IsAccountNameValid)
                sb.Append("AccountName: ").Append(AccountName).Append(", ");
            if (IsRollValid)
                sb.Append("Roll: ").Append(Roll).Append(", ");
            if (IsComValid)
                sb.Append("Com: ").Append(Com).Append(", ");
            if (IsOpenValid)
                sb.Append("Open: ").Append(Open).Append(", ");
            if (IsValueDateValid)
                sb.Append("ValueDate: ").Append(ValueDate).Append(", ");
            if (IsGrossPLValid)
                sb.Append("GrossPL: ").Append(GrossPL).Append(", ");
            if (IsCloseValid)
                sb.Append("Close: ").Append(Close).Append(", ");
            if (IsVisiblePLValid)
                sb.Append("VisiblePL: ").Append(VisiblePL).Append(", ");
            if (IsCurrencyValid)
                sb.Append("Currency: ").Append(Currency).Append(", ");
            if (IsIsBuyValid)
                sb.Append("IsBuy: ").Append(IsBuy).Append(", ");
            if (IsAmountKValid)
                sb.Append("AmountK: ").Append(AmountK).Append(", ");
            if (IsCurrencyPointValid)
                sb.Append("CurrencyPoint: ").Append(CurrencyPoint).Append(", ");
            if (IsOpenTimeValid)
                sb.Append("OpenTime: ").Append(OpenTime).Append(", ");
            if (IsCloseTimeValid)
                sb.Append("CloseTime: ").Append(CloseTime).Append(", ");
            if (IsIsTotalValid)
                sb.Append("IsTotal: ").Append(IsTotal).Append(", ");
            return sb.ToString();
        }
    }
}
