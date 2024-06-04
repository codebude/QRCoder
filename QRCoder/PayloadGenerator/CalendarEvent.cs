using System;

namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a calendar entry/event payload.
    /// </summary>
    public class CalendarEvent : Payload
    {
        private readonly string subject, start, end;
        private readonly string? description, location;
        private readonly EventEncoding encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarEvent"/> class.
        /// </summary>
        /// <param name="subject">Subject/title of the calendar event.</param>
        /// <param name="description">Description of the event.</param>
        /// <param name="location">Location (latitude:longitude or address) of the event.</param>
        /// <param name="start">Start time (including UTC offset) of the event.</param>
        /// <param name="end">End time (including UTC offset) of the event.</param>
        /// <param name="allDayEvent">Indicates if it is a full day event.</param>
        /// <param name="encoding">Type of encoding (universal or iCal).</param>
        public CalendarEvent(string subject, string? description, string? location, DateTimeOffset start, DateTimeOffset end, bool allDayEvent, EventEncoding encoding = EventEncoding.Universal) : this(subject, description, location, start.UtcDateTime, end.UtcDateTime, allDayEvent, encoding)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarEvent"/> class.
        /// </summary>
        /// <param name="subject">Subject/title of the calendar event.</param>
        /// <param name="description">Description of the event.</param>
        /// <param name="location">Location (latitude:longitude or address) of the event.</param>
        /// <param name="start">Start time of the event.</param>
        /// <param name="end">End time of the event.</param>
        /// <param name="allDayEvent">Indicates if it is a full day event.</param>
        /// <param name="encoding">Type of encoding (universal or iCal).</param>
        public CalendarEvent(string subject, string? description, string? location, DateTime start, DateTime end, bool allDayEvent, EventEncoding encoding = EventEncoding.Universal)
        {
            this.subject = subject;
            this.description = description;
            this.location = location;
            this.encoding = encoding;
            string dtFormatStart = "yyyyMMdd", dtFormatEnd = "yyyyMMdd";
            if (!allDayEvent)
            {
                dtFormatStart = dtFormatEnd = "yyyyMMddTHHmmss";
                if (start.Kind == DateTimeKind.Utc)
                    dtFormatStart = "yyyyMMddTHHmmssZ";
                if (end.Kind == DateTimeKind.Utc)
                    dtFormatEnd = "yyyyMMddTHHmmssZ";
            }
            this.start = start.ToString(dtFormatStart);
            this.end = end.ToString(dtFormatEnd);
        }

        /// <summary>
        /// Returns a string representation of the calendar event payload.
        /// </summary>
        /// <returns>A string representation of the calendar event in the VEVENT format.</returns>
        public override string ToString()
        {
            var vEvent = $"BEGIN:VEVENT{Environment.NewLine}";
            vEvent += $"SUMMARY:{subject}{Environment.NewLine}";
            vEvent += !string.IsNullOrEmpty(description) ? $"DESCRIPTION:{description}{Environment.NewLine}" : "";
            vEvent += !string.IsNullOrEmpty(location) ? $"LOCATION:{location}{Environment.NewLine}" : "";
            vEvent += $"DTSTART:{start}{Environment.NewLine}";
            vEvent += $"DTEND:{end}{Environment.NewLine}";
            vEvent += "END:VEVENT";

            if (encoding == EventEncoding.iCalComplete)
                vEvent = $@"BEGIN:VCALENDAR{Environment.NewLine}VERSION:2.0{Environment.NewLine}{vEvent}{Environment.NewLine}END:VCALENDAR";

            return vEvent;
        }

        /// <summary>
        /// Specifies the encoding type for the calendar event.
        /// </summary>
        public enum EventEncoding
        {
            /// <summary>
            /// iCalendar complete encoding.
            /// </summary>
            iCalComplete,

            /// <summary>
            /// Universal encoding.
            /// </summary>
            Universal
        }
    }
}
