using Shouldly;

namespace Clawfoot.Status.Tests;

public class StatusCastingTests
{
    private const int TEST_VALUE = 5;
    
    private static Status<int> DoThingStatus()
    {
        return TEST_VALUE;
    }

    [Fact]
    public void WithStatus_AsGeneric_ReturnsGenericStatus()
    {
        Status status = new Status();

        Status<int> converted = status.As<int>();
        
        converted.Success.ShouldBeTrue();
        status.Success.ShouldBeTrue();
    }
    
    [Fact]
    public void WithStatus_CastsToGenericStatus()
    {
        Status status = new Status();
        
        int result = status.InvokeResult(() => DoThingStatus());
        
        result.ShouldBe(TEST_VALUE);
        status.Success.ShouldBeTrue();
        status.Errors.ShouldBeEmpty();
    }
}