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
using System.Collections.Generic;
using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{
    /// <summary>
    /// This algorithm is a test case for adding forex symbols at a resolution differing from an existing internal feed.
    /// Both symbols are added in the Initialize method.
    /// </summary>
    public class ForexInternalFeedRegressionAlgorithm : QCAlgorithm
    {
        private readonly Dictionary<Symbol, int> _dataPointsPerSymbol = new Dictionary<Symbol, int>();

        public override void Initialize()
        {
            SetStartDate(2013, 10, 7);
            SetEndDate(2013, 10, 8);
            SetCash(100000);

            var eurgbp = AddForex("EURGBP", Resolution.Daily);
            _dataPointsPerSymbol.Add(eurgbp.Symbol, 0);

            var gbpusd = AddForex("GBPUSD");
            _dataPointsPerSymbol.Add(gbpusd.Symbol, 0);
        }

        public override void OnData(Slice data)
        {
            foreach (var kvp in data)
            {
                var symbol = kvp.Key;
                _dataPointsPerSymbol[symbol]++;

                Log($"{Time} {symbol.Value} {kvp.Value.Price}");
            }
        }

        public override void OnEndOfAlgorithm()
        {
            // (1440 minutes/day * 2 days) + 1
            const int expectedDataPointsPerSymbol = 2881;

            foreach (var kvp in _dataPointsPerSymbol)
            {
                var symbol = kvp.Key;
                var actualDataPoints = _dataPointsPerSymbol[symbol];
                Log($"Data points for symbol {symbol.Value}: {actualDataPoints}");

                if (actualDataPoints != expectedDataPointsPerSymbol)
                {
                    throw new Exception($"Data point count mismatch for symbol {symbol.Value}: expected: {expectedDataPointsPerSymbol}, actual: {actualDataPoints}");
                }
            }
        }
    }
}
