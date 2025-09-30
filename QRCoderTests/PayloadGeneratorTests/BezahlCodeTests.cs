using System;
using QRCoder;
using Shouldly;
using Xunit;
using static QRCoder.PayloadGenerator.BezahlCode;

namespace QRCoderTests.PayloadGeneratorTests;

public class BezahlCodeTests
{
    [Fact]
    public void bezahlcode_generator_can_generate_payload_singlepayment_minimal()
    {
        var account = "001194700";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10.00m;
#pragma warning disable CS0612
        var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount);
#pragma warning restore CS0612
        generator
            .ToString()
            .ShouldBe("bank://singlepayment?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&amount=10,00&currency=EUR&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "");
    }


    [Fact]
    public void bezahlcode_generator_can_generate_payload_singlepayment_full()
    {
        var account = "001194700";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var amount = 10.00m;
        var postingKey = 69;
        var currency = Currency.USD;
#pragma warning disable CS0612
        var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account, bnc, amount, "", 0, null, null, reason, postingKey, currency, DateTime.Now);
#pragma warning restore CS0612
        generator
            .ToString()
            .ShouldBe("bank://singlepayment?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&postingkey=69&amount=10,00&reason=Thanks%20for%20all%20your%20efforts&currency=USD&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "");
    }


    [Fact]
    public void bezahlcode_generator_can_generate_payload_singledirectdebit()
    {
        var account = "001194700";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var amount = 10.00m;
        var postingKey = 69;
        var currency = Currency.USD;
#pragma warning disable CS0612
        var generator = new PayloadGenerator.BezahlCode(AuthorityType.singledirectdebit, name, account, bnc, amount, "", 0, null, null, reason, postingKey, currency, DateTime.Now);
#pragma warning restore CS0612
        generator
            .ToString()
            .ShouldBe("bank://singledirectdebit?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&postingkey=69&amount=10,00&reason=Thanks%20for%20all%20your%20efforts&currency=USD&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "");
    }


    [Fact]
    public void bezahlcode_generator_can_generate_payload_periodicsinglepayment()
    {
        var account = "001194700";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var amount = 10.00m;
        var postingKey = 69;
        var periodicTimeunit = "W";
        var periodicTimeunitRotation = 2;
        var periodicFirstExecutionDate = DateTime.Now;
        var periodicLastExecutionDate = DateTime.Now.AddMonths(3);
        var currency = Currency.USD;
#pragma warning disable CS0612
        var generator = new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepayment, name, account, bnc, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, reason, postingKey, currency, DateTime.Now);
#pragma warning restore CS0612
        generator
            .ToString()
            .ShouldBe("bank://periodicsinglepayment?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&postingkey=69&amount=10,00&reason=Thanks%20for%20all%20your%20efforts&currency=USD&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "&periodictimeunit=W&periodictimeunitrotation=2&periodicfirstexecutiondate=" + periodicFirstExecutionDate.ToString("ddMMyyyy") + "&periodiclastexecutiondate=" + periodicLastExecutionDate.ToString("ddMMyyyy"));
    }


    [Fact]
    public void bezahlcode_generator_can_generate_payload_singlepaymentsepa_minimal()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10.00m;

        var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount);

        generator
            .ToString()
            .ShouldBe("bank://singlepaymentsepa?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&amount=10,00&currency=EUR&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "");
    }


    [Fact]
    public void bezahlcode_generator_can_generate_payload_singlepaymentsepa_full()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var sepaReference = "Fake SEPA reference";
        var amount = 10.00m;
        var currency = Currency.USD;

        var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban, bic, amount, "", 0, null, null, "", "", new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now);

        generator
            .ToString()
            .ShouldBe("bank://singlepaymentsepa?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&separeference=Fake%20SEPA%20reference&amount=10,00&reason=Thanks%20for%20all%20your%20efforts&currency=USD&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "");
    }


    [Fact]
    public void bezahlcode_generator_can_generate_payload_singledirectdebitsepa()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var creditorId = "DE 02 TSV 01234567890";
        var mandateId = "987543CB2";
        var sepaReference = "Fake SEPA reference";
        var amount = 10.00m;
        var currency = Currency.USD;

        var generator = new PayloadGenerator.BezahlCode(AuthorityType.singledirectdebitsepa, name, iban, bic, amount, "", 0, null, null, creditorId, mandateId, new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now);

        generator
            .ToString()
            .ShouldBe("bank://singledirectdebitsepa?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&separeference=Fake%20SEPA%20reference&creditorid=DE%2002%20TSV%2001234567890&mandateid=987543CB2&dateofsignature=01032017&amount=10,00&reason=Thanks%20for%20all%20your%20efforts&currency=USD&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "");
    }


    [Fact]
    public void bezahlcode_generator_can_generate_payload_periodicsinglepaymentsepa()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var sepaReference = "Fake SEPA reference";
        var amount = 10.00m;
        var periodicTimeunit = "M";
        var periodicTimeunitRotation = 1;
        var periodicFirstExecutionDate = DateTime.Now;
        var periodicLastExecutionDate = DateTime.Now.AddMonths(3);
        var currency = Currency.USD;

        var generator = new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepaymentsepa, name, iban, bic, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, "", "", new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now);

        generator
            .ToString()
            .ShouldBe("bank://periodicsinglepaymentsepa?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&separeference=Fake%20SEPA%20reference&amount=10,00&reason=Thanks%20for%20all%20your%20efforts&currency=USD&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "&periodictimeunit=M&periodictimeunitrotation=1&periodicfirstexecutiondate=" + periodicFirstExecutionDate.ToString("ddMMyyyy") + "&periodiclastexecutiondate=" + periodicLastExecutionDate.ToString("ddMMyyyy"));
    }


    [Fact]
    public void bezahlcode_generator_can_generate_payload_contact()
    {
        var account = "001194700";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";

        var generator = new PayloadGenerator.BezahlCode(AuthorityType.contact, name, account: account, bnc: bnc);

        generator
            .ToString()
            .ShouldBe("bank://contact?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000");
    }


    [Fact]
    public void bezahlcode_generator_can_generate_payload_contact_full()
    {
        var account = "001194700";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";

        var generator = new PayloadGenerator.BezahlCode(AuthorityType.contact, name, account, bnc, "", "", "New business contact.");

        generator
            .ToString()
            .ShouldBe("bank://contact?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&reason=New%20business%20contact.");
    }



    [Fact]
    public void bezahlcode_generator_can_generate_payload_contactv2_classic()
    {
        var account = "001194700";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";

        var generator = new PayloadGenerator.BezahlCode(AuthorityType.contact_v2, name, account: account, bnc: bnc);

        generator
            .ToString()
            .ShouldBe("bank://contact_v2?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000");
    }


    [Fact]
    public void bezahlcode_generator_can_generate_payload_contactv2_sepa()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";

        var generator = new PayloadGenerator.BezahlCode(AuthorityType.contact_v2, name, iban: iban, bic: bic);

        generator
            .ToString()
            .ShouldBe("bank://contact_v2?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER");
    }



    [Fact]
    public void bezahlcode_generator_can_generate_payload_contactv2_sepa_full()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";

        var generator = new PayloadGenerator.BezahlCode(AuthorityType.contact_v2, name, "", "", iban, bic, "A new v2 contact.");

        generator
            .ToString()
            .ShouldBe("bank://contact_v2?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&reason=A%20new%20v2%20contact.");
    }



    [Fact]
    public void bezahlcode_generator_should_handle_account_whitespaces()
    {
        var account = "01 194700";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10.00m;
#pragma warning disable CS0612
        var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount);
#pragma warning restore CS0612
        generator
            .ToString()
            .ShouldBe("bank://singlepayment?name=Wikimedia%20F%C3%B6rdergesellschaft&account=01194700&bnc=100205000&amount=10,00&currency=EUR&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "");
    }


    [Fact]
    public void bezahlcode_generator_should_handle_bnc_whitespaces()
    {
        var account = "001194700";
        var bnc = "10020 5000";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10.00m;
#pragma warning disable CS0612
        var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount);
#pragma warning restore CS0612
        generator
            .ToString()
            .ShouldBe("bank://singlepayment?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&amount=10,00&currency=EUR&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "");
    }


    [Fact]
    public void bezahlcode_generator_should_handle_iban_whitespaces()
    {
        var iban = "DE33 100205000 0011947 00";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10.00m;

        var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount);

        generator
            .ToString()
            .ShouldBe("bank://singlepaymentsepa?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&amount=10,00&currency=EUR&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "");
    }


    [Fact]
    public void bezahlcode_generator_should_handle_bic_whitespaces()
    {
        var iban = "DE33100205000001194700";
        var bic = "BF SWDE3 3BER";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10.00m;

        var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount);

        generator
            .ToString()
            .ShouldBe("bank://singlepaymentsepa?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&amount=10,00&currency=EUR&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "");
    }


    [Fact]
    public void bezahlcode_generator_should_add_decimals()
    {
        var account = "001194700";
        var bnc = "10020 5000";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10;
#pragma warning disable CS0612
        var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount);
#pragma warning restore CS0612
        generator
            .ToString()
            .ShouldBe("bank://singlepayment?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&amount=10,00&currency=EUR&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "");
    }


    [Fact]
    public void bezahlcode_generator_should_throw_wrong_contact_constructor_exception()
    {
        var account = "0001194700";
        var bnc = "10020 5000";
        var name = "Wikimedia Fördergesellschaft";
#pragma warning disable CS0612
        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account, bnc, "", "", "New business contact."));
#pragma warning restore CS0612
        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("The constructor without an amount may only ne used with authority types 'contact' and 'contact_v2'.");
    }


    [Fact]
    public void bezahlcode_generator_should_throw_wrong_contact_v2_constructor_exception()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepaymentsepa, name, iban: iban, bic: bic));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("The constructor without an amount may only ne used with authority types 'contact' and 'contact_v2'.");
    }


    [Fact]
    public void bezahlcode_generator_should_throw_wrong_nonsepa_constructor_exception()
    {
        var account = "0001194700";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, account: account, bnc: bnc, amount: amount));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("The constructor with 'account' and 'bnc' may only be used with 'non SEPA' authority types. Either choose another authority type or switch constructor.");
    }

    [Fact]
    public void bezahlcode_generator_should_throw_wrong_nonsepa_constructor_periodic_exception()
    {
        var account = "0001194700";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var amount = 10.00m;
        var postingKey = 69;
        var periodicTimeunit = "";
        var periodicTimeunitRotation = 2;
        var periodicFirstExecutionDate = DateTime.Now;
        var periodicLastExecutionDate = DateTime.Now.AddMonths(3);
        var currency = Currency.USD;
#pragma warning disable CS0612
        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepayment, name, account, bnc, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, reason, postingKey, currency, DateTime.Now));
#pragma warning restore CS0612
        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("When using 'periodicsinglepayment' as authority type, the parameters 'periodicTimeunit' and 'periodicTimeunitRotation' must be set.");
    }


    [Fact]
    public void bezahlcode_generator_should_throw_wrong_sepa_constructor_exception()
    {
        var iban = "DE33 100205000 0011947 00";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10.00m;
#pragma warning disable CS0612
        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, iban: iban, bic: bic, amount: amount));
#pragma warning restore CS0612
        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("The constructor with 'iban' and 'bic' may only be used with 'SEPA' authority types. Either choose another authority type or switch constructor.");

    }

    [Fact]
    public void bezahlcode_generator_should_throw_wrong_sepa_constructor_periodic_exception()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var sepaReference = "Fake SEPA reference";
        var amount = 10.00m;
        var periodicTimeunit = "M";
        var periodicTimeunitRotation = 0;
        var periodicFirstExecutionDate = DateTime.Now;
        var periodicLastExecutionDate = DateTime.Now.AddMonths(3);
        var currency = Currency.USD;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepaymentsepa, name, iban, bic, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, "", "", new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("When using 'periodicsinglepaymentsepa' as authority type, the parameters 'periodicTimeunit' and 'periodicTimeunitRotation' must be set.");

    }


    [Fact]
    public void bezahlcode_generator_should_throw_name_too_long_exception()
    {
        var iban = "DE33 100205000 0011947 00";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft has really really really long name, over 71 chars";
        var amount = 10.00m;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("(Payee-)Name must be shorter than 71 chars.");

    }


    [Fact]
    public void bezahlcode_generator_should_throw_reason_too_long_exception()
    {
        var iban = "DE33 100205000 0011947 00";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "A long long long reason text which may resolve in an exception" + new string('.', 200);
        var amount = 10.00m;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount, reason: reason));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("Reasons texts have to be shorter than 141 chars.");

    }


    [Fact]
    public void bezahlcode_generator_should_throw_invalid_account_exception()
    {
        var account = "1194700AD";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10.00m;
#pragma warning disable CS0612
        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount));
#pragma warning restore CS0612
        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("The account entered isn't valid.");
    }


    [Fact]
    public void bezahlcode_generator_should_throw_invalid_bnc_exception()
    {
        var account = "001194700";
        var bnc = "10020500023545626226262";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10.00m;
#pragma warning disable CS0612
        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount));
#pragma warning restore CS0612
        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("The bnc entered isn't valid.");
    }


    [Fact]
    public void bezahlcode_generator_should_throw_invalid_postingkey_exception()
    {
        var account = "001194700";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";
        var postingKey = 101;
        var amount = 10.00m;
#pragma warning disable CS0612
        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount, postingKey: postingKey));
#pragma warning restore CS0612
        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("PostingKey must be within 0 and 99.");
    }


    [Fact]
    public void bezahlcode_generator_should_throw_invalid_iban_exception()
    {
        var iban = "DE33100205AZB000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10.00m;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("The IBAN entered isn't valid.");

    }


    [Fact]
    public void bezahlcode_generator_should_throw_invalid_bic_exception()
    {
        var iban = "DE33100205000001194700";
        var bic = "B2FSWDE33BER99871ABC99998";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10.00m;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("The BIC entered isn't valid.");

    }


    [Fact]
    public void bezahlcode_generator_should_throw_separeference_too_long_exception()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var creditorId = "DE 02 TSV 01234567890";
        var mandateId = "987543CB2";
        var sepaReference = "Fake SEPA reference which is also much to long for the reference field.";
        var amount = 10.00m;
        var currency = Currency.USD;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singledirectdebitsepa, name, iban, bic, amount, "", 0, null, null, creditorId, mandateId, new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("SEPA reference texts have to be shorter than 36 chars.");

    }


    [Fact]
    public void bezahlcode_generator_should_throw_invalid_creditorid_exception()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var creditorId = "12DE 02 TSV 01234567890";
        var mandateId = "987543CB2";
        var sepaReference = "Fake SEPA reference.";
        var amount = 10.00m;
        var currency = Currency.USD;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singledirectdebitsepa, name, iban, bic, amount, "", 0, null, null, creditorId, mandateId, new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("The creditorId entered isn't valid.");
    }


    [Fact]
    public void bezahlcode_generator_should_throw_invalid_mandateid_exception()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var creditorId = "DE 02 TSV 01234567890";
        var mandateId = "ÄÖ987543CB2 1990 2017";
        var sepaReference = "Fake SEPA reference.";
        var amount = 10.00m;
        var currency = Currency.USD;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singledirectdebitsepa, name, iban, bic, amount, "", 0, null, null, creditorId, mandateId, new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("The mandateId entered isn't valid.");
    }

    [Fact]
    public void bezahlcode_generator_should_throw_amount_too_much_digits_exception()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 10.001m;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("Amount must have less than 3 digits after decimal point.");

    }


    [Fact]
    public void bezahlcode_generator_should_throw_amount_too_big_exception()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var amount = 1000000000m;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");

    }


    [Fact]
    public void bezahlcode_generator_should_throw_invalid_executiondate_exception()
    {
        var account = "001194700";
        var bnc = "100205000";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var amount = 10.00m;
        var postingKey = 69;
        var executionDate = new DateTime(2017, 1, 1);
        var currency = Currency.USD;
#pragma warning disable CS0612
        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account, bnc, amount, "", 0, null, null, reason, postingKey, currency, executionDate));
#pragma warning restore CS0612
        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("Execution date must be today or in future.");
    }


    [Fact]
    public void bezahlcode_generator_should_throw_invalid_periodictimeunit_exception()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var sepaReference = "Fake SEPA reference";
        var amount = 10.00m;
        var periodicTimeunit = "Z";
        var periodicTimeunitRotation = 1;
        var periodicFirstExecutionDate = DateTime.Now;
        var periodicLastExecutionDate = DateTime.Now.AddMonths(3);
        var currency = Currency.USD;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepaymentsepa, name, iban, bic, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, "", "", new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("The periodicTimeunit must be either 'M' (monthly) or 'W' (weekly).");
    }


    [Fact]
    public void bezahlcode_generator_should_throw_invalid_periodictimeunitrotation_exception()
    {
        var iban = "DE33100205000001194700";
        var bic = "BFSWDE33BER";
        var name = "Wikimedia Fördergesellschaft";
        var reason = "Thanks for all your efforts";
        var sepaReference = "Fake SEPA reference";
        var amount = 10.00m;
        var periodicTimeunit = "M";
        var periodicTimeunitRotation = 128;
        var periodicFirstExecutionDate = DateTime.Now;
        var periodicLastExecutionDate = DateTime.Now.AddMonths(3);
        var currency = Currency.USD;

        var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepaymentsepa, name, iban, bic, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, "", "", new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now));

        Assert.NotNull(exception);
        Assert.IsType<BezahlCodeException>(exception);
        exception.Message.ShouldBe("The periodicTimeunitRotation must be 1 or greater. (It means repeat the payment every 'periodicTimeunitRotation' weeks/months.");
    }
}
