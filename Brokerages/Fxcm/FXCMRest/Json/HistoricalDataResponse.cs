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
    /// HistoricalData response Json entity.
    /// </summary>
    class HistoricalDataResponse : BaseResponse
    {
        /// <summary>
        /// Instrumentid. Integer from Offer trading table
        /// </summary>
        [JsonProperty("instrument_id")]
        public string Instrumentid{ get; set; }

        /// <summary>
        /// Timeframe of the candles
        /// </summary>
        [JsonProperty("period_id")]
        public string Periodid { get; set; }

        /// <summary>
        /// Candles information.
        /// </summary>
        [JsonProperty("candles")]
        public List<List<double>> CandlesData { get; set; }

        /// <summary>
        /// Converts Json response object to a model entity.
        /// </summary>
        /// <returns>The entity to return to a user.</returns>
        /// <exception cref="Exception">If a response is an error response.</exception>
        public IList<FXCMRest.Models.Candle> ToEntity()
        {
            CheckExecuted();

            var candles = new List<FXCMRest.Models.Candle>();
            if (CandlesData?.Count > 0)
            {
                foreach (var candleData in CandlesData)
                {
                    if (candleData?.Count >= 10)
                    {
                        FXCMRest.Models.Candle candle = new FXCMRest.Models.Candle();
                        candle.Timestamp = Util.FromUnixTimeSeconds((long)candleData[0]);
                        candle.BidOpen = candleData[1];
                        candle.BidClose = candleData[2];
                        candle.BidHigh = candleData[3];
                        candle.BidLow = candleData[4];
                        candle.AskOpen = candleData[5];
                        candle.AskClose = candleData[6];
                        candle.AskHigh = candleData[7];
                        candle.AskLow = candleData[8];
                        candle.TickQty = candleData[9];

                        candles.Add(candle);
                    }
                }
            }

            return candles;
        }
    }
}
