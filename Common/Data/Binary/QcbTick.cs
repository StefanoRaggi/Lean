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

    public class QcbTick
    {
        private readonly long _streamLength;

        private readonly decimal _tickSize;
        private readonly int _timeSliceTicks;

        private long _lastTimeValue = 0;
        private long _lastPriceTicks = 0;
        private long _lastVolume = 0;
        private long _lastExtraPriceTicks = 0;

        public DateTime Timestamp;
        public decimal BidPrice;
        public decimal AskPrice;
        public long Volume;

        public QcbTick(decimal tickSize, int timeSliceTicks, long streamLength)
        {
            _tickSize = tickSize;
            _timeSliceTicks = timeSliceTicks;

            _streamLength = streamLength;
        }

        public QcbTick Read(BinaryReader reader)
        {
            if (reader.BaseStream.Position == _streamLength)
                return null;

            byte tickHeader = reader.ReadByte();

            byte tickType = (byte)(tickHeader & QcbTickType.MaskTickType);
            if (tickType != QcbTickType.BidAsk)
                throw new Exception("Invalid TickType != BidAsk");

            if ((tickHeader & QcbTickType.MaskTickOptions) != 0)
            {
                byte tickOptions = reader.ReadByte();

                if ((tickHeader & QcbTickType.HasTime) != 0)
                {
                    byte code = (byte)(tickOptions & QcbTickOption.MaskDeltaTime);
                    long timeDelta = ReadDeltaValue(reader, code);
                    Timestamp = new DateTime((_lastTimeValue + timeDelta) * _timeSliceTicks);
                    _lastTimeValue += timeDelta;
                }

                if ((tickHeader & QcbTickType.HasPrice) != 0)
                {
                    byte code = (byte)((tickOptions & QcbTickOption.MaskDeltaPrice) >> 2);
                    long priceDelta = ReadDeltaValue(reader, code);
                    BidPrice = (_lastPriceTicks + priceDelta) * _tickSize;
                    _lastPriceTicks += priceDelta;
                }

                if ((tickHeader & QcbTickType.HasVolume) != 0)
                {
                    byte code = (byte)((tickOptions & QcbTickOption.MaskDeltaVolume) >> 4);
                    long volumeDelta = ReadDeltaValue(reader, code);
                    Volume = _lastVolume + volumeDelta;
                    _lastVolume += volumeDelta;
                }

                if ((tickHeader & QcbTickType.HasExtraPrice) != 0)
                {
                    byte code = (byte)((tickOptions & QcbTickOption.MaskDeltaExtraPrice) >> 6);
                    long extraPriceDelta = ReadDeltaValue(reader, code);
                    AskPrice = (_lastExtraPriceTicks + extraPriceDelta) * _tickSize;
                    _lastExtraPriceTicks += extraPriceDelta;
                }
            }

            return this;
        }

        private long ReadDeltaValue(BinaryReader reader, byte code)
        {
            switch (code)
            {
                case QcbValueSizeCode.Int8:
                    return reader.ReadSByte();

                case QcbValueSizeCode.Int16:
                    return reader.ReadInt16();

                case QcbValueSizeCode.Int32:
                    return reader.ReadInt32();

                case QcbValueSizeCode.Int64:
                    return reader.ReadInt64();

                default:
                    throw new Exception("Invalid delta size code");
            }
        }

        public void Write(BinaryWriter writer, DateTime timestamp, decimal bid, decimal ask)
        {
            long timeValue = timestamp.Ticks / _timeSliceTicks;
            long timeDelta = timeValue - _lastTimeValue;
            _lastTimeValue = timeValue;

            long priceTicks = (long)(bid / _tickSize);
            long priceDelta = priceTicks - _lastPriceTicks;
            _lastPriceTicks = priceTicks;

            const long volumeDelta = 0;

            long extraPriceTicks = (long)(ask / _tickSize);
            long extraPriceDelta = extraPriceTicks - _lastExtraPriceTicks;
            _lastExtraPriceTicks = extraPriceTicks;

            byte codeTime = GetValueSizeCode(timeDelta);
            byte codePrice = GetValueSizeCode(priceDelta);
            const byte codeVolume = 0;
            byte codeExtraPrice = GetValueSizeCode(extraPriceDelta);

            byte tickHeader = QcbTickType.BidAsk;
            if (timeDelta != 0)
            {
                tickHeader |= QcbTickType.HasTime;
            }
            if (priceDelta != 0)
            {
                tickHeader |= QcbTickType.HasPrice;
            }
            if (volumeDelta != 0)
            {
                tickHeader |= QcbTickType.HasVolume;
            }
            if (extraPriceDelta != 0)
            {
                tickHeader |= QcbTickType.HasExtraPrice;
            }

            writer.Write(tickHeader);

            byte tickOptions = codeTime;
            tickOptions |= (byte)(codePrice << 2);
            tickOptions |= (byte)(codeVolume << 4);
            tickOptions |= (byte)(codeExtraPrice << 6);

            if ((tickHeader & QcbTickType.MaskTickOptions) != 0)
            {
                writer.Write(tickOptions);

                WriteDeltaValue(writer, codeTime, timeDelta);
                WriteDeltaValue(writer, codePrice, priceDelta);
                WriteDeltaValue(writer, codeVolume, volumeDelta);
                WriteDeltaValue(writer, codeExtraPrice, extraPriceDelta);
            }
        }

        private void WriteDeltaValue(BinaryWriter writer, byte code, long value)
        {
            if (value == 0)
                return;

            switch (code)
            {
                case QcbValueSizeCode.Int8:
                    writer.Write((SByte)value);
                    break;

                case QcbValueSizeCode.Int16:
                    writer.Write((Int16)value);
                    break;

                case QcbValueSizeCode.Int32:
                    writer.Write((Int32)value);
                    break;

                case QcbValueSizeCode.Int64:
                    writer.Write((Int64)value);
                    break;

                default:
                    throw new Exception("Invalid delta size code");
            }
        }

        private byte GetValueSizeCode(long value)
        {
            if (value >= (long)SByte.MinValue && value <= (long)SByte.MaxValue)
                return QcbValueSizeCode.Int8;

            if (value >= (long)Int16.MinValue && value <= (long)Int16.MaxValue)
                return QcbValueSizeCode.Int16;

            if (value >= (long)Int32.MinValue && value <= (long)Int32.MaxValue)
                return QcbValueSizeCode.Int32;

            return QcbValueSizeCode.Int64;
        }

    }
}