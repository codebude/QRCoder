using System;
using QRCoder;
using Shouldly;
using Xunit;

namespace QRCoderTests.PayloadGeneratorTests;

public class RussiaPaymentOrderTests
{
    [Fact]
    public void russiapayment_generator_can_generate_payload_mandatory_fields()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "ОАО \"БАНК\"";
        var name = "ООО «Три кита»";
        var correspAcc = "30101810965770000413";
        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc);

        generator
            .ToString()
            .ShouldBe($"ST00012|Name={name}|PersonalAcc={account}|BankName={bankName}|BIC={bic}|CorrespAcc={correspAcc}|");
    }

    [Fact]
    public void russiapayment_generator_can_generate_payload_encoding_win1251()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "ОАО \"БАНК\"";
        var name = "ООО «Три кита»";
        var correspAcc = "30101810965770000413";
        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc, null, PayloadGenerator.RussiaPaymentOrder.CharacterSets.windows_1251);

        byte[] targetBytes = new byte[] { 83, 84, 48, 48, 48, 49, 49, 124, 78, 97, 109, 101, 61, 206, 206, 206, 32, 171, 210, 240, 232, 32, 234, 232, 242, 224, 187, 124, 80, 101, 114, 115, 111, 110, 97, 108, 65, 99, 99, 61, 52, 48, 55, 48, 50, 56, 49, 48, 49, 51, 56, 50, 53, 48, 49, 50, 51, 48, 49, 55, 124, 66, 97, 110, 107, 78, 97, 109, 101, 61, 206, 192, 206, 32, 34, 193, 192, 205, 202, 34, 124, 66, 73, 67, 61, 48, 52, 52, 53, 50, 53, 50, 50, 53, 124, 67, 111, 114, 114, 101, 115, 112, 65, 99, 99, 61, 51, 48, 49, 48, 49, 56, 49, 48, 57, 54, 53, 55, 55, 48, 48, 48, 48, 52, 49, 51, 124 };
        var payloadBytes = generator.ToBytes();

        Assert.True(targetBytes.Length == payloadBytes.Length, $"Byte array lengths different. Expected: {targetBytes.Length}, Actual: {payloadBytes.Length}");
        for (int i = 0; i < targetBytes.Length; i++)
        {
            Assert.True(targetBytes[i] == payloadBytes[i],
                        $"Expected: '{targetBytes[i]}', Actual: '{payloadBytes[i]}' at offset {i}."
            );
        }
    }

    [Fact]
    public void russiapayment_generator_can_generate_payload_encoding_koi8()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "ОАО \"БАНК\"";
        var name = "ООО «Три кита»";
        var correspAcc = "30101810965770000413";
        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc, null, PayloadGenerator.RussiaPaymentOrder.CharacterSets.koi8_r);

        byte[] targetBytes = new byte[] { 83, 84, 48, 48, 48, 49, 51, 124, 78, 97, 109, 101, 61, 239, 239, 239, 32, 60, 244, 210, 201, 32, 203, 201, 212, 193, 62, 124, 80, 101, 114, 115, 111, 110, 97, 108, 65, 99, 99, 61, 52, 48, 55, 48, 50, 56, 49, 48, 49, 51, 56, 50, 53, 48, 49, 50, 51, 48, 49, 55, 124, 66, 97, 110, 107, 78, 97, 109, 101, 61, 239, 225, 239, 32, 34, 226, 225, 238, 235, 34, 124, 66, 73, 67, 61, 48, 52, 52, 53, 50, 53, 50, 50, 53, 124, 67, 111, 114, 114, 101, 115, 112, 65, 99, 99, 61, 51, 48, 49, 48, 49, 56, 49, 48, 57, 54, 53, 55, 55, 48, 48, 48, 48, 52, 49, 51, 124 };
        var payloadBytes = generator.ToBytes();

        Assert.True(targetBytes.Length == payloadBytes.Length, $"Byte array lengths different. Expected: {targetBytes.Length}, Actual: {payloadBytes.Length}");
        for (int i = 0; i < targetBytes.Length; i++)
        {
            Assert.True(targetBytes[i] == payloadBytes[i],
                        $"Expected: '{targetBytes[i]}', Actual: '{payloadBytes[i]}' at offset {i}."
            );
        }
    }

    [Fact]
    public void russiapayment_generator_can_generate_payload_custom_separator()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "ОАО | \"БАНК\"";
        var name = "ООО «Три кита»";
        var correspAcc = "30101810400000000225";
        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc);

        generator
            .ToString()
            .ShouldBe($"ST00012#Name={name}#PersonalAcc={account}#BankName={bankName}#BIC={bic}#CorrespAcc={correspAcc}#");
    }

    [Fact]
    public void russiapayment_generator_should_throw_no_separator_exception()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "ОАО | \"БАНК\"";
        var name = "|@;:^_~{}!#$%&()*+,/"; //All chars that could be used as separator
        var correspAcc = "30101810400000000225";
        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc);

        var exception = Record.Exception(() => generator.ToString());
        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.RussiaPaymentOrder.RussiaPaymentOrderException>(exception);
        exception.Message.ShouldBe("No valid separator found.");
    }

    [Fact]
    public void russiapayment_generator_should_throw_data_too_long_exception()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "A very very very very very long bank name";
        // We use € symbol for the test case, because it needs 2-bytes. Otherwise we couldn't generate more than 300 bytes
        // of mandatory data to trigger the test case and stay at the same time within the 160 chars field validation limit
        var name = "A very €€€€ €€€€ €€€€ €€€€ very very very very very very very very very very very very very very very very very very very very very very very very ver long name";
        var correspAcc = "30101810400000000225";
        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc);

        var exception = Record.Exception(() => generator.ToString());
        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.RussiaPaymentOrder.RussiaPaymentOrderException>(exception);
        exception.Message.ShouldStartWith("Data too long");
    }

    [Fact]
    public void russiapayment_generator_should_throw_no_data_too_long_exception()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "ОАО | \"БАНК\"";
        var name = "A name";
        var correspAcc = "30101810400000000225";
        var optionalFields = new PayloadGenerator.RussiaPaymentOrder.OptionalFields()
        {
            FirstName = "Another long long long long long long long long long long long long long long firstname",
            LastName = "Another long long long long long long long long long long long long long long lastname",
            Category = "A pretty long long long long long long long long long long long long long category",
            Sum = "125000"
        };
        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc, optionalFields);

        // Should throw no exception as the 300 byte limit applies only to the mandatory fields
        // See https://github.com/Shane32/QRCoder/issues/392
        var exception = Record.Exception(() => generator.ToString());
        Assert.Null(exception);
    }

    [Fact]
    public void russiapayment_generator_should_throw_must_not_be_null_exception()
    {
        string account = null;
        var bic = "044525225";
        var bankName = "ОАО | \"БАНК\"";
        var name = "|@;:^_~{}!#$%&()*+,/";
        var correspAcc = "30101810400000000225";

        var exception = Record.Exception(() => new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc));
        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.RussiaPaymentOrder.RussiaPaymentOrderException>(exception);
        exception.Message.ShouldBe($"The input for 'PersonalAcc' must not be null.");
    }

    [Fact]
    public void russiapayment_generator_should_throw_unmatched_pattern_exception()
    {
        string account = "40702810138250123017";
        var bic = "abcd"; //Invalid BIC
        var bankName = "ОАО | \"БАНК\"";
        var name = "|@;:^_~{}!#$%&()*+,/";
        var correspAcc = "30101810400000000225";

        var exception = Record.Exception(() => new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc));
        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.RussiaPaymentOrder.RussiaPaymentOrderException>(exception);
        exception.Message.ShouldBe("The input for 'BIC' (abcd) doesn't match the pattern ^\\d{9}$");
    }

    [Fact]
    public void russiapayment_generator_can_generate_payload_some_additional_fields()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "=ОАО \"БАНК\"";
        var name = "ООО «Три кита»";
        var correspAcc = "30101810400000000225";
        var optionalFields = new PayloadGenerator.RussiaPaymentOrder.OptionalFields()
        {
            FirstName = "Raffael",
            LastName = "Herrmann",
            Sum = "125000"
        };

        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc, optionalFields);

        generator
            .ToString()
            .ShouldBe($"ST00012|Name={name}|PersonalAcc={account}|BankName={bankName}|BIC={bic}|CorrespAcc={correspAcc}|Sum={optionalFields.Sum}|LastName={optionalFields.LastName}|FirstName={optionalFields.FirstName}|");
    }

    [Fact]
    public void russiapayment_generator_can_generate_payload_all_additional_fields_pt1()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "=ОАО \"БАНК\"";
        var name = "ООО «Три кита»";
        var correspAcc = "30101810400000000225";
        var optionalFields = new PayloadGenerator.RussiaPaymentOrder.OptionalFields()
        {
            FirstName = "R",
            MiddleName = "C",
            LastName = "Hann",
            Sum = "1250",
            AddAmount = "10",
            BirthDate = new DateTime(1990, 1, 1),
            Category = "1",
            CBC = "CBC1",
            ChildFio = "J Doe",
            ClassNum = "1",
            Contract = "99",
        };

        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc, optionalFields);

        generator
            .ToString()
            .ShouldBe($"ST00012|Name={name}|PersonalAcc={account}|BankName={bankName}|BIC={bic}|CorrespAcc={correspAcc}|Sum={optionalFields.Sum}|CBC=CBC1|LastName=Hann|FirstName=R|MiddleName=C|Contract=99|ChildFio=J Doe|BirthDate=01.01.1990|Category=1|ClassNum=1|AddAmount=10|");
    }

    [Fact]
    public void russiapayment_generator_can_generate_payload_all_additional_fields_pt2()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "=ОАО \"БАНК\"";
        var name = "ООО «Три кита»";
        var correspAcc = "30101810400000000225";
        var optionalFields = new PayloadGenerator.RussiaPaymentOrder.OptionalFields()
        {
            CounterId = "1234",
            CounterVal = "9999",
            DocDate = new DateTime(2021, 11, 8),
            DocIdx = "A1",
            DocNo = "11",
            DrawerStatus = "D1",
            ExecId = "77",
            Flat = "5a",
            InstNum = "987",
            KPP = "KPP1",
            OKTMO = "112233"
        };

        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc, optionalFields);

        generator
            .ToString()
            .ShouldBe($"ST00012|Name={name}|PersonalAcc={account}|BankName={bankName}|BIC={bic}|CorrespAcc={correspAcc}|DrawerStatus=D1|KPP=KPP1|OKTMO=112233|DocNo=11|DocDate=08.11.2021|DocIdx=A1|Flat=5a|CounterId=1234|CounterVal=9999|InstNum=987|ExecId=77|");
    }


    [Fact]
    public void russiapayment_generator_can_generate_payload_all_additional_fields_pt3()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "=ОАО \"БАНК\"";
        var name = "ООО «Три кита»";
        var correspAcc = "30101810400000000225";
        var optionalFields = new PayloadGenerator.RussiaPaymentOrder.OptionalFields()
        {
            PayeeINN = "INN1",
            PayerAddress = "Street 1, 123 City",
            PayerIdNum = "555",
            PayerIdType = "X",
            PayerINN = "INN2",
            PaymPeriod = "12",
            PaymTerm = "A",
            PaytReason = "01",
            PensAcc = "SNILS_NO"
        };

        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc, optionalFields);

        generator
            .ToString()
            .ShouldBe($"ST00012|Name={name}|PersonalAcc={account}|BankName={bankName}|BIC={bic}|CorrespAcc={correspAcc}|PayeeINN=INN1|PayerINN=INN2|PaytReason=01|PayerAddress=Street 1, 123 City|PensAcc=SNILS_NO|PayerIdType=X|PayerIdNum=555|PaymTerm=A|PaymPeriod=12|");
    }

    [Fact]
    public void russiapayment_generator_can_generate_payload_all_additional_fields_pt4()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "=ОАО \"БАНК\"";
        var name = "ООО «Три кита»";
        var correspAcc = "30101810400000000225";
        var optionalFields = new PayloadGenerator.RussiaPaymentOrder.OptionalFields()
        {
            PersAcc = "2222",
            PersonalAccount = "3333",
            Phone = "0012345",
            Purpose = "Test",
            QuittDate = new DateTime(2021, 2, 1),
            QuittId = "7",
            RegType = "y",
            RuleId = "2",
            ServiceName = "Bank"
        };

        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc, optionalFields);

        generator
            .ToString()
            .ShouldBe($"ST00012|Name={name}|PersonalAcc={account}|BankName={bankName}|BIC={bic}|CorrespAcc={correspAcc}|Purpose=Test|PersonalAccount=3333|PersAcc=2222|Phone=0012345|ServiceName=Bank|QuittId=7|QuittDate=01.02.2021|RuleId=2|RegType=y|");
    }

    [Fact]
    public void russiapayment_generator_can_generate_payload_all_additional_fields_pt5()
    {
        var account = "40702810138250123017";
        var bic = "044525225";
        var bankName = "=ОАО \"БАНК\"";
        var name = "ООО «Три кита»";
        var correspAcc = "30101810400000000225";
        var optionalFields = new PayloadGenerator.RussiaPaymentOrder.OptionalFields()
        {
            SpecFio = "T. Eacher",
            TaxPaytKind = "99",
            TaxPeriod = "31",
            TechCode = PayloadGenerator.RussiaPaymentOrder.TechCode.ГИБДД_налоги_пошлины_бюджетные_платежи,
            UIN = "1a2b"
        };

        var generator = new PayloadGenerator.RussiaPaymentOrder(name, account, bankName, bic, correspAcc, optionalFields);

        generator
            .ToString()
            .ShouldBe($"ST00012|Name={name}|PersonalAcc={account}|BankName={bankName}|BIC={bic}|CorrespAcc={correspAcc}|TaxPeriod=31|TaxPaytKind=99|SpecFio=T. Eacher|UIN=1a2b|TechCode=ГИБДД_налоги_пошлины_бюджетные_платежи|");
    }
}
