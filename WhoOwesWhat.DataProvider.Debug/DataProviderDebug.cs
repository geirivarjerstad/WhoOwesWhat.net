using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoOwesWhat.DataProvider.Debug
{
    public interface IDataProviderDebug
    {
        void ResetDatabase();
    }

    public class DataProviderDebug : IDataProviderDebug
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        public DataProviderDebug(IWhoOwesWhatContext whoOwesWhatContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
        }

        public void ResetDatabase()
        {
            _whoOwesWhatContext.ResetDatabase();
        }
    }
}
