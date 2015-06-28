﻿/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/

using QuantConnect.Data;

namespace QuantConnect.Lean.Engine.DataFeeds.Transport
{
    /// <summary>
    /// Defines a transport mechanism for data from its source into various reader methods
    /// </summary>
    public interface IStreamReader
    {
        /// <summary>
        /// Gets the transport medium of this stream
        /// </summary>
        SubscriptionTransportMedium TransportMedium { get; }

        /// <summary>
        /// Gets whether or not there's more data to be read in the stream
        /// </summary>
        bool EndOfStream { get; }

        /// <summary>
        /// Gets the data stream reader used
        /// </summary>
        DataStreamReader GetDataStreamReader();

        /// <summary>
        /// Closes the stream
        /// </summary>
        void Close();

        /// <summary>
        /// Disposes of the stream
        /// </summary>
        void Dispose();
    }
}
