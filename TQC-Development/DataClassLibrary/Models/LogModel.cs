using DataClassLibrary.Enums;
using System;

namespace DataClassLibrary.Models
{
    public struct LogModel
    {
        public LogModel(LoggingSeverity logSeverity, string source, string message, string createdBy, DateTime createdDateTime)
        {
            LogSeverity = logSeverity;
            Source = source;
            Message = message;
            CreatedBy = createdBy;
            CreatedDateTime = createdDateTime;
        }

        public readonly LoggingSeverity LogSeverity { get; }

        public readonly string Source { get; }

        public readonly string Message { get; }

        public readonly string CreatedBy { get; }

        public readonly DateTime CreatedDateTime { get; }
    }
}
