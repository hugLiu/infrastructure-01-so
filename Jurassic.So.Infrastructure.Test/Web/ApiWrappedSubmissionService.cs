using System;
using Jurassic.PKS.WebAPI.Submission;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jurassic.So.Infrastructure.Test
{
    [TestClass]
    public class ApiWrappedSubmissionServiceTest
    {
        private ApiWrappedSubmissionService ApiService { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            var url = "http://localhost:2000/api/SubmissionService";
            this.ApiService = new ApiWrappedSubmissionService(url);
        }
        [TestMethod]
        public void Upload()
        {
            var file = @"E:\项目\文档\如何提高写产品文案的能力？.pdf";
            var result = this.ApiService.Upload(file);
            Assert.IsNotNull(result);
        }
    }
}
