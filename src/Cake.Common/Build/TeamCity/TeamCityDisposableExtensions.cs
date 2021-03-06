﻿using System;

namespace Cake.Common.Build.TeamCity
{
    /// <summary>
    /// A set of extensions for allowing "using" with TeamCity "blocks".
    /// </summary>
    public static class TeamCityDisposableExtensions
    {
        /// <summary>
        /// Writes the start of a TeamCity message block, then returns a disposable that write the end on Dispose.
        /// </summary>
        /// <param name="teamCityProvider">TeamCity provider.</param>
        /// <param name="blockName">The name of the report block.</param>
        /// <returns>A disposable wrapper the writes the report block end.</returns>
        public static TeamCityActionDisposable Block(this TeamCityProvider teamCityProvider, string blockName)
        {
            if (teamCityProvider == null)
            {
                throw new ArgumentNullException("teamCityProvider");
            }
            teamCityProvider.WriteStartBlock(blockName);
            return new TeamCityActionDisposable(teamCityProvider, tc => tc.WriteEndBlock(blockName));
        }

        /// <summary>
        /// Writes the start of a TeamCity build block, then returns a disposable that write the end on Dispose.
        /// </summary>
        /// <param name="teamCityProvider">TeamCity provider.</param>
        /// <param name="compilerName">The name of the build block.</param>
        /// <returns>A disposable wrapper the writes the build block end.</returns>
        public static TeamCityActionDisposable BuildBlock(this TeamCityProvider teamCityProvider, string compilerName)
        {
            if (teamCityProvider == null)
            {
                throw new ArgumentNullException("teamCityProvider");
            }
            teamCityProvider.WriteStartBuildBlock(compilerName);
            return new TeamCityActionDisposable(teamCityProvider, tc => tc.WriteEndBuildBlock(compilerName));
        }

        /// <summary>
        /// Disposable helper for writing TeamCity message blocks.
        /// </summary>
        public sealed class TeamCityActionDisposable : IDisposable
        {
            private readonly ITeamCityProvider _teamCityProvider;
            private readonly Action<ITeamCityProvider> _disposeAction;

            /// <summary>
            /// Initializes a new instance of the <see cref="TeamCityActionDisposable"/> class.
            /// </summary>
            /// <param name="teamCityProvider">TeamCity provider.</param>
            /// <param name="disposeAction">The action to do on Dispose.</param>
            public TeamCityActionDisposable(ITeamCityProvider teamCityProvider, Action<ITeamCityProvider> disposeAction)
            {
                _teamCityProvider = teamCityProvider;
                _disposeAction = disposeAction;
            }

            /// <summary>
            /// Writes the end block for this message block.
            /// </summary>
            public void Dispose()
            {
                _disposeAction(_teamCityProvider);
            }
        }
    }
}