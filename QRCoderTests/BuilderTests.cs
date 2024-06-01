#if NET5_0_OR_GREATER && SYSTEM_DRAWING
using QRCoder;
using QRCoder.Builders.Renderers.Implementations;
using Shouldly;
using Xunit;

namespace QRCoderTests
{
    public class BuilderTests
    {
        [Fact]
        public void EmailAsAscii()
        {
            var code = QRCodeBuilder.CreateEmail("test@microsoft.com")
                .WithSubject("Testing")
                .WithBody("Hello World!")
                .WithErrorCorrection(QRCodeGenerator.ECCLevel.H)
                .RenderWith<AsciiRenderer>()
                .WithQuietZone(false)
                .ToString();

            code.ShouldBe(@"██████████████  ██        ████████████    ██████  ██      ████          ██  ██████████████
██          ██  ██████    ██████████    ██████    ██    ██  ████  ██  ██    ██          ██
██  ██████  ██  ██████                ████  ██      ████  ██  ██████  ██    ██  ██████  ██
██  ██████  ██    ██      ██████        ██    ██████    ████          ████  ██  ██████  ██
██  ██████  ██      ████████    ██    ████████████  ██  ██████  ██  ██████  ██  ██████  ██
██          ██  ████████████████        ██      ████              ██        ██          ██
██████████████  ██  ██  ██  ██  ██  ██  ██  ██  ██  ██  ██  ██  ██  ██  ██  ██████████████
                ██████████████  ██████████      ██          ██  ██████  ██                
    ██████  ██  ██  ██    ██████████  ████████████    ██████  ██  ██████  ██████    ██████
    ████      ██████████      ██      ████  ██  ████  ██████████      ██████  ██  ██    ██
    ██  ██  ████    ██    ██████      ████  ████    ██  ████  ██    ██  ████  ██  ██  ██  
  ██  ██████  ██  ██  ████████  ██████████████████████  ████  ████    ████████████  ████  
████████  ████  ██    ██████    ██████        ████    ████  ████  ██████                ██
██    ████      ██        ██      ████████████  ██  ██    ██████      ██████  ██████  ████
██  ██  ██  ██      ████        ██  ██  ██  ████  ████  ██    ██    ████  ████    ██████  
██  ██  ████      ██  ██████    ████  ████████████████      ██  ██      ██████████████████
  ████    ████████  ██  ████████  ██            ████  ██████████  ██      ████        ████
    ██            ████              ██  ████    ██  ██████  ████████  ██████  ████      ██
████  ██  ██████  ██████    ██████  ██████  ██    ██  ██    ██      ████  ██████  ██████  
  ████  ████  ████      ██    ██████  ██    ████  ██████████  ██████████    ████████████  
        ████████████████  ██      ████████████████  ██        ██      ████████████    ████
██████████      ██          ████████  ████      ██████    ██  ██████  ████      ██  ██  ██
██████████  ██  ████████████    ██████  ██  ██  ██  ██  ██  ██      ██  ██  ██  ████  ██  
████    ██      ██    ██  ██    ██  ██████      ██    ████    ████  ██  ██      ██████████
        ██████████        ██  ██    ████████████████  ██          ██  ██████████████  ████
        ██          ████████  ██  ████  ████████  ████    ██  ██      ██    ██    ██    ██
    ██████████          ██    ██  ██    ████  ██  ██████    ████  ██████  ██  ████    ██  
████  ██  ██        ████  ████    ██        ████  ██    ██      ██    ██    ████  ████    
████    ██████  ██          ██    ██  ████    ████    ██      ██  ██          ████    ████
  ██  ██  ██      ██      ████████      ██████████  ████    ██        ██    ██    ████  ██
██████    ████████████    ████        ████  ██  ████  ██  ████  ████████████      ██████  
██    ██████  ██████  ██████  ████      ██████          ██  ██  ██    ██    ██████  ██    
    ██  ██  ████  ████  ██  ████    ████    ██████████    ██████  ██      ██    ████  ██  
████  ████            ████        ██  ████  ██    ██        ██    ██  ██  ██████    ██████
        ██  ██████  ██          ██    ██  ████████████    ██████  ██████████  ████████    
  ████████      ██  ██        ██        ██████  ████  ██████    ██  ████    ████  ██████  
██    ████  ██  ████    ██████          ██████████████    ██████        ██████████    ████
                ██    ██  ██████      ████      ██          ██  ██    ████      ██  ██  ██
██████████████    ██  ██  ██  ██    ██████  ██  ██  ████████  ████      ██  ██  ████  ██  
██          ██          ████        ██████      ████████      ████  ██████      ██████    
██  ██████  ██  ████  ████  ██    ██    ██████████  ██    ██  ██        ████████████      
██  ██████  ██  ████  ████    ██  ██████      ██████████    ████          ██  ████████████
██  ██████  ██  ██  ████████████  ██  ████  ██        ██  ██  ██  ██  ████    ██  ████    
██          ██        ████  ██    ██    ██  ██    ████████  ██  ██    ██  ██    ██████    
██████████████    ██      ████████        ████  ██      ████      ██    ██  ██        ██  ", StringCompareShould.IgnoreLineEndings);
        }

        [Fact]
        public void PhoneAsBitmap()
        {
            var image = QRCodeBuilder.CreatePhoneNumber("1234567890")
                .WithErrorCorrection(QRCodeGenerator.ECCLevel.H)
                .RenderWith<SystemDrawingRenderer>(20)
                .WithQuietZone(false)
                .ToBitmap();
        }

        [Fact]
        public void MmsAsBase64()
        {
            var base64 = QRCodeBuilder.CreateMMS("1234567890", "Hello")
                .RenderWith<PngRenderer>()
                .ToBase64String();

            base64.ShouldBe("iVBORw0KGgoAAAANSUhEUgAAAUoAAAFKAQAAAABTUiuoAAABdUlEQVR4nO2aSQ7DIAwAkfqAPilf50l9QCQK3qBtpPSGD8MhzTK5jIVtSEv7d9QCigEMYAADGMhhoNh49PNXsXtHP9NxgKYyIBftNdB2FqUOvfSnoHkM+DM703v9cgQbNLEBAyTOoMkNaA5dXwLNZsCTpte0sz+4ya+guwxEQ7LMxbveBXSTgTlKeUpDYjFdBmgWAx5EK2ytdl7afrsHmsuAtPgS2DEX9fBs/hJoKgM2+foaumrmtHSp0f3Mr6C7DUgHUiWS0efbPYs4aCYDkTm9xPkCwPasQPMYWLpETZVx+ClxoNsNSImzwibzbszAQdWfdQHofgM+A70D8bOLpAm63YD8RNKc4zq/gu40MMeyr6i9/8icoKkMeBhjs2PuVPmaDTSPgbkBrPMuvodZxQNNZSA+rnyNubkImtCA7n1IqpwrNdC0BmKTypdmoNkMeNL8+Fx5Kg+azEA0JL6kljpn/20DzWXgnwGKAQxgAAMYqNvLxhsMs1uBEKIWRQAAAABJRU5ErkJggg==");
        }
    }
}
#endif