using Shouldly;

namespace Clawfoot.Status.Tests;

// Largely testing for build errors around ambiguous methods
public class GenericInvokeExtensionsTests
{
    private const int TEST_VALUE = 5;

    private static Status<int> DoThingStatus()
    {
        return TEST_VALUE;
    }
    
    private static Status<int> DoThingIStatus()
    {
        return Status.Ok(TEST_VALUE);
    }
    
    [Fact]
    public void Status_WithStatus_Ensure_NoAmbiguousInvoke()
    {
        Status<int> status = new Status<int>();
        
        int result = status.InvokeResult(() => DoThingStatus());
        
        result.ShouldBe(TEST_VALUE);
    }
    
    [Fact]
    public void IStatus_WithStatus_Ensure_NoAmbiguousInvoke()
    {
        Status<int> status = new Status<int>();
        
        int result = status.InvokeResult(() => DoThingIStatus());
        
        result.ShouldBe(TEST_VALUE);
    }

    [Fact]
    public void StatusT_Do()
    {
        var status = new Status<int>();
        
        (Status s, int result) = status.Do(() => DoThingIStatus());
    }
    
    [Fact]
    public void Status_Do()
    {
        var status = new Status();
        
        Status s = status.Do(() => DoThingIStatus());
    }

    [Fact]
    public async Task InvokeResultAsync_WithReturningTaskStatusResult_ReturnsStatusResult()
    {
        async Task<Status<int>> DoThingStatusAsync()
        {
            return TEST_VALUE;
        }
        
        (Status status, int result) = await Status.InvokeResultAsync(async () => await DoThingStatusAsync());
        
        result.ShouldBe(TEST_VALUE);
        status.HasErrors.ShouldBeFalse();
    }
}