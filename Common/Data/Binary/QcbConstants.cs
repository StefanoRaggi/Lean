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

namespace QuantConnect.Data.Binary
{
    // Important: this is an experimental data format and is subject to change

    public static class QcbValueSizeCode
    {
        public const byte Int8 = 0;
        public const byte Int16 = 1;
        public const byte Int32 = 2;
        public const byte Int64 = 3;
    }

    public static class QcbTickType
    {
        public const byte Trade = 0;
        public const byte Bid = (1 << 0);
        public const byte Ask = (1 << 1);
        public const byte BidAsk = Bid | Ask;

        public const byte HasTime = (1 << 3);
        public const byte HasPrice = (1 << 4);
        public const byte HasVolume = (1 << 5);
        public const byte HasExtraPrice = (1 << 6);

        public const byte MaskTickType = Bid | Ask;
        public const byte MaskTickOptions = HasTime | HasPrice | HasVolume | HasExtraPrice;
    }

    public static class QcbTickOption
    {
        public const byte MaskDeltaTime = (1 << 0) | (1 << 1);
        public const byte MaskDeltaPrice = (1 << 2) | (1 << 3);
        public const byte MaskDeltaVolume = (1 << 4) | (1 << 5);
        public const byte MaskDeltaExtraPrice = (1 << 6) | (1 << 7);
    }

}