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

    public class QcbFileHeader
    {
        public int Version;
        public int Flags;
        public decimal TickSize;     // TickSize for Futures, PipetteSize for Forex
        public int TimeSliceTicks;

        public QcbFileHeader()
        {
            Version = 1;
            Flags = 0;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(Flags);
            writer.Write(TickSize);
            writer.Write(TimeSliceTicks);
        }

        public void Read(BinaryReader reader)
        {
            Version = reader.ReadInt32();
            Flags = reader.ReadInt32();
            TickSize = reader.ReadDecimal();
            TimeSliceTicks = reader.ReadInt32();
        }
    }
}