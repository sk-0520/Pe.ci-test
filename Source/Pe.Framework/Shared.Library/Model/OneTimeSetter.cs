using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public class OneTimeSetter<TValue>
    {
        #region variable

        TValue _value;

        #endregion

        public OneTimeSetter()
            : this(false)
        { }

        public OneTimeSetter(bool unsettedGetException)
        {
            UnsettedGetException = unsettedGetException;
        }

        #region property

        public bool IsSetted { get; private set; }
        public bool UnsettedGetException { get; }

        public TValue Value
        {
            get
            {
                if(IsSetted) {
                    return this._value;
                }

                if(UnsettedGetException) {
                    throw new InvalidOperationException();
                }

                return default(TValue);
            }
            set
            {
                if(IsSetted) {
                    throw new InvalidOperationException();
                }

                this._value = value;
                IsSetted = true;
            }
        }

        #endregion

        #region object

        public override string ToString()
        {
            if(IsSetted) {
                return this._value?.ToString() ?? null;
            }
            return base.ToString();
        }

        #endregion
    }

    public class OneTimeFlagSetter : OneTimeSetter<bool>
    {
        public OneTimeFlagSetter()
        { }
    }
}
