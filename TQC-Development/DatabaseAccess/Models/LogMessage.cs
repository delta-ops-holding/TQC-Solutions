using System;
using System.Collections.Generic;
using System.Text;
using DatabaseAccess.Enums;

namespace DatabaseAccess.Models
{
    public struct LogMessage
    {
        public LogMessage(LogSeverity logSeverity, string source, string message, string createdBy, DateTime createdDate)
        {
            LogSeverity = logSeverity;
            Source = source;
            Message = message;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
        }

        public readonly LogSeverity LogSeverity { get; }
        public readonly string Source { get; }
        public readonly string Message { get; }
        public readonly string CreatedBy { get; }
        public readonly DateTime CreatedDate { get; }
    }
}
