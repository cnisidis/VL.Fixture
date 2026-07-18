using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixture
{
    public abstract class AbstractValueChannel
    {
        /// <summary>
        /// Value Channel unique name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Corresponding Logical Channels (Definition->ActiveMode->Channels)
        /// </summary>
        /// this could be just an int (to lookup) but size - resolution matters a lot, so better carry whole info
        public Dictionary<int, GenericChannel> GenericChannels = new();


        public object Value { get;  set; }

        public AbstractValueChannel(string Name)
        {
            this.Name = Name;
        }

        //public abstract Spread<byte> ToBytes();
        

    }

    public class ValueChannel<T> : AbstractValueChannel
    {
        public ValueChannel(string Name):base(Name)
        {

        }

        public void SetValue(T value)
        {
            this.Value = value;
        }

        public T GetValue()=>(T)this.Value;

    }
}
