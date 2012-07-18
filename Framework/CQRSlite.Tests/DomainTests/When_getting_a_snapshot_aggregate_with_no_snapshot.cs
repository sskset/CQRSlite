using System;
using CQRSlite.Domain;
using CQRSlite.Eventing;
using CQRSlite.Infrastructure;
using CQRSlite.Snapshotting;
using CQRSlite.Tests.TestSubstitutes;
using NUnit.Framework;

namespace CQRSlite.Tests.DomainTests
{
	[TestFixture]
    public class When_getting_a_snapshot_aggregate_with_no_snapshot
    {
        private TestSnapshotAggregate _aggregate;

		[SetUp]
        public void Setup()
        {
            var eventStore = new TestEventStore();
            var eventPublisher = new TestEventPublisher();
            var snapshotStore = new NullSnapshotStore();
            var snapshotStrategy = new DefaultSnapshotStrategy();
		    var repository = new SnapshotRepository(snapshotStore, snapshotStrategy, new Repository(eventStore, eventPublisher), eventStore);
            var session = new Session(repository);
            _aggregate = session.Get<TestSnapshotAggregate>(Guid.NewGuid());
        }

	    private class NullSnapshotStore : ISnapshotStore
	    {
	        public Snapshot Get(Guid id)
	        {
	            return null;
	        }

	        public void Save(Snapshot snapshot)
	        {
	        }
	    }

	    [Test]
        public void Should_load_events()
        {
            Assert.True(_aggregate.Loaded);
        }

        [Test]
        public void Should_not_load_snapshot()
        {
            Assert.False(_aggregate.Restored);
        }
    }
}