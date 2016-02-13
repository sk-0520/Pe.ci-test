/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
    public class UserInformationSender: DisposeFinalizeBase
    {
        public UserInformationSender(Uri uri, RunningInformationSettingModel runningInformation)
            : base()
        {
            Uri = uri;
            AppInformationCollection = new AppInformationCollection();
            RunningInformation = runningInformation;

            CreateSendData();
        }

        #region property

        public Uri Uri { get; private set; }
        public RunningInformationSettingModel RunningInformation { get; private set; }

        AppInformationCollection AppInformationCollection { get; set; }
        protected HttpClient Sender { get; private set; }
        public FormUrlEncodedContent SendData { get; private set; }

        #endregion

        #region function

        void CreateSendData()
        {
            // LINQでうまい方法が思いつかん
            var map = new Dictionary<string, string>();
            foreach(var info in AppInformationCollection.Get()) {
                foreach(var item in info.Items) {
                    var key = string.Format("{0}-{1}", info.Title, item.Key);
                    map[key] = item.Value != null ? item.Value.ToString() : string.Empty;
                }
            }
            map["app-key"] = "USER";
            map["app-result"] = "json";
            map["auto-id"] = RunningInformation.UserId;

#if DEBUG
            System.Diagnostics.Debug.WriteLine(string.Join("\t", map.Keys));
#endif
            SendData = new FormUrlEncodedContent(map);
        }

        public Task<HttpResponseMessage> SendAync()
        {
            Sender = new HttpClient();

            return Sender.PostAsync(Uri, SendData);
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Sender != null) {
                    Sender.Dispose();
                    Sender = null;
                }
                if(SendData != null) {
                    SendData.Dispose();
                    SendData = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
