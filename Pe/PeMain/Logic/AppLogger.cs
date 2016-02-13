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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
    public class AppLogger: Logger
    {
        #region variable

        ILogAppender _logCollector;
        bool _isStock;

        #endregion

        public AppLogger()
            : base()
        {
            LoggerConfig.EnabledAll = true;
            LoggerConfig.PutsDebug = true;
            //LoggerConfig.PutsConsole = true;
        }

        #region property

        public ILogAppender LogCollector
        {
            get { return this._logCollector; }
            set
            {
                if(this._logCollector != null) {
                    // イベントとか切る用
                }

                this._logCollector = value;

                if(this._logCollector == null) {
                    LoggerConfig.PutsCustom = false;
                } else {
                    LoggerConfig.PutsCustom = true;
                }
            }
        }

        public List<LogItemModel> StockItems { get; private set; }

        public bool IsStock
        {
            get { return this._isStock; }
            set
            {
                this._isStock = value;
                if(this._isStock) {
                    if(StockItems == null) {
                        StockItems = new List<LogItemModel>();
                    }
                } else {
                    if(StockItems != null) {
                        StockItems.Clear();
                        StockItems = null;
                    }
                }
            }
        }

        #endregion

        #region Logger

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Information("exit!");
                }
            }
            base.Dispose(disposing);
        }

        protected override void PutsCustom(LogItemModel item)
        {
            if(LogCollector != null) {
                LogCollector.AddLog(item);
            }
        }

        protected override void Puts(LogItemModel item)
        {
            if(IsStock) {
                StockItems.Add(item);
            }
            base.Puts(item);
        }

        protected override void PutsConsole(LogItemModel item)
        {
            Console.WriteLine(PutsOutput(item, 'C'));
        }

        protected override void PutsDebug(LogItemModel item)
        {
            Console.WriteLine(PutsOutput(item, 'D'));
        }

        #endregion
    }
}
