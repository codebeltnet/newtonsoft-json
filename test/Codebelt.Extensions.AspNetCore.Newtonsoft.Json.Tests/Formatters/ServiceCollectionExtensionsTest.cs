using System;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Authentication;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task AddNewtonsoftJsonExceptionResponseFormatter_ShouldCaptureException_RenderAsExceptionDescriptor_UsingNewtonsoftJson_WithSensitivityAll()
        {
            using var response = await WebHostTestFactory.RunAsync(
                services =>
                {
                    services.AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.FaultDetails);
                    services.AddNewtonsoftJsonExceptionResponseFormatter();
                    services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.All);
                },
                app =>
                {
                    app.UseFaultDescriptorExceptionHandler();
                    app.Use(async (context, next) =>
                    {
                        try
                        {
                            throw new ArgumentException("This is an inner exception message ...", nameof(app))
                            {
                                Data =
                                {
                                    { "1st", "data value" }
                                },
                                HelpLink = "https://www.savvyio.net/"
                            };
                        }
                        catch (Exception e)
                        {
                            throw new NotFoundException("Main exception - look out for inner!", e);
                        }

                        await next(context);
                    });
                },
                responseFactory: client =>
                {
                    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                    return client.GetAsync("/");
                }, hostFixture: null);

            var body = await response.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match("""
                              {
                                "error": {
                                  "instance": "http://localhost/",
                                  "status": 404,
                                  "code": "NotFound",
                                  "message": "Main exception - look out for inner!",
                                  "failure": {
                                    "type": "Cuemon.AspNetCore.Http.NotFoundException",
                                    "source": "Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Tests",
                                    "message": "Main exception - look out for inner!",
                                    "stack": [
                                      "at Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters.ServiceCollectionExtensionsTest.<>c.<<AddNewtonsoftJsonExceptionResponseFormatter_ShouldCaptureException_RenderAsExceptionDescriptor_UsingNewtonsoftJson_WithSensitivityAll>*",
                                      "--- End of stack trace from previous location ---",
                                      "at Microsoft.AspNetCore.Diagnostics.ExceptionHandler*"
                                    ],
                                    "headers": {},
                                    "statusCode": 404,
                                    "reasonPhrase": "Not Found",
                                    "inner": {
                                      "type": "System.ArgumentException",
                                      "source": "Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Tests",
                                      "message": "This is an inner exception message ... (Parameter 'app')",
                                      "stack": [
                                        "at Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters.ServiceCollectionExtensionsTest.<>c.<<AddNewtonsoftJsonExceptionResponseFormatter_ShouldCaptureException_RenderAsExceptionDescriptor_UsingNewtonsoftJson_WithSensitivityAll>*"
                                      ],
                                      "data": {
                                        "1st": "data value"
                                      },
                                      "paramName": "app"
                                    }
                                  }
                                },
                                "evidence": {
                                  "request": {
                                    "location": "http://localhost/",
                                    "method": "GET",
                                    "headers": {
                                      "accept": "application/json",
                                      "host": "localhost"
                                    },
                                    "query": [],
                                    "cookies": [],
                                    "body": ""
                                  }
                                },
                                "traceId": "*"
                              }
                              """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
        }

        [Fact]
        public async Task AddNewtonsoftJsonExceptionResponseFormatter_ShouldCaptureException_RenderAsProblemDetails_UsingNewtonsoftJson_WithSensitivityAll()
        {
            using var response = await WebHostTestFactory.RunAsync(
                services =>
                {
                    services.AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.ProblemDetails);
                    services.AddNewtonsoftJsonExceptionResponseFormatter();
                    services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = FaultSensitivityDetails.All);
                },
                app =>
                {
                    app.UseFaultDescriptorExceptionHandler();
                    app.Use(async (context, next) =>
                    {
                        try
                        {
                            throw new ArgumentException("This is an inner exception message ...", nameof(app))
                            {
                                Data =
                                {
                                    { "1st", "data value" }
                                },
                                HelpLink = "https://www.savvyio.net/"
                            };
                        }
                        catch (Exception e)
                        {
                            throw new NotFoundException("Main exception - look out for inner!", e);
                        }

                        await next(context);
                    });
                },
                responseFactory: client =>
                {
                    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                    return client.GetAsync("/");
                }, hostFixture: null);

            var body = await response.Content.ReadAsStringAsync();

            TestOutput.WriteLine(body);

            Assert.True(Match("""
                              {
                                "type": "about:blank",
                                "title": "NotFound",
                                "status": 404,
                                "detail": "Main exception - look out for inner!",
                                "instance": "http://localhost/",
                                "traceId": "*",
                                "failure": {
                                  "type": "Cuemon.AspNetCore.Http.NotFoundException",
                                  "source": "Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Tests",
                                  "message": "Main exception - look out for inner!",
                                  "stack": [
                                    "at Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters.ServiceCollectionExtensionsTest.<>c.<<AddNewtonsoftJsonExceptionResponseFormatter_ShouldCaptureException_RenderAsProblemDetails_UsingNewtonsoftJson_WithSensitivityAll>*",
                                    "--- End of stack trace from previous location ---",
                                    "at Microsoft.AspNetCore.Diagnostics.ExceptionHandler*"
                                  ],
                                  "headers": {},
                                  "statusCode": 404,
                                  "reasonPhrase": "Not Found",
                                  "inner": {
                                    "type": "System.ArgumentException",
                                    "source": "Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Tests",
                                    "message": "This is an inner exception message ... (Parameter 'app')",
                                    "stack": [
                                      "at Codebelt.Extensions.AspNetCore.Newtonsoft.Json.Formatters.ServiceCollectionExtensionsTest.<>c.<<AddNewtonsoftJsonExceptionResponseFormatter_ShouldCaptureException_RenderAsProblemDetails_UsingNewtonsoftJson_WithSensitivityAll>*"
                                    ],
                                    "data": {
                                      "1st": "data value"
                                    },
                                    "paramName": "app"
                                  }
                                },
                                "request": {
                                  "location": "http://localhost/",
                                  "method": "GET",
                                  "headers": {
                                    "accept": "application/json",
                                    "host": "localhost"
                                  },
                                  "query": [],
                                  "cookies": [],
                                  "body": ""
                                }
                              }
                              """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
        }

        [Theory]
        [InlineData(FaultSensitivityDetails.All)]
        [InlineData(FaultSensitivityDetails.None)]
        public async Task AddNewtonsoftJsonExceptionResponseFormatter_AuthorizationResponseHandler_BasicScheme_ShouldRenderResponseInJsonByNewtonsoft_UsingAspNetBootstrapping(FaultSensitivityDetails sensitivityDetails)
        {
            using (var startup = WebHostTestFactory.Create(services =>
                   {
                       services.AddNewtonsoftJsonExceptionResponseFormatter();
                       services.AddAuthorizationResponseHandler();
                       services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                           .AddBasic(o =>
                           {
                               o.RequireSecureConnection = false;
                               o.Authenticator = (username, password) => null;
                           });
                       services.AddAuthorization(o =>
                       {
                           o.FallbackPolicy = new AuthorizationPolicyBuilder()
                               .AddAuthenticationSchemes(BasicAuthorizationHeader.Scheme)
                               .RequireAuthenticatedUser()
                               .Build();

                       });
                       services.AddRouting();
                       services.PostConfigureAllExceptionDescriptorOptions(o => o.SensitivityDetails = sensitivityDetails);
                   }, app =>
                   {
                       app.UseRouting();
                       app.UseAuthentication();
                       app.UseAuthorization();
                       app.UseEndpoints(endpoints =>
                       {
                           endpoints.MapGet("/", context => context.Response.WriteAsync($"Hello {context.User.Identity!.Name}"));
                       });
                   }, hostFixture: null))
            {
                var client = startup.Host.GetTestClient();
                var bb = new BasicAuthorizationHeaderBuilder()
                    .AddUserName("Agent")
                    .AddPassword("Test");

                client.DefaultRequestHeaders.Add(HeaderNames.Authorization, bb.Build().ToString());
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

                var result = await client.GetAsync("/");
                var content = await result.Content.ReadAsStringAsync();

                TestOutput.WriteLine(content);

                Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
                Assert.Equal("Basic realm=\"AuthenticationServer\"", result.Headers.WwwAuthenticate.ToString());
                if (sensitivityDetails == FaultSensitivityDetails.All)
                {
                    Assert.Equal("""
                                 {
                                   "error": {
                                     "status": 401,
                                     "code": "Unauthorized",
                                     "message": "The request has not been applied because it lacks valid authentication credentials for the target resource.",
                                     "failure": {
                                       "type": "Cuemon.AspNetCore.Http.UnauthorizedException",
                                       "message": "The request has not been applied because it lacks valid authentication credentials for the target resource.",
                                       "headers": {},
                                       "statusCode": 401,
                                       "reasonPhrase": "Unauthorized",
                                       "inner": {
                                         "type": "System.Security.SecurityException",
                                         "message": "Unable to authenticate Agent."
                                       }
                                     }
                                   }
                                 }
                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
                }
                else
                {
                    Assert.Equal("""
                                 {
                                   "error": {
                                     "status": 401,
                                     "code": "Unauthorized",
                                     "message": "The request has not been applied because it lacks valid authentication credentials for the target resource."
                                   }
                                 }
                                 """.ReplaceLineEndings(), content.ReplaceLineEndings());
                }
            }
        }

    }
}
