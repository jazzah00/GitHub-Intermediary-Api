using GitHub_Intermediary_Api.Models;
using GitHub_Intermediary_Api.Services;

namespace GitHub_Intermediary_Api_xTest.Services {
    public class ConverterTest {
        private readonly Converter _Converter;

        public ConverterTest() {
            _Converter = new Converter();
        }

        [Fact]
        public void ConvertToXml_ReturnValid_ForObject() {
            var obj = new {
                FirstName = "John",
                MiddleName = "",
                LastName = "Smith",
                Age = 30
            };
            string expectedXml = "<Root><FirstName>John</FirstName><MiddleName></MiddleName><LastName>Smith</LastName><Age>30</Age></Root>";

            string result = _Converter.ConvertToXml(obj, "Root");

            Assert.Equal(expectedXml, result.Replace("\r\n", "").Replace(">  <", "><"));
        }

        [Fact]
        public void ConvertToXml_ReturnValid_ForUser() {
            User user = new() {
                Name = "Oliver Queen",
                Login = "Arrow",
                Company = "Queen Industries",
                Followers = 170,
                Public_Repos = 8
            };
            string expectedXml = "<User><Name>Oliver Queen</Name><Login>Arrow</Login><Company>Queen Industries</Company><Followers>170</Followers><Public_Repos>8</Public_Repos><Average_Followers_Per_Repository>21</Average_Followers_Per_Repository></User>";

            string result = _Converter.ConvertToXml(user, "User");

            Assert.Equal(expectedXml, result.Replace("\r\n", "").Replace(">  <", "><"));
        }

        [Fact]
        public void ConvertToXml_ReturnValid_ForObjectCollection() {
            var obj = new {
                Name = "Oliver Queen",
                Login = "Arrow",
                Supporters = new List<User> {
                    new() {
                        Name = "Felicity Smoak",
                        Login = "Overwatch",
                        Company = "",
                        Followers = 160,
                        Public_Repos = 8
                    },
                    new() {
                        Name = "John Diggle",
                        Login = "Spartan",
                        Company = "US Army",
                        Followers = 170,
                        Public_Repos = 8
                    }
                }
            };
            string expectedXml = "<ArrowVerse><Name>Oliver Queen</Name><Login>Arrow</Login><Supporters><Supporter><Name>Felicity Smoak</Name><Login>Overwatch</Login><Company></Company><Followers>160</Followers><Public_Repos>8</Public_Repos><Average_Followers_Per_Repository>20</Average_Followers_Per_Repository></Supporter><Supporter><Name>John Diggle</Name><Login>Spartan</Login><Company>US Army</Company><Followers>170</Followers><Public_Repos>8</Public_Repos><Average_Followers_Per_Repository>21</Average_Followers_Per_Repository></Supporter></Supporters></ArrowVerse>";

            string result = _Converter.ConvertToXml(obj, "ArrowVerse");
            Assert.Equal(expectedXml, result.Replace("\r\n", "").Replace(">      <", "><").Replace(">     <", "><").Replace(">    <", "><").Replace(">   <", "><").Replace(">  <", "><").Replace("> <", "><"));
        }
    }
}