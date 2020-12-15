using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assignment2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Tests
{
    [TestClass()]
    public class FacebookServiceTests
    {
        [TestMethod()]
        public void GetAccountAsyncTest()
        {
            FacebookClient fb = new FacebookClient();
            FacebookService service = new FacebookService(fb);
            var post = service.PostOnWallAsync("EAAyOBEo3SgwBABfwsupr5IGtLgzLjOK5l3EWasARtrPGVDh0VFkzazL55t7ICfXtjTV71XUplvyYGDNXlMZC1DvOmiJkxFmTXCdJdESN1lVbvS12VoFuvCBL1GsSmNkltZABPA6Y2UIyL6o7riaDsGx1Kr2qfctfyKiVNOEYcZCXeuSerJoDOVjspZA0UeUZD","test");
            Task.WaitAll(post);
            var accountAsync = service.GetPostsAsync("EAAyOBEo3SgwBABfwsupr5IGtLgzLjOK5l3EWasARtrPGVDh0VFkzazL55t7ICfXtjTV71XUplvyYGDNXlMZC1DvOmiJkxFmTXCdJdESN1lVbvS12VoFuvCBL1GsSmNkltZABPA6Y2UIyL6o7riaDsGx1Kr2qfctfyKiVNOEYcZCXeuSerJoDOVjspZA0UeUZD");
            Task.WaitAll(accountAsync);
            Assert.AreEqual("test", accountAsync.Result);
        }
    }
}