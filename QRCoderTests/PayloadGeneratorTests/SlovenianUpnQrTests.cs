namespace QRCoderTests.PayloadGeneratorTests;

public class SlovenianUpnQrTests
{
    [Fact]
    public void slovenian_upnqr_generator_should_generate_payload_simple()
    {
        var payerName = "Janez Novak";
        var payerAddress = "Slovenska cesta 1";
        var payerPlace = "1000 Ljubljana";
        var recipientName = "Podjetje d.o.o.";
        var recipientAddress = "Trzaska cesta 10";
        var recipientPlace = "2000 Maribor";
        var recipientIban = "SI56011006001234567";
        var description = "Payment for services";
        var amount = 150.50;

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount);

        generator
            .ToString()
            .ShouldBe("UPNQR\n\n\n\n\nJanez Novak\nSlovenska cesta 1\n1000 Ljubljana\n00000015050\n\n\nOTHR\nPayment for services\n\nSI56011006001234567\nSI00\nPodjetje d.o.o.\nTrzaska cesta 10\n2000 Maribor\n167\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_generate_payload_with_deadline()
    {
        var payerName = "Marko Kovac";
        var payerAddress = "Celovska cesta 20";
        var payerPlace = "1000 Ljubljana";
        var recipientName = "Test Company";
        var recipientAddress = "Main Street 5";
        var recipientPlace = "3000 Celje";
        var recipientIban = "SI56020170014356205";
        var description = "Invoice payment";
        var amount = 99.99;
        var deadline = new DateTime(2024, 12, 31);

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount, deadline);

        var payload = generator.ToString();

        payload.ShouldContain("31.12.2024\n");
        payload.ShouldContain("00000009999\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_generate_payload_with_custom_code()
    {
        var payerName = "Ana Horvat";
        var payerAddress = "Dunajska cesta 100";
        var payerPlace = "1000 Ljubljana";
        var recipientName = "Service Provider";
        var recipientAddress = "Business Park 1";
        var recipientPlace = "4000 Kranj";
        var recipientIban = "SI56031234567890123";
        var description = "Monthly subscription";
        var amount = 25.00;
        var code = "GDSV";

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount,
            recipientSiModel: "SI00", recipientSiReference: "", code: code);

        var payload = generator.ToString();

        payload.ShouldContain("GDSV\n");
        payload.ShouldContain("00000002500\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_generate_payload_with_reference()
    {
        var payerName = "Peter Zupan";
        var payerAddress = "Smartinska cesta 50";
        var payerPlace = "1000 Ljubljana";
        var recipientName = "Utility Company";
        var recipientAddress = "Energy Street 15";
        var recipientPlace = "1000 Ljubljana";
        var recipientIban = "SI56191234567890123";
        var description = "Electricity bill";
        var amount = 75.25;
        var recipientSiModel = "SI12";
        var recipientSiReference = "00012345678901234";

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount,
            recipientSiModel: recipientSiModel, recipientSiReference: recipientSiReference);

        var payload = generator.ToString();

        payload.ShouldContain("SI1200012345678901234\n");
        payload.ShouldContain("00000007525\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_limit_payer_name_length()
    {
        var payerName = "This is a very long name that exceeds the maximum allowed length of 33 characters";
        var payerAddress = "Address 1";
        var payerPlace = "City";
        var recipientName = "Recipient";
        var recipientAddress = "Recipient Address";
        var recipientPlace = "Recipient City";
        var recipientIban = "SI56011006001234567";
        var description = "Payment";
        var amount = 100.00;

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount);

        var payload = generator.ToString();

        // Name should be truncated to 33 characters
        payload.ShouldContain("This is a very long name that exc\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_limit_description_length()
    {
        var payerName = "Payer";
        var payerAddress = "Address";
        var payerPlace = "City";
        var recipientName = "Recipient";
        var recipientAddress = "Recipient Address";
        var recipientPlace = "Recipient City";
        var recipientIban = "SI56011006001234567";
        var description = "This is a very long description that exceeds the maximum allowed length of 42 characters for the purpose field";
        var amount = 50.00;

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount);

        var payload = generator.ToString();

        // Description should be truncated to 42 characters
        payload.ShouldContain("This is a very long description that excee\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_format_amount_correctly()
    {
        var payerName = "Payer";
        var payerAddress = "Address";
        var payerPlace = "City";
        var recipientName = "Recipient";
        var recipientAddress = "Recipient Address";
        var recipientPlace = "Recipient City";
        var recipientIban = "SI56011006001234567";
        var description = "Payment";

        // Test various amounts
        var generator1 = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, 0.01);
        generator1.ToString().ShouldContain("00000000001\n");

        var generator2 = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, 1.00);
        generator2.ToString().ShouldContain("00000000100\n");

        var generator3 = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, 1234.56);
        generator3.ToString().ShouldContain("00000123456\n");

        var generator4 = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, 999999.99);
        generator4.ToString().ShouldContain("00099999999\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_calculate_checksum_correctly()
    {
        var payerName = "Test Payer";
        var payerAddress = "Test Address";
        var payerPlace = "Test City";
        var recipientName = "Test Recipient";
        var recipientAddress = "Test Recipient Address";
        var recipientPlace = "Test Recipient City";
        var recipientIban = "SI56011006001234567";
        var description = "Test Payment";
        var amount = 100.00;

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount);

        var payload = generator.ToString();

        // Checksum should be at the end as a 3-digit number
        var lines = payload.Split('\n');
        var checksumLine = lines[lines.Length - 2]; // Second to last line (last is empty)
        checksumLine.Length.ShouldBe(3);
        int.TryParse(checksumLine, out _).ShouldBeTrue();
    }

    [Fact]
    public void slovenian_upnqr_generator_should_trim_whitespace()
    {
        var payerName = "  Payer Name  ";
        var payerAddress = "  Address  ";
        var payerPlace = "  City  ";
        var recipientName = "  Recipient  ";
        var recipientAddress = "  Recipient Address  ";
        var recipientPlace = "  Recipient City  ";
        var recipientIban = "  SI56011006001234567  ";
        var description = "  Payment  ";
        var amount = 50.00;

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount);

        var payload = generator.ToString();

        payload.ShouldContain("Payer Name\n");
        payload.ShouldContain("Address\n");
        payload.ShouldContain("City\n");
        payload.ShouldContain("SI56011006001234567\n");
        payload.ShouldContain("Payment\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_uppercase_code_and_iban()
    {
        var payerName = "Payer";
        var payerAddress = "Address";
        var payerPlace = "City";
        var recipientName = "Recipient";
        var recipientAddress = "Recipient Address";
        var recipientPlace = "Recipient City";
        var recipientIban = "si56011006001234567";
        var description = "Payment";
        var amount = 100.00;
        var code = "othr";

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount,
            code: code);

        var payload = generator.ToString();

        payload.ShouldContain("OTHR\n");
        payload.ShouldContain("SI56011006001234567\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_set_correct_qr_properties()
    {
        var payerName = "Payer";
        var payerAddress = "Address";
        var payerPlace = "City";
        var recipientName = "Recipient";
        var recipientAddress = "Recipient Address";
        var recipientPlace = "Recipient City";
        var recipientIban = "SI56011006001234567";
        var description = "Payment";
        var amount = 100.00;

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount);

        generator.Version.ShouldBe(15);
        generator.EccLevel.ShouldBe(QRCodeGenerator.ECCLevel.M);
        generator.EciMode.ShouldBe(QRCodeGenerator.EciMode.Iso8859_2);
    }

    [Fact]
    public void slovenian_upnqr_generator_should_handle_zero_amount()
    {
        var payerName = "Payer";
        var payerAddress = "Address";
        var payerPlace = "City";
        var recipientName = "Recipient";
        var recipientAddress = "Recipient Address";
        var recipientPlace = "Recipient City";
        var recipientIban = "SI56011006001234567";
        var description = "Payment";
        var amount = 0.00;

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount);

        var payload = generator.ToString();

        payload.ShouldContain("00000000000\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_limit_iban_length()
    {
        var payerName = "Payer";
        var payerAddress = "Address";
        var payerPlace = "City";
        var recipientName = "Recipient";
        var recipientAddress = "Recipient Address";
        var recipientPlace = "Recipient City";
        var recipientIban = "SI56011006001234567890123456789012345"; // Too long
        var description = "Payment";
        var amount = 100.00;

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount);

        var payload = generator.ToString();

        // IBAN should be truncated to 34 characters
        payload.ShouldContain("SI56011006001234567890123456789012\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_limit_reference_length()
    {
        var payerName = "Payer";
        var payerAddress = "Address";
        var payerPlace = "City";
        var recipientName = "Recipient";
        var recipientAddress = "Recipient Address";
        var recipientPlace = "Recipient City";
        var recipientIban = "SI56011006001234567";
        var description = "Payment";
        var amount = 100.00;
        var recipientSiModel = "SI12";
        var recipientSiReference = "123456789012345678901234567890"; // Too long

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount,
            recipientSiModel: recipientSiModel, recipientSiReference: recipientSiReference);

        var payload = generator.ToString();

        // Reference should be truncated to 22 characters
        payload.ShouldContain("SI121234567890123456789012\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_handle_empty_deadline()
    {
        var payerName = "Payer";
        var payerAddress = "Address";
        var payerPlace = "City";
        var recipientName = "Recipient";
        var recipientAddress = "Recipient Address";
        var recipientPlace = "Recipient City";
        var recipientIban = "SI56011006001234567";
        var description = "Payment";
        var amount = 100.00;

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount,
            deadline: null);

        var payload = generator.ToString();

        // Should have empty line for deadline
        var lines = payload.Split('\n');
        lines[13].ShouldBe(""); // Deadline line should be empty
    }

    [Fact]
    public void slovenian_upnqr_generator_should_uppercase_si_model()
    {
        var payerName = "Payer";
        var payerAddress = "Address";
        var payerPlace = "City";
        var recipientName = "Recipient";
        var recipientAddress = "Recipient Address";
        var recipientPlace = "Recipient City";
        var recipientIban = "SI56011006001234567";
        var description = "Payment";
        var amount = 100.00;
        var recipientSiModel = "si12";

        var generator = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, amount,
            recipientSiModel: recipientSiModel);

        var payload = generator.ToString();

        payload.ShouldContain("SI12\n");
    }

    [Fact]
    public void slovenian_upnqr_generator_should_round_amount_correctly()
    {
        var payerName = "Payer";
        var payerAddress = "Address";
        var payerPlace = "City";
        var recipientName = "Recipient";
        var recipientAddress = "Recipient Address";
        var recipientPlace = "Recipient City";
        var recipientIban = "SI56011006001234567";
        var description = "Payment";

        // Test rounding
        var generator1 = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, 10.555);
        generator1.ToString().ShouldContain("00000001056\n"); // Rounds to 10.56

        var generator2 = new PayloadGenerator.SlovenianUpnQr(
            payerName, payerAddress, payerPlace,
            recipientName, recipientAddress, recipientPlace,
            recipientIban, description, 10.554);
        generator2.ToString().ShouldContain("00000001055\n"); // Rounds to 10.55
    }
}