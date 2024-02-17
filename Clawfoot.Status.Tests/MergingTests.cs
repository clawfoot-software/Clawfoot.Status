using Shouldly;
using FakeItEasy;

namespace Clawfoot.Status.Tests;

public class MergingTests
{
    private const string ERROR_MESSAGE = "Error";
    private const int TEST_VALUE = 5;
    private const int TEST_VALUE2 = 55;
    
    [Fact]
    public void StatusT_WithStatus_Merges()
    {
        Status<int> resultStatus = new Status<int>(TEST_VALUE);
        Status status = new Status();
        
        status.AddError(ERROR_MESSAGE);
        
        Status<int>? merged = resultStatus.MergeStatuses(status);
        
        merged.ShouldBe(resultStatus);
        resultStatus.Success.ShouldBeFalse();
        resultStatus.Result.ShouldBe(TEST_VALUE);
        resultStatus.Errors.Count().ShouldBe(1);
        resultStatus.Errors.First().Message.ShouldBe(ERROR_MESSAGE);
    }
    
    [Fact]
    public void StatusT_WithStatusT_Merges()
    {
        Status<int> resultStatus = new Status<int>(TEST_VALUE);
        Status<int> resultStatus2 = new Status<int>();
        
        resultStatus2.AddError(ERROR_MESSAGE);
        
        Status<int>? merged = resultStatus.MergeStatuses(resultStatus2);
        
        merged.ShouldBe(resultStatus);
        resultStatus.Success.ShouldBeFalse();
        resultStatus.Result.ShouldBe(TEST_VALUE);
        resultStatus.Errors.Count().ShouldBe(1);
        resultStatus.Errors.First().Message.ShouldBe(ERROR_MESSAGE);
    }
    
    [Fact]
    public void Status_WithStatusT_Merges()
    {
        Status status = new Status();
        Status<int> resultStatus = new Status<int>();
        
        status.AddError(ERROR_MESSAGE);
        
        Status merged = status.MergeStatuses(resultStatus);
        
        merged.ShouldBe(status);
        status.Success.ShouldBeFalse();
        status.Errors.Count().ShouldBe(1);
        status.Errors.First().Message.ShouldBe(ERROR_MESSAGE);
    }

    [Fact]
    public void Regression_GenericStatus_MergeInNonGeneric_ShouldNeverCallAsT()
    {
        // Regression test to ensure that we are avoiding implicit casts instead of direct implementation calls
        // The implicit Status -> Status<T> cast calls .As<T>() and was causing a stack overflow
        
        Status<int> status = new Status<int>();
        Status status2 = A.Fake<Status>();
        
        status.MergeStatuses(status2);
        
        A.CallTo(() => status2.As<int>()).MustNotHaveHappened();
    }
}