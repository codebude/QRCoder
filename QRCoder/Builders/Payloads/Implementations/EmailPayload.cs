using System;

namespace QRCoder.Builders.Payloads.Implementations;

public class EmailPayload : PayloadBase
{
    public EmailPayload(string address)
    {
        _address = address;
    }

    private readonly string _address { get; set; }
    private string? _subject { get; set; }
    private string? _body { get; set; }
    private PayloadGenerator.Mail.MailEncoding _encoding { get; set; } = PayloadGenerator.Mail.MailEncoding.MAILTO;

    public EmailPayload WithSubject(string? subject)
    {
        _subject = subject;
        return this;
    }

    public EmailPayload WithBody(string? body)
    {
        _body = body;
        return this;
    }

    public EmailPayload WithEncoding(PayloadGenerator.Mail.MailEncoding encoding)
    {
        if (encoding != PayloadGenerator.Mail.MailEncoding.MAILTO && encoding != PayloadGenerator.Mail.MailEncoding.SMTP && encoding != PayloadGenerator.Mail.MailEncoding.MATMSG)
        {
            throw new ArgumentOutOfRangeException(nameof(encoding));
        }
        _encoding = encoding;
        return this;
    }

    protected override string Value => new PayloadGenerator.Mail(_address, _subject, _body, _encoding).ToString();
}
