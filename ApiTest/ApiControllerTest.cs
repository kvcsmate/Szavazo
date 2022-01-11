using Persistence.DTO;
using Persistence;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ApiTest
{
    public class ApiControllerTest :IDisposable
    {
        private readonly SzavazoDbContext _context;
        private readonly SzavazoService _service;
        private readonly PollsController _pollsController;
        public ApiControllerTest()
        {
            var options = new DbContextOptionsBuilder<SzavazoDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            _context = new SzavazoDbContext(options);

            TestDbInit.Initialize(_context);
            _service = new SzavazoService(_context);
            _pollsController = new PollsController(_service);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        //[Fact]
        //public void GetPollsTest()
        //{
        //    // Act
        //    var result = _pollsController.Get();

        //    // Assert
        //    var content = Assert.IsAssignableFrom<List<PollDto>>(result);
        //    Assert.Equal(5, content.Count());
        //    //Assert.True(content[4].End<DateTime.Now);
        //}
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void GetPollBindingsTest(int pollId)
        {
            var result = _pollsController.GetPollBindings(pollId);
            var content = Assert.IsAssignableFrom < List < PollBindingDto>>(result);
            
            
                foreach (var item in content)
                {
                    Assert.Equal(item.PollId, pollId);
                }

        }
        [Theory]
        [InlineData(234)]
        public void GetPollBindingsNullTest(int pollId)
        {
            var result = _pollsController.GetPollBindings(pollId);
            var content = Assert.IsAssignableFrom<List<PollBindingDto>>(result);
            Assert.Empty(content);
        }
        [Theory]
        [InlineData(234)]
        public void GetAnswerNullTest(int pollId)
        {
            var result = _pollsController.GetAnswers(pollId);
            var content = Assert.IsAssignableFrom<List<AnswerDto>>(result);
            Assert.Empty(content);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetAnswers(int pollId)
        {
            var result = _pollsController.GetAnswers(pollId);
            var content = Assert.IsAssignableFrom<List<AnswerDto>>(result);
            switch (pollId)
            {
                case 1:Assert.Equal(3, content.Count());break;
                case 2:
                    Assert.Equal(4, content.Count()); break;
                case 3:
                    Assert.Equal(4, content.Count()); break;
                case 4:
                    Assert.Equal(3, content.Count()); break;
                case 5:
                    Assert.Equal(3, content.Count()); break;
            }
        }
        
    }
}
