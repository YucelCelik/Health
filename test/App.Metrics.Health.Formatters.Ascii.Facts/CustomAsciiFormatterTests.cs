﻿// <copyright file="CustomAsciiFormatterTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.IO;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Health.Formatters.Ascii.Facts.Fixtures;
using App.Metrics.Health.Formatters.Ascii.Facts.TestHelpers;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Formatters.Ascii.Facts
{
    public class CustomAsciiFormatterTests
    {
        private readonly HealthFixture _fixture;

        public CustomAsciiFormatterTests()
        {
            _fixture = new HealthFixture();
        }

        [Fact]
        public async Task Can_apply_custom_ascii_health_formatting()
        {
            // Arrange
            string result;
            _fixture.HealthCheckRegistry.AddCheck("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            var formatter = new CustomAsciiOutputFormatter();

            // Act
            var healthStatus = await _fixture.Health.ReadAsync();

            using (var stream = new MemoryStream())
            {
                await formatter.WriteAsync(stream, healthStatus, Encoding.UTF8);

                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            // Assert
            result.Should().Be("Overall: Healthy\ntest OK Healthy\n");
        }
    }
}