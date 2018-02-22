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
using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{
    /// <summary>
    /// This algorithm is a test case to count expected data points with forex symbols at daily resolution.
    /// </summary>
    public class ForexDailyDataPointsRegressionAlgorithm : QCAlgorithm
    {
        private int _count;

        public override void Initialize()
        {
            SetStartDate(2013, 10, 7);  //Set Start Date
            SetEndDate(2013, 10, 9);    //Set End Date
            SetCash(100000);             //Set Strategy Cash

            AddForex("EURGBP", Resolution.Daily);
        }

        public override void OnData(Slice data)
        {
            foreach (var kvp in data)
            {
                _count++;
                Log($"{Time} {kvp.Key.Value} {kvp.Value.Price}");
            }
        }

        public override void OnEndOfAlgorithm()
        {
            // one point per day
            const int expectedDataPoints = 3;
            Log($"Data points: {_count}");

            if (_count != 3)
            {
                throw new Exception($"Data point count mismatch: expected: {expectedDataPoints}, actual: {_count}");
            }
        }
    }
}
