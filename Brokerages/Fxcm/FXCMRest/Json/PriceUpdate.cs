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

using System.Collections.Generic;

namespace FXCMRest.Json
{
    /// <summary>
    /// PriceUpdate Json entity.
    /// </summary>
    class PriceUpdate
    {
        /// <summary>
        /// Timestamp of last update, Epoch timestamp in milliseconds.
        /// </summary>
        public long Updated { get; set; }

        /// <summary>
        /// Current rates. Array of Numbers, [Bid, Ask, Session High, Session Low]
        /// </summary>
        public List<double> Rates { get; set; }

        /// <summary>
        /// String representing the symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Converts Json entity to a model entity.
        /// </summary>
        /// <returns>The entity to return to a user.</returns>
        public FXCMRest.PriceUpdate ToEntity()
        {
            var priceUpdate = new FXCMRest.PriceUpdate();
            if (Rates?.Count >= 4)
            {
                priceUpdate.Bid = Rates[0];
                priceUpdate.Ask = Rates[1];
                priceUpdate.High = Rates[2];
                priceUpdate.Low = Rates[3];
            }
            priceUpdate.Symbol = Symbol;
            priceUpdate.Updated = Util.FromUnixTimeMilliseconds(Updated);

            return priceUpdate;
        }
    }
}
