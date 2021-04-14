using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerUpload7.BLL.Services;
using ServerUpload7.DAL.Interfaces;
using Xunit;
using Moq;

namespace ServerUpload7.Tests
{
    public class MaterialServiceTests
    {
        [Fact]
        public void CrNameTest()
        {
            var mock = new Mock<IUnitOfWork>();
            var TestingService = new MaterialsService(mock.Object);

            var res = TestingService.CrName("test.pdf", 1);
            Assert.Equal("test_v1.pdf", res);
        }
    }
}
