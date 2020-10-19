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

using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

using FXCMRest.Json;

namespace FXCMRest
{
    /// <summary>
    /// Http requests executor. It's used to send RESTful requests.
    /// Responses are expected in Json format.
    /// </summary>
    class HttpRequestExecutor
    {
        private string mHost;
        private string mAccessToken;
        private string mSessionId;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="host">The server host.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="sessionId">Socket.IO session id.</param>
        public HttpRequestExecutor(string host, string accessToken, string sessionId)
        {
            mHost = host;
            mAccessToken = accessToken;
            mSessionId = sessionId;
        }

        /// <summary>
        /// Executes Http request synchronously.
        /// </summary>
        /// <param name="method">The Http method.</param>
        /// <param name="resource">The resource URI.</param>
        /// <param name="statusCode">The Http status code.</param>
        /// <returns>The response's Json object.</returns>
        /// <exception cref="WebException">If the server returns error status code.</exception>
        /// <exception cref="Exception">If an error occurred.</exception>
        public T Execute<T>(string method, string resource)
        {
            return Execute<T>(method, resource, null);
        }

        /// <summary>
        /// Executes Http request synchronously.
        /// </summary>
        /// <param name="method">The Http method.</param>
        /// <param name="resource">The resource URI.</param>
        /// <param name="parameters">The request's parameters.</param>
        /// <returns>The response's Json object.</returns>
        /// <exception cref="WebException">If the server returns error status code.</exception>
        /// <exception cref="Exception">If an error occurred.</exception>
        public T Execute<T>(string method, string resource, NameValueCollection parameters)
        {
            string queryString = ToQueryString(parameters);

            // prepare URL
            string url = mHost + resource;
            bool postContent = false;
            if (queryString?.Length > 0)
            {
                if (string.Equals(method, "GET", StringComparison.CurrentCultureIgnoreCase))
                    url += "?" + queryString;
                else
                    postContent = true;
            }

            // create request
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "request";
            request.Method = method;
            request.Headers["Authorization"] = "Bearer " + mSessionId + mAccessToken;

            // append parameters to body if required
            if (postContent)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(queryString);
                request.ContentLength = bytes.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
            }

            // execute the request
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream newStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(newStream);
                string result = sr.ReadToEnd();

                // deserialize Json response object
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        private string ToQueryString(NameValueCollection parameters)
        {
            if (parameters == null)
                return null;

            StringBuilder sb = new StringBuilder();

            bool first = true;
            for (int i = 0; i < parameters.Count; i++)
            {
                string key = parameters.GetKey(i);
                string[] values = parameters.GetValues(i);

                foreach (string value in values)
                {
                    if (!first)
                        sb.Append('&');
                    else
                        first = false;

                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}={1}", Uri.EscapeUriString(key), !String.IsNullOrEmpty(value) ? Uri.EscapeUriString(value) : "");
                }
            }

            return sb.ToString();
        }
    }
}
