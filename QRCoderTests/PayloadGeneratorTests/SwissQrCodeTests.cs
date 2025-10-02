using System;
using QRCoder;
using Shouldly;
using Xunit;
using static QRCoder.PayloadGenerator.SwissQrCode.AdditionalInformation;
using static QRCoder.PayloadGenerator.SwissQrCode.Reference;
using static QRCoder.QRCodeGenerator;

namespace QRCoderTests.PayloadGeneratorTests;

public class SwissQrCodeTests
{
    [Fact]
    public void swissqrcode_generator_should_throw_reference_not_allowed()
    {
        var refType = ReferenceType.NON;
        var reference = "1234567890123456";
        var refTextType = ReferenceTextType.CreditorReferenceIso11649;

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference, refTextType));

        Assert.NotNull(exception);
        Assert.IsType<SwissQrCodeReferenceException>(exception);
        exception.Message.ShouldBe("Reference is only allowed when referenceType not equals \"NON\"");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_missing_reftexttype()
    {
        var refType = ReferenceType.SCOR;
        var reference = "1234567890123456";

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference));

        Assert.NotNull(exception);
        Assert.IsType<SwissQrCodeReferenceException>(exception);
        exception.Message.ShouldBe("You have to set an ReferenceTextType when using the reference text.");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_qrr_ref_too_long()
    {
        var refType = ReferenceType.QRR;
        var reference = "9900050000000003200710123031234654574398214093682164062138462089364";
        var refTextType = ReferenceTextType.QrReference;

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference, refTextType));

        Assert.NotNull(exception);
        Assert.IsType<SwissQrCodeReferenceException>(exception);
        exception.Message.ShouldBe("QR-references have to be shorter than 28 chars.");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_qrr_ref_wrong_char()
    {
        var refType = ReferenceType.QRR;
        var reference = "99000ABCDF5000032007101230";
        var refTextType = ReferenceTextType.QrReference;

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference, refTextType));

        Assert.NotNull(exception);
        Assert.IsType<SwissQrCodeReferenceException>(exception);
        exception.Message.ShouldBe("QR-reference must exist out of digits only.");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_qrr_ref_checksum_invalid()
    {
        var refType = ReferenceType.QRR;
        var reference = "990005000000000320071012304";
        var refTextType = ReferenceTextType.QrReference;

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference, refTextType));

        Assert.NotNull(exception);
        Assert.IsType<SwissQrCodeReferenceException>(exception);
        exception.Message.ShouldBe("QR-references is invalid. Checksum error.");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_iso11649_ref_too_long()
    {
        var refType = ReferenceType.QRR;
        var reference = "99000500000000032007101230312346545743982162138462089364";
        var refTextType = ReferenceTextType.CreditorReferenceIso11649;

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference, refTextType));

        Assert.NotNull(exception);
        Assert.IsType<SwissQrCodeReferenceException>(exception);
        exception.Message.ShouldBe("Creditor references (ISO 11649) have to be shorter than 26 chars.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_unstructured_msg_too_long()
    {
        var billInformation = "This is sample bill information with a length below 140.";
        var unstructuredMessage = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum";

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.AdditionalInformation(unstructuredMessage, billInformation));

        Assert.NotNull(exception);
        Assert.IsType<SwissQrCodeAdditionalInformationException>(exception);
        exception.Message.ShouldBe("Unstructured message and bill information must be shorter than 141 chars in total/combined.");
    }



    [Fact]
    public void swissqrcode_generator_should_generate_iban()
    {
        var iban = "CH2609000000857666015";
        var ibanType = PayloadGenerator.SwissQrCode.Iban.IbanType.Iban;

        var generator = new PayloadGenerator.SwissQrCode.Iban(iban, ibanType);

        generator
            .ToString()
            .ShouldBe("CH2609000000857666015");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_iban_2()
    {
        var iban = "CH47048350000GABRIELS";
        var ibanType = PayloadGenerator.SwissQrCode.Iban.IbanType.Iban;

        var generator = new PayloadGenerator.SwissQrCode.Iban(iban, ibanType);

        generator
            .ToString()
            .ShouldBe("CH47048350000GABRIELS");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_iban_qr()
    {
        var iban = "CH2430043000000789012";
        var ibanType = PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban;

        var generator = new PayloadGenerator.SwissQrCode.Iban(iban, ibanType);

        generator
            .ToString()
            .ShouldBe("CH2430043000000789012");
    }

    [Fact]
    public void swissqrcode_generator_should_remove_spaces_iban()
    {
        var iban = "CH26 0900 0000 8576 6601 5";
        var ibanType = PayloadGenerator.SwissQrCode.Iban.IbanType.Iban;

        var generator = new PayloadGenerator.SwissQrCode.Iban(iban, ibanType);

        generator
            .ToString()
            .ShouldBe("CH2609000000857666015");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_invalid_iban()
    {
        var iban = "CHC2609000000857666015";
        var ibanType = PayloadGenerator.SwissQrCode.Iban.IbanType.Iban;

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Iban(iban, ibanType));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Iban.SwissQrCodeIbanException>(exception);
        exception.Message.ShouldBe("The IBAN entered isn't valid.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_invalid_qriban()
    {
        var iban = "CHC2609000000857666015";
        var ibanType = PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban;

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Iban(iban, ibanType));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Iban.SwissQrCodeIbanException>(exception);
        exception.Message.ShouldBe("The QR-IBAN entered isn't valid.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_ivalid_iban_country()
    {
        var iban = "DE2609000000857666015";
        var ibanType = PayloadGenerator.SwissQrCode.Iban.IbanType.Iban;

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Iban(iban, ibanType));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Iban.SwissQrCodeIbanException>(exception);
        exception.Message.ShouldBe("The IBAN must start with \"CH\" or \"LI\".");
    }



    [Fact]
    public void swissqrcode_generator_should_generate_contact_simple()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";

        var generator = PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, null, null);

        generator
            .ToString()
            .ShouldBe("S\r\nJohn Doe\r\n\r\n\r\n3003\r\nBern\r\nCH\r\n");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_contact_full()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var generator = PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber);

        generator
            .ToString()
            .ShouldBe("S\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_name_empty()
    {
        var name = "";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
        exception.Message.ShouldBe("Name must not be empty.");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_name_too_long()
    {
        var name = "John Dorian Peter Charles Lord of the Rings and Master of Disaster Grayham";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
        exception.Message.ShouldBe("Name must be shorter than 71 chars.");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_street_too_long()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude in der wunderschönen aber auch ziemlich teuren Stadt Bern in der Schweiz";
        var houseNumber = "1";

        var exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
        exception.Message.ShouldBe("Street must be shorter than 71 chars.");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_street_with_illegal_char()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude 1 ♥";
        var houseNumber = "1";

        var exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
        exception.Message.ShouldBe(@"Street must match the following pattern as defined in pain.001: ^([a-zA-Z0-9\.,;:'\ \+\-/\(\)?\*\[\]\{\}\\`´~ ^|]|[!""#%&<>÷=@_$£¡¢¤¥¦§¨©ª«¬®¯°±²³µ¶·¸¹º»¼½¾¿×Ø€]|[àáâäãåāăąçćĉċčďđèéêëēĕėęěĝğġģĥħìíîïĩīĭįıĳķĸĺļľŀłñńņňŉŋòóôöōŏőõŕŗřśŝşšșţťŧțùúûüũūŭůűųŵýÿŷźżžßÀÁÂÄÃÅĀĂĄÇĆĈĊČĎĐÈÉÊËĒĔĖĘĚĜĞĠĢĤĦÌÍÎÏĨĪĬĮİĲĴĵĶĹĻĽĿŁÑŃŅŇŊÒÓÔÖÕŌŎŐŔŖŘŚŜŞŠȘŢŤŦȚÙÚÛÜŨŪŬŮŰŲŴÝŶŸŹŻŽÆÐÞæðøþŒœſ])*$");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_housenumber_too_long()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "123456789123456789";

        var exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
        exception.Message.ShouldBe("House number must be shorter than 17 chars.");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_zip_empty()
    {
        var name = "John Doe";
        var zip = "";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
        exception.Message.ShouldBe("Zip code must not be empty.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_zip_too_long()
    {
        var name = "John Doe";
        var zip = "30031234567891234";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
        exception.Message.ShouldBe("Zip code must be shorter than 17 chars.");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_zip_has_illegal_char()
    {
        var name = "John Doe";
        var zip = "3003CHF♥";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
        exception.Message.ShouldBe(@"Zip code must match the following pattern as defined in pain.001: ^([a-zA-Z0-9\.,;:'\ \+\-/\(\)?\*\[\]\{\}\\`´~ ^|]|[!""#%&<>÷=@_$£¡¢¤¥¦§¨©ª«¬®¯°±²³µ¶·¸¹º»¼½¾¿×Ø€]|[àáâäãåāăąçćĉċčďđèéêëēĕėęěĝğġģĥħìíîïĩīĭįıĳķĸĺļľŀłñńņňŉŋòóôöōŏőõŕŗřśŝşšșţťŧțùúûüũūŭůűųŵýÿŷźżžßÀÁÂÄÃÅĀĂĄÇĆĈĊČĎĐÈÉÊËĒĔĖĘĚĜĞĠĢĤĦÌÍÎÏĨĪĬĮİĲĴĵĶĹĻĽĿŁÑŃŅŇŊÒÓÔÖÕŌŎŐŔŖŘŚŜŞŠȘŢŤŦȚÙÚÛÜŨŪŬŮŰŲŴÝŶŸŹŻŽÆÐÞæðøþŒœſ])*$");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_city_empty()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
        exception.Message.ShouldBe("City must not be empty.");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_city_too_long()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Berner-Sangerhausen-Ober-Hinter-der-Alm-Stadt-am-Unter-Über-Berg";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
        exception.Message.ShouldBe("City name must be shorter than 36 chars.");
    }


    [Fact]
    public void swissqrcode_generator_should_throw_wrong_countrycode()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Bern";
        var country = "CHE";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
        exception.Message.ShouldBe("Country must be a valid \"two letter\" country code as defined by ISO 3166-1, but it isn't.");
    }



    [Fact]
    public void swissqrcode_generator_should_generate_swisscode_simple()
    {
        var creditor = PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new PayloadGenerator.SwissQrCode.Iban("CH2430043000000789012", PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban);
        var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR, "990005000000000320071012303", ReferenceTextType.QrReference);
        var currency = PayloadGenerator.SwissQrCode.Currency.EUR;

        var generator = new PayloadGenerator.SwissQrCode(iban, currency, creditor, reference);

        generator
            .ToString()
            .ShouldBe("SPC\r\n0200\r\n1\r\nCH2430043000000789012\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nEUR\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nQRR\r\n990005000000000320071012303\r\n\r\nEPD");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_swisscode_full()
    {
        var contactGeneral = PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new PayloadGenerator.SwissQrCode.Iban("CH2430043000000789012", PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban);
        var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR, "990005000000000320071012303", ReferenceTextType.QrReference);
        var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
        var additionalInformation = new PayloadGenerator.SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var amount = 100.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);

        var generator = new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, additionalInformation, contactGeneral, amount, reqDateOfPayment, contactGeneral);

        generator
            .ToString()
            .ShouldBe("SPC\r\n0200\r\n1\r\nCH2430043000000789012\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n100.25\r\nCHF\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\nQRR\r\n990005000000000320071012303\r\nThis is my unstructured message.\r\nEPD\r\nSome bill information here...");
    }

    [Fact]
    public void swissqrcode_generator_sets_encoding_parameters()
    {
        var creditor = PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new PayloadGenerator.SwissQrCode.Iban("CH2430043000000789012", PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban);
        var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR, "990005000000000320071012303", ReferenceTextType.QrReference);
        var currency = PayloadGenerator.SwissQrCode.Currency.EUR;

        var payload = new PayloadGenerator.SwissQrCode(iban, currency, creditor, reference);

        payload.EccLevel.ShouldBe<ECCLevel>(ECCLevel.M);
        payload.EciMode.ShouldBe<EciMode>(EciMode.Utf8);
        payload.Version.ShouldBe(-1);
    }

    [Fact]
    public void swissqrcode_generator_should_generate_clean_end_linebreaks()
    {
        var contactGeneral = PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new PayloadGenerator.SwissQrCode.Iban("CH2430043000000789012", PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban);
        var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR, "990005000000000320071012303", ReferenceTextType.QrReference);
        var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
        var additionalInformation = new PayloadGenerator.SwissQrCode.AdditionalInformation("This is my unstructured message.");
        var amount = 100.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);

        var generator = new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, additionalInformation, contactGeneral, amount, reqDateOfPayment, contactGeneral);

        generator
            .ToString()
            .ShouldBe("SPC\r\n0200\r\n1\r\nCH2430043000000789012\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n100.25\r\nCHF\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\nQRR\r\n990005000000000320071012303\r\nThis is my unstructured message.\r\nEPD");
    }


    [Fact]
    public void swissqrcode_generator_should_generate_swisscode_full_alt()
    {
        var contactGeneral = PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new PayloadGenerator.SwissQrCode.Iban("CH2430043000000789012", PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban);
        var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR, "990005000000000320071012303", ReferenceTextType.QrReference);
        var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
        var additionalInformation = new PayloadGenerator.SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var amount = 100.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);

        var generator = new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, additionalInformation, contactGeneral, amount, reqDateOfPayment, contactGeneral, "alt1", "alt2");

        generator
            .ToString()
            .ShouldBe("SPC\r\n0200\r\n1\r\nCH2430043000000789012\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n100.25\r\nCHF\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\nQRR\r\n990005000000000320071012303\r\nThis is my unstructured message.\r\nEPD\r\nSome bill information here...\r\nalt1\r\nalt2");
    }

    [Fact]
    public void swissqrcode_generator_should_not_generate_space_as_thousands_separator()
    {
        var contactGeneral = PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new PayloadGenerator.SwissQrCode.Iban("CH2609000000857666015", PayloadGenerator.SwissQrCode.Iban.IbanType.Iban);
        var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.SCOR, "99000500000000032003", ReferenceTextType.CreditorReferenceIso11649);
        var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
        var additionalInformation = new PayloadGenerator.SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var amount = 1234567.89m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);

        var generator = new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, additionalInformation, contactGeneral, amount, reqDateOfPayment, contactGeneral, "alt1", "alt2");

        generator
            .ToString()
            .ShouldBe("SPC\r\n0200\r\n1\r\nCH2609000000857666015\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n1234567.89\r\nCHF\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\nSCOR\r\n99000500000000032003\r\nThis is my unstructured message.\r\nEPD\r\nSome bill information here...\r\nalt1\r\nalt2");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_amount_too_big()
    {
        var contactGeneral = PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new PayloadGenerator.SwissQrCode.Iban("CH2609000000857666015", PayloadGenerator.SwissQrCode.Iban.IbanType.Iban);
        var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR, "990005000000000320071012303", ReferenceTextType.QrReference);
        var additionalInformation = new PayloadGenerator.SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
        var amount = 1234567891.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, additionalInformation, contactGeneral, amount, reqDateOfPayment, contactGeneral));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.SwissQrCodeException>(exception);
        exception.Message.ShouldBe("Amount (including decimals) must be shorter than 13 places.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_incompatible_reftype()
    {
        var contactGeneral = PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new PayloadGenerator.SwissQrCode.Iban("CH2430043000000789012", PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban);
        var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.NON);
        var additionalInformation = new PayloadGenerator.SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
        var amount = 100.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, additionalInformation, contactGeneral, amount, reqDateOfPayment, contactGeneral));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.SwissQrCodeException>(exception);
        exception.Message.ShouldBe("If QR-IBAN is used, you have to choose \"QRR\" as reference type!");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_alt1_too_long()
    {
        var contactGeneral = PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new PayloadGenerator.SwissQrCode.Iban("CH2430043000000789012", PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban);
        var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR);
        var additionalInformation = new PayloadGenerator.SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
        var amount = 100.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);
        var alt1 = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean ma";

        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, additionalInformation, contactGeneral, amount, reqDateOfPayment, contactGeneral, alt1));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.SwissQrCodeException>(exception);
        exception.Message.ShouldBe("Alternative procedure information block 1 must be shorter than 101 chars.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_alt2_too_long()
    {
        var contactGeneral = PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new PayloadGenerator.SwissQrCode.Iban("CH2430043000000789012", PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban);
        var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR);
        var additionalInformation = new PayloadGenerator.SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
        var amount = 100.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);
        var alt1 = "lorem ipsum";
        var alt2 = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean ma";
        var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, additionalInformation, contactGeneral, amount, reqDateOfPayment, contactGeneral, alt1, alt2));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.SwissQrCodeException>(exception);
        exception.Message.ShouldBe("Alternative procedure information block 2 must be shorter than 101 chars.");
    }

    [Fact]
    public void swissqrcode_generator_should_validate_two_lettercodes()
    {
        string name = "John Doe";
        string zip = "12345";
        string city = "Gotham City";

        // Should work, as DE is a valid country code
        string country = "DE";
        var exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country));
        Assert.Null(exception);

        // Should work, as de is a valid country code and case should be ignored
        country = "de";
        exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country));
        Assert.Null(exception);

        // Should work, as XK is is defined as special case (not officially ISO-3166-1,but used in the wild)
        // See https://en.wikipedia.org/wiki/XK_(user_assigned_code) and https://github.com/Shane32/QRCoder/issues/420
        country = "XK";
        exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country));
        Assert.Null(exception);


        // Should throw exception, as ZZ isn't a valid country code
        country = "ZZ";
        exception = Record.Exception(() => PayloadGenerator.SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
        exception.Message.ShouldBe("Country must be a valid \"two letter\" country code as defined by ISO 3166-1, but it isn't.");
    }
}
