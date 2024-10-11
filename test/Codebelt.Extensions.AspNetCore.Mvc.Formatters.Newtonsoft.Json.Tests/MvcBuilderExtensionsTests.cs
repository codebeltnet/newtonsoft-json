using System.Net.Http.Headers;
using System.Threading.Tasks;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Filters;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
{
    public class MvcBuilderExtensionsTests : Test
    {
        public MvcBuilderExtensionsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(FaultSensitivityDetails.All)]
        [InlineData(FaultSensitivityDetails.Evidence)]
        [InlineData(FaultSensitivityDetails.FailureWithStackTraceAndData)]
        [InlineData(FaultSensitivityDetails.FailureWithData)]
        [InlineData(FaultSensitivityDetails.FailureWithStackTrace)]
        [InlineData(FaultSensitivityDetails.Failure)]
        [InlineData(FaultSensitivityDetails.None)]
        public async Task OnException_ShouldCaptureException_RenderAsProblemDetails_UsingNewtonsoftJson(FaultSensitivityDetails sensitivity)
        {
            using var response = await WebHostTestFactory.RunAsync(
                services =>
                {
                    services
                        .AddControllers(o => o.Filters.AddFaultDescriptor())
                        .AddApplicationPart(typeof(StatusCodesController).Assembly)
                        .AddNewtonsoftJsonFormatters()
                        .AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.ProblemDetails);
                    services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = sensitivity);
                },
                app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(routes => { routes.MapControllers(); });
                },
                responseFactory: client =>
                {
                    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                    return client.GetAsync("/statuscodes/XXX/serverError");
                });

            var body = await response.Content.ReadAsStringAsync();
            TestOutput.WriteLine(body);

            switch (sensitivity)
            {
                case FaultSensitivityDetails.All:
                    Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "System.NotSupportedException",
                                          "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                          "message": "Main exception - look out for inner!",
                                          "stack": [
                                            "at Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets.StatusCodesController.Get_XXX(String app) *",
                                            "at *",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextExceptionFilterAsync>*"
                                          ],
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "stack": [
                                              "at Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets.StatusCodesController.Get_XXX(String app)*"
                                            ],
                                            "data": {
                                              "app": "serverError"
                                            },
                                            "paramName": "app"
                                          }
                                        },
                                        "request": {
                                          "location": "http://localhost/statuscodes/XXX/serverError",
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
                    break;
                case FaultSensitivityDetails.Evidence:
                    Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*",
                                        "request": {
                                          "location": "http://localhost/statuscodes/XXX/serverError",
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
                    break;
                case FaultSensitivityDetails.FailureWithStackTraceAndData:
                    Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "System.NotSupportedException",
                                          "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                          "message": "Main exception - look out for inner!",
                                          "stack": [
                                            "at Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets.StatusCodesController.Get_XXX(String app) *",
                                            "at *",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextExceptionFilterAsync>*"
                                          ],
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "stack": [
                                              "at Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets.StatusCodesController.Get_XXX(String app)*"
                                            ],
                                            "data": {
                                              "app": "serverError"
                                            },
                                            "paramName": "app"
                                          }
                                        }
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.FailureWithData:
                    Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "System.NotSupportedException",
                                          "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                          "message": "Main exception - look out for inner!",
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "data": {
                                              "app": "serverError"
                                            },
                                            "paramName": "app"
                                          }
                                        }
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.FailureWithStackTrace:
                    Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "System.NotSupportedException",
                                          "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                          "message": "Main exception - look out for inner!",
                                          "stack": [
                                            "at Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets.StatusCodesController.Get_XXX(String app) *",
                                            "at *",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()",
                                            "--- End of stack trace from previous location ---",
                                            "at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextExceptionFilterAsync>*"
                                          ],
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "stack": [
                                              "at Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets.StatusCodesController.Get_XXX(String app)*"
                                            ],
                                            "paramName": "app"
                                          }
                                        }
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.Failure:
                    Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*",
                                        "failure": {
                                          "type": "System.NotSupportedException",
                                          "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                          "message": "Main exception - look out for inner!",
                                          "inner": {
                                            "type": "System.ArgumentException",
                                            "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                            "message": "This is an inner exception message ... (Parameter 'app')",
                                            "paramName": "app"
                                          }
                                        }
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.None:
                    Assert.True(Match("""
                                      {
                                        "type": "about:blank",
                                        "title": "InternalServerError",
                                        "status": 500,
                                        "detail": "An unhandled exception was raised by *",
                                        "instance": "http://localhost/statuscodes/XXX/serverError",
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
            }
        }

        [Theory]
        [InlineData(FaultSensitivityDetails.All)]
        [InlineData(FaultSensitivityDetails.Evidence)]
        [InlineData(FaultSensitivityDetails.FailureWithStackTraceAndData)]
        [InlineData(FaultSensitivityDetails.FailureWithData)]
        [InlineData(FaultSensitivityDetails.FailureWithStackTrace)]
        [InlineData(FaultSensitivityDetails.Failure)]
        [InlineData(FaultSensitivityDetails.None)]
        public async Task OnException_ShouldCaptureException_RenderAsDefault_UsingNewtonsoftJson(FaultSensitivityDetails sensitivity)
        {
            using var response = await WebHostTestFactory.RunAsync(
                services =>
                {
                    services
                        .AddControllers(o => o.Filters.AddFaultDescriptor())
                        .AddApplicationPart(typeof(StatusCodesController).Assembly)
                        .AddNewtonsoftJsonFormatters()
                        .AddFaultDescriptorOptions(o => o.FaultDescriptor = PreferredFaultDescriptor.FaultDetails);
                    services.PostConfigureAllOf<IExceptionDescriptorOptions>(o => o.SensitivityDetails = sensitivity);
                },
                app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(routes => { routes.MapControllers(); });
                },
                responseFactory: client =>
                {
                    client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
                    return client.GetAsync("/statuscodes/XXX/serverError");
                });

            var body = await response.Content.ReadAsStringAsync();
            TestOutput.WriteLine(body);

            switch (sensitivity)
            {
                case FaultSensitivityDetails.All:
                    Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *.",
                                          "failure": {
                                            "type": "System.NotSupportedException",
                                            "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                            "message": "Main exception - look out for inner!",
                                            "stack": [
                                              "at Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets.StatusCodesController.Get_XXX(String app) *",
                                              "at *",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextExceptionFilterAsync>*"
                                            ],
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "stack": [
                                                "at Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets.StatusCodesController.Get_XXX(String app)*"
                                              ],
                                              "data": {
                                                "app": "serverError"
                                              },
                                              "paramName": "app"
                                            }
                                          }
                                        },
                                        "evidence": {
                                          "request": {
                                            "location": "http://localhost/statuscodes/XXX/serverError",
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
                    break;
                case FaultSensitivityDetails.Evidence:
                    Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *."
                                        },
                                        "evidence": {
                                          "request": {
                                            "location": "http://localhost/statuscodes/XXX/serverError",
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
                    break;
                case FaultSensitivityDetails.FailureWithStackTraceAndData:
                    Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *.",
                                          "failure": {
                                            "type": "System.NotSupportedException",
                                            "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                            "message": "Main exception - look out for inner!",
                                            "stack": [
                                              "at Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets.StatusCodesController.Get_XXX(String app) *",
                                              "at *",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextExceptionFilterAsync>*"
                                            ],
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "stack": [
                                                "at Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets.StatusCodesController.Get_XXX(String app)*"
                                              ],
                                              "data": {
                                                "app": "serverError"
                                              },
                                              "paramName": "app"
                                            }
                                          }
                                        },
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.FailureWithData:
                    Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *.",
                                          "failure": {
                                            "type": "System.NotSupportedException",
                                            "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                            "message": "Main exception - look out for inner!",
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "data": {
                                                "app": "serverError"
                                              },
                                              "paramName": "app"
                                            }
                                          }
                                        },
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.FailureWithStackTrace:
                    Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *.",
                                          "failure": {
                                            "type": "System.NotSupportedException",
                                            "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                            "message": "Main exception - look out for inner!",
                                            "stack": [
                                              "at Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets.StatusCodesController.Get_XXX(String app) *",
                                              "at *",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute*",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()",
                                              "--- End of stack trace from previous location ---",
                                              "at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextExceptionFilterAsync>*"
                                            ],
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "stack": [
                                                "at Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Assets.StatusCodesController.Get_XXX(String app)*"
                                              ],
                                              "paramName": "app"
                                            }
                                          }
                                        },
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.Failure:
                    Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *.",
                                          "failure": {
                                            "type": "System.NotSupportedException",
                                            "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                            "message": "Main exception - look out for inner!",
                                            "inner": {
                                              "type": "System.ArgumentException",
                                              "source": "Codebelt.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Tests",
                                              "message": "This is an inner exception message ... (Parameter 'app')",
                                              "paramName": "app"
                                            }
                                          }
                                        },
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
                case FaultSensitivityDetails.None:
                    Assert.True(Match("""
                                      {
                                        "error": {
                                          "instance": "http://localhost/statuscodes/XXX/serverError",
                                          "status": 500,
                                          "code": "InternalServerError",
                                          "message": "An unhandled exception was raised by *."
                                        },
                                        "traceId": "*"
                                      }
                                      """.ReplaceLineEndings(), body.ReplaceLineEndings(), o => o.ThrowOnNoMatch = true));
                    break;
            }
        }
    }
}
