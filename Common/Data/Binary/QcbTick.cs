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

namespace QuantConnect.Data.Binary
{
    // Important: this is an experimental data format and is subject to change

    public class QcbTick
    {
        private readonly decimal _tickSize;
        private readonly int _timeSliceTicks;

        private long _lastTimeValue;
        private long _lastPriceTicks;
        private long _lastVolume;
        private long _lastExtraPriceTicks;

        private long _timeValue;
        private long _bidPriceTicks;
        private long _askPriceTicks;
        private long _volume;

        public DateTime GetTimestamp()
        {
            return new DateTime(_timeValue  * _timeSliceTicks);
        }

        public decimal GetBidPrice()
        {
            return _tickSize * _bidPriceTicks;
        }

        public decimal GetAskPrice()
        {
            return _tickSize * _askPriceTicks;
        }

        public decimal GetMidPrice()
        {
            return _tickSize * (_bidPriceTicks + _askPriceTicks) / 2;
        }

        public long GetVolume()
        {
            return _volume;
        }

        public QcbTick(decimal tickSize, int timeSliceTicks)
        {
            _tickSize = tickSize;
            _timeSliceTicks = timeSliceTicks;
        }

        private unsafe void CopyBytes(byte* source, byte[] dest, int count)
        {
            byte* p = source;
            for (int i = 0; i < count; i++)
            {
                unchecked { dest[i] = *p++; }
            }
        }

        public unsafe void Read(byte* buffer, ref long position, long length, byte[] overflow, ref int overflowLength)
        {
            byte* p = buffer + position;

            byte* pStart = p;
            int byteCount = 0;

            byte tickHeader = *p++;
            position++;
            byteCount++;

            if (position == length)
            {
                CopyBytes(pStart, overflow, byteCount);
                overflowLength = byteCount;
                return;
            }

            byte tickType = (byte)(tickHeader & QcbTickType.MaskTickType);
            if (tickType != QcbTickType.BidAsk)
                throw new Exception("Invalid TickType != BidAsk");

            if ((tickHeader & QcbTickType.MaskTickOptions) != 0)
            {
                byte tickOptions = *p++;
                position++;
                byteCount++;

                if (position == length)
                {
                    CopyBytes(pStart, overflow, byteCount);
                    overflowLength = byteCount;
                    return;
                }

                long timeDelta = 0;
                long priceDelta = 0;
                long volumeDelta = 0;
                long extraPriceDelta = 0;

                if ((tickHeader & QcbTickType.HasTime) != 0)
                {
                    byte code = (byte)(tickOptions & QcbTickOption.MaskDeltaTime);

                    if (position + 8 >= length)
                    {
                        byteCount += (int)(length - position);
                        CopyBytes(pStart, overflow, byteCount);
                        overflowLength = byteCount;
                        return;
                    }

                    timeDelta = ReadDeltaValue(ref p, ref position, code);
                    byteCount += 1 << code;
                }

                if ((tickHeader & QcbTickType.HasPrice) != 0)
                {
                    byte code = (byte)((tickOptions & QcbTickOption.MaskDeltaPrice) >> 2);

                    if (position + 8 >= length)
                    {
                        byteCount += (int)(length - position);
                        CopyBytes(pStart, overflow, byteCount);
                        overflowLength = byteCount;
                        return;
                    }

                    priceDelta = ReadDeltaValue(ref p, ref position, code);
                    byteCount += 1 << code;
                }

                if ((tickHeader & QcbTickType.HasVolume) != 0)
                {
                    byte code = (byte)((tickOptions & QcbTickOption.MaskDeltaVolume) >> 4);

                    if (position + 8 >= length)
                    {
                        byteCount += (int)(length - position);
                        CopyBytes(pStart, overflow, byteCount);
                        overflowLength = byteCount;
                        return;
                    }

                    volumeDelta = ReadDeltaValue(ref p, ref position, code);
                    byteCount += 1 << code;
                }

                if ((tickHeader & QcbTickType.HasExtraPrice) != 0)
                {
                    byte code = (byte)((tickOptions & QcbTickOption.MaskDeltaExtraPrice) >> 6);

                    if (position + 8 >= length)
                    {
                        byteCount += (int)(length - position);
                        CopyBytes(pStart, overflow, byteCount);
                        overflowLength = byteCount;
                        return;
                    }

                    extraPriceDelta = ReadDeltaValue(ref p, ref position, code);
                    byteCount += 1 << code;
                }

                if ((tickHeader & QcbTickType.HasTime) != 0)
                {
                    _timeValue = _lastTimeValue + timeDelta;
                    _lastTimeValue += timeDelta;
                }

                if ((tickHeader & QcbTickType.HasPrice) != 0)
                {
                    _bidPriceTicks = _lastPriceTicks + priceDelta;
                    _lastPriceTicks += priceDelta;
                }

                if ((tickHeader & QcbTickType.HasVolume) != 0)
                {
                    _volume = _lastVolume + volumeDelta;
                    _lastVolume += volumeDelta;
                }

                if ((tickHeader & QcbTickType.HasExtraPrice) != 0)
                {
                    _askPriceTicks = _lastExtraPriceTicks + extraPriceDelta;
                    _lastExtraPriceTicks += extraPriceDelta;
                }
            }
        }

        private unsafe static long ReadDeltaValue(ref byte* buffer, ref long position, byte code)
        {
            long delta;

            switch (code)
            {
                case QcbValueSizeCode.Int8:
                    delta = *buffer;
                    buffer += sizeof(byte);
                    position += sizeof (byte);
                    break;

                case QcbValueSizeCode.Int16:
                    delta = *(short*)buffer;
                    buffer += sizeof(short);
                    position += sizeof (short);
                    break;

                case QcbValueSizeCode.Int32:
                    delta = *(int*)buffer;
                    buffer += sizeof(int);
                    position += sizeof (int);
                    break;

                case QcbValueSizeCode.Int64:
                    delta = *(long*)buffer;
                    buffer += sizeof(long);
                    position += sizeof (long);
                    break;

                default:
                    throw new Exception("Invalid delta size code");
            }

            return delta;
        }

    }
}