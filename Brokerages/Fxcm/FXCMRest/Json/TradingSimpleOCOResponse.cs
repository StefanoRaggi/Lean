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
    /// TradingSimpleOCO response Json entity.
    /// </summary>
    class TradingSimpleOCOResponse : BaseResponse
    {
        public class DataResult
        {
            /// <summary>
            /// Defines the type of the order
            /// </summary>
            [JsonProperty("type")]
            public int Type { get; set; }

            /// <summary>
            /// The order identifier.
            /// </summary>
            [JsonProperty("orderId")]
            public string OrderId { get; set; }
        }

        [JsonProperty("data")]
        public List<DataResult> Data { get; set; }

        /// <summary>
        /// Converts Json response object to a list of strings.
        /// </summary>
        /// <returns>The list of strings to return to a user.</returns>
        /// <exception cref="Exception">If a response is an error response.</exception>
        public IList<string> ToOrdersList()
        {
            CheckExecuted();

            var orders = new List<string>();
            if (Data?.Count > 0)
            {
                foreach (var order in Data)
                {
                    if (order != null && !string.IsNullOrEmpty(order.OrderId))
                    {
                        orders.Add(order.OrderId);
                    }
                }
            }
            return orders;
        }
    }
}
