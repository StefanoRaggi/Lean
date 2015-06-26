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
*/

using System.IO;

namespace QuantConnect.Data.Binary
{
    // Important: this is an experimental data format and is subject to change

    public class QcbFile
    {
        //private readonly BinaryWriter _writer;
        private readonly BinaryReader _reader;
        private QcbTick _tick;
        public QcbFileHeader Header;

        //public QcbFile(BinaryWriter writer)
        //{
        //    _writer = writer;
        //}

        public QcbFile(BinaryReader reader)
        {
            _reader = reader;

            ReadHeader();
        }

        //public void Write(QcbFileHeader header)
        //{
        //    header.Write(_writer);

        //    Header = header;

        //    _tick = new QcbTick(header.TickSize, header.TimeSliceTicks);
        //}

        //public void WriteTick(DateTime timestamp, decimal bid, decimal ask)
        //{
        //    _tick.Write(_writer, timestamp, bid, ask);
        //}

        private void ReadHeader()
        {
            Header = new QcbFileHeader();

            Header.Read(_reader);

            _tick = new QcbTick(Header.TickSize, Header.TimeSliceTicks, _reader.BaseStream.Length);
        }

        public QcbTick ReadTick()
        {
            return _tick.Read(_reader);
        }

        public void Close()
        {
            if (_reader != null)
            {
                _reader.Close();
            }
            //if (_writer != null)
            //{
            //    _writer.Close();
            //}
        }

        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }
            //if (_writer != null)
            //{
            //    _writer.Dispose();
            //}
        }
    }
}