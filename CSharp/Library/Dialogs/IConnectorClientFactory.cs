﻿// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Bot Framework: http://botframework.com
// 
// Bot Builder SDK Github:
// https://github.com/Microsoft/BotBuilder
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Bot.Builder.Dialogs.Internals
{
    /// <summary>
    /// Factory for IConnectorClient.
    /// </summary>
    public interface IConnectorClientFactory
    {
        /// <summary>
        /// Make the IConnectorClient implementation.
        /// </summary>
        /// <returns>The IConnectorClient implementation.</returns>
        IConnectorClient Make();
    }

    public sealed class DetectEmulatorFactory : IConnectorClientFactory
    {
        private readonly Message message;
        private readonly Uri emulator;
        public DetectEmulatorFactory(Message message, Uri emulator)
        {
            SetField.NotNull(out this.message, nameof(message), message);
            SetField.NotNull(out this.emulator, nameof(emulator), emulator);
        }
        IConnectorClient IConnectorClientFactory.Make()
        {
            var channel = this.message.From;
            var isEmulator = channel?.ChannelId?.Equals("emulator", StringComparison.OrdinalIgnoreCase);
            if (isEmulator ?? false)
            {
                return new ConnectorClient(this.emulator, new ConnectorClientCredentials());
            }
            else
            {
                return new ConnectorClient();
            }
        }
    }
}
