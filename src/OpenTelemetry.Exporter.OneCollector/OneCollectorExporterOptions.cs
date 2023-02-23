// <copyright file="OneCollectorExporterOptions.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace OpenTelemetry.Exporter.OneCollector;

/// <summary>
/// Contains options for the <see cref="OneCollectorExporter{T}"/> class.
/// </summary>
public abstract class OneCollectorExporterOptions
{
    internal OneCollectorExporterOptions()
    {
    }

    /// <summary>
    /// Gets or sets the OneCollector instrumentation key.
    /// </summary>
    /// <remarks>
    /// Note: Instrumentation key is required.
    /// </remarks>
    [Required]
    public string? InstrumentationKey { get; set; }

    /// <summary>
    /// Gets the OneCollector transport options.
    /// </summary>
    public OneCollectorExporterTransportOptions TransportOptions { get; } = new();

    /// <summary>
    /// Gets the OneCollector tenant token.
    /// </summary>
    internal string? TenantToken { get; private set; }

    internal virtual void Validate()
    {
        if (string.IsNullOrWhiteSpace(this.InstrumentationKey))
        {
            throw new InvalidOperationException($"{nameof(this.InstrumentationKey)} was not specified on {this.GetType().Name} options.");
        }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        var positionOfFirstDash = this.InstrumentationKey.IndexOf('-', StringComparison.OrdinalIgnoreCase);
#else
        var positionOfFirstDash = this.InstrumentationKey!.IndexOf('-');
#endif
        if (positionOfFirstDash < 0)
        {
            throw new InvalidOperationException($"{nameof(this.InstrumentationKey)} specified on {this.GetType().Name} options is invalid.");
        }

        this.TenantToken = this.InstrumentationKey.Substring(0, positionOfFirstDash);

        this.TransportOptions.Validate();
    }
}
