using Socializer.Domain.Common;

namespace Socializer.UnitTests.Common;

file class TestEntity : BaseAuditableEntity;

public class BaseAuditableEntityTests
{
    [Fact]
    public void NewEntity_HasUniqueNonEmptyId()
    {
        var entity = new TestEntity();

        Assert.NotEqual(Guid.Empty, entity.Id);
    }

    [Fact]
    public void NewEntity_SetsCreatedAtCloseToNow()
    {
        var before = DateTime.UtcNow;
        var entity = new TestEntity();
        var after = DateTime.UtcNow;

        Assert.InRange(entity.CreatedAt, before, after);
        Assert.Null(entity.UpdatedAt);
    }
}
