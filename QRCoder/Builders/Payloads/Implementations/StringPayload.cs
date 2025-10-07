namespace QRCoder.Builders.Payloads.Implementations;

public class StringPayload : PayloadBase
{
    private string _data;

    public StringPayload(string data)
    {
        _data = data;
    }

    protected override string Value => _data;
}
