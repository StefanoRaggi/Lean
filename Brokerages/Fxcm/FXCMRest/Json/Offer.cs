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
    /// Offer model Json entity.
    /// </summary>
    public class Offer
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
        /// The interest amount added to the account balance for holding a one lot long (buy) position overnight. In the case of FX instruments, lot size is determined by the system base unit size. In the case of CFD instruments, lot size equals to one contract. The interest amount is expressed in the account currency and can be positive or negative.
        /// </summary>
        [JsonProperty("rollB")]
        public double? RollB { get; set; }

        /// <summary>
        /// The interest amount added to the account balance for holding a one lot short (sell) position overnight. In the case of FX instruments, lot size is determined by the system base unit size. In the case of CFD instruments, lot size equals to one contract. The interest amount is expressed in the account currency and can be positive or negative.
        /// </summary>
        [JsonProperty("rollS")]
        public double? RollS { get; set; }

        /// <summary>
        /// The price precision of the instrument. It defines number of digits after the decimal point in the instrument price quote.
        /// </summary>
        [JsonProperty("fractionDigits")]
        public int? FractionDigits { get; set; }

        /// <summary>
        /// The size of one pip. It used to define the smallest move the instrument can make. In the case of FX instruments, it is expressed in the instrument counter currency. In the case of CFD instruments, it is expressed in the instrument native currency.
        /// </summary>
        [JsonProperty("pip")]
        public double? Pip { get; set; }

        /// <summary>
        /// Sorting index of the instrument.
        /// </summary>
        [JsonProperty("defaultSortOrder")]
        public int? DefaultSortOrder { get; set; }

        /// <summary>
        /// The symbol of the instrument.
        /// </summary>
        [JsonProperty("Currency")]
        public string Currency { get; set; }

        /// <summary>
        /// The type of the instrument.
        /// </summary>
        [JsonProperty("instrumentType")]
        public FXCMRest.Models.InstrumentType? InstrumentType { get; set; }

        /// <summary>
        /// The simulated delivery date. The date and time when the position opened in the instrument could be automatically closed. The value of this field is provided in the yyyyMMdd format. It is applicable only when instrument trades on account with the day netting trading. Otherwise, the default DateTime.
        /// </summary>
        [JsonProperty("valueDate")]
        [JsonConverter(typeof(Converters.ValueDateConverter))]
        public DateTime? ValueDate { get; set; }

        /// <summary>
        /// The date and time of the last update of the instrument.
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(Converters.Iso8601TimeConverter))]
        public DateTime? Time { get; set; }

        /// <summary>
        /// The current market price the instrument can be sold at.
        /// </summary>
        [JsonProperty("sell")]
        public double? Sell { get; set; }

        /// <summary>
        /// The current market price the instrument can be bought at.
        /// </summary>
        [JsonProperty("buy")]
        public double? Buy { get; set; }

        /// <summary>
        /// The usage of the sell price. It defines whether the sell price of the instrument is available for trading or not.
        /// </summary>
        [JsonProperty("sellTradable")]
        public bool? SellTradable { get; set; }

        /// <summary>
        /// The usage of the buy price. It defines whether the buy price of the instrument is available for trading or not.
        /// </summary>
        [JsonProperty("buyTradable")]
        public bool? BuyTradable { get; set; }

        /// <summary>
        /// The highest buy price of the instrument for the current trading day.
        /// </summary>
        [JsonProperty("high")]
        public double? High { get; set; }

        /// <summary>
        /// The lowest sell price of the instrument for the current trading day.
        /// </summary>
        [JsonProperty("low")]
        public double? Low { get; set; }

        /// <summary>
        /// The tick volume of the current minute. The value of this field represents the number of ticks happened during the current minute.
        /// </summary>
        [JsonProperty("volume")]
        public double? Volume { get; set; }

        /// <summary>
        /// Minimum price change for the instrument
        /// </summary>
        [JsonProperty("pipFraction")]
        public double? PipFraction { get; set; }

        /// <summary>
        /// Difference between Buy and Sell price in pips.
        /// </summary>
        [JsonProperty("spread")]
        public double? Spread { get; set; }

        /// <summary>
        /// Maintenance margin level.
        /// </summary>
        [JsonProperty("mmr")]
        public double? Mmr { get; set; }

        /// <summary>
        /// Entry margin level.
        /// </summary>
        [JsonProperty("emr")]
        public double? Emr { get; set; }

        /// <summary>
        /// Limitation margin level.
        /// </summary>
        [JsonProperty("lmr")]
        public double? Lmr { get; set; }

        /// <summary>
        /// The cost of one pip per lot. It is expressed in the account currency and used to calculate the P/L value in the account currency.
        /// </summary>
        [JsonProperty("pipCost")]
        public double? PipCost { get; set; }

        /// <summary>
        /// Converts Json object to a model entity.
        /// </summary>
        /// <returns>The entity to return to a user.</returns>
        /// <exception cref="Exception">If a response is an error response.</exception>
        public FXCMRest.Models.Offer ToEntity()
        {
            var entity = new FXCMRest.Models.Offer();
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
            if (RollB != null)
            {
                entity.RollB = RollB.Value;
                entity.IsRollBValid = true;
            }
            if (RollS != null)
            {
                entity.RollS = RollS.Value;
                entity.IsRollSValid = true;
            }
            if (FractionDigits != null)
            {
                entity.FractionDigits = FractionDigits.Value;
                entity.IsFractionDigitsValid = true;
            }
            if (Pip != null)
            {
                entity.Pip = Pip.Value;
                entity.IsPipValid = true;
            }
            if (DefaultSortOrder != null)
            {
                entity.DefaultSortOrder = DefaultSortOrder.Value;
                entity.IsDefaultSortOrderValid = true;
            }
            if (Currency != null)
            {
                entity.Currency = Currency;
                entity.IsCurrencyValid = true;
            }
            if (InstrumentType != null)
            {
                entity.InstrumentType = InstrumentType.Value;
                entity.IsInstrumentTypeValid = true;
            }
            if (ValueDate != null)
            {
                entity.ValueDate = ValueDate.Value;
                entity.IsValueDateValid = true;
            }
            if (Time != null)
            {
                entity.Time = Time.Value;
                entity.IsTimeValid = true;
            }
            if (Sell != null)
            {
                entity.Sell = Sell.Value;
                entity.IsSellValid = true;
            }
            if (Buy != null)
            {
                entity.Buy = Buy.Value;
                entity.IsBuyValid = true;
            }
            if (SellTradable != null)
            {
                entity.SellTradable = SellTradable.Value;
                entity.IsSellTradableValid = true;
            }
            if (BuyTradable != null)
            {
                entity.BuyTradable = BuyTradable.Value;
                entity.IsBuyTradableValid = true;
            }
            if (High != null)
            {
                entity.High = High.Value;
                entity.IsHighValid = true;
            }
            if (Low != null)
            {
                entity.Low = Low.Value;
                entity.IsLowValid = true;
            }
            if (Volume != null)
            {
                entity.Volume = Volume.Value;
                entity.IsVolumeValid = true;
            }
            if (PipFraction != null)
            {
                entity.PipFraction = PipFraction.Value;
                entity.IsPipFractionValid = true;
            }
            if (Spread != null)
            {
                entity.Spread = Spread.Value;
                entity.IsSpreadValid = true;
            }
            if (Mmr != null)
            {
                entity.Mmr = Mmr.Value;
                entity.IsMmrValid = true;
            }
            if (Emr != null)
            {
                entity.Emr = Emr.Value;
                entity.IsEmrValid = true;
            }
            if (Lmr != null)
            {
                entity.Lmr = Lmr.Value;
                entity.IsLmrValid = true;
            }
            if (PipCost != null)
            {
                entity.PipCost = PipCost.Value;
                entity.IsPipCostValid = true;
            }
            return entity;
        }
    }
}
