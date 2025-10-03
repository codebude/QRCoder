namespace QRCoderTests.PayloadGeneratorTests;

public class GeolocationTests
{
    [Fact]
    public void geolocation_should_build_type_GEO()
    {
        var latitude = "51.227741";
        var longitude = "6.773456";
        var encoding = PayloadGenerator.Geolocation.GeolocationEncoding.GEO;

        var generator = new PayloadGenerator.Geolocation(latitude, longitude, encoding);

        generator.ToString().ShouldBe("geo:51.227741,6.773456");
    }


    [Fact]
    public void geolocation_should_build_type_GoogleMaps()
    {
        var latitude = "51.227741";
        var longitude = "6.773456";
        var encoding = PayloadGenerator.Geolocation.GeolocationEncoding.GoogleMaps;

        var generator = new PayloadGenerator.Geolocation(latitude, longitude, encoding);

        generator.ToString().ShouldBe("http://maps.google.com/maps?q=51.227741,6.773456");
    }


    [Fact]
    public void geolocation_should_escape_input()
    {
        var latitude = "51,227741";
        var longitude = "6,773456";
        var encoding = PayloadGenerator.Geolocation.GeolocationEncoding.GEO;

        var generator = new PayloadGenerator.Geolocation(latitude, longitude, encoding);

        generator.ToString().ShouldBe("geo:51.227741,6.773456");
    }


    [Fact]
    public void geolocation_should_add_unused_params()
    {
        var latitude = "51.227741";
        var longitude = "6.773456";

        var generator = new PayloadGenerator.Geolocation(latitude, longitude);

        generator.ToString().ShouldBe("geo:51.227741,6.773456");
    }
}
