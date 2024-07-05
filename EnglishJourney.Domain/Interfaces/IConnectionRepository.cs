﻿using EnglishJourney.Domain.Entities;

namespace EnglishJourney.Domain.Interfaces
{
    public interface IConnectionRepository
    {
        Task Commit();

        Task<int> CreateAttribute(ConnectionAttribute connectionAttribute);

        Task<int> CreateTopic(ConnectionTopic connectionTopic);

        Task DeleteAttribute(ConnectionAttribute connectionAttribute);

        Task DeleteTopic(ConnectionTopic connectionTopic);

        Task<IEnumerable<ConnectionTopic>?> GetAllTopics(string userId);

        Task<ConnectionTopic?> GetTopicsByName(string topic, string userId);

        Task<ConnectionAttribute?> GetAttributeById(int attributeId);

        Task<ConnectionTopic?> GetTopicById(int topicId);
    }
}