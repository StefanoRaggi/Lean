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
    /// The instrument type.
    /// </summary>
    public enum InstrumentType
    {
        Forex = 1,
        Indices = 2,
        Commodity = 3,
        Treasury = 4,
        Bullion = 5,
        Shares = 6,
        FXIndex = 7,
    }

    /// <summary>
    /// The Offer entity.
    /// </summary>
    public class Offer
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
        /// The interest amount added to the account balance for holding a one lot long (buy) position overnight. In the case of FX instruments, lot size is determined by the system base unit size. In the case of CFD instruments, lot size equals to one contract. The interest amount is expressed in the account currency and can be positive or negative.
        /// </summary>
        public double RollB { get; set; }

        /// <summary>
        /// Returns true if RollB is valid.
        /// </summary>
        public bool IsRollBValid { get; set; }

        /// <summary>
        /// The interest amount added to the account balance for holding a one lot short (sell) position overnight. In the case of FX instruments, lot size is determined by the system base unit size. In the case of CFD instruments, lot size equals to one contract. The interest amount is expressed in the account currency and can be positive or negative.
        /// </summary>
        public double RollS { get; set; }

        /// <summary>
        /// Returns true if RollS is valid.
        /// </summary>
        public bool IsRollSValid { get; set; }

        /// <summary>
        /// The price precision of the instrument. It defines number of digits after the decimal point in the instrument price quote.
        /// </summary>
        public int FractionDigits { get; set; }

        /// <summary>
        /// Returns true if FractionDigits is valid.
        /// </summary>
        public bool IsFractionDigitsValid { get; set; }

        /// <summary>
        /// The size of one pip. It used to define the smallest move the instrument can make. In the case of FX instruments, it is expressed in the instrument counter currency. In the case of CFD instruments, it is expressed in the instrument native currency.
        /// </summary>
        public double Pip { get; set; }

        /// <summary>
        /// Returns true if Pip is valid.
        /// </summary>
        public bool IsPipValid { get; set; }

        /// <summary>
        /// Sorting index of the instrument.
        /// </summary>
        public int DefaultSortOrder { get; set; }

        /// <summary>
        /// Returns true if DefaultSortOrder is valid.
        /// </summary>
        public bool IsDefaultSortOrderValid { get; set; }

        /// <summary>
        /// The symbol of the instrument.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Returns true if Currency is valid.
        /// </summary>
        public bool IsCurrencyValid { get; set; }

        /// <summary>
        /// The type of the instrument.
        /// </summary>
        public InstrumentType InstrumentType { get; set; }

        /// <summary>
        /// Returns true if InstrumentType is valid.
        /// </summary>
        public bool IsInstrumentTypeValid { get; set; }

        /// <summary>
        /// The simulated delivery date. The date and time when the position opened in the instrument could be automatically closed. The value of this field is provided in the yyyyMMdd format. It is applicable only when instrument trades on account with the day netting trading. Otherwise, the default DateTime.
        /// </summary>
        public DateTime ValueDate { get; set; }

        /// <summary>
        /// Returns true if ValueDate is valid.
        /// </summary>
        public bool IsValueDateValid { get; set; }

        /// <summary>
        /// The date and time of the last update of the instrument.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Returns true if Time is valid.
        /// </summary>
        public bool IsTimeValid { get; set; }

        /// <summary>
        /// The current market price the instrument can be sold at.
        /// </summary>
        public double Sell { get; set; }

        /// <summary>
        /// Returns true if Sell is valid.
        /// </summary>
        public bool IsSellValid { get; set; }

        /// <summary>
        /// The current market price the instrument can be bought at.
        /// </summary>
        public double Buy { get; set; }

        /// <summary>
        /// Returns true if Buy is valid.
        /// </summary>
        public bool IsBuyValid { get; set; }

        /// <summary>
        /// The usage of the sell price. It defines whether the sell price of the instrument is available for trading or not.
        /// </summary>
        public bool SellTradable { get; set; }

        /// <summary>
        /// Returns true if SellTradable is valid.
        /// </summary>
        public bool IsSellTradableValid { get; set; }

        /// <summary>
        /// The usage of the buy price. It defines whether the buy price of the instrument is available for trading or not.
        /// </summary>
        public bool BuyTradable { get; set; }

        /// <summary>
        /// Returns true if BuyTradable is valid.
        /// </summary>
        public bool IsBuyTradableValid { get; set; }

        /// <summary>
        /// The highest buy price of the instrument for the current trading day.
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// Returns true if High is valid.
        /// </summary>
        public bool IsHighValid { get; set; }

        /// <summary>
        /// The lowest sell price of the instrument for the current trading day.
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// Returns true if Low is valid.
        /// </summary>
        public bool IsLowValid { get; set; }

        /// <summary>
        /// The tick volume of the current minute. The value of this field represents the number of ticks happened during the current minute.
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// Returns true if Volume is valid.
        /// </summary>
        public bool IsVolumeValid { get; set; }

        /// <summary>
        /// Minimum price change for the instrument
        /// </summary>
        public double PipFraction { get; set; }

        /// <summary>
        /// Returns true if PipFraction is valid.
        /// </summary>
        public bool IsPipFractionValid { get; set; }

        /// <summary>
        /// Difference between Buy and Sell price in pips.
        /// </summary>
        public double Spread { get; set; }

        /// <summary>
        /// Returns true if Spread is valid.
        /// </summary>
        public bool IsSpreadValid { get; set; }

        /// <summary>
        /// Maintenance margin level.
        /// </summary>
        public double Mmr { get; set; }

        /// <summary>
        /// Returns true if Mmr is valid.
        /// </summary>
        public bool IsMmrValid { get; set; }

        /// <summary>
        /// Entry margin level.
        /// </summary>
        public double Emr { get; set; }

        /// <summary>
        /// Returns true if Emr is valid.
        /// </summary>
        public bool IsEmrValid { get; set; }

        /// <summary>
        /// Limitation margin level.
        /// </summary>
        public double Lmr { get; set; }

        /// <summary>
        /// Returns true if Lmr is valid.
        /// </summary>
        public bool IsLmrValid { get; set; }

        /// <summary>
        /// The cost of one pip per lot. It is expressed in the account currency and used to calculate the P/L value in the account currency.
        /// </summary>
        public double PipCost { get; set; }

        /// <summary>
        /// Returns true if PipCost is valid.
        /// </summary>
        public bool IsPipCostValid { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Offer: ");
            if (IsRatePrecisionValid)
                sb.Append("RatePrecision: ").Append(RatePrecision).Append(", ");
            if (IsOfferIdValid)
                sb.Append("OfferId: ").Append(OfferId).Append(", ");
            if (IsRollBValid)
                sb.Append("RollB: ").Append(RollB).Append(", ");
            if (IsRollSValid)
                sb.Append("RollS: ").Append(RollS).Append(", ");
            if (IsFractionDigitsValid)
                sb.Append("FractionDigits: ").Append(FractionDigits).Append(", ");
            if (IsPipValid)
                sb.Append("Pip: ").Append(Pip).Append(", ");
            if (IsDefaultSortOrderValid)
                sb.Append("DefaultSortOrder: ").Append(DefaultSortOrder).Append(", ");
            if (IsCurrencyValid)
                sb.Append("Currency: ").Append(Currency).Append(", ");
            if (IsInstrumentTypeValid)
                sb.Append("InstrumentType: ").Append(InstrumentType).Append(", ");
            if (IsValueDateValid)
                sb.Append("ValueDate: ").Append(ValueDate).Append(", ");
            if (IsTimeValid)
                sb.Append("Time: ").Append(Time).Append(", ");
            if (IsSellValid)
                sb.Append("Sell: ").Append(Sell).Append(", ");
            if (IsBuyValid)
                sb.Append("Buy: ").Append(Buy).Append(", ");
            if (IsSellTradableValid)
                sb.Append("SellTradable: ").Append(SellTradable).Append(", ");
            if (IsBuyTradableValid)
                sb.Append("BuyTradable: ").Append(BuyTradable).Append(", ");
            if (IsHighValid)
                sb.Append("High: ").Append(High).Append(", ");
            if (IsLowValid)
                sb.Append("Low: ").Append(Low).Append(", ");
            if (IsVolumeValid)
                sb.Append("Volume: ").Append(Volume).Append(", ");
            if (IsPipFractionValid)
                sb.Append("PipFraction: ").Append(PipFraction).Append(", ");
            if (IsSpreadValid)
                sb.Append("Spread: ").Append(Spread).Append(", ");
            if (IsMmrValid)
                sb.Append("Mmr: ").Append(Mmr).Append(", ");
            if (IsEmrValid)
                sb.Append("Emr: ").Append(Emr).Append(", ");
            if (IsLmrValid)
                sb.Append("Lmr: ").Append(Lmr).Append(", ");
            if (IsPipCostValid)
                sb.Append("PipCost: ").Append(PipCost).Append(", ");
            return sb.ToString();
        }
    }
}
