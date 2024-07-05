using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using EnglishJourney.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnglishJourney.Infrastructure.Repositories
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly EnglishJourneyDbContext dbContext;

        public ConnectionRepository(EnglishJourneyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Commit()
            => await dbContext.SaveChangesAsync();

        public async Task<int> CreateAttribute(ConnectionAttribute connectionAttribute)
        {
            dbContext.Add(connectionAttribute);
            await dbContext.SaveChangesAsync();
            return connectionAttribute.Id;
        }

        public async Task<int> CreateTopic(ConnectionTopic connectionTopic)
        {
            dbContext.Add(connectionTopic);
            await dbContext.SaveChangesAsync();
            return connectionTopic.Id;
        }

        public async Task DeleteAttribute(ConnectionAttribute connectionAttribute)
        {
            dbContext.Remove(connectionAttribute);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteTopic(ConnectionTopic connectionTopic)
        {
            dbContext.Remove(connectionTopic);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ConnectionTopic>?> GetAllTopics(string userId)
            => await dbContext.ConnectionTopics.Include(c => c.Attributes).Where(c => c.UserId == userId).OrderByDescending(n => n.LastModified).ToListAsync();

        public async Task<ConnectionAttribute?> GetAttributeById(int attributeId)
            => await dbContext.ConnectionAtrributes.Include(c => c.Topic).FirstOrDefaultAsync(c => c.Id == attributeId);

        public async Task<ConnectionTopic?> GetTopicById(int topicId)
            => await dbContext.ConnectionTopics.Include(c => c.Attributes).FirstOrDefaultAsync(c => c.Id == topicId);

        public Task<ConnectionTopic?> GetTopicsByName(string topic, string userId)
            => dbContext.ConnectionTopics.Where(c => c.UserId == userId).FirstOrDefaultAsync(c => c.Topic.ToLower() == topic.ToLower());
    }
}