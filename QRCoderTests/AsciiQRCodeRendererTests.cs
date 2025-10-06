namespace QRCoderTests;

public class AsciiQRCodeRendererTests
{
    [Fact]
    public void can_render_ascii_qrcode()
    {
        var targetCode = """
                                                                      
                                                                      
                                                                      
                                                                      
                    ██████████████  ████  ██    ██████████████        
                    ██          ██  ████    ██  ██          ██        
                    ██  ██████  ██  ██  ██  ██  ██  ██████  ██        
                    ██  ██████  ██  ██      ██  ██  ██████  ██        
                    ██  ██████  ██  ██████████  ██  ██████  ██        
                    ██          ██              ██          ██        
                    ██████████████  ██  ██  ██  ██████████████        
                                    ██████████                        
                      ████  ██  ████    ██████  ██  ██████████        
                    ██        ██        ██      ██    ██  ████        
                        ████  ██████  ██████        ██████  ██        
                    ████      ██  ██████  ██    ██        ██          
                      ████    ████  ██  ██      ██  ██  ████          
                                    ██    ██  ██  ██  ██              
                    ██████████████  ██  ████  ██████    ██            
                    ██          ██    ██    ████  ██████              
                    ██  ██████  ██  ██████  ████████    ██  ██        
                    ██  ██████  ██    ██        ██      ████          
                    ██  ██████  ██  ██████  ██      ██      ██        
                    ██          ██  ██  ██      ██      ██████        
                    ██████████████    ██    ██  ██  ██  ██  ██        
                                                                      
                                                                      
                                                                      
                                                                      
            """;

        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
        var asciiCode = new AsciiQRCode(data).GetGraphic(1);

        asciiCode.ShouldBe(targetCode, StringCompareShould.IgnoreLineEndings);
    }

    [Fact]
    public void can_render_small_ascii_qrcode()
    {
        var targetCode = """
            █████████████████████████████
            █████████████████████████████
            ████ ▄▄▄▄▄ █  █▄▀█ ▄▄▄▄▄ ████
            ████ █   █ █ █▄█ █ █   █ ████
            ████ █▄▄▄█ █▄▄▄▄▄█ █▄▄▄█ ████
            ████▄▄▄▄▄▄▄█ ▀ ▀ █▄▄▄▄▄▄▄████
            ████▀▄▄█▄▀▄▄██ ▄▄█ █▄ ▄  ████
            ████▀▀▄▄█ ▄ ▀ ▄ ██▀█▄▄▄▀▄████
            █████▄▄██▄▄█ █▄▀█▀▄▀▄▀▄▄█████
            ████ ▄▄▄▄▄ █▄▀▄▄▀ ▄ ▀▀▄██████
            ████ █   █ █▄ ▄█▄▄ ▄██ ▀▄████
            ████ █▄▄▄█ █ ▄ █▄█▀█▄█▀▀ ████
            ████▄▄▄▄▄▄▄██▄██▄█▄█▄█▄█▄████
            █████████████████████████████
            ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀
            """;

        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
        var asciiCode = new AsciiQRCode(data).GetGraphicSmall();

        asciiCode.ShouldBe(targetCode, StringCompareShould.IgnoreLineEndings);
    }

    [Fact]
    public void can_render_small_ascii_qrcode_without_quietzones()
    {
        var targetCode = """
             ▄▄▄▄▄ █  █▄▀█ ▄▄▄▄▄ 
             █   █ █ █▄█ █ █   █ 
             █▄▄▄█ █▄▄▄▄▄█ █▄▄▄█ 
            ▄▄▄▄▄▄▄█ ▀ ▀ █▄▄▄▄▄▄▄
            ▀▄▄█▄▀▄▄██ ▄▄█ █▄ ▄  
            ▀▀▄▄█ ▄ ▀ ▄ ██▀█▄▄▄▀▄
            █▄▄██▄▄█ █▄▀█▀▄▀▄▀▄▄█
             ▄▄▄▄▄ █▄▀▄▄▀ ▄ ▀▀▄██
             █   █ █▄ ▄█▄▄ ▄██ ▀▄
             █▄▄▄█ █ ▄ █▄█▀█▄█▀▀ 
            ▄▄▄▄▄▄▄██▄██▄█▄█▄█▄█▄
            """;

        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
        var asciiCode = new AsciiQRCode(data).GetGraphicSmall(drawQuietZones: false);
        asciiCode.ShouldBe(targetCode, StringCompareShould.IgnoreLineEndings);
    }

    [Fact]
    public void can_render_small_ascii_qrcode_inverted()
    {
        var targetCode = """
                                         
                                         
                █▀▀▀▀▀█ ██ ▀▄ █▀▀▀▀▀█    
                █ ███ █ █ ▀ █ █ ███ █    
                █ ▀▀▀ █ ▀▀▀▀▀ █ ▀▀▀ █    
                ▀▀▀▀▀▀▀ █▄█▄█ ▀▀▀▀▀▀▀    
                ▄▀▀ ▀▄▀▀  █▀▀ █ ▀█▀██    
                ▄▄▀▀ █▀█▄█▀█  ▄ ▀▀▀▄▀    
                 ▀▀  ▀▀ █ ▀▄ ▄▀▄▀▄▀▀     
                █▀▀▀▀▀█ ▀▄▀▀▄█▀█▄▄▀      
                █ ███ █ ▀█▀ ▀▀█▀  █▄▀    
                █ ▀▀▀ █ █▀█ ▀ ▄ ▀ ▄▄█    
                ▀▀▀▀▀▀▀  ▀  ▀ ▀ ▀ ▀ ▀    
                                         
                                         
            """;

        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
        var asciiCode = new AsciiQRCode(data).GetGraphicSmall(invert: true);
        asciiCode.ShouldBe(targetCode, StringCompareShould.IgnoreLineEndings);
    }

    [Fact]
    public void can_render_small_ascii_qrcode_with_custom_eol()
    {
        var targetCode = """
            █████████████████████████████
            █████████████████████████████
            ████ ▄▄▄▄▄ █  █▄▀█ ▄▄▄▄▄ ████
            ████ █   █ █ █▄█ █ █   █ ████
            ████ █▄▄▄█ █▄▄▄▄▄█ █▄▄▄█ ████
            ████▄▄▄▄▄▄▄█ ▀ ▀ █▄▄▄▄▄▄▄████
            ████▀▄▄█▄▀▄▄██ ▄▄█ █▄ ▄  ████
            ████▀▀▄▄█ ▄ ▀ ▄ ██▀█▄▄▄▀▄████
            █████▄▄██▄▄█ █▄▀█▀▄▀▄▀▄▄█████
            ████ ▄▄▄▄▄ █▄▀▄▄▀ ▄ ▀▀▄██████
            ████ █   █ █▄ ▄█▄▄ ▄██ ▀▄████
            ████ █▄▄▄█ █ ▄ █▄█▀█▄█▀▀ ████
            ████▄▄▄▄▄▄▄██▄██▄█▄█▄█▄█▄████
            █████████████████████████████
            ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀
            """;

        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
        var asciiCode = new AsciiQRCode(data).GetGraphicSmall(endOfLine: "\r\n");
        asciiCode.ShouldBe(targetCode, StringCompareShould.IgnoreLineEndings);
    }

    [Fact]
    public void can_render_ascii_qrcode_without_quietzones()
    {
        var targetCode = """
            ██████████████  ████  ██    ██████████████
            ██          ██  ████    ██  ██          ██
            ██  ██████  ██  ██  ██  ██  ██  ██████  ██
            ██  ██████  ██  ██      ██  ██  ██████  ██
            ██  ██████  ██  ██████████  ██  ██████  ██
            ██          ██              ██          ██
            ██████████████  ██  ██  ██  ██████████████
                            ██████████                
              ████  ██  ████    ██████  ██  ██████████
            ██        ██        ██      ██    ██  ████
                ████  ██████  ██████        ██████  ██
            ████      ██  ██████  ██    ██        ██  
              ████    ████  ██  ██      ██  ██  ████  
                            ██    ██  ██  ██  ██      
            ██████████████  ██  ████  ██████    ██    
            ██          ██    ██    ████  ██████      
            ██  ██████  ██  ██████  ████████    ██  ██
            ██  ██████  ██    ██        ██      ████  
            ██  ██████  ██  ██████  ██      ██      ██
            ██          ██  ██  ██      ██      ██████
            ██████████████    ██    ██  ██  ██  ██  ██
            """;

        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
        var asciiCode = new AsciiQRCode(data).GetGraphic(1, drawQuietZones: false);

        asciiCode.ShouldBe(targetCode, StringCompareShould.IgnoreLineEndings);
    }

    [Fact]
    public void can_render_ascii_qrcode_with_custom_symbols()
    {
        var targetCode = """
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                    XXXXXXXXXXXXXX    XX        XXXXXXXXXXXXXX        
                    XXXXXXXXXXXXXX    XX        XXXXXXXXXXXXXX        
                    XX          XX        XXXX  XX          XX        
                    XX          XX        XXXX  XX          XX        
                    XX  XXXXXX  XX  XXXX        XX  XXXXXX  XX        
                    XX  XXXXXX  XX  XXXX        XX  XXXXXX  XX        
                    XX  XXXXXX  XX    XX    XX  XX  XXXXXX  XX        
                    XX  XXXXXX  XX    XX    XX  XX  XXXXXX  XX        
                    XX  XXXXXX  XX  XXXX    XX  XX  XXXXXX  XX        
                    XX  XXXXXX  XX  XXXX    XX  XX  XXXXXX  XX        
                    XX          XX  XX      XX  XX          XX        
                    XX          XX  XX      XX  XX          XX        
                    XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        
                    XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        
                                      XX  XX                          
                                      XX  XX                          
                      XX    XX  XX  XXXXXX  XXXX  XXXX  XX            
                      XX    XX  XX  XXXXXX  XXXX  XXXX  XX            
                      XXXXXX  XX  XXXX      XX    XX  XX  XXXX        
                      XXXXXX  XX  XXXX      XX    XX  XX  XXXX        
                      XXXXXX    XXXXXXXXXX      XXXXXXXXXX            
                      XXXXXX    XXXXXXXXXX      XXXXXXXXXX            
                    XX  XX  XX    XX  XX    XXXXXX  XX  XX            
                    XX  XX  XX    XX  XX    XXXXXX  XX  XX            
                    XXXXXX      XXXX  XX  XX  XXXX      XX  XX        
                    XXXXXX      XXXX  XX  XX  XXXX      XX  XX        
                                    XXXXXX    XXXX      XX  XX        
                                    XXXXXX    XXXX      XX  XX        
                    XXXXXXXXXXXXXX        XXXXXX            XX        
                    XXXXXXXXXXXXXX        XXXXXX            XX        
                    XX          XX          XX    XX  XX              
                    XX          XX          XX    XX  XX              
                    XX  XXXXXX  XX  XXXXXXXXXX  XXXXXXXXXXXXXX        
                    XX  XXXXXX  XX  XXXXXXXXXX  XXXXXXXXXXXXXX        
                    XX  XXXXXX  XX    XX  XXXX    XX  XX  XXXX        
                    XX  XXXXXX  XX    XX  XXXX    XX  XX  XXXX        
                    XX  XXXXXX  XX    XXXXXX    XXXXXXXXXX            
                    XX  XXXXXX  XX    XXXXXX    XXXXXXXXXX            
                    XX          XX  XX        XXXX  XX  XX  XX        
                    XX          XX  XX        XXXX  XX  XX  XX        
                    XXXXXXXXXXXXXX    XX    XXXXXX      XXXXXX        
                    XXXXXXXXXXXXXX    XX    XXXXXX      XXXXXX        
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
            """;

        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("A", QRCodeGenerator.ECCLevel.Q);
        var asciiCode = new AsciiQRCode(data).GetGraphic(2, "X", " ");

        asciiCode.ShouldBe(targetCode, StringCompareShould.IgnoreLineEndings);
    }

    [Fact]
    public void can_instantate_parameterless()
    {
        var asciiCode = new AsciiQRCode();
        asciiCode.ShouldNotBeNull();
        asciiCode.ShouldBeOfType<AsciiQRCode>();
    }

    [Fact]
    public void can_render_ascii_qrcode_from_helper()
    {
        var targetCode = """
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                    XXXXXXXXXXXXXX    XX        XXXXXXXXXXXXXX        
                    XXXXXXXXXXXXXX    XX        XXXXXXXXXXXXXX        
                    XX          XX        XXXX  XX          XX        
                    XX          XX        XXXX  XX          XX        
                    XX  XXXXXX  XX  XXXX        XX  XXXXXX  XX        
                    XX  XXXXXX  XX  XXXX        XX  XXXXXX  XX        
                    XX  XXXXXX  XX    XX    XX  XX  XXXXXX  XX        
                    XX  XXXXXX  XX    XX    XX  XX  XXXXXX  XX        
                    XX  XXXXXX  XX  XXXX    XX  XX  XXXXXX  XX        
                    XX  XXXXXX  XX  XXXX    XX  XX  XXXXXX  XX        
                    XX          XX  XX      XX  XX          XX        
                    XX          XX  XX      XX  XX          XX        
                    XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        
                    XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        
                                      XX  XX                          
                                      XX  XX                          
                      XX    XX  XX  XXXXXX  XXXX  XXXX  XX            
                      XX    XX  XX  XXXXXX  XXXX  XXXX  XX            
                      XXXXXX  XX  XXXX      XX    XX  XX  XXXX        
                      XXXXXX  XX  XXXX      XX    XX  XX  XXXX        
                      XXXXXX    XXXXXXXXXX      XXXXXXXXXX            
                      XXXXXX    XXXXXXXXXX      XXXXXXXXXX            
                    XX  XX  XX    XX  XX    XXXXXX  XX  XX            
                    XX  XX  XX    XX  XX    XXXXXX  XX  XX            
                    XXXXXX      XXXX  XX  XX  XXXX      XX  XX        
                    XXXXXX      XXXX  XX  XX  XXXX      XX  XX        
                                    XXXXXX    XXXX      XX  XX        
                                    XXXXXX    XXXX      XX  XX        
                    XXXXXXXXXXXXXX        XXXXXX            XX        
                    XXXXXXXXXXXXXX        XXXXXX            XX        
                    XX          XX          XX    XX  XX              
                    XX          XX          XX    XX  XX              
                    XX  XXXXXX  XX  XXXXXXXXXX  XXXXXXXXXXXXXX        
                    XX  XXXXXX  XX  XXXXXXXXXX  XXXXXXXXXXXXXX        
                    XX  XXXXXX  XX    XX  XXXX    XX  XX  XXXX        
                    XX  XXXXXX  XX    XX  XXXX    XX  XX  XXXX        
                    XX  XXXXXX  XX    XXXXXX    XXXXXXXXXX            
                    XX  XXXXXX  XX    XXXXXX    XXXXXXXXXX            
                    XX          XX  XX        XXXX  XX  XX  XX        
                    XX          XX  XX        XXXX  XX  XX  XX        
                    XXXXXXXXXXXXXX    XX    XXXXXX      XXXXXX        
                    XXXXXXXXXXXXXX    XX    XXXXXX      XXXXXX        
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
                                                                      
            """;

        //Create QR code                   
        var asciiCode = AsciiQRCodeHelper.GetQRCode("A", 2, "X", " ", QRCodeGenerator.ECCLevel.Q);
        asciiCode.ShouldBe(targetCode, StringCompareShould.IgnoreLineEndings);
    }
}



