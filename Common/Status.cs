using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class Status
    {
        public const short InComplete = -1;
        public const short Pending = 0;
        public const short Active = 1;
        public const short Locked = 2;
        public const short Approved = 3;
        public const short Rejected = 4;
        public const short PartiallyFailed = 5;
        public const short InProcess = 6;
        public const short TemproryLocked = 7;
        public const short Used = 8;
        public const short Deleted = 9;
        public const short Failed = 10;
    }
}
