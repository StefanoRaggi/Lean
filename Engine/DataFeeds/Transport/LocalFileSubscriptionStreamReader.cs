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

using System.IO;
using Ionic.Zip;
using QuantConnect.Data;

namespace QuantConnect.Lean.Engine.DataFeeds.Transport
{
    /// <summary>
    /// Represents a stream reader capable of reading lines from disk
    /// </summary>
    public class LocalFileSubscriptionStreamReader : IStreamReader
    {
        private readonly ZipFile _zipFile;
        private readonly DataStreamReader _reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileSubscriptionStreamReader"/> class.
        /// </summary>
        /// <param name="source">The local file to be read</param>
        public LocalFileSubscriptionStreamReader(string source)
        {
            string ext = source.GetExtension();

            Stream stream;
            DataStreamFormat dataFormat;
            // get the data format and unzip if necessary
            if (ext == ".zip")
            {
                stream = Compression.Unzip(source, out _zipFile, out dataFormat);
            }
            else
            {
                // Commented lines still here for unfinished performance testing purposes
                //byte[] bytes = File.ReadAllBytes(source);
                //stream = new MemoryStream(bytes);

                stream = new FileStream(source, FileMode.Open, FileAccess.Read);

                dataFormat = ext == ".qcb" ? DataStreamFormat.Qcb : DataStreamFormat.Csv;
            }

            _reader = new DataStreamReader(stream, dataFormat);
        }

        /// <summary>
        /// Gets <see cref="SubscriptionTransportMedium.LocalFile"/>
        /// </summary>
        public SubscriptionTransportMedium TransportMedium
        {
            get { return SubscriptionTransportMedium.LocalFile; }
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
        /// Closes the stream
        /// </summary>
        public void Close()
        {
            if (_reader != null)
            {
                _reader.Close();
            }
        }

        /// <summary>
        /// Disposes of the stream
        /// </summary>
        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_zipFile != null)
            {
                _zipFile.Dispose();
            }
        }
    }
}