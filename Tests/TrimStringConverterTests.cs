using System.Text.Json;

namespace Tests;

public class TrimStringTests
{
    private readonly JsonSerializerOptions _options;

    public TrimStringTests()
    {
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _options.Converters.Add(new TrimStringConverter());
    }
    
    [Theory]
    [InlineData("John ")]
    [InlineData(" John ")]
    [InlineData("    John    ")]
    public void ReturnsExpectedForStringWithSpaces(string input)
    {
        var request = new Request { Name = input };
        var requestString = JsonSerializer.Serialize(request);
        var result = JsonSerializer.Deserialize<Request>(requestString, _options);
        
        Assert.Equal(input.Trim(), result.Name);
    }

    [Fact]
    public void ReturnsExpectedForEmptyString()
    {
        var request = new Request { Name = string.Empty };
        var requestString  = JsonSerializer.Serialize(request);
        var result = JsonSerializer.Deserialize<Request>(requestString, _options);
         
        Assert.Null(result.Name);       
    }

    [Fact]
    public void ReturnsExpectedForNull()
    {
        var request = new Request { Name = null };
        var requestString  = JsonSerializer.Serialize(request);
        var result = JsonSerializer.Deserialize<Request>(requestString, _options);
      
        Assert.Null(result.Name);              
    }
}

public class Request
{
    public string? Name { get; set; }
}