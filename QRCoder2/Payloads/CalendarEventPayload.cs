using System;

namespace QRCoder2.Payloads
{
    public class CalendarEventPayload : PayloadBase
    {
        private readonly string subject, description, location, start, end;
        private readonly EventEncoding encoding;

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
        public CalendarEventPayload(string subject, string description, string location, DateTime start, DateTime end, bool allDayEvent, EventEncoding encoding = EventEncoding.Universal)
        {
            this.subject = subject;
            this.description = description;
            this.location = location;
            this.encoding = encoding;
            string dtFormat = allDayEvent ? "yyyyMMdd" : "yyyyMMddTHHmmss";
            this.start = start.ToString(dtFormat);
            this.end = end.ToString(dtFormat);
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