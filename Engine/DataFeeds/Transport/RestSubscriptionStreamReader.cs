/*
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
using RestSharp;

namespace QuantConnect.Lean.Engine.DataFeeds.Transport
{
    /// <summary>
    /// Represents a stream reader capable of polling a rest client
    /// </summary>
    public class RestSubscriptionStreamReader : IStreamReader
    {
        private readonly RestClient _client;
        private readonly RestRequest _request;
        private readonly DataStreamReader _reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestSubscriptionStreamReader"/> class.
        /// </summary>
        /// <param name="source">The source url to poll with a GET</param>
        public RestSubscriptionStreamReader(string source)
        {
            _client = new RestClient(source);
            _request = new RestRequest(Method.GET);

            var response = _client.Execute(_request);
            if (response != null)
            {
                _reader = new DataStreamReader(response.Content.ToStream(), DataStreamFormat.Csv);
            }
        }

        /// <summary>
        /// Gets <see cref="SubscriptionTransportMedium.Rest"/>
        /// </summary>
        public SubscriptionTransportMedium TransportMedium
        {
            get { return SubscriptionTransportMedium.Rest; }
        }

        /// <summary>
        /// Gets whether or not there's more data to be read in the stream
        /// </summary>
        public bool EndOfStream
        {
            get { return _reader == null || _reader.EndOfStream; }
        }

        /// <summary>
        /// Gets the data stream reader used
        /// </summary>
        public DataStreamReader GetDataStreamReader()
        {
            return _reader;
        }

        /// <summary>
        /// This stream reader doesn't require closing
        /// </summary>
        public void Close()
        {
            if (_reader != null)
            {
                _reader.Close();
            }
        }

        /// <summary>
        /// This stream reader doesn't require disposal
        /// </summary>
        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }
        }
    }
}