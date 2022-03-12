using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class OrderStatus
    {
        public const short Pending = 0;
        public const short paid = 1;
        public const short Process = 3;
        public const short Cancel = 4;
        public const short Completed = 5;
    }
}
