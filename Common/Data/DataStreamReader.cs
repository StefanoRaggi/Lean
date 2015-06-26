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

using System;
using System.IO;
using QuantConnect.Data.Binary;

namespace QuantConnect.Data
{
    /// <summary>
    /// Represents a generic data stream reader
    /// </summary>
    public class DataStreamReader : IDisposable
    {
        private readonly Stream _baseStream;
        private readonly StreamReader _streamReader;
        private readonly QcbFile _binaryReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStreamReader"/> class.
        /// </summary>
        /// <param name="stream">The underlying stream reader</param>
        /// <param name="dataFormat">The data format of the stream</param>
        public DataStreamReader(Stream stream, DataStreamFormat dataFormat)
        {
            _baseStream = stream;
            DataFormat = dataFormat;

            if (dataFormat == DataStreamFormat.Qcb)
            {
                _binaryReader = new QcbFile(new BinaryReader(stream));
            }
            else
            {
                _streamReader = new StreamReader(stream);
            }
        }

        /// <summary>
        /// Gets the data format of the underlying stream
        /// </summary>
        public DataStreamFormat DataFormat { get; private set; }

        /// <summary>
        /// Returns the reader for the underlying binary stream
        /// </summary>
        public QcbFile BinaryReader { get { return _binaryReader; } }

        /// <summary>
        /// Returns the reader for the underlying text stream
        /// </summary>
        public StreamReader TextReader { get { return _streamReader; } }

        /// <summary>
        /// Gets whether or not there's more data to be read in the stream
        /// </summary>
        public bool EndOfStream 
        {
            get { return _baseStream == null || _baseStream.Position == _baseStream.Length; }
        }

        /// <summary>
        /// Closes the stream reader
        /// </summary>
        public void Close()
        {
            if (_baseStream != null)
            {
                _baseStream.Close();
            }

            if (_streamReader != null)
            {
                _streamReader.Close();
            }

            if (_binaryReader != null)
            {
                _binaryReader.Close();
            }
        }

        /// <summary>
        /// Disposes of the stream reader
        /// </summary>
        public void Dispose()
        {
            if (_baseStream != null)
            {
                _baseStream.Dispose();
            }

            if (_streamReader != null)
            {
                _streamReader.Dispose();
            }

            if (_binaryReader != null)
            {
                _binaryReader.Dispose();
            }
        }
    }
}
