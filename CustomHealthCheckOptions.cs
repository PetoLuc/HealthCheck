using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCheck
{
	public class CustomHealthCheckOptions : HealthCheckOptions
	{
		public CustomHealthCheckOptions() : base()
		{
			//var jsonSerializerOption = new JsonSerializerOptions
			//{
			//	WriteIndented = true
			//};
			ResponseWriter = async (context, report) =>
			  {
				  context.Response.ContentType = MediaTypeNames.Application.Json;
				  context.Response.StatusCode = StatusCodes.Status200OK;

				  var result = JsonSerializer.Serialize(new
				  {
					  checks = report.Entries.Select(e => new
					  {
						  name = e.Key,
						  responseTime = e.Value.Duration.TotalMilliseconds,
						  status = e.Value.Status.ToString(),
						  description = e.Value.Description
					  }),
					  totalStatus = report.Status,
					  totalResponseTime = report.TotalDuration.TotalMilliseconds
				  }, new JsonSerializerOptions { WriteIndented = true });

				  await context.Response.WriteAsync(result);
			  };
		}
	}
}
