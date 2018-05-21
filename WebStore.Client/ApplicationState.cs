using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WebStore
{
    public class ApplicationState
    {
        private long _userCount;

        // Szálbiztos kezelés
        public long UserCount
        {
            get => Interlocked.Read(ref _userCount);
            set => Interlocked.Exchange(ref _userCount, value);
        }
    }
}
