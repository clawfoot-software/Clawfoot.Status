using Shouldly;

namespace Clawfoot.Status.Tests;

public class StatusDeconstructTests
{
    [Fact]
    public async Task WithAsyncCall_DeconstructedInIf_ShouldDeconstruct()
    {
        async Task<Status> GetErrorStatus()
        {
            await Task.Delay(1);
            return Status.Error("Error");
        }

        if ((await GetErrorStatus() is (false, Status status)))
        {
            status.Success.ShouldBeFalse();
        }
        else
        {
            throw new Exception();
        }
    }
}