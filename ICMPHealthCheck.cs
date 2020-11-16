using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck
{
	public class ICMPHealthCheck : IHealthCheck
	{
		private string Host { get; set; }
		private int Timeout { get; set; }

		public ICMPHealthCheck(string host, int timeout)		
		{
			Host = host;
			Timeout = timeout;
		}
		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			try
			{
				using (var ping = new Ping())
				{
					var reply = await ping.SendPingAsync(Host);
					switch(reply.Status)
					{
						case IPStatus.Success:
							string message = $"IMCP to {Host} took {reply.RoundtripTime}";
							return (reply.RoundtripTime > Timeout) ? HealthCheckResult.Degraded(message) : HealthCheckResult.Healthy(message);
						default:
							string error = $"IMCP to {Host} failed {reply.Status}";
							return HealthCheckResult.Unhealthy(error);
					}
				}
			}
			catch (Exception ex)
			{
				return HealthCheckResult.Unhealthy($"IMCP to {Host} failed {ex.Message}");
			}
		}
	}
}
