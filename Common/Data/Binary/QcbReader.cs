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

using System;
using System.IO;

namespace QuantConnect.Data.Binary
{
    // Important: this is an experimental data format and is subject to change

    public unsafe class QcbReader : IDisposable
    {
        private const int BufferSize = 4096;

        private readonly byte[] _buffer = new byte[BufferSize];

        private readonly Stream _stream;
        private readonly QcbHeader _header;
        private readonly QcbTick _tick;
        private long _chunkPosition;
        private long _chunkLength;
        private readonly byte[] _overflow = new byte[256];
        private int _overflowLength;

        public QcbReader(Stream stream)
        {
            _stream = stream;

            _header = ReadHeader();

            _tick = new QcbTick(_header.TickSize, _header.TimeSliceTicks);
        }

        public Stream BaseStream
        {
            get { return _stream; }
        }

        private QcbHeader ReadHeader()
        {
            byte[] buffer = new byte[sizeof(QcbHeader)];
            _stream.Read(buffer, 0, sizeof (QcbHeader));

            var header = new QcbHeader();

            fixed (byte* p = &buffer[0])
            {
                QcbHeader* h = (QcbHeader*)p;

                header.Version = h->Version;
                header.Flags = h->Flags;
                header.TickSize = BytesToDecimal((byte*)&(h->TickSize));
                header.TimeSliceTicks = h->TimeSliceTicks;
            }

            return header;
        }

        public QcbTick ReadTick()
        {
            if (_chunkPosition == _chunkLength)
            {
                // read new buffer from stream
                _chunkLength = _stream.Read(_buffer, 0, BufferSize);
                _chunkPosition = 0;
            }

            // read tick from buffer
            fixed (byte* p = &_buffer[0])
            {
                _tick.Read(p, ref _chunkPosition, _chunkLength, _overflow, ref _overflowLength);
            }

            // tick split between two buffers
            if (_overflowLength > 0)
            {
                // copy remaining bytes from overflow buffer
                Buffer.BlockCopy(_overflow, 0, _buffer, 0, _overflowLength);

                // read new buffer from stream
                _chunkLength = _stream.Read(_buffer, _overflowLength, BufferSize - _overflowLength) + _overflowLength;
                _chunkPosition = 0;
                _overflowLength = 0;

                // read tick from new buffer
                fixed (byte* p = &_buffer[0])
                {
                    _tick.Read(p, ref _chunkPosition, _chunkLength, _overflow, ref _overflowLength);
                }
            }

            return _tick;
        }

        public void Close()
        {
            if (_stream != null)
            {
                _stream.Close();
            }
        }

        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Dispose();
            }
        }

        const byte DecimalSignBit = 128;
        private static decimal BytesToDecimal(byte* src)
        {
            return new decimal(
                *(int*)src,
                *(int*)(src + 4),
                *(int*)(src + 8),
                *(src + 15) == DecimalSignBit,
                *(src + 14));
        }

    }
}
