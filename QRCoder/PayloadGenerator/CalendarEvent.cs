using System;

namespace QRCoder
{
    public static partial class PayloadGenerator
    {
        public class CalendarEvent : Payload
        {
            private readonly string subject, start, end;
            private readonly string? description, location;
            private readonly EventEncoding encoding;

            /// <summary>
            /// Generates a calender entry/event payload.
            /// </summary>
            /// <param name="subject">Subject/title of the calender event</param>
            /// <param name="description">Description of the event</param>
            /// <param name="location">Location (lat:long or address) of the event</param>
            /// <param name="start">Start time (incl. UTC offset) of the event</param>
            /// <param name="end">End time (incl. UTC offset) of the event</param>
            /// <param name="allDayEvent">Is it a full day event?</param>
            /// <param name="encoding">Type of encoding (universal or iCal)</param>
            public CalendarEvent(string subject, string? description, string? location, DateTimeOffset start, DateTimeOffset end, bool allDayEvent, EventEncoding encoding = EventEncoding.Universal) : this(subject, description, location, start.UtcDateTime, end.UtcDateTime, allDayEvent, encoding)
            {
            }

            /// <summary>
            /// Generates a calender entry/event payload.
            /// </summary>
            /// <param name="subject">Subject/title of the calender event</param>
            /// <param name="description">Description of the event</param>
            /// <param name="location">Location (lat:long or address) of the event</param>
            /// <param name="start">Start time of the event</param>
            /// <param name="end">End time of the event</param>
            /// <param name="allDayEvent">Is it a full day event?</param>
            /// <param name="encoding">Type of encoding (universal or iCal)</param>
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

            public override string ToString()
            {
                var vEvent = $"BEGIN:VEVENT{Environment.NewLine}";
                vEvent += $"SUMMARY:{this.subject}{Environment.NewLine}";
                vEvent += !string.IsNullOrEmpty(this.description) ? $"DESCRIPTION:{this.description}{Environment.NewLine}" : "";
                vEvent += !string.IsNullOrEmpty(this.location) ? $"LOCATION:{this.location}{Environment.NewLine}" : "";
                vEvent += $"DTSTART:{this.start}{Environment.NewLine}";
                vEvent += $"DTEND:{this.end}{Environment.NewLine}";
                vEvent += "END:VEVENT";

                if (this.encoding == EventEncoding.iCalComplete)
                    vEvent = $@"BEGIN:VCALENDAR{Environment.NewLine}VERSION:2.0{Environment.NewLine}{vEvent}{Environment.NewLine}END:VCALENDAR";

                return vEvent;
            }

            public enum EventEncoding
            {
                iCalComplete,
                Universal
            }
        }
    }
}
