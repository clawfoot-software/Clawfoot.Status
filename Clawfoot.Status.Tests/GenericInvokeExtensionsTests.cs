using Clawfoot.Status.Interfaces;
using Shouldly;

namespace Clawfoot.Status.Tests;

public class GenericInvokeExtensionsTests
{
    private const int TEST_VALUE = 5;

    private static Status<int> DoThingStatus()
    {
        return TEST_VALUE;
    }
    
    private static IStatus<int> DoThingIStatus()
    {
        return Status.AsSuccess(TEST_VALUE);
    }
    
    [Fact]
    public void Status_WithStatus_Ensure_NoAmbiguousInvoke()
    {
        Status<int> status = new Status<int>();
        
        int result = status.InvokeResult(() => DoThingStatus());
        
        result.ShouldBe(TEST_VALUE);
    }
    
    [Fact]
    public void Status_WithIStatus_Ensure_NoAmbiguousInvoke()
    {
        IStatus<int> status = new Status<int>();
        
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
    public void IStatus_WithIStatus_Ensure_NoAmbiguousInvoke()
    {
        IStatus<int> status = new Status<int>();
        
        int result = status.InvokeResult(() => DoThingIStatus());
        
        result.ShouldBe(TEST_VALUE);
    }
}