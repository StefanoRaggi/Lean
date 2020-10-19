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
    /// GetModel response Json entity.
    /// I.e. GetOffers, GetOrders etc
    /// </summary>
    class GetModelResponse : BaseResponse
    {
        /// <summary>
        /// Received models.
        /// </summary>
        public class Models
        {
            public IList<FXCMRest.Models.Offer> Offers { get; set; }
            public IList<FXCMRest.Models.ClosedPosition> ClosedPositions { get; set; }
            public IList<FXCMRest.Models.OpenPosition> OpenPositions { get; set; }
            public IList<FXCMRest.Models.Order> Orders { get; set; }
            public IList<FXCMRest.Models.Account> Accounts { get; set; }
            public IList<FXCMRest.Models.Summary> Summary { get; set; }
        }

        [JsonProperty("offers")]
        public List<Offer> Offers { get; set; }

        [JsonProperty("open_positions")]
        public List<OpenPosition> OpenPositions { get; set; }

        [JsonProperty("closed_positions")]
        public List<ClosedPosition> ClosedPositions { get; set; }

        [JsonProperty("orders")]
        public List<Order> Orders { get; set; }

        [JsonProperty("accounts")]
        public List<Account> Accounts { get; set; }

        [JsonProperty("summary")]
        public List<Summary> Summary { get; set; }

        /// <summary>
        /// Converts Json response object to a model entity.
        /// </summary>
        /// <returns>The entity to return to a user.</returns>
        /// <exception cref="Exception">If a response is an error response.</exception>
        public Models ToEntity()
        {
            CheckExecuted();

            var models = new Models();

            if (Offers != null)
            {
                models.Offers = new List<FXCMRest.Models.Offer>();
                foreach (Offer offer in Offers)
                    models.Offers.Add(offer.ToEntity());
            }
            if (OpenPositions != null)
            {
                models.OpenPositions = new List<FXCMRest.Models.OpenPosition>();
                foreach (OpenPosition openPosition in OpenPositions)
                    models.OpenPositions.Add(openPosition.ToEntity());
            }
            if (ClosedPositions != null)
            {
                models.ClosedPositions = new List<FXCMRest.Models.ClosedPosition>();
                foreach (ClosedPosition closedPosition in ClosedPositions)
                    models.ClosedPositions.Add(closedPosition.ToEntity());
            }
            if (Orders != null)
            {
                models.Orders = new List<FXCMRest.Models.Order>();
                foreach (Order order in Orders)
                    models.Orders.Add(order.ToEntity());
            }
            if (Accounts != null)
            {
                models.Accounts = new List<FXCMRest.Models.Account>();
                foreach (Account account in Accounts)
                    models.Accounts.Add(account.ToEntity());
            }
            if (Summary != null)
            {
                models.Summary = new List<FXCMRest.Models.Summary>();
                foreach (Summary summary in Summary)
                    models.Summary.Add(summary.ToEntity());
            }

            return models;
        }
    }
}
