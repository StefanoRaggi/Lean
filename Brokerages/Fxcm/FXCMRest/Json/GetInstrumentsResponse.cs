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
    /// GetInstruments response Json entity.
    /// </summary>
    class GetInstrumentsResponse : BaseResponse
    {
        public class ResponseData
        {
            /// <summary>
            /// List of instruments.
            /// </summary>
            [JsonProperty("instrument")]
            public List<Instrument> Instrument { get; set; }
        }

        public class Instrument
        {
            /// <summary>
            /// Representation of the instrument.
            /// </summary>
            [JsonProperty("symbol")]
            public string Symbol;

            /// <summary>
            /// Is symbol visible in Offers table.
            /// </summary>
            [JsonProperty("visible")]
            public bool Visible;

            /// <summary>
            /// Ordering number.
            /// </summary>
            [JsonProperty("order")]
            public int Order;
        }

        /// <summary>
        /// Data of the response.
        /// </summary>
        [JsonProperty("data")]
        public ResponseData Data { get; set; }

        /// <summary>
        /// Converts Json response object to a model entity.
        /// </summary>
        /// <returns>The entity to return to a user.</returns>
        /// <exception cref="Exception">If a response is an error response.</exception>
        public IList<FXCMRest.Instrument> ToEntity()
        {
            CheckExecuted();

            var instruments = new List<FXCMRest.Instrument>();
            if (Data?.Instrument != null)
            {
                foreach (Instrument instrument in Data.Instrument)
                    instruments.Add(new FXCMRest.Instrument
                    {
                        Symbol = instrument.Symbol,
                        Visible = instrument.Visible,
                        Order = instrument.Order
                    });
            }

            return instruments;
        }
    }
}
