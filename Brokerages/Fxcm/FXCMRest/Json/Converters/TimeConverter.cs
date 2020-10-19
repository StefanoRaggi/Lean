/*
 * Copyright (c) 2018: FXCM Group, LLC
 *
 * FXCM Group, LLC and each of its affiliates and subsidiaries are herein referred
 * to as, "FXCM".
 *
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 * 3. FXCM's name may not be used to endorse or promote products derived from
 *    this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY FXCM "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL FXCM BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF
 * THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FXCMRest.Json.Converters
{
    /// <summary>
    /// Json DateTime converter for Time properties.
    /// </summary>
    class TimeConverter : IsoDateTimeConverter
    {
        public override Object ReadJson(JsonReader reader, Type objectType,
            Object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                // if an empty or null string is specified then return the default date
                string dateText = reader.Value.ToString();
                if (string.IsNullOrEmpty(dateText))
                    return default(DateTime);

                // choose one of possible formats
                string format1 = "MMddyyyyHHmmss";
                string format2 = "MMddyyyyHHmmssfff";
                string format = dateText.Length == format1.Length ? format1 : format2;
                return DateTime.ParseExact(dateText, format, Culture);
            }
            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }
}
